using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.ComponentModel;

namespace ET
{
    [Config]
    public partial class DungeonThemeContentConfigCategory : Singleton<DungeonThemeContentConfigCategory>, IMerge
    {
        [BsonElement]
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        private Dictionary<int, DungeonThemeContentConfig> dict = new();
		
        public void Merge(object o)
        {
            DungeonThemeContentConfigCategory s = o as DungeonThemeContentConfigCategory;
            foreach (var kv in s.dict)
            {
                this.dict.Add(kv.Key, kv.Value);
            }
        }
		
        public DungeonThemeContentConfig Get(int id)
        {
            this.dict.TryGetValue(id, out DungeonThemeContentConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (DungeonThemeContentConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, DungeonThemeContentConfig> GetAll()
        {
            return this.dict;
        }

        public DungeonThemeContentConfig GetOne()
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

	public partial class DungeonThemeContentConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		public int Id { get; set; }
		/// <summary>主题包ID</summary>
		public int ThemePackageId { get; set; }
		/// <summary>抽象标签</summary>
		public string Tag { get; set; }
		/// <summary>真实内容</summary>
		public int TotalId { get; set; }
		/// <summary>注释</summary>
		public string Desc { get; set; }

	}
}
