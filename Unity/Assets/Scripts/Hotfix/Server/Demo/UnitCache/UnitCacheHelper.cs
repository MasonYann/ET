using System;

namespace ET.Server
{
    public static class UnitCacheHelper
    {
        /// <summary>
        /// 保存 Unit 及 Unit 身上的组件到缓存服及数据库中。
        /// </summary>
        /// <param name="unit"></param>
        public static void AddOrUpdateUnitAllCache(Unit unit)
        {
            //创建消息
            Other2UnitCache_AddOrUpdateUnit message = Other2UnitCache_AddOrUpdateUnit.Create();
            message.UnitId = unit.Id;

            //往消息列表中添加消息类型和对应的数据
            message.EntityTypes.Add(unit.GetType().FullName);
            message.EntityBytes.Add(unit.ToBson());

            //遍历组件，将实现 IUnitCache 接口的组件添加到消息列表中
            foreach (Entity entity in unit.Components.Values)
            {
                Type type = entity.GetType();
                //判断 Entity 是否实现了 IUnitCache 接口
                if (!typeof(IUnitCache).IsAssignableFrom(type))
                {
                    continue;
                }

                message.EntityTypes.Add(type.FullName);
                byte[] bytes = entity.ToBson();
                message.EntityBytes.Add(bytes);

                EventSystem.Instance.Invoke((long)SceneType.UnitCache, new AddToBytes() { Unit = unit, Type = type, Bytes = bytes });
            }

            //发送消息，将数据发送到缓存服并写入数据库
            StartSceneConfig unitCacheConfig = StartSceneConfigCategory.Instance.GetOneBySceneType(unit.Zone(), SceneType.UnitCache);
            unit.Root().GetComponent<MessageSender>().Call(unitCacheConfig.ActorId, message).Coroutine();
        }

        /// <summary>
        /// 获取玩家缓存。
        /// </summary>
        /// <param name="gateScene"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public static async ETTask<Unit> GetUnitCache(Scene gateScene, Scene mapScene, long unitId)
        {
            //通过起服配置获取游戏缓存服的地址
            StartSceneConfig unitCacheConfig = StartSceneConfigCategory.Instance.GetOneBySceneType(gateScene.Zone(), SceneType.UnitCache);
            // StartSceneConfig unitCacheConfig = StartSceneConfigCategory.Instance.UnitCacheConfig;
            ActorId instanceId = unitCacheConfig.ActorId;
            //声明查询 UnitCache 缓存服的消息
            Other2UnitCache_GetUnit message = Other2UnitCache_GetUnit.Create();
            message.UnitId = unitId;

            //发送消息到数据缓存服（服务器之间进行通讯
            UnitCache2Other_GetUnit queryUnit =
                    (UnitCache2Other_GetUnit)await gateScene.Root().GetComponent<MessageSender>().Call(instanceId, message);
            //如果消息回复不成功，或者缓存数量小于0，就返回，没有查询到数据
            if (queryUnit.Error != ErrorCode.ERR_Success || queryUnit.EntityList.Count <= 0)
            {
                return null;
            }

            //这个序列号会对应 Unit 实体列表的索引值
            Unit unit = null;
            //如果查询到数据，通过组件名称列表，查询 Unit 所在的序列号
            int indexOf = queryUnit.ComponentNameList.IndexOf("ET.Unit");
            //Unit 实体为空就返回空
            if (indexOf >= 0)
            {
                if (queryUnit.EntityList[indexOf] != null)
                {
                    //获取数据库中存储的 Unit 实体数据
                    unit = MongoHelper.Deserialize<Entity>(queryUnit.EntityList[indexOf]) as Unit;
                }
            }

            if (unit == null)
            {
                return null;
            }

            //把 Unit 实体挂载到 MapScene 上
            mapScene.GetComponent<UnitComponent>().AddChild(unit);
            //添加 Unit 数据库保存组件
            if (unit.GetComponent<UnitDBSaveComponent>() == null)
            {
                unit.AddComponent<UnitDBSaveComponent>();
            }

            //获取 Unit 身上挂在的组件数据，并添加到 Unit 实体上
            for (int i = 0; i < queryUnit.EntityList.Count; i++)
            {
                //跳过 Unit 实体，Unit 实体已经进行了反序列化操作
                if (i == indexOf)
                {
                    continue;
                }

                byte[] entityBytes = queryUnit.EntityList[i];
                //根据组件名获取缓存数据的类型
                Type type = CodeTypes.Instance.GetType(queryUnit.ComponentNameList[i]);

                EventSystem.Instance.Invoke((long)SceneType.UnitCache, new AddToBytes() { Unit = unit, Type = type, Bytes = entityBytes });
            }

            return unit;
        }
    }
}