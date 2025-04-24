namespace ET
{
    [UniqueId(100, 10000)]
    public static class TimerInvokeType
    {
        // 框架层100-200，逻辑层的timer type从200起
        public const int WaitTimer = 100;
        public const int SessionIdleChecker = 101;
        public const int MessageLocationSenderChecker = 102;
        public const int MessageSenderChecker = 103;
        
        // 框架层100-200，逻辑层的timer type 200-300
        public const int MoveTimer = 201;
        public const int AITimer = 202;
        public const int SessionAcceptTimeout = 203;
        
        public const int AccountSessionCheckOutTime = 204;  //Session 连接超时检测（默认十分钟）

        public const int PlayerOfflineOutTime = 205;        //Gate 网关连接超时检测（默认十秒钟）
        
        public const int SaveChangeDBData = 206;            //UnitCache 数据缓存服保存数据到数据库
        
        public const int RoomUpdate = 301;
    }
}