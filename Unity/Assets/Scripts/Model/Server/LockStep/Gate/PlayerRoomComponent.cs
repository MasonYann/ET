namespace ET.Server
{
    /// <summary>
    /// 玩家房间组件。
    /// </summary>
    [ComponentOf(typeof(Player))]
    public class PlayerRoomComponent : Entity, IAwake
    {
        public ActorId RoomActorId { get; set; }
    }
}