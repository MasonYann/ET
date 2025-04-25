using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ET.Server
{
    [MessageSessionHandler(SceneType.Realm)]
    [FriendOfAttribute(typeof(Account))]
    public class C2R_LoginAccountHandler : MessageSessionHandler<C2R_LoginAccount, R2C_LoginAccount>
    {
        protected override async ETTask Run(Session session, C2R_LoginAccount request, R2C_LoginAccount response)
        {
            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedly;
                session.Disconnect().Coroutine();
                return;
            }
            
            if (string.IsNullOrEmpty(request.AccountName) || string.IsNullOrEmpty(request.Password))
            {
                response.Error = ErrorCode.ERR_LoginInfoIsNull;
                session.Disconnect().Coroutine();
                return;
            }

            if (!Regex.IsMatch(request.AccountName.Trim(), @"^(?=.*[0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{6,15}$"))
            {
                response.Error = ErrorCode.ERR_AccountNameFormError;
                session.Disconnect().Coroutine();
                return;
            }

            if (!Regex.IsMatch(request.Password.Trim(), @"^[A-Za-z0-9]+$"))
            {
                response.Error = ErrorCode.ERR_PasswordFormError;
                session.Disconnect().Coroutine();
                return;
            }


            CoroutineLockComponent coroutineLockComponent = session.Root().GetComponent<CoroutineLockComponent>();
            //SessionLockingComponent 用来锁住 Session 连接，防止外挂模仿 Session 连接
            using (session.AddComponent<SessionLockingComponent>())
            {
                //用来锁住用户名，防止两个设备的用户同时连接
                using (await coroutineLockComponent.Wait(CoroutineLockType.LoginAccount, request.AccountName.GetLongHashCode()))
                {
                    DBComponent dbComponent = session.Root().GetComponent<DBManagerComponent>().GetZoneDB(session.Zone());

                    List<Account> accountInfoList = await dbComponent.Query<Account>(d => d.AccountName.Equals(request.AccountName));
                    Account account = null;
                    
                    if (accountInfoList != null && accountInfoList.Count > 0)
                    {
                        account = accountInfoList[0];
                        session.AddChild(account);
                        if (account.AccountType == (int)AccountType.BlackList)
                        {
                            response.Error = ErrorCode.ERR_AccountInBlackListError;
                            session.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }


                        if (!account.Password.Equals(request.Password))
                        {
                            response.Error = ErrorCode.ERR_LoginPasswordError;
                            session.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }
                    }
                    else
                    {
                        account = session.AddChild<Account>();
                        account.AccountName = request.AccountName.Trim();
                        account.Password    = request.Password;
                        account.CreateTime  = TimeInfo.Instance.ServerNow();
                        account.AccountType = (int)AccountType.General;
                        await dbComponent.Save<Account>(account);
                    }
                    
                    //发送消息给 LoginCenter 账号中心服务器，查看账号登录状态
                    R2L_LoginAccountRequest r2LLoginAccountRequest = R2L_LoginAccountRequest.Create();
                    r2LLoginAccountRequest.AccountName = request.AccountName;


                    StartSceneConfig loginCenterConfig = StartSceneConfigCategory.Instance.LoginCenterConfig;
                    var loginAccountResponse =  await session.Fiber().Root.GetComponent<MessageSender>().Call(loginCenterConfig.ActorId, r2LLoginAccountRequest) as L2R_LoginAccountRequest;
                    
                    if (loginAccountResponse.Error != ErrorCode.ERR_Success)
                    {
                        response.Error = loginAccountResponse.Error;
                        session?.Disconnect().Coroutine();
                        account?.Dispose();
                        return;
                    }

                    //获取并断开当前账户旧 Session 连接
                    Session otherSession  = session.Root().GetComponent<AccountSessionsComponent>().Get(request.AccountName);
                   
                    otherSession?.Send( A2C_Disconnect.Create());
                    otherSession?.Disconnect().Coroutine();
                    //添加当前 Session 到账号的 Session 组件中
                    session.Root().GetComponent<AccountSessionsComponent>().Add(request.AccountName, session);
                    session.AddComponent<AccountCheckOutTimeComponent, string>(request.AccountName);

                    //生成一个随机Token，移除旧 Token，添加新 Token 到 Token 组件中
                    string Token = TimeInfo.Instance.ServerNow().ToString() + RandomGenerator.RandomNumber(int.MinValue, int.MaxValue).ToString();
                    session.Root().GetComponent<TokenComponent>().Remove(request.AccountName);
                    session.Root().GetComponent<TokenComponent>().Add(request.AccountName, Token);
                    
                    response.Token = Token;
                    
                    account?.Dispose();
                }
            }
        }
    }
}