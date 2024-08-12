using System;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;

namespace ET
{
    /// <summary>
    /// 帧同步 Unit 组件。
    /// </summary>
    [ChildOf(typeof(LSUnitComponent))]
    [MemoryPackable]
    public partial class LSUnit: LSEntity, IAwake, ISerializeToEntity
    {
        public TSVector Position
        {
            get;
            set;
        }

        [MemoryPackIgnore]
        [BsonIgnore]
        public TSVector Forward
        {
            get => this.Rotation * TSVector.forward;
            set => this.Rotation = TSQuaternion.LookRotation(value, TSVector.up);
        }
        
        public TSQuaternion Rotation
        {
            get;
            set;
        }
    }
}