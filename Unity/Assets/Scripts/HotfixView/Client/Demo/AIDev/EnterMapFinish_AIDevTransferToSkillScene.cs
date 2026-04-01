namespace ET.Client
{
    [Event(SceneType.Current)]
    public class SceneChangeFinish_AIDevTransferToSkillScene: AEvent<Scene, SceneChangeFinish>
    {
        protected override async ETTask Run(Scene scene, SceneChangeFinish args)
        {
            Scene root = scene.Root();
            if (!AIDevModeHelper.IsEnabled(root))
            {
                return;
            }

            if (scene.Name == AIDevModeHelper.TestSceneName)
            {
                return;
            }

            AIDevModeRuntimeComponent runtimeComponent = root.GetComponent<AIDevModeRuntimeComponent>();
            if (runtimeComponent == null)
            {
                runtimeComponent = root.AddComponent<AIDevModeRuntimeComponent>();
            }

            if (runtimeComponent.HasRedirectedToTestScene)
            {
                return;
            }

            ClientSenderComponent clientSenderComponent = root.GetComponent<ClientSenderComponent>();
            if (clientSenderComponent == null)
            {
                return;
            }

            runtimeComponent.HasRedirectedToTestScene = true;
            await clientSenderComponent.Call(C2M_TransferMap.Create());
        }
    }
}
