using System;
namespace ET.Server
{
    public struct LRUUnitCacheDelete
    {
        public EntityRef<LRUCache> LruCache;
        public long Key;
    }
     
    /// <summary>
    /// 把组件数据转换为实体组件并挂载到 Unit 实体上的事件结构体。
    /// </summary>
    public struct AddToBytes
    {
        public EntityRef<Unit> Unit;
        public Type Type;
        public byte[] Bytes;
    }
}