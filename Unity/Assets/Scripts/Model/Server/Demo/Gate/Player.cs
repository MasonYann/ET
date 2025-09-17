namespace ET.Server
{
    /// <summary>
    /// Player 状态
    /// </summary>
    public enum PlayerState
    {
        Disconnect,
        Gate,
        Game,
    }
    
    
    [ChildOf(typeof(PlayerComponent))]
    public sealed class Player : Entity, IAwake<string>
    {
        //Player 的账号
        public string Account { get; set; }

        //Player 登录状态
        public PlayerState PlayerState { get; set; }
        
        //Player 的 UnitId
        public long UnitId { get; set; }
        
        //Player 的 Session 连接
        public Session ClientSession { get; set; }
        
        //Player 在世界聊天服务器的 UnitId
        public long ChatInfoInstanceId { get; set; }
    }
}