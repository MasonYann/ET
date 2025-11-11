using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class SceneChangeFinishEvent_CreateUIHelp : AEvent<Scene, SceneChangeFinish>
    {
        protected override async ETTask Run(Scene scene, SceneChangeFinish args)
        {
             // await scene.GetComponent<UIComponent>().ShowWindowAsync(WindowID.WindowID_Helper);

             Log.Debug(scene.Name);
             var uiComponent = scene.GetComponent<UIComponent>();
             if (uiComponent.IsWindowVisible(WindowID.WindowID_Loading))
             {
                 uiComponent.CloseWindow(WindowID.WindowID_Loading);
             }
             else
             {
                 Log.Warning("Loading window is not visible when trying to close it");
             }
             await scene.GetComponent<UIComponent>().ShowWindowAsync(WindowID.WindowID_Main);
             await ETTask.CompletedTask;
             
        }
    }
}