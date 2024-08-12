namespace ET.Server
{
    public static class GateMapFactory
    {
        /// <summary>
        /// 创建网关 Scene。
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="id"></param>
        /// <param name="instanceId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static async ETTask<Scene> Create(Entity parent, long id, long instanceId, string name)
        {
            await ETTask.CompletedTask;
            Scene scene = EntitySceneFactory.CreateScene(parent, id, instanceId, SceneType.Map, name);

            scene.AddComponent<UnitComponent>();
            scene.AddComponent<AOIManagerComponent>();
            scene.AddComponent<RoomManagerComponent>();
            
            scene.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.UnOrderedMessage);
            
            return scene;
        }
        
    }
}