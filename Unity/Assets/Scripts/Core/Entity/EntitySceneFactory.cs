namespace ET
{
    public static class EntitySceneFactory
    {
        /// <summary>
        /// 创建 Scene。
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="id"></param>
        /// <param name="instanceId"></param>
        /// <param name="sceneType"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Scene CreateScene(Entity parent, long id, long instanceId, SceneType sceneType, string name)
        {
            Scene scene = new(parent.Fiber(), id, instanceId, sceneType, name);
            parent?.AddChild(scene);
            return scene;
        }
    }
}