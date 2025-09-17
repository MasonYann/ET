using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ChildOf]
    public class Item: Entity, IAwake<int>, IDestroy, ISerializeToEntity
    {
        /// <summary>
        /// 物品配置 Id。
        /// </summary>
        public int ConfigId = 0;

        /// <summary>
        /// 物品品质。
        /// </summary>
        public int Quality = 0;

        [BsonIgnore]
        public ItemConfig Config => ItemConfigCategory.Instance.Get(this.ConfigId);
    }
}