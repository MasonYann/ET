namespace ET
{
    [Event(SceneType.Main)]
    public class EntryEvent1_InitShare: AEvent<Scene, EntryEvent1>
    {
        protected override async ETTask Run(Scene root, EntryEvent1 args)
        {
            root.AddComponent<TimerComponent>(); // 添加定时器组件，用于处理定时任务
            root.AddComponent<CoroutineLockComponent>(); // 添加协程锁组件，用于协程同步控制
            root.AddComponent<ObjectWait>(); // 添加对象等待组件，用于对象异步等待操作
            root.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.UnOrderedMessage); // 添加邮箱组件，用于消息处理，设置为无序消息模式
            root.AddComponent<ProcessInnerSender>(); // 添加进程内发送者组件，用于进程内通信
            
            await ETTask.CompletedTask;
        }
    }
}