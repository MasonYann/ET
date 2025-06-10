using System;
using System.Collections.Generic;

namespace ET.Server
{
	/// <summary>
	/// 匹配服务器请求 Map 服务器申请一个房间。
	/// </summary>
	[MessageHandler(SceneType.Map)]
	public class Match2Map_GetRoomHandler : MessageHandler<Scene, Match2Map_GetRoom, Map2Match_GetRoom>
	{
		protected override async ETTask Run(Scene root, Match2Map_GetRoom request, Map2Match_GetRoom response)
		{
			//RoomManagerComponent roomManagerComponent = root.GetComponent<RoomManagerComponent>();
			
			// 从根 Fiber 上创建一个子 Fiber，用于创建房间纤程。
			Fiber fiber = root.Fiber();
			//  创建房间纤程
			int fiberId = await FiberManager.Instance.Create(SchedulerType.ThreadPool, fiber.Zone, SceneType.RoomRoot, "RoomRoot");
			ActorId roomRootActorId = new(fiber.Process, fiberId);

			// 发送消息给房间纤程，初始化
			RoomManager2Room_Init roomManager2RoomInit = RoomManager2Room_Init.Create();
			roomManager2RoomInit.PlayerIds.AddRange(request.PlayerIds);
			await root.GetComponent<MessageSender>().Call(roomRootActorId, roomManager2RoomInit);
			
			response.ActorId = roomRootActorId;
			await ETTask.CompletedTask;
		}
	}
}