namespace ET
{
	// 这个可弄个配置表生成
    public static class NumericType
    {
	    public const int Max = 10000;

	    public const int Speed = 1000;
	    public const int SpeedBase = Speed * 10 + 1;
	    public const int SpeedAdd = Speed * 10 + 2;
	    public const int SpeedPct = Speed * 10 + 3;
	    public const int SpeedFinalAdd = Speed * 10 + 4;
	    public const int SpeedFinalPct = Speed * 10 + 5;

	    public const int Hp = 1001;
	    public const int HpBase = Hp * 10 + 1;

	    public const int MaxHp = 1002;
	    public const int MaxHpBase = MaxHp * 10 + 1;
	    public const int MaxHpAdd = MaxHp * 10 + 2;
	    public const int MaxHpPct = MaxHp * 10 + 3;
	    public const int MaxHpFinalAdd = MaxHp * 10 + 4;
	    public const int MaxHpFinalPct = MaxHp * 10 + 5;

	    public const int AOI = 1003;
	    public const int AOIBase = AOI * 10 + 1;
	    public const int AOIAdd = AOI * 10 + 2;
	    public const int AOIPct = AOI * 10 + 3;
	    public const int AOIFinalAdd = AOI * 10 + 4;
	    public const int AOIFinalPct = AOI * 10 + 5;
	    
	    public const int Power = 3001; //力量
	    
	    public const int PhysicalStrength = 3002; //体力

	    public const int Agile = 3003; //敏捷值

	    public const int Spirit = 3004; //精神
	    
	    public const int AttributePoint = 3005; //属性点
	    
	    public const int CombatEffectiveness = 3006; //战力值
	    
	    public const int Level = 3007;
	    
	    public const int Gold  = 3008;
	    
	    public const int Exp   = 3009;

	    public const int AdventureState = 3010;   //关卡冒险状态
	    
	    public const int DyingState     = 3011;      //垂死状态
	    
	    public const int AdventureStartTime = 3012;   //关卡开始冒险的时间

	    public const int IsAlive = 3013;    //存活状态  0为死亡 1为活着


	    public const int BattleRandomSeed = 3014;    //战斗随机数种子
	    
	    public const int MaxBagCapacity  = 3015;   //背包最大负重


	    public const int IronStone = 3016; //铁矿石

	    public const int Fur       = 3017; //皮毛
    }
}
