namespace ET.Server
{
    [ChildOf(typeof(LRUCache))]
    public class LRUNode : Entity, IAwake<long>, IDestroy
    {
        public long Key;

        // 访问频率
        public int Frequency;
    }
}