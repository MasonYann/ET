using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    /// <summary>
    /// 装备信息组件。
    /// </summary>
    [ComponentOf(typeof (Item))]
    public class EquipInfoComponent: Entity, IAwake, IDestroy, ISerializeToEntity, IDeserialize
    {
        /// <summary>
        /// 装备属性是否已生成。
        /// </summary>
        public bool IsInited = false;

        /// <summary>
        /// 装备品质。
        /// </summary>
        public int Score = 0;

        /// <summary>
        /// 装备词条列表。
        /// </summary>
        [BsonIgnore]
        public List<AttributeEntry> EntryList = new List<AttributeEntry>();
    }
}