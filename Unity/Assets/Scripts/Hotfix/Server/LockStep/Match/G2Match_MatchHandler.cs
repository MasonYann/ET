using System;


namespace ET.Server
{
	/// <summary>
	/// 处理 Gate 服务器发到 Match 服务器的匹配请求。
	/// </summary>
	[MessageHandler(SceneType.Match)]
	public class G2Match_MatchHandler : MessageHandler<Scene, G2Match_Match, Match2G_Match>
	{
		protected override async ETTask Run(Scene scene, G2Match_Match request, Match2G_Match response)
		{
			MatchComponent matchComponent = scene.GetComponent<MatchComponent>();
			matchComponent.Match(request.Id).Coroutine();
			await ETTask.CompletedTask;
		}
	}
}