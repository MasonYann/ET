namespace ET
{
    public partial class DungeonEventConfigCategory
    {
        public string GetRandom(int eventPoolId)
        {
            //TODO : 这里先简单返回一个固定值，后续完善随机逻辑
            return "EASY";
        }
    }
}