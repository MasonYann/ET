using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class SceneChangeFinish_ShowCurrentSceneUI : AEvent<Scene, SceneChangeFinish>
    {
        protected override async ETTask Run(Scene scene, SceneChangeFinish args)
        {
            var uiComponent = scene.Root().GetComponent<UIComponent>();
            if (uiComponent.IsWindowVisible(WindowID.WindowID_Loading))
            {
                uiComponent.CloseWindow(WindowID.WindowID_Loading);
            }
            else
            {
                Log.Warning("Loading window is not visible when trying to close it");
            }
             
            await scene.Root().GetComponent<UIComponent>().ShowWindowAsync(WindowID.WindowID_Main);
             
            await ETTask.CompletedTask;
             
        }
    }
}