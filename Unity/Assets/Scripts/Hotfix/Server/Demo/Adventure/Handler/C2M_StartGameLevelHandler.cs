using System;

namespace ET.Server
{
    [MessageHandler(SceneType.Map)]  
    public class C2M_StartGameLevelHandler : MessageLocationHandler<Unit, C2M_StartGameLevel, M2C_StartGameLevel>
    {
        protected override async ETTask Run(Unit unit, C2M_StartGameLevel request, M2C_StartGameLevel response)
        {
            //----------------------------------------------------------
            // 1. 给玩家挂上副本组件，初始化第1关
            var dungeon = unit.AddComponent<DungeonComponent>();
            dungeon.LevelId = 1001; // 海伯利安
            dungeon.ThemePackageId = 101; // 森林皮肤

            // 2. 模拟推进到第1天
            dungeon.NextDay();
            // 预期日志：
            // [Dungeon] 第 1 天，随机到了事件标签: EASY
            // [Dungeon] 翻译成功! 准备生成怪物 UnitID: 1002
            // [Factory] 怪物创建成功! ... 血量:100

            // 3. 假设场景里有个怪 (通过 ID 或 Filter 找到刚才生成的怪)
            Unit monster = dungeon.Unit;
            Log.Info($"[Test] 找到怪物 UnitID: {monster.Id} 血量:{monster.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp)}");

            Log.Info($"[Test] 模拟玩家攻击...");
            // 4. 模拟玩家攻击怪物
            BattleHelper.Attack(unit, monster);
            // 预期日志：
            // [战斗] ... 造成 10 点伤害. 剩余血量: 90

          
            response.Error = ErrorCode.ERR_Success;
            await ETTask.CompletedTask;
            // NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            //
            // //已经在关卡战斗状态
            // if (numericComponent.GetAsInt(NumericType.AdventureState) != 0)
            // {
            //     response.Error = ErrorCode.ERR_AlreadyAdventureState;
            //     return;
            // }
            //
            // //死亡状态
            // if (numericComponent.GetAsInt(NumericType.DyingState) != 0)
            // {
            //     response.Error = ErrorCode.ERR_AdventureInDying;
            //     return;
            // }
            //
            // //关卡是否在配置中
            // if (!BattleLevelConfigCategory.Instance.Contain(request.LevelId))
            // {
            //     response.Error = ErrorCode.ERR_AdventureErrorLevel;
            //     return;
            // }
            //
            // //角色等级是否满足进入当前关卡最低的等级
            // BattleLevelConfig config = BattleLevelConfigCategory.Instance.Get(request.LevelId);
            // if (numericComponent[NumericType.Level] < config.MiniEnterLevel[0])
            // {
            //     response.Error = ErrorCode.ERR_AdventureLevelNotEnough;
            //     return;
            // }
            //
            // //角色冒险状态改为关卡 Id
            // numericComponent.Set(NumericType.AdventureState, request.LevelId);
            // //关卡战斗开始时间
            // numericComponent.Set(NumericType.AdventureStartTime, TimeInfo.Instance.ServerNow());
            //
            // //设置本次战斗的随机数种子，保证客户端的战斗中的每次随机产生的数值能在服务器中复现
            // numericComponent.Set(NumericType.BattleRandomSeed, RandomGenerator.RandUInt32());
            //
            // await ETTask.CompletedTask;
        }
    }
}