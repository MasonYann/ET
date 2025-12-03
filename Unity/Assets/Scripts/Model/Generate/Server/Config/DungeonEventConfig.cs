using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    [Config]
    public partial class DungeonEventConfigCategory : Singleton<DungeonEventConfigCategory>, IMerge
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<int, DungeonEventConfig> dict = new();
		
        public void Merge(object o)
        {
            DungeonEventConfigCategory s = o as DungeonEventConfigCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public DungeonEventConfig Get(int id)
        {
            this.dict.TryGetValue(id, out DungeonEventConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (DungeonEventConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, DungeonEventConfig> GetAll()
        {
            return this.dict;
        }

        public DungeonEventConfig GetOne()
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

	public partial class DungeonEventConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		public int Id { get; set; }
		/// <summary>剧本ID</summary>
		public int EventPoolId { get; set; }
		/// <summary>天数</summary>
		public int DayIndex { get; set; }
		/// <summary>事件类型</summary>
		public int EventType { get; set; }
		/// <summary>抽象标签</summary>
		public string EventParam { get; set; }
		/// <summary>权重</summary>
		public int Weight { get; set; }
		/// <summary>注释</summary>
		public string Desc { get; set; }

	}
}
