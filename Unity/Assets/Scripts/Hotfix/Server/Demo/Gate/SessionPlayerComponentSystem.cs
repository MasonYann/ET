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

            Player player = self.Player;
            if (player == null || player.IsDisposed)
            {
                return;
            }

            Session session = self.GetParent<Session>();
            PlayerSessionComponent playerSessionComponent = player.GetComponent<PlayerSessionComponent>();
            if (playerSessionComponent != null && playerSessionComponent.Session == session)
            {
                playerSessionComponent.Session = null;

                if (player.GetComponent<PlayerOfflineOutTimeComponent>() == null)
                {
                    player.AddComponent<PlayerOfflineOutTimeComponent>();
                }

                if (player.PlayerState == PlayerState.Game)
                {
                    root.GetComponent<MessageLocationSenderComponent>().Get(LocationType.Unit).Send(player.UnitId, G2M_SessionDisconnect.Create());
                }
            }

            self.Player = null;
        }
        
        [EntitySystem]
        private static void Awake(this SessionPlayerComponent self) 
        {

        }
    } 
} 
