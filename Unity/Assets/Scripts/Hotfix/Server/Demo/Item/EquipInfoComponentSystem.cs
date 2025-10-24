namespace ET.Server
{
    [FriendOf(typeof(Item))]
    [FriendOf(typeof(AttributeEntry))]
    [FriendOf(typeof(EquipInfoComponent))]
    [EntitySystemOf(typeof(EquipInfoComponent))]
    public static partial class EquipInfoComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.EquipInfoComponent self)
        {
            self.GenerateEntries();
        }

        [EntitySystem]
        private static void Destroy(this ET.EquipInfoComponent self)
        {
            self.IsInited = false;
            self.Score = 0;
            foreach (var entry in self.EntryList)
            {
                entry?.Dispose();
            }

            self.EntryList.Clear();
        }

        [EntitySystem]
        private static void Deserialize(this ET.EquipInfoComponent self)
        {
            foreach (var entity in self.Children.Values)
            {
                self.EntryList.Add(entity as AttributeEntry);
            }
        }

        /// <summary>
        /// 生成装备信息组件。
        /// </summary>
        /// <param name="self"></param>
        public static void GenerateEntries(this EquipInfoComponent self)
        {
            if (self.IsInited)
            {
                return;
            }

            self.IsInited = true;
            self.CreateEntry();
        }

        /// <summary>
        /// 创建属性词条。
        /// </summary>
        /// <param name="self"></param>
        public static void CreateEntry(this EquipInfoComponent self)
        {
            //获取物品配置
            ItemConfig itemConfig = self.GetParent<Item>().Config;
            //获取词条随机配置
            EntryRandomConfig entryRandomConfig = EntryRandomConfigCategory.Instance.Get(itemConfig.EntryRandomId);

            //创建普通词条
            int entryCount = RandomGenerator.RandomNumber(entryRandomConfig.EntryRandMinCount + self.GetParent<Item>().Quality, entryRandomConfig
                    .EntryRandMaxCount + self.GetParent<Item>().Quality);
            for (int i = 0; i < entryCount; i++)
            {
                EntryConfig entryConfig =
                        EntryConfigCategory.Instance.GetRandomEntryConfigByLevel((int)EntryType.Common, entryRandomConfig.EntryLevel);
                if (entryConfig == null)
                {
                    continue;
                }

                AttributeEntry attributeEntry = self.AddChild<AttributeEntry>();
                attributeEntry.Type = EntryType.Common;
                attributeEntry.Key = entryConfig.AttributeType;
                attributeEntry.Value = RandomGenerator.RandomNumber(entryConfig.AttributeMinValue,
                    entryConfig.AttributeMaxValue + self.GetParent<Item>().Quality);
                self.EntryList.Add(attributeEntry);
                self.Score += entryConfig.EntryScore;
            }

            //创建特殊词条
            entryCount = RandomGenerator.RandomNumber(entryRandomConfig.SpecialEntryRandMinCount, entryRandomConfig.SpecialEntryRandMaxCount);
            for (int i = 0; i < entryCount; i++)
            {
                EntryConfig entryConfig =
                        EntryConfigCategory.Instance.GetRandomEntryConfigByLevel((int)EntryType.Special, entryRandomConfig.SpecialEntryLevel);
                if (entryConfig == null)
                {
                    continue;
                }

                AttributeEntry attributeEntry = self.AddChild<AttributeEntry>();
                attributeEntry.Type = EntryType.Special;
                attributeEntry.Key = entryConfig.AttributeType;
                attributeEntry.Value = RandomGenerator.RandomNumber(entryConfig.AttributeMinValue, entryConfig.AttributeMaxValue);
                self.EntryList.Add(attributeEntry);
                self.Score += entryConfig.EntryScore;
            }
        }

        /// <summary>
        /// EquipInfoComponent 转装备信息 Proto。
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static EquipInfoProto ToMessage(this EquipInfoComponent self)
        {
            EquipInfoProto equipInfoProto = EquipInfoProto.Create();
            equipInfoProto.Id = self.Id;
            equipInfoProto.Score = self.Score;

            for (int i = 0; i < self.EntryList.Count; i++)
            {
                equipInfoProto.AttributeEntryProtoList.Add(self.EntryList[i].ToMessage());
            }

            return equipInfoProto;
        }
    }
}