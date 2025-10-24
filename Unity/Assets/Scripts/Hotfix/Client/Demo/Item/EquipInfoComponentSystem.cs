namespace ET.Client
{
    [FriendOf(typeof(EquipInfoComponent))]
    [EntitySystemOf(typeof(EquipInfoComponent))]
    public static partial class EquipInfoComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.EquipInfoComponent self)
        {
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
        }
        
        /// <summary>
        /// equipInfo 转 EquipInfoComponent。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="equipInfoProto"></param>
        public static void FromMessage(this EquipInfoComponent self, EquipInfoProto equipInfoProto)
        { 
            self.Score = equipInfoProto.Score;
            for (int i = 0; i < self.EntryList.Count; i++)
            {
                self.EntryList[i]?.Dispose();
            }
            self.EntryList.Clear();
            for (int i = 0; i < equipInfoProto.AttributeEntryProtoList.Count; i++)
            {
                AttributeEntry attributeEntry = self.AddChild<AttributeEntry>();
                attributeEntry.FromMessage(equipInfoProto.AttributeEntryProtoList[i]);
                self.EntryList.Add(attributeEntry);
            }
            self.IsInited = true;
        }
    }
}