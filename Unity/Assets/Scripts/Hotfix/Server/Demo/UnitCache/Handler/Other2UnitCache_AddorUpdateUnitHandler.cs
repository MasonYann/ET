namespace ET.Server
{
    [MessageHandler(SceneType.UnitCache)]
    public class Other2UnitCache_AddorUpdateUnitHandler:MessageHandler<Scene,Other2UnitCache_AddOrUpdateUnit,UnitCache2Other_AddOrUpdateUnit>
    {
        protected override async ETTask Run(Scene scene, Other2UnitCache_AddOrUpdateUnit request, UnitCache2Other_AddOrUpdateUnit response)
        {
            UpdateUnitCacheAsync(scene, request, response).Coroutine();
            await ETTask.CompletedTask;
        }

        /// <summary>
        /// 添加或更新 UnitCache。
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        private static async ETTask UpdateUnitCacheAsync(Scene scene, Other2UnitCache_AddOrUpdateUnit request, UnitCache2Other_AddOrUpdateUnit response)
        {
            UnitCacheComponent unitCacheComponent = scene.GetComponent<UnitCacheComponent>();
            
            long unitId = request.UnitId;
            using (ListComponent<Entity> entityList = ListComponent<Entity>.Create())
            {
                for (int i = 0; i < request.EntityBytes.Count; i++)
                {
                    Entity entity = MongoHelper.Deserialize<Entity>(request.EntityBytes[i]);
                    entityList.Add(entity);
                }
                await unitCacheComponent.AddOrUpdate(unitId, entityList);
            }
        }
    }
}