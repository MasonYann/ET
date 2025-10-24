using System.Collections.Generic;

namespace ET
{
    public partial class EntryConfigCategory
    {
        //词条配置字典。
        //字典：词条类型 -> 词条等级 -> 词条配置
        private Dictionary<int, MultiMap<int, EntryConfig>> EntryConfigsDict = new Dictionary<int, MultiMap<int, EntryConfig>>();

        /// <summary>
        /// 初始化结束，添加词条到词条字典。
        /// </summary>
        public override void EndInit()
        {
            foreach (var config in this.dict.Values)
            {
                if (!this.EntryConfigsDict.ContainsKey(config.EntryType))
                {
                    this.EntryConfigsDict.Add(config.EntryType, new MultiMap<int, EntryConfig>());
                }
                this.EntryConfigsDict[config.EntryType].Add(config.EntryLevel, config);
            }
        }

        /// <summary>
        /// 通过词条类型和词条等级获取装备词条配置。
        /// </summary>
        /// <param name="entryType"></param>
        /// <param name="entryLevel"></param>
        /// <returns></returns>
        public EntryConfig GetRandomEntryConfigByLevel(int entryType, int entryLevel)
        {
            if (!this.EntryConfigsDict.ContainsKey(entryType))
            {
                return null;
            }
            MultiMap<int,EntryConfig> entryConfigsMap = this.EntryConfigsDict[entryType];
            if (!entryConfigsMap.ContainsKey(entryLevel))
            {
                return null;
            }

            var configList = entryConfigsMap[entryLevel];
            int index = RandomGenerator.RandomNumber(0, configList.Count);
            return configList[index];
        }
    }
}