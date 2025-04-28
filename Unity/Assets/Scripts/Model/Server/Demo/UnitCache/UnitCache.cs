using System.Collections.Generic;

namespace ET.Server
{
    public interface IUnitCache
    {
    }

    /// <summary>
    /// 数据缓存服缓存信息。
    /// </summary>
    [ChildOf(typeof(UnitCacheComponent))]
    public class UnitCache : Entity, IAwake, IDestroy
    {
        public string key;

        // public Dictionary<long, EntityRef<Entity>> CacheComponentDic = new Dictionary<long, EntityRef<Entity>>(); 
        public Dictionary<long, Entity> CacheComponentDic = new Dictionary<long, Entity>();
    }
}