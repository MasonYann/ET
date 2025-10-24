using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    /// <summary>
    /// 装备栏容器。
    /// </summary>
    [ComponentOf]
    public class EquipmentsComponent : Entity, IAwake, IDestroy, ITransfer, IDeserialize
    {
        /// <summary>
        /// 装备位置和装备字典。
        /// </summary>
        [BsonIgnore]
        public Dictionary<int, Item> EquipItems = new Dictionary<int, Item>();

        /// <summary>
        /// 装备消息。
        /// </summary>
        [BsonIgnore]
        public M2C_ItemUpdateOpInfo message = M2C_ItemUpdateOpInfo.Create();
    }
}