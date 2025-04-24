using System;
using System.Collections.Generic;

namespace ET.Server
{
    [MessageHandler(SceneType.UnitCache)]
    [FriendOf(typeof(UnitCacheComponent))]
    public class Other2UnitCache_GetUnitHandler : MessageHandler<Scene, Other2UnitCache_GetUnit, UnitCache2Other_GetUnit>
    {
        protected override async ETTask Run(Scene scene, Other2UnitCache_GetUnit request, UnitCache2Other_GetUnit response)
        {
            UnitCacheComponent unitCacheComponent = scene.GetComponent<UnitCacheComponent>();
            Dictionary<string, Entity> dic = ObjectPool.Instance.Fetch(typeof(Dictionary<string, Entity>)) as Dictionary<string, Entity>;
            try
            {
                //查看请求的组件数量是否为 0，为 0 的情况下，说明当前不是获取某一个组件的缓存数据，而是获取 Unit 实体，和 Unit 实体身上所有缓存组件的缓存数据
                if (request.ComponentNameList.Count == 0)
                {
                    dic.Add("ET.Unit", null);
                    foreach (string s in unitCacheComponent.UnitCacheKeyList)
                    {
                        if (s == "ET.Unit")
                        {
                            continue;
                        }

                        dic.Add(s, null);
                    }
                }
                //不为 0 ，获取某一个组件的缓存数据
                else
                {
                    foreach (string s in request.ComponentNameList)
                    {
                        dic.Add(s, null);
                    }
                }

                long unitId = request.UnitId;
                CoroutineLockComponent coroutineLockComponent = scene.GetComponent<CoroutineLockComponent>();
                using (await coroutineLockComponent.Wait(CoroutineLockType.UnitCacheGet, unitId))
                {
                    //更新 LRU 缓存
                    unitCacheComponent.CallCache(unitId);

                    using (ListComponent<string> keyList = ListComponent<string>.Create())
                    {
                        foreach (string key in dic.Keys)
                        {
                            keyList.Add(key);
                        }

                        foreach (string key in keyList)
                        {
                            Entity entity = await unitCacheComponent.Get(request.UnitId, key);
                            dic[key] = entity;
                        }
                    }

                    foreach (var info in dic)
                    {
                        response.ComponentNameList.Add(info.Key);
                        //在 C# 中，?? 是空合并运算符（Null-Coalescing Operator）。它的作用是：如果左侧的操作数为 null，则返回右侧的操作数；否则返回左侧的操作数。
                        response.EntityList.Add(info.Value?.ToBson() ?? null);
                    }
                }

                // foreach (var entity in dic)
                // {
                //     Log.Debug(entity.Key.ToString() + "\t" + entity.Value.ToString());
                // }
            }
            finally
            {
                dic.Clear();
                ObjectPool.Instance.Recycle(dic);
            }
        }
    }
}