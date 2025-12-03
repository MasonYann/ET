namespace ET.Server
{
    [ComponentOf(typeof(Unit))] // 挂在玩家Unit身上
    public class DungeonComponent : Entity, IAwake, IDestroy
    {
        /// <summary> 当前关卡ID </summary>
        public int LevelId { get; set; }
        
        /// <summary> 当前天数 </summary>
        public int CurrentDay { get; set; }
        
        /// <summary> 缓存：当前关卡的主题包ID (从LevelConfig查出来的) </summary>
        public int ThemePackageId { get; set; }
        
        /// <summary> 缓存：当前关卡的节奏剧本ID </summary>
        public int EventPoolId { get; set; }
        
        public Unit Unit { get; set; }
    }
}