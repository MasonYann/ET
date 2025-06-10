using System;


namespace ET.Server
{
	/// <summary>
	/// 处理 Match 服务器到 Gate 网关服务器的匹配成功通知。（匹配成功消息处理器）
	/// </summary>
	[MessageHandler(SceneType.Gate)]
	public class Match2G_NotifyMatchSuccessHandler : MessageHandler<Player, Match2G_NotifyMatchSuccess>
	{
		protected override async ETTask Run(Player player, Match2G_NotifyMatchSuccess message)
		{
			// 玩家加入房间，添加玩家房间组件，并设置房间ID
			player.AddComponent<PlayerRoomComponent>().RoomActorId = message.ActorId;
			
			// 向玩家发送匹配成功消息
			player.GetComponent<PlayerSessionComponent>().Session.Send(message);
			await ETTask.CompletedTask;
		}
	}
}