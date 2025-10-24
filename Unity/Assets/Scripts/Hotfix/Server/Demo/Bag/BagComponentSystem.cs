using System.Collections.Generic;

namespace ET.Server
{
    [FriendOf(typeof(BagComponent))]
    [FriendOf(typeof(Item))]
    [EntitySystemOf(typeof(BagComponent))]
    public static partial class BagComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.BagComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ET.BagComponent self)
        {
            foreach (var item in self.ItemsDict.Values)
            {
                item?.Dispose();
            }

            self.ItemsDict.Clear();
            self.ItemsMap.Clear();
        }

        /// <summary>
        /// 物品组件反序列化时添加物品到组件列表中。
        /// 1.从缓存服或数据库中反序列化时调用；
        /// 2.从 gate 网关服务器进程传到 Map 服务器时调用；
        /// </summary>
        [EntitySystem]
        private static void Deserialize(this ET.BagComponent self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                self.AddContainer(entity as Item);
            }
        }

        /// <summary>
        /// 是否达到最大负载。
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool IsMaxLoad(this BagComponent self)
        {
            return self.ItemsDict.Count == self.GetParent<Unit>().GetComponent<NumericComponent>()[NumericType.MaxBagCapacity];
        }

        /// <summary>
        /// 添加物品到组件列表中。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool AddContainer(this BagComponent self, Item item)
        {
            if (self.ItemsDict.ContainsKey(item.Id))
            {
                return false;
            }

            self.ItemsDict.Add(item.Id, item);
            self.ItemsMap.Add(item.Config.Type, item);
            return true;
        }

        /// <summary>
        /// 在组件列表中移除物品。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="item"></param>
        public static void RemoveContainer(this BagComponent self, Item item)
        {
            self.ItemsDict.Remove(item.Id);
            self.ItemsMap.Remove(item.Config.Type, item);
        }

        /// <summary>
        /// 通过配置 Id 添加物品。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="configId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool AddItemByConfigId(this BagComponent self, int configId, int count = 1)
        {
            if (!ItemConfigCategory.Instance.Contain(configId))
            {
                return false;
            }

            if (count <= 0)
            {
                return false;
            }

            for (int i = 0; i < count; i++)
            {
                Item newItem = ItemFactory.Create(self, configId);

                if (!self.AddItem(newItem))
                {
                    Log.Error("添加物品失败！");
                    newItem?.Dispose();
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 通过配置 Id 获取物品列表。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="configId"></param>
        /// <param name="list"></param>
        public static void GetItemListByConfigId(this BagComponent self, int configId, List<Item> list)
        {
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(configId);
            foreach (var selfItem in self.ItemsMap[itemConfig.Type])
            {
                if (selfItem.ConfigId == configId)
                {
                    list.Add(selfItem);
                }
            }
        }

        /// <summary>
        /// 获取某个物品种类的物品数量。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public static int GetItemCountByItemType(this BagComponent self, ItemType itemType)
        {
            if (!self.ItemsMap.ContainsKey((int)itemType))
            {
                return 0;
            }

            return self.ItemsMap[(int)itemType].Count;
        }

        /// <summary>
        /// 是否可以添加物品。
        /// </summary>
        /// <param name="bagComponent"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool IsCanAddItem(this BagComponent self, Item item)
        {
            if (item == null || item.IsDisposed)
            {
                return false;
            }

            if (!ItemConfigCategory.Instance.Contain(item.ConfigId))
            {
                return false;
            }

            if (self.IsMaxLoad())
            {
                return false;
            }

            if (self.ItemsDict.ContainsKey(item.Id))
            {
                return false;
            }

            if (self.Parent == self)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 是否可以通过配置 Id 添加物品。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="configId"></param>
        /// <returns></returns>
        public static bool IsCanAddItemByConfigId(this BagComponent self, int configId)
        {
            //如果物品配置文件不包含该配置 Id
            if (!ItemConfigCategory.Instance.Contain(configId))
            {
                return false;
            }

            if (self.IsMaxLoad())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 是否可以添加物品列表。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static bool IsCanAddItemList(this BagComponent self, List<Item> items)
        {
            if (items.Count <= 0)
            {
                return false;
            }

            if (self.ItemsDict.Count + items.Count > self.GetParent<Unit>().GetComponent<NumericComponent>()[NumericType.MaxBagCapacity])
            {
                return false;
            }

            foreach (var item in items)
            {
                if (item == null || item.IsDisposed)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 往背包中添加物品。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="item"></param>
        public static bool AddItem(this BagComponent self, Item item)
        {
            if (item == null || item.IsDisposed)
            {
                Log.Error("item is null!");
                return false;
            }

            if (self.IsMaxLoad())
            {
                Log.Error("bag is IsMaxLoad!");
                return false;
            }

            if (!self.AddContainer(item))
            {
                Log.Error("Add Container is Error!");
                return false;
            }

            if (item.Parent != self)
            {
                self.AddChild(item);
            }

            ItemUpdateNoticeHelper.SyncAddItem(self.GetParent<Unit>(), item, self.message);
            return true;
        }

        /// <summary>
        /// 从背包中移除物品。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="item"></param>
        public static void RemoveItem(this BagComponent self, Item item)
        {
            self.RemoveContainer(item);
            ItemUpdateNoticeHelper.SyncRemoveItem(self.GetParent<Unit>(), item, self.message);
            item?.Dispose();
        }

        /// <summary>
        /// 从背包中移除物品但不销毁。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Item RemoveItemNoDispose(this BagComponent self, Item item)
        {
            self.RemoveContainer(item);
            ItemUpdateNoticeHelper.SyncRemoveItem(self.GetParent<Unit>(), item, self.message);
            return item;
        }

        /// <summary>
        /// 通过物品 Id 查看物品存在背包中。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static bool IsItemExist(this BagComponent self, long itemId)
        {
            self.ItemsDict.TryGetValue(itemId, out Item item);
            return item != null && !item.IsDisposed;
        }

        /// <summary>
        /// 从背包中获取物品。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public static Item GetItemById(this BagComponent self, long itemId)
        {
            self.ItemsDict.TryGetValue(itemId, out Item item);
            return item;
        }

        /// <summary>
        /// 清空背包物品。
        /// </summary>
        /// <param name="self"></param>
        public static void Clear(this BagComponent self)
        {
            ForeachHelper.Foreach(self.ItemsDict, (id, item) => { item?.Dispose(); });
            self.ItemsDict.Clear();
            self.ItemsMap.Clear();
        }
    }
}