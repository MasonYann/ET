using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class MatchComponent: Entity, IAwake
    {
        /// <summary>
        /// 等待匹配的玩家。
        /// </summary>
        public List<long> waitMatchPlayers = new List<long>();
    }

}