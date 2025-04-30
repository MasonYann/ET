namespace ET.Client
{
    [Event(SceneType.Current)]
    public class SceneChangeFinishEvent_CreateUIHelp : AEvent<Scene, SceneChangeFinish>
    {
        protected override async ETTask Run(Scene scene, SceneChangeFinish args)
        {
             // await scene.GetComponent<UIComponent>().ShowWindowAsync(WindowID.WindowID_Helper);
             scene.GetComponent<UIComponent>().CloseWindow(WindowID.WindowID_Loading);
             await scene.GetComponent<UIComponent>().ShowWindowAsync(WindowID.WindowID_Main);
             await ETTask.CompletedTask;
        }
    }
}