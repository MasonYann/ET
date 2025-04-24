namespace ET.Server
{
    [EntitySystemOf(typeof(UnitCacheComponent))]
    [FriendOf(typeof(UnitCacheComponent))]
    [FriendOf(typeof(UnitCache))]
    public static partial class UnitCacheComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UnitCacheComponent self)
        {
            //将列表清空
            self.UnitCacheKeyList.Clear();
            //循环游戏列表中的类型
            foreach (var type in CodeTypes.Instance.GetTypes().Values)
            {
                //如果类型继承了 IUnitCache 接口，就把类型放入列表当中
                if (type != typeof(IUnitCache) && typeof(IUnitCache).IsAssignableFrom(type))
                {
                    self.UnitCacheKeyList.Add(type.FullName);
                }
            }

            //接着循环列表，生成对应的 UnitCache
            foreach (string key in self.UnitCacheKeyList)
            {
                UnitCache unitCache = self.AddChild<UnitCache>();
                unitCache.key = key;
                //将 UnitCache 放入字典中
                self.UnitCacheDic.Add(key, unitCache);
            }

            //添加 LRUCache 组件
            self.AddComponent<LRUCache>();
        }

        [EntitySystem]
        private static void Destroy(this UnitCacheComponent self)
        {
            //释放组件列表中的每一个 UnitCache
            foreach (EntityRef<UnitCache> unitCacheRef in self.UnitCacheDic.Values)
            {
                UnitCache unitCache = unitCacheRef;
                unitCache?.Dispose();
            }

            //清空列表 
            self.UnitCacheDic.Clear();
        }

        /// <summary>
        /// LRUCache 缓存组件调用。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="id"></param>
        public static void CallCache(this UnitCacheComponent self, long id)
        {
            self.GetComponent<LRUCache>().Call(id);
        }

        /// <summary>
        /// 添加或者更新数据缓存服的缓存数据。
        /// </summary>
        /// <param name="self">数据缓存组件</param>
        /// <param name="id">缓存UnitId</param>
        /// <param name="entityList">缓存类型列表</param>
        public static async ETTask AddOrUpdate(this UnitCacheComponent self, long id, ListComponent<Entity> entityList)
        {
            using (ListComponent<Entity> list = ListComponent<Entity>.Create())
            {
                //增加缓存数据时刷新 LRUCache
                self.CallCache(id);
                foreach (Entity entity in entityList)
                {
                    //获取 Enity 类型名字作为 key
                    string key = entity.GetType().Name;
                    UnitCache unitCache = default;
                    //如果缓存组件中没有缓存 Enity 类型
                    if (!self.UnitCacheDic.TryGetValue(key, out EntityRef<UnitCache> unitCacheRef))
                    {
                        //把 UnitCache 添加到 缓存组件上
                        unitCache = self.AddChild<UnitCache>();
                        unitCache.key = key;
                        self.UnitCacheDic.Add(key, unitCache);
                    }
                    else
                    {
                        unitCache = unitCacheRef;
                    }

                    //把类型放入 UnitCache 中
                    unitCache.AddOrUpdate(entity);
                    //把 UnitCache 添加到列表中
                    list.Add(entity);
                }

                if (list.Count > 0)
                {
                    //把列表中的缓存信息存入 Mongo 数据库中
                    await self.Root().GetComponent<DBManagerComponent>().GetZoneDB(self.Zone()).Save(id, list);
                }
            }
        }

        /// <summary>
        /// 获取数据缓存服的缓存数据。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="unitId">缓存UnitId</param>
        /// <param name="key">实体类型的名字</param> 
        /// <returns>存在在缓存服上的 Enitity 游戏数据</returns>
        public static async ETTask<Entity> Get(this UnitCacheComponent self, long unitId, string key)
        {
            UnitCache unitCache = default;
            //如果组件列表中不存在要查询的 unit 类型，就创建一个 unitCache 加入列表中
            if (!self.UnitCacheDic.TryGetValue(key, out EntityRef<UnitCache> unitCacheRef))
            {
                unitCache = self.AddChild<UnitCache>();
                unitCache.key = key;
                self.UnitCacheDic.Add(key, unitCache);
            }
            else
            {
                unitCache = unitCacheRef;
            }

            return await unitCache.Get(unitId);
        }

        /// <summary>
        /// 删除数据缓存服中的缓存数据。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public static async ETTask Delete(this UnitCacheComponent self, long unitId)
        {
            //开启数据缓存服获取数据库信息的携程锁
            using (await self.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.UnitCacheGet, unitId))
            {
                foreach (UnitCache unitCache in self.UnitCacheDic.Values)
                {
                    unitCache.Delete(unitId);
                }
            }
        }
     
    }
}