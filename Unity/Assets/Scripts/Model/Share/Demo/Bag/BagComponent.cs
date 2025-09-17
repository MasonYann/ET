using System.Collections.Generic;
#if SERVER
using MongoDB.Bson.Serialization.Attributes;
#endif

namespace ET
{
    // [ComponentOf]
    // public class BagComponent: Entity, IAwake, IDestroy
    // {
    //     /// <summary>
    //     /// 储存物品 Id 和物品。
    //     /// </summary>
    //     public Dictionary<long, Item> ItemsDict = new Dictionary<long, Item>();
    //
    //     /// <summary>
    //     /// 储存物品类型和物品。
    //     /// </summary>
    //     public MultiMap<int, Item> ItemsMap = new MultiMap<int, Item>();
    // }
}
