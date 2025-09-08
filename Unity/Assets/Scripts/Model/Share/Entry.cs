using MemoryPack;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;

namespace ET
{
    public struct EntryEvent1
    {
    }   
    
    public struct EntryEvent2
    {
    } 
    
    public struct EntryEvent3
    {
    }
    
    public static class Entry
    {
        public static void Init()
        {
            
        }
        
        public static void Start()
        {
            StartAsync().Coroutine();
        }
        
        private static async ETTask StartAsync()
        {
            WinPeriod.Init();

            // 注册Mongo type
            MongoRegister.Init();
            // 注册Entity序列化器
            EntitySerializeRegister.Init();
            // 添加框架核心组件单例
            World.Instance.AddSingleton<IdGenerater>(); // ID生成器
            World.Instance.AddSingleton<OpcodeType>(); // 操作码类型管理器
            World.Instance.AddSingleton<ObjectPool>(); // 对象池管理器
            World.Instance.AddSingleton<MessageQueue>(); // 消息队列
            World.Instance.AddSingleton<NetServices>(); // 网络服务
            World.Instance.AddSingleton<NavmeshComponent>(); // 导航网格组件
            World.Instance.AddSingleton<LogMsg>(); // 日志消息组件
            
            // 创建需要reload的code singleton
            CodeTypes.Instance.CreateCode();
            
            await World.Instance.AddSingleton<ConfigLoader>().LoadAsync();

            await FiberManager.Instance.Create(SchedulerType.Main, ConstFiberId.Main, 0, SceneType.Main, "");
        }
    }
}