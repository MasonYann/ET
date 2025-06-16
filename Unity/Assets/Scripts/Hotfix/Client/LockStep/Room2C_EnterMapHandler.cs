namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class Room2C_EnterMapHandler: MessageHandler<Scene, Room2C_Start>
    {
        protected override async ETTask Run(Scene root, Room2C_Start message)
        {
            Log.Debug("所有玩家的场景加载完毕！继续执行开始游戏！");
            root.GetComponent<ObjectWait>().Notify(new WaitType.Wait_Room2C_Start() {Message = message});
            await ETTask.CompletedTask;
        }
    }
}