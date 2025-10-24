using System;

namespace ET.Server
{

    [FriendOf(typeof(Item))]
    [EntitySystemOf(typeof(Item))]
    public static partial class ItemSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Item self, int configId)
        {
            self.ConfigId = configId;
        }
        [EntitySystem]
        private static void Destroy(this ET.Item self)
        {
            self.Quality = 0;
            self.ConfigId = 0;
        }
        /// <summary>
        /// Item 转单个物品 Proto。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="isAllInfo"></param>
        /// <returns></returns>
        public static ItemInfo ToMessage(this Item self, bool isAllInfo = true)
        {
            ItemInfo itemInfo = ItemInfo.Create();
            itemInfo.ItemUid = self.Id;
            itemInfo.ItemConfigId = self.ConfigId;
            itemInfo.ItemQuality = self.Quality;
        
            if (!isAllInfo)
            {
                return itemInfo;
            }
        
            EquipInfoComponent equipInfoComponent = self.GetComponent<EquipInfoComponent>();
        
            if (equipInfoComponent != null)
            {
                itemInfo.EquipInfo = equipInfoComponent.ToMessage();
            }
        
            return itemInfo;
        }
    }
}