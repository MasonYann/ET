using System;

namespace ET.Client
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
        /// 单个物品 Proto 转 Item。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="itemInfo"></param>
        public static void FromMessage(this Item self, ItemInfo itemInfo)
        {
            self.Id = itemInfo.ItemUid;
            self.ConfigId = itemInfo.ItemConfigId;
            self.Quality = itemInfo.ItemQuality;

            if (itemInfo.EquipInfo != null)
            {
                EquipInfoComponent equipInfoComponent = self.GetComponent<EquipInfoComponent>();

                if (equipInfoComponent == null)
                {
                    equipInfoComponent = self.AddComponent<EquipInfoComponent>();
                }

                equipInfoComponent.FromMessage(itemInfo.EquipInfo);
            }
        }
    }
}