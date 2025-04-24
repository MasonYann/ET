namespace ET.Server
{
    [EntitySystemOf(typeof(SessionPlayerComponent))]
    public static partial class SessionPlayerComponentSystem
    {
        [EntitySystem]
        private static void Destroy(this SessionPlayerComponent self)
        {
            Scene root = self.Root();
            if (root.IsDisposed)
            {
                return;
            }

            
            // 发送断线消息 因为使用统一下线流程，故注释
            //root.GetComponent<MessageLocationSenderComponent>().Get(LocationType.Unit).Send(self.Player.Id, G2M_SessionDisconnect.Create());

            //TODD 作业:根据是否是二次登陆决定是否执行Player的下线流程
            
            
            self.Player = null;
        }
        
        [EntitySystem]
        private static void Awake(this SessionPlayerComponent self) 
        {

        }
    } 
} 