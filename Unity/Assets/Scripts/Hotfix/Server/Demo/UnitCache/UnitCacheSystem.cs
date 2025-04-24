namespace ET.Server
{
    [EntitySystemOf(typeof(UnitCache))]
    [FriendOf(typeof(UnitCache))]
    public static partial class UnitCacheSystem
    {
        [EntitySystem]
        private static void Awake(this UnitCache self)
        {
        }

        [EntitySystem]
        private static void Destroy(this UnitCache self)
        {
            //释放列表中的所有实体
            foreach (EntityRef<Entity> entityRef in self.CacheComponentDic.Values)
            {
                Entity entity = entityRef;
                entity.Dispose();
            }

            //清空自身的列表，释放 key
            self.CacheComponentDic.Clear();
            self.key = default;
        }

        /// <summary>
        /// 把 Entity 添加到 UnitCache 的列表中。
        /// </summary>
        /// <param name="self">实体 Id</param>
        /// <param name="entity">实体 Id 的数据</param>
        public static void AddOrUpdate(this UnitCache self, Entity entity)
        {
            if (entity == null)
            {
                return;
            }

            //entity 的 Id 和 unitId 一致
            if (self.CacheComponentDic.TryGetValue(entity.Id, out EntityRef<Entity> oldEntityRef))
            {
                //如果之前保存的 entity 与新的 entity 不同，就释放并移除他
                Entity oldEntity = oldEntityRef;
                if (entity != oldEntity)
                {
                    oldEntity.Dispose();
                }

                self.CacheComponentDic.Remove(entity.Id);
            }

            //然后添加新的 entity
            self.CacheComponentDic.Add(entity.Id, entity);
        }

        /// <summary>
        /// 获取 UnitCache 列表中的数据。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="unitId">实体 Id</param>
        /// <returns>实体 Id 的数据</returns>
        public static async ETTask<Entity> Get(this UnitCache self, long unitId)
        {
            //声明一个实体
            Entity entity = null;
            //通过实体 Id 查询实体引用，
            //如果没有找到实体 Id 的数据，就从数据库中加载实体 Id 的数据到缓存服
            //如果获取到实体引用，就直接返回实体引用
            if (!self.CacheComponentDic.TryGetValue(unitId, out EntityRef<Entity> entityRef))
            {
                entity = await self.Root().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone()).Query<Entity>(unitId, self.key);
                if (entity != null)
                {
                    self.AddOrUpdate(entity);
                }
            }
            else
            {
                entity = entityRef;
            }

            return entity;
        }

        /// <summary>
        /// 删除 UnitCache 列表中的数据。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="id">实体 Id</param>
        public static void Delete(this UnitCache self, long id)
        {
            if (self.CacheComponentDic.TryGetValue(id, out EntityRef<Entity> entityRef))
            {
                self.CacheComponentDic.Remove(id);
                Entity entity = entityRef;
                entity.Dispose();
            }
        }
    }
}