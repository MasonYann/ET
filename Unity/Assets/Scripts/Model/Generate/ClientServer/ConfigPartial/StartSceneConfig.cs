using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

namespace ET
{
    public partial class StartSceneConfigCategory
    {
        public MultiMap<int, StartSceneConfig> Gates = new();
        
        public MultiMap<int, StartSceneConfig> ProcessScenes = new();
        
        public Dictionary<long, Dictionary<string, StartSceneConfig>> ClientScenesByName = new();

        public StartSceneConfig LocationConfig;

        public List<StartSceneConfig> Realms = new();
        
        public List<StartSceneConfig> Routers = new();
        
        public List<StartSceneConfig> Maps = new();

        public StartSceneConfig Match;
        
        public StartSceneConfig Benchmark;
        
        //登录中心服
        public StartSceneConfig LoginCenterConfig;
        //数据缓存服
        public StartSceneConfig UnitCacheConfig;
        
        public List<StartSceneConfig> GetByProcess(int process)
        {
            return this.ProcessScenes[process];
        }
        
        public StartSceneConfig GetBySceneName(int zone, string name)
        {
            return this.ClientScenesByName[zone][name];
        }

        public StartSceneConfig GetOneBySceneType(int id, SceneType sceneType)
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }

            List<StartSceneConfig> matchingItems = new List<StartSceneConfig>();

            foreach (var vConfig in this.dict)
            {
                Log.Debug($"{vConfig.Key}\t{vConfig.Value.SceneType}\t{vConfig.Value.ActorId}");
                if (vConfig.Value.SceneType == sceneType.ToString())
                {
                    matchingItems.Add(vConfig.Value);
                }
            }
            Log.Debug(matchingItems.Count.ToString() + id.ToString());
            
            if (matchingItems.Count < id)
            {
                throw new Exception($"配置找不到，配置表名: {nameof(StartSceneConfig)}，配置id: {id}，由于数组越界，配置中找不到目标 id 对应的配置表！");
            }

            
            StartSceneConfig item = matchingItems[id - 1];

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof(StartSceneConfig)}，配置id: {id}");
            }

            return item;
        }
        
        public override void EndInit()
        {
            foreach (StartSceneConfig startSceneConfig in this.GetAll().Values)
            {
                this.ProcessScenes.Add(startSceneConfig.Process, startSceneConfig);
                
                if (!this.ClientScenesByName.ContainsKey(startSceneConfig.Zone))
                {
                    this.ClientScenesByName.Add(startSceneConfig.Zone, new Dictionary<string, StartSceneConfig>());
                }
                this.ClientScenesByName[startSceneConfig.Zone].Add(startSceneConfig.Name, startSceneConfig);
                
                switch (startSceneConfig.Type)
                {
                    case SceneType.Realm:
                        this.Realms.Add(startSceneConfig);
                        break;
                    case SceneType.Gate:
                        this.Gates.Add(startSceneConfig.Zone, startSceneConfig);
                        break;
                    case SceneType.Location:
                        this.LocationConfig = startSceneConfig;
                        break;
                    case SceneType.Router:
                        this.Routers.Add(startSceneConfig);
                        break;
                    case SceneType.Map:
                        this.Maps.Add(startSceneConfig);
                        break;
                    case SceneType.Match:
                        this.Match = startSceneConfig;
                        break;
                    case SceneType.BenchmarkServer:
                        this.Benchmark = startSceneConfig;
                        break;
                    case SceneType.LoginCenter:
                        this.LoginCenterConfig = startSceneConfig;
                        break;
                    case SceneType.UnitCache:
                        this.UnitCacheConfig = startSceneConfig;
                        break;
                }
            }
        }
    }
    
    public partial class StartSceneConfig
    {
        public ActorId ActorId;
        
        public SceneType Type;

        public StartProcessConfig StartProcessConfig
        {
            get
            {
                return StartProcessConfigCategory.Instance.Get(this.Process);
            }
        }
        
        public StartZoneConfig StartZoneConfig
        {
            get
            {
                return StartZoneConfigCategory.Instance.Get(this.Zone);
            }
        }

        // 内网地址外网端口，通过防火墙映射端口过来
        private IPEndPoint innerIPPort;

        public IPEndPoint InnerIPPort
        {
            get
            {
                if (innerIPPort == null)
                {
                    this.innerIPPort = NetworkHelper.ToIPEndPoint($"{this.StartProcessConfig.InnerIP}:{this.Port}");
                }

                return this.innerIPPort;
            }
        }

        private IPEndPoint outerIPPort;

        // 外网地址外网端口
        public IPEndPoint OuterIPPort
        {
            get
            {
                if (this.outerIPPort == null)
                {
                    this.outerIPPort = NetworkHelper.ToIPEndPoint($"{this.StartProcessConfig.OuterIP}:{this.Port}");
                }

                return this.outerIPPort;
            }
        }

        public override void EndInit()
        {
            this.ActorId = new ActorId(this.Process, this.Id, 1);
            this.Type = EnumHelper.FromString<SceneType>(this.SceneType);
        }
    }
}