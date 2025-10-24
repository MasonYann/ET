namespace ET.Server
{
    public static partial class ItemFactory
    {
        /// <summary>
        /// 通过 configId 生成 Item。
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="configId"></param>
        /// <returns></returns>
        public static Item Create(Entity parent, int configId)
        {
            if (!ItemConfigCategory.Instance.Contain(configId))
            {
                Log.Error($"当前所创建的物品 Id 不存在:{configId}");
                return null;
            }

            Unit unit = parent as Unit;
            Item item = unit.AddChild<Item, int>(configId);
            //随机品质
            item.RandomQuality();

            AddComponentByItemType(item);
            return item;
        }

        /// <summary>
        /// 通过物品类型添加组件。
        /// </summary>
        /// <param name="item"></param>
        public static void AddComponentByItemType(Item item)
        {
            switch ((ItemType)item.Config.Type)
            {
                //武器 防具 戒指
                case ItemType.Weapon:
                case ItemType.Armor:
                case ItemType.Ring:
                {
                    //添加装备组件
                    item.AddComponent<EquipInfoComponent>();
                }
                    break;
                //道具
                case ItemType.Prop:
                {
                }
                    break;
            }
        }
    }
}