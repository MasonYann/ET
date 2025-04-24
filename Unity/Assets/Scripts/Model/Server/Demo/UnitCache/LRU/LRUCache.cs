using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(UnitCacheComponent))]
    public class LRUCache : Entity, IAwake, IDestroy
    {
        public Dictionary<long, EntityRef<LRUNode>> LRUNodeDic = new Dictionary<long, EntityRef<LRUNode>>();
        public Dictionary<int, LinkedList<EntityRef<LRUNode>>> FrequencyDic = new Dictionary<int, LinkedList<EntityRef<LRUNode>>>();
        public int MinFrequency;
    }
}