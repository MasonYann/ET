using System;
using UnityEngine.SceneManagement;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class SceneChangeStart_AddComponent: AEvent<Scene, SceneChangeStart>
    {
        protected override async ETTask Run(Scene root, SceneChangeStart args)
        {
            try
            {
                Scene currentScene = root.CurrentScene();

                //加载loading界面
                root.GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Loading);
                // currentScene.GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Loading);
                
                ResourcesLoaderComponent resourcesLoaderComponent = currentScene.GetComponent<ResourcesLoaderComponent>();
                // 加载场景资源
                await resourcesLoaderComponent.LoadSceneAsync($"Assets/Bundles/Scenes/{currentScene.Name}.unity", LoadSceneMode.Single);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

        }
    }
}