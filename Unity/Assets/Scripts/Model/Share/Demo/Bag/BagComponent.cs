using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf]
    public class BagComponent: Entity, IAwake, IDestroy, IDeserialize, ITransfer
    {
        /// <summary>
        /// 储存物品 Id 和物品。
        /// </summary>
        [BsonIgnore]
        public Dictionary<long, Item> ItemsDict = new Dictionary<long, Item>();
    
        /// <summary>
        /// 储存物品类型和物品。
        /// </summary>
        [BsonIgnore]
        public MultiMap<int, Item> ItemsMap = new MultiMap<int, Item>();
        
        /// <summary>
        /// 更新物品信息。
        /// </summary>
        [BsonIgnore]
        public M2C_ItemUpdateOpInfo message => M2C_ItemUpdateOpInfo.Create();
    }
}
