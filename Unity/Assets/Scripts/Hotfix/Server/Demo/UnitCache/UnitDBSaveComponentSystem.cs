using System;

namespace ET.Server
{
    [Invoke(TimerInvokeType.SaveChangeDBData)]
    public class UnitDBSaveComponentTimer : ATimer<UnitDBSaveComponent>
    {
        protected override void Run(UnitDBSaveComponent self)
        {
            try
            {
                self?.SaveChange().Coroutine();
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }
    }

    [EntitySystemOf(typeof(UnitDBSaveComponent))]
    [FriendOf(typeof(UnitDBSaveComponent))]
    public  static partial class UnitDBSaveComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.UnitDBSaveComponent self)
        {
            //正式上线部署，每 10 - 15 分钟随机存储落地一次
            // self.Timer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(RandomGenerator.RandomNumber(10,16) * 60 * 1000, TimerInvokeType.SaveChangeDBData, self);

            // 开发时，每4秒落地一次
            self.Timer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(4 * 1000, TimerInvokeType.SaveChangeDBData, self);
        }

        [EntitySystem]
        private static void Destroy(this ET.Server.UnitDBSaveComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        /// <summary>
        /// 添加变动的组件类型和组件数据。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="type"></param>
        /// <param name="bytes"></param>
        public static void AddToBytes(this UnitDBSaveComponent self, Type type, byte[] bytes)
        {
            self.Bytes[type] = bytes;
        }

        /// <summary>
        /// 添加变动的组件类型。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="type"></param>
        public static void AddChange(this UnitDBSaveComponent self, Type type)
        {
            self.EntityChangeTypeSet.Add(type);
        }

        /// <summary>
        /// 携程锁异步保存组件数据。
        /// </summary>
        /// <param name="self"></param>
        public static async ETTask SaveChange(this UnitDBSaveComponent self)
        {
            CoroutineLockComponent coroutineLockComponent = self.Root().GetComponent<CoroutineLockComponent>();
            //等 Unit 实体处理完网络消息之后再对他进行缓存数据更新
            using (await coroutineLockComponent.Wait(CoroutineLockType.Mailbox, self.GetParent<Unit>().InstanceId))
            {
                self.SaveChangeNoWait();
            }
        }

        /// <summary>
        /// 同步保存组件数据。
        /// </summary>
        /// <param name="self"></param>
        public static void SaveChangeNoWait(this UnitDBSaveComponent self)
        {
            if (self.IsDisposed || self.Parent == null)
            {
                return;
            }

            if (self.Root() == null)
            {
                return;
            }

            Unit unit = self.GetParent<Unit>();
            if (unit == null || unit.IsDisposed)
            {
                return;
            }

            if (self.EntityChangeTypeSet.Count <= 0)
            {
                return;
            }

            Other2UnitCache_AddOrUpdateUnit message = Other2UnitCache_AddOrUpdateUnit.Create();
            message.UnitId = unit.Id;
            message.EntityTypes.Add(unit.GetType().FullName);
            message.EntityBytes.Add(unit.ToBson());

            foreach (Type type in self.EntityChangeTypeSet)
            {
                Entity entity = unit.GetComponent(type);
                if (entity == null || entity.IsDisposed)
                {
                    continue;
                }

                Log.Debug("开始保存变化部分 Entity 数据：" + type.FullName);
                byte[] bytes = entity.ToBson();
                message.EntityTypes.Add(type.FullName);
                message.EntityBytes.Add(bytes);
                self.AddToBytes(type, bytes);
            }
            
            self.EntityChangeTypeSet.Clear();

            StartSceneConfig unitCacheConfig = StartSceneConfigCategory.Instance.GetOneBySceneType(unit.Zone(), SceneType.UnitCache);
            self.Root()?.GetComponent<MessageSender>().Call(unitCacheConfig.ActorId,message).Coroutine();
        }
    }
}