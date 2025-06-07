namespace ET
{
    public static class LSConstValue
    {
        /// <summary>
        /// 匹配人数。
        /// </summary>
        public const int MatchCount = 1;
        // public const int MatchCount = 2;
        /// <summary>
        /// 帧同步更新间隔。
        /// </summary>
        public const int UpdateInterval = 50;
        /// <summary>
        /// 帧同步每秒帧数。  
        /// </summary>
        public const int FrameCountPerSecond = 1000 / UpdateInterval;
        /// <summary>
        /// 帧同步保存帧数。
        /// </summary>
        public const int SaveLSWorldFrameCount = 60 * FrameCountPerSecond;
    }
}