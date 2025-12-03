namespace ET
{
	// 这个可弄个配置表生成
	/// <summary>
	/// 数值类型枚举类，定义游戏中所有数值属性的类型ID
	/// 每个基础属性都有对应的Base(基础值)、Add(增加值)、Pct(百分比加成)、FinalAdd(最终增加值)、FinalPct(最终百分比加成)
	/// 计算公式：最终值 = (基础值 + 增加值) * (1 + 百分比加成) + 最终增加值) * (1 + 最终百分比加成)
	/// </summary>
	public static class NumericType
	{
		public const int Max = 10000;

		/// <summary>
		/// 移动速度
		/// SpeedBase: 基础移动速度
		/// SpeedAdd: 移动速度增加值（固定值）
		/// SpeedPct: 移动速度百分比加成
		/// SpeedFinalAdd: 最终移动速度增加值
		/// SpeedFinalPct: 最终移动速度百分比加成
		/// </summary>
		public const int Speed = 1000;
		public const int SpeedBase = Speed * 10 + 1;
		public const int SpeedAdd = Speed * 10 + 2;
		public const int SpeedPct = Speed * 10 + 3;
		public const int SpeedFinalAdd = Speed * 10 + 4;
		public const int SpeedFinalPct = Speed * 10 + 5;

		/// <summary>
		/// 最大生命值
		/// MaxHpBase: 基础最大生命值
		/// MaxHpAdd: 最大生命值增加值（固定值）
		/// MaxHpPct: 最大生命值百分比加成
		/// MaxHpFinalAdd: 最终最大生命值增加值
		/// MaxHpFinalPct: 最终最大生命值百分比加成
		/// </summary>
		public const int MaxHp = 1001;
		public const int MaxHpBase = MaxHp * 10 + 1;
		public const int MaxHpAdd = MaxHp * 10 + 2;
		public const int MaxHpPct = MaxHp * 10 + 3;
		public const int MaxHpFinalAdd = MaxHp * 10 + 4;
		public const int MaxHpFinalPct = MaxHp * 10 + 5;

		/// <summary>
		/// 视野范围(Area of Interest)
		/// AOIBase: 基础视野范围
		/// AOIAdd: 视野范围增加值（固定值）
		/// AOIPct: 视野范围百分比加成
		/// AOIFinalAdd: 最终视野范围增加值
		/// AOIFinalPct: 最终视野范围百分比加成
		/// </summary>
		public const int AOI = 1002;
		public const int AOIBase = AOI * 10 + 1;
		public const int AOIAdd = AOI * 10 + 2;
		public const int AOIPct = AOI * 10 + 3;
		public const int AOIFinalAdd = AOI * 10 + 4;
		public const int AOIFinalPct = AOI * 10 + 5;

		public const int MaxMp = 1003;
		public const int MaxMpBase = MaxMp * 10 + 1;
		public const int MaxMpAdd = MaxMp * 10 + 2;
		public const int MaxMpPct = MaxMp * 10 + 3;
		public const int MaxMpFinalAdd = MaxMp * 10 + 4;
		public const int MaxMpFinalPct = MaxMp * 10 + 5;
		
		public const int DamageValue = 1011;         //伤害
		public const int DamageValueBase = DamageValue * 10 + 1;
		public const int DamageValueAdd = DamageValue * 10 + 2;
		public const int DamageValuePct = DamageValue * 10 + 3;
		public const int DamageValueFinalAdd = DamageValue * 10 + 4;
		public const int DamageValueFinalPct = DamageValue * 10 + 5;
		
		public const int AdditionalDdamage = 1012;         //伤害追加
	
		/// <summary>
		/// 当前生命值
		/// HpBase: 基础当前生命值
		/// </summary>
		public const int Hp = 1013;  // 生命值
		public const int HpBase = Hp * 10 + 1;
		public const int HpAdd = Hp * 10 + 2;
		public const int HpPct = Hp * 10 + 3;
		public const int HpFinalAdd = Hp * 10 + 4;
		public const int HpFinalPct = Hp * 10 + 5;
		
		public const int MP = 1014; //法力值
		public const int MPBase = MP * 10 + 1;
		public const int MPAdd = MP * 10 + 2;
		public const int MPPct = MP * 10 + 3;
		public const int MPFinalAdd = MP * 10 + 4;
		public const int MPFinalPct = MP * 10 + 5;

		public const int Armor = 1015; //护甲
		public const int ArmorBase = Armor * 10 + 1;
		public const int ArmorAdd = Armor * 10 + 2;
		public const int ArmorPct = Armor * 10 + 3;
		public const int ArmorFinalAdd = Armor * 10 + 4;
		public const int ArmorFinalPct = Armor * 10 + 5;
	    
		public const int ArmorAddition = 1015; //护甲追加
		
		public const int Dodge = 1017;           //闪避
		public const int DodgeBase = Dodge * 10 + 1;
		public const int DodgeAdd = Dodge * 10 + 2;
		public const int DodgePct = Dodge * 10 + 3;
		public const int DodgeFinalAdd = Dodge * 10 + 4;
		public const int DodgeFinalPct = Dodge * 10 + 5;

		public const int DodgeAddition = 1018;   // 闪避追加
	    
		public const int CriticalHitRate = 1019; //暴击率
		public const int CriticalHitRateBase = CriticalHitRate * 10 + 1;
		public const int CriticalHitRateAdd = CriticalHitRate * 10 + 2;
		public const int CriticalHitRatePct = CriticalHitRate * 10 + 3;
		public const int CriticalHitRateFinalAdd = CriticalHitRate * 10 + 4;
		public const int CriticalHitRateFinalPct = CriticalHitRate * 10 + 5;

		
		/// <summary>
		/// 力量属性
		/// </summary>
		public const int Power = 3001; //力量

		/// <summary>
		/// 体力属性
		/// </summary>
		public const int PhysicalStrength = 3002; //体力

		/// <summary>
		/// 敏捷属性
		/// </summary>
		public const int Agile = 3003; //敏捷值

		/// <summary>
		/// 精神属性
		/// </summary>
		public const int Spirit = 3004; //精神

		/// <summary>
		/// 属性点（可用于升级时分配给各项基础属性）
		/// </summary>
		public const int AttributePoint = 3005; //属性点

		/// <summary>
		/// 战斗力值
		/// </summary>
		public const int CombatEffectiveness = 3006; //战力值

		/// <summary>
		/// 玩家等级
		/// </summary>
		public const int Level = 3007;

		/// <summary>
		/// 金币数量
		/// </summary>
		public const int Gold = 3008;

		/// <summary>
		/// 经验值
		/// </summary>
		public const int Exp = 3009;

		/// <summary>
		/// 关卡冒险状态（表示当前是否在进行关卡挑战）
		/// </summary>
		public const int AdventureState = 3010;   //关卡冒险状态

		/// <summary>
		/// 垂死状态（表示角色是否处于濒死状态）
		/// </summary>
		public const int DyingState = 3011;      //垂死状态

		/// <summary>
		/// 关卡开始冒险的时间（时间戳）
		/// </summary>
		public const int AdventureStartTime = 3012;   //关卡开始冒险的时间

		/// <summary>
		/// 存活状态（0为死亡，1为活着）
		/// </summary>
		public const int IsAlive = 3013;    //存活状态  0为死亡 1为活着

		/// <summary>
		/// 战斗随机数种子（用于保证战斗结果的一致性）
		/// </summary>
		public const int BattleRandomSeed = 3014;    //战斗随机数种子

		/// <summary>
		/// 背包最大容量
		/// </summary>
		public const int MaxBagCapacity = 3015;   //背包最大负重

		/// <summary>
		/// 铁矿石数量（某种游戏资源）
		/// </summary>
		public const int IronStone = 3016; //铁矿石

		/// <summary>
		/// 皮毛数量（某种游戏资源）
		/// </summary>
		public const int Fur = 3017; //皮毛
	}
}