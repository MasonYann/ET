using System;

namespace ET.Server
{
    /// <summary>
    /// 客户端请求进入游戏的处理器
    /// </summary>
    [MessageSessionHandler(SceneType.Gate)]
    public class C2G_EnterGameHandler : MessageSessionHandler<C2G_EnterGame, G2C_EnterGame>
    {
        protected override async ETTask Run(Session session, C2G_EnterGame request, G2C_EnterGame response)
        {
            //SessionLockingComponent 用来锁住 Session 连接，防止外挂模仿 Session 连接
            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedly;
                return;
            }

            //获取 SessionPlayer 组件，获得 Player 实体
            SessionPlayerComponent sessionPlayerComponent = session.GetComponent<SessionPlayerComponent>();
            if (null == sessionPlayerComponent)
            {
                response.Error = ErrorCode.ERR_SessionPlayerError;
                return;
            }

            Player player = sessionPlayerComponent.Player;

            if (player == null || player.IsDisposed)
            {
                response.Error = ErrorCode.ERR_NonePlayerError;
                return;
            }

            CoroutineLockComponent coroutineLockComponent = session.Root().GetComponent<CoroutineLockComponent>();

            long instanceId = session.InstanceId;

            // 添加会话锁定组件，防止重复请求
            using (session.AddComponent<SessionLockingComponent>())
            {
                // 使用协程锁，确保同一账号不会并发登录
                using (await coroutineLockComponent.Wait(CoroutineLockType.LoginGate, player.Account.GetLongHashCode()))
                {
                    // 检查会话是否仍然有效
                    if (instanceId != session.InstanceId || player.IsDisposed)
                    {
                        response.Error = ErrorCode.ERR_PlayerSessionError;
                        return;
                    }

                    // 检查玩家是否已经在游戏中
                    if (player.PlayerState == PlayerState.Game)
                    {
                        try
                        {
                            // 处理二次登录逻辑
                            G2M_SecondLogin g2MSecondLogin = G2M_SecondLogin.Create();
                            IResponse reqEnter = await session.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.Unit).Call(player.UnitId, g2MSecondLogin);
                            if (reqEnter.Error == ErrorCode.ERR_Success)
                            {
                                Log.Console("作业:二次登陆逻辑，补全下发切换场景消息");
                                return;
                            }

                            Log.Error("二次登录失败  " + reqEnter.Error + " | " + reqEnter.Message);
                            response.Error = ErrorCode.ERR_ReEnterGameError;
                            await DisconnectHelper.KickPlayerNoLock(player);
                            session.Disconnect().Coroutine();
                        }
                        catch (Exception e)
                        {
                            Log.Error("二次登录失败  " + e);
                            response.Error = ErrorCode.ERR_ReEnterGameError2;
                            await DisconnectHelper.KickPlayerNoLock(player);

                            session.Disconnect().Coroutine();
                        }

                        return;
                    }

                    try
                    {
                        // 在Gate上动态创建一个Map Scene，把Unit从DB中加载放进来，然后传送到真正的Map中，这样登陆跟传送的逻辑就完全一样了
                        // GateMapComponent gateMapComponent = player.AddComponent<GateMapComponent>();
                        // gateMapComponent.Scene = await GateMapFactory.Create(gateMapComponent, player.Id, IdGenerater.Instance.GenerateInstanceId(), "GateMap");
                        //
                        // Scene scene = gateMapComponent.Scene;
                        //
                        // // 这里可以从DB中加载Unit 
                        // Unit unit = UnitFactory.Create(scene, player.Id, UnitType.Player);
                        // long unitId = unit.Id;

                        //从缓存服中获取 Unit
                        (bool isNewPlayer, Unit unit) = await UnitLoadHelper.LoadUnit(player);

                        //登录邮件服
                        // await LoginMailServer(player, unit);

                        long unitId = unit.Id;

                        //拿到游戏 Map1 服的配置
                        StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(session.Zone(), "Map1");

                        // 等到一帧的最后面再传送，先让G2C_EnterMap返回，否则传送消息可能比G2C_EnterMap还早
                        TransferHelper.TransferAtFrameFinish(unit, startSceneConfig.ActorId, startSceneConfig.Name).Coroutine();
                        player.UnitId = unitId;
                        response.MyUnitId = unitId;
                        player.PlayerState = PlayerState.Game;
                    }
                    catch (Exception e)
                    {
                        Log.Error($"角色进入游戏逻辑服出现问题 账号Id: {player.Account}  角色Id: {player.Id}   异常信息： {e}");
                        response.Error = ErrorCode.ERR_EnterGameError;
                        await DisconnectHelper.KickPlayerNoLock(player);
                        session.Disconnect().Coroutine();
                    }
                }
            }
        }

        /// <summary>
        /// 登录邮件服。
        /// </summary>
        /// <param name="player">玩家实体</param>
        /// <param name="unit">玩家单位</param>
        /// <returns>错误码</returns>
        public static async ETTask<int> LoginMailServer(Player player, Unit unit)
        {
            StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(player.Zone(), "Mail");

            // G2Mail_LoginMailServer g2MailLoginMailServer = G2Mail_LoginMailServer.Create();
            // g2MailLoginMailServer.UnitId = unit.Id;
            //
            // Mail2G_LoginMailServer mail2GGetMail = (Mail2G_LoginMailServer)await player.Root().GetComponent<MessageSender>()
            //         .Call(startSceneConfig.ActorId, g2MailLoginMailServer);
            // return mail2GGetMail.Error;

            await ETTask.CompletedTask;
            return 0;
        }
    }
}