using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class MapServerComponent : Entity, IAwake, IDestroy
    {
        public HashSet<long> OnlinePlayerIds { get; set; } = new();

        // Map服务器特有的配置
        //服务器ID
        public int MapId { get; set; }

        // 服务器名称
        public string MapName { get; set; }

        // 最大玩家数
        public int MaxPlayers { get; set; } = 10000;
    }
}