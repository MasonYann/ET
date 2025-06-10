using UnityEngine.SceneManagement;

namespace ET.Client
{
    // [Event(SceneType.LockStep)]
    [Event(SceneType.Demo)]
    public class LSSceneChangeStart_AddComponent : AEvent<Scene, LSSceneChangeStart>
    {
        protected override async ETTask Run(Scene clientScene, LSSceneChangeStart args)
        {
            Scene root = clientScene.Root();
            
            Room room = clientScene.GetComponent<Room>();
            ResourcesLoaderComponent resourcesLoaderComponent = room.AddComponent<ResourcesLoaderComponent>();
            room.AddComponent<UIComponent>();

            // 创建loading界面
            root.GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Loading);
            
            // 创建房间UI
            // await UIHelper.Create(args.Room, UIType.UILSRoom, UILayer.Low);

            // 加载场景资源
            await resourcesLoaderComponent.LoadSceneAsync($"Assets/Bundles/Scenes/{room.Name}.unity", LoadSceneMode.Single);
        }
    }
}