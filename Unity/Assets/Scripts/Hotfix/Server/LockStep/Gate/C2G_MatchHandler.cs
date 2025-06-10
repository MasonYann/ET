namespace ET.Server
{
	/// <summary>
	/// 处理客户端发到 Gate 网关服务器的匹配请求。
	/// </summary>
	[MessageSessionHandler(SceneType.Gate)]
	public class C2G_MatchHandler : MessageSessionHandler<C2G_Match, G2C_Match>
	{
		protected override async ETTask Run(Session session, C2G_Match request, G2C_Match response)
		{
			Player player = session.GetComponent<SessionPlayerComponent>().Player;

			StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.Match;

			//  匹配成功，发送消息给 Match 服务器，请求一个房间
			G2Match_Match g2MatchMatch = G2Match_Match.Create();
			g2MatchMatch.Id = player.Id;
			await session.Root().GetComponent<MessageSender>().Call(startSceneConfig.ActorId, g2MatchMatch);
		}
	}
}