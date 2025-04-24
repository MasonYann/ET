using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 数据缓存服缓存信息记录组件，用于记录缓存信息。
    /// </summary>
    [ComponentOf(typeof(Scene))]
    public class UnitCacheComponent : Entity, IAwake, IDestroy
    {
        public Dictionary<string, EntityRef<UnitCache>> UnitCacheDic = new Dictionary<string, EntityRef<UnitCache>>();

        public List<string> UnitCacheKeyList = new List<string>();
    }
}