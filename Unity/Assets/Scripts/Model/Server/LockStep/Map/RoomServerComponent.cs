using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 房间玩家容器。
    /// </summary>
    [ComponentOf(typeof(Room))]
    public class RoomServerComponent: Entity, IAwake<List<long>>
    {
    }
}