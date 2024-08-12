namespace ET.Server
{
    /// <summary>
    /// 房间玩家。
    /// </summary>
    [ChildOf(typeof (RoomServerComponent))]
    public class RoomPlayer: Entity, IAwake
    {
        public int Progress { get; set; }

        public bool IsOnline { get; set; } = true;
    }
}