namespace ET.Client
{
    public static class ItemFactory
    {
        /// <summary>
        /// 通过 configId 生成 Item。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="configId"></param>
        /// <returns></returns>
        public static Item Create(Entity self, int configId)
        {
            Item item = self?.AddChild<Item, int>(configId);
            return item;
        }

        /// <summary>
        /// 通过物品信息生成 Item。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="itemInfo"></param>
        /// <returns></returns>
        public static Item Create(Entity self, ItemInfo itemInfo)
        {
            Item item = self?.AddChild<Item, int>(itemInfo.ItemConfigId);
            item?.FromMessage(itemInfo);
            return item;
        }
    }
}