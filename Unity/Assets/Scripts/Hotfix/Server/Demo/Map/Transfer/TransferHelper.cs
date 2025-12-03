using System.Collections.Generic;
using MongoDB.Bson;

namespace ET.Server
{
    public static partial class TransferHelper
    {
        /// <summary>
        /// Gate 网关服务器传送到 Map 服务器。
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="sceneInstanceId"></param>
        /// <param name="sceneName"></param>
        public static async ETTask TransferAtFrameFinish(Unit unit, ActorId sceneInstanceId, string sceneName)
        {
            await unit.Fiber().WaitFrameFinish();

            await TransferHelper.Transfer(unit, sceneInstanceId, sceneName);
        }
        

        public static async ETTask Transfer(Unit unit, ActorId sceneInstanceId, string sceneName)
        {
            Scene root = unit.Root();
            
            // location加锁
            long unitId = unit.Id;
            
            M2M_UnitTransferRequest request = M2M_UnitTransferRequest.Create();
            request.OldActorId = unit.GetActorId();
            request.Unit = unit.ToBson();
            foreach (Entity entity in unit.Components.Values)
            {
                if (entity is ITransfer)
                {
                    request.Entitys.Add(entity.ToBson());
                }
            }
            unit.Dispose();
            
            //锁住 location，不再接收发给Unit的消息
            await root.GetComponent<LocationProxyComponent>().Lock(LocationType.Unit, unitId, request.OldActorId);
            await root.GetComponent<MessageSender>().Call(sceneInstanceId, request);
            
            
            
           
            // // 1. 给玩家挂上副本组件，初始化第1关
            // var dungeon = unit.AddComponent<DungeonComponent>();
            // dungeon.LevelId = 1001; // 海伯利安
            // dungeon.ThemePackageId = 101; // 森林皮肤
            //
            // // 2. 模拟推进到第1天
            // dungeon.NextDay();
            // // 预期日志：
            // // [Dungeon] 第 1 天，随机到了事件标签: EASY
            // // [Dungeon] 翻译成功! 准备生成怪物 UnitID: 1002
            // // [Factory] 怪物创建成功! ... 血量:100
            //
            // // 3. 假设场景里有个怪 (通过 ID 或 Filter 找到刚才生成的怪)
            // Unit monster = dungeon.Unit;
            // Log.Info($"[Test] 找到怪物 UnitID: {monster.Id} 血量:{monster.GetComponent<NumericComponent>().GetAsInt(NumericType.Hp)}");
            //
            // Log.Info($"[Test] 模拟玩家攻击...");
            // // 4. 模拟玩家攻击怪物
            // BattleHelper.Attack(unit, monster);
            // // 预期日志：
            // // [战斗] ... 造成 10 点伤害. 剩余血量: 90
        }
    }
}