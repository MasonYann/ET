namespace ET.Client
{
    [FriendOf(typeof(AttributeEntry))]
    [EntitySystemOf(typeof(AttributeEntry))]
    public static partial class AttributeEntrySystem
    {
        [EntitySystem]
        private static void Awake(this ET.AttributeEntry self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ET.AttributeEntry self)
        {
            self.Key = 0;
            self.Value = 0;
            self.Type = EntryType.Common;
        }

        /// <summary>
        /// attributeEntryProto 转 AttributeEntry。
        /// </summary>  
        /// <param name="self"></param>
        /// <param name="attributeEntryProto"></param>
        public static void FromMessage(this ET.AttributeEntry self, AttributeEntryProto attributeEntryProto)
        {
            self.Id = attributeEntryProto.Id;
            self.Key = attributeEntryProto.Key;
            self.Value = attributeEntryProto.Value;
            self.Type = (EntryType)attributeEntryProto.EntryType;
        }
    }
}