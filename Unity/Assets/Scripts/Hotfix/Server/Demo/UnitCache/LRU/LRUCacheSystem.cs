using System.Collections.Generic;

namespace ET.Server
{
    [EntitySystemOf(typeof(LRUCache))]
    [FriendOf(typeof(LRUCache))]
    [FriendOf(typeof(LRUNode))]
    public static partial class LRUCacheSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.LRUCache self)
        {
            self.MinFrequency = 0;
            self.FrequencyDic.Add(0, new LinkedList<EntityRef<LRUNode>>());
        }

        [EntitySystem]
        private static void Destroy(this ET.Server.LRUCache self)
        {
            self.LRUNodeDic.Clear();
            self.FrequencyDic.Clear();
            self.MinFrequency = 0;
        }

        /// <summary>
        /// 缓存服频率更新调用。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="key"></param>
        public static void Call(this LRUCache self, long key)
        {
            EntityRef<LRUNode> nodeRef;
            LRUNode node;
            //查看节点字典中是否存在节点
            //存在的话删除节点
            if (self.LRUNodeDic.TryGetValue(key, out nodeRef))
            {
                node = nodeRef;
                self.FrequencyDic[node.Frequency].Remove(node);
                node.Frequency++;
                if (!self.FrequencyDic.ContainsKey(node.Frequency))
                {
                    self.FrequencyDic.Add(node.Frequency, new LinkedList<EntityRef<LRUNode>>());
                }

                self.FrequencyDic[node.Frequency].AddLast(node);
                if (self.FrequencyDic[self.MinFrequency].Count == 0)
                {
                    self.MinFrequency = node.Frequency;
                }

                return;
            }

            node = self.AddChild<LRUNode, long>(key);
            node.Frequency = 0;

            self.FrequencyDic[0].AddLast(node);
            self.MinFrequency = 0;
            self.LRUNodeDic[key] = node;

            //缓存数量大于3000时，删除一个节点
            if (self.LRUNodeDic.Count >= 3000)
            {
                LRUNode fn = self.FrequencyDic[self.MinFrequency].First.Value;
                long unitId = fn.Key;
                self.FrequencyDic[self.MinFrequency].RemoveFirst();
                self.LRUNodeDic.Remove(unitId);
                fn.Dispose();

                EventSystem.Instance.Invoke((long)SceneType.UnitCache, new LRUUnitCacheDelete() { LruCache = self, Key = unitId });
            }
        }
    }
}