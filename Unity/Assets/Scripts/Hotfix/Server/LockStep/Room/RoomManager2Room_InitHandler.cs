using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 处理从 RoomManager 服务器到具体 Room 服务器的初始化请求。（房间初始化消息处理器）
    /// </summary>
    [MessageHandler(SceneType.RoomRoot)]
    public class RoomManager2Room_InitHandler: MessageHandler<Scene, RoomManager2Room_Init, Room2RoomManager_Init>
    {
        protected override async ETTask Run(Scene root, RoomManager2Room_Init request, Room2RoomManager_Init response)
        {
            //创建房间实体并挂载到场景根节点上。
            Room room = root.AddComponent<Room>();
            // 设置房间名称标识（用于调试和日志）
            room.Name = "Server";
            //初始化房间服务器组件，保存参与战斗的玩家ID列表
            room.AddComponent<RoomServerComponent, List<long>>(request.PlayerIds);

            // 创建锁步战斗世界实例
            // SceneType.LockStepServer 表示这是服务端战斗模拟场景
            room.LSWorld = new LSWorld(SceneType.LockStepServer);
            await ETTask.CompletedTask;
        }
    }
}