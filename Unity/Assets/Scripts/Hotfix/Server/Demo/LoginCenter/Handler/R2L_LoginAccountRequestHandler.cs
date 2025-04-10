namespace ET.Server
{
    [MessageHandler(SceneType.LoginCenter)]
    public class R2L_LoginAccountRequestHandler : MessageHandler<Scene, R2L_LoginAccountRequest, L2R_LoginAccountRequest>
    {
        protected override async ETTask Run(Scene scene, R2L_LoginAccountRequest request,
            L2R_LoginAccountRequest response)
        {
            long accountId = request.AccountName.GetLongHashCode();

            CoroutineLockComponent coroutineLockComponent = scene.GetComponent<CoroutineLockComponent>();
            using (await coroutineLockComponent.Wait(CoroutineLockType.LoginCenterLock, accountId))
            {
                //如果账号中心服务器没有该账号信息，表示当前账号没有登录
                if (!scene.GetComponent<LoginInfoRecordComponent>().IsExist(accountId))
                {
                    return;
                }

                //获取上一个客户端所连接的区服 Id
                int zone = scene.GetComponent<LoginInfoRecordComponent>().Get(accountId);
                //获取上一个客户端所连接的 gate 网关配置
                StartSceneConfig gateConfig = RealmGateAddressHelper.GetGate(zone, request.AccountName);

                //通知上一个客户端连接的 gate 网关，下线该客户端
                L2G_DisconnectGateUnit l2GDisconnectGateUnit = L2G_DisconnectGateUnit.Create();
                l2GDisconnectGateUnit.AccountName = request.AccountName;
                var g2LDisconnectGateUnit = (G2L_DisconnectGateUnit)await scene.GetComponent<MessageSender>()
                    .Call(gateConfig.ActorId, l2GDisconnectGateUnit);

                response.Error = g2LDisconnectGateUnit.Error;
            }
        }
    }
}