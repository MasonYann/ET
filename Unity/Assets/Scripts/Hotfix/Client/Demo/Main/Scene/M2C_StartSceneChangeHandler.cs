namespace ET.Client
{
	[MessageHandler(SceneType.Demo)]
	public class M2C_StartSceneChangeHandler : MessageHandler<Scene, M2C_StartSceneChange>
	{
		protected override async ETTask Run(Scene root, M2C_StartSceneChange message)
		{
			//等待客户端切换场景
			await SceneChangeHelper.SceneChangeTo(root, message.SceneName, message.SceneInstanceId);
		}
	}
}
