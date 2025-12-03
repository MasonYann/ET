namespace ET.Server
{
    /// <summary>
    /// 放置玩家组件 - 管理玩家的放置状态和收益
    /// </summary>
    [ComponentOf(typeof(Player))]
    public class PlacementPlayerComponent : Entity, IAwake, IDestroy
    {
        /// <summary>是否正在放置中</summary>
        public bool IsPlacing { get; set; }
        
        /// <summary>开始放置的时间戳</summary>
        public long StartPlacementTime { get; set; }
        
        /// <summary>最后一次领取收益的时间</summary>
        public long LastClaimTime { get; set; }
        
        /// <summary>放置位置坐标</summary>
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        
        /// <summary>当前放置的收益倍率</summary>
        public float RewardMultiplier { get; set; } = 1.0f;
        
        /// <summary>累计放置收益</summary>
        public long AccumulatedRewards { get; set; }
    }
}