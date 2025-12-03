using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    [Config]
    public partial class DungeonLevelConfigCategory : Singleton<DungeonLevelConfigCategory>, IMerge
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<int, DungeonLevelConfig> dict = new();
		
        public void Merge(object o)
        {
            DungeonLevelConfigCategory s = o as DungeonLevelConfigCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public DungeonLevelConfig Get(int id)
        {
            this.dict.TryGetValue(id, out DungeonLevelConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (DungeonLevelConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, DungeonLevelConfig> GetAll()
        {
            return this.dict;
        }

        public DungeonLevelConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            
            var enumerator = this.dict.Values.GetEnumerator();
            enumerator.MoveNext();
            return enumerator.Current; 
        }
    }

	public partial class DungeonLevelConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		public int Id { get; set; }
		/// <summary>名字</summary>
		public string Name { get; set; }
		/// <summary>天数</summary>
		public int TotalDays { get; set; }
		/// <summary>关卡所需体力</summary>
		public int EnergyCost { get; set; }
		/// <summary>属性倍率</summary>
		public int AttributeRatio { get; set; }
		/// <summary>剧本ID</summary>
		public int EventPoolId { get; set; }
		/// <summary>主题包ID</summary>
		public int ThemeId { get; set; }
		/// <summary>c背景图资源</summary>
		public string BgRes { get; set; }

	}
}
