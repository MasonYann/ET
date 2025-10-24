namespace ET
{
    public enum EntryType
    {
        Common = 1, //普通词条
        Special = 2, //特殊词条
    }

    /// <summary>
    /// 单个装备信息词条
    /// </summary>
    [ChildOf]
    public class AttributeEntry: Entity, IAwake, IDestroy, ISerializeToEntity
    {
        /// <summary>
        /// 词条数值属性类型。
        /// </summary>
        public int Key;

        /// <summary>
        /// 词条数值属性值。
        /// </summary>
        public long Value;

        /// <summary>
        /// 词条类型。
        /// </summary>
        public EntryType Type;
    }
}