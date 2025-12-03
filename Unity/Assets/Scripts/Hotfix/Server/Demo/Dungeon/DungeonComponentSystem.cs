namespace ET.Server
{
    [EntitySystemOf(typeof(DungeonComponent))]
    [FriendOf(typeof(DungeonComponent))]
    public static partial class DungeonComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ET.Server.DungeonComponent self)
        {
            self.CurrentDay = 0;
        }

        [EntitySystem]
        private static void Destroy(this ET.Server.DungeonComponent self)
        {
        }

        public static void NextDay(this ET.Server.DungeonComponent self)
        {
            self.CurrentDay++;

            // 1. 查剧本表 (EventConfig) - 为了演示，我们硬编码模拟随机到了 "EASY"
            // 实际逻辑应该调用: DungeonEventConfigCategory.Instance.GetRandom(self.EventPoolId)...
            string randomTag = DungeonEventConfigCategory.Instance.GetRandom(self.EventPoolId);
            Log.Info($"[Dungeon] 第 {self.CurrentDay} 天，随机到了事件标签: {randomTag}");

            int realUnitId = DungeonThemeContentConfigCategory.Instance.GetRealContentId(randomTag, self.ThemePackageId);
            Log.Info($"[Dungeon] 第 {self.CurrentDay} 天，真实事件ID: {realUnitId}");

            // 2. 创建怪物单元
            Unit monsterUnit = UnitFactory.CreateMonster(self.Scene(), realUnitId);
            M2C_CreateUnits createUnits = M2C_CreateUnits.Create();
            createUnits.Units.Add(UnitHelper.CreateUnitInfo(monsterUnit));
            self.Unit = monsterUnit;
            MapMessageHelper.SendToClient(self.GetParent<Unit>(), createUnits);
        }
    }
}