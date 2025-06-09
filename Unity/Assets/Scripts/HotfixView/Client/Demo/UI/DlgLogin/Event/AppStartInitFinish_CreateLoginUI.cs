namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class AppStartInitFinish_CreateLoginUI: AEvent<Scene, AppStartInitFinish>
    {
        protected override async ETTask Run(Scene root, AppStartInitFinish args)
        {
            await root.GetComponent<UIComponent>().ShowWindowAsync(WindowID.WindowID_Login);
            //提前加载 Loading 界面资源
            await root.GetComponent<UIComponent>().PreLoadWindowAsync(WindowID.WindowID_Loading);
        }
    }
}