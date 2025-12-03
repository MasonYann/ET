namespace ET.Server
{
    public static class BattleHelper
    {
        // 模拟攻击：Attacker 打 Target
        public static void Attack(Unit attacker, Unit target)
        {
            if (target == null || target.IsDisposed) return;

            var attackerNumeric = attacker.GetComponent<NumericComponent>();
            var targetNumeric = target.GetComponent<NumericComponent>();

            // 1. 获取攻击力 (ET会自动计算 Base + Add + Pct)
            int damage = attackerNumeric.GetAsInt(NumericType.DamageValue);
            
            // 2. 扣血
            int currentHp = targetNumeric.GetAsInt(NumericType.Hp);
            int newHp = currentHp - damage;
            targetNumeric.Set(NumericType.Hp, newHp);

            Log.Info($"[战斗] {attacker.Id} 攻击了 {target.Id}, 造成 {damage} 点伤害. 剩余血量: {newHp}");

            // 3. 死亡判断
            if (newHp <= 0)
            {
                OnDead(target);
            }
        }

        private static void OnDead(Unit unit)
        {
            Log.Info($"[战斗] 单位 {unit.Id} 死亡!");
            // 这里可以处理掉落逻辑...
            unit.Dispose(); // 销毁实体
        }
    }
}