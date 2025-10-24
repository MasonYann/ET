using System.Collections.Generic;
using System.IO;

namespace ET.Server
{
    /// <summary>
    /// 地图消息助手类，提供地图服务器中的消息处理相关方法
    /// </summary>
    public static partial class MapMessageHelper
    {
        /// <summary>
        /// 通知单位添加
        /// 当有新的单位进入视野时，向指定单位发送创建单位的消息
        /// </summary>
        /// <param name="unit">接收消息的单位</param>
        /// <param name="sendUnit">需要被创建的单位</param>
        public static void NoticeUnitAdd(Unit unit, Unit sendUnit)
        {
            M2C_CreateUnits createUnits = M2C_CreateUnits.Create();
            createUnits.Units.Add(UnitHelper.CreateUnitInfo(sendUnit));
            MapMessageHelper.SendToClient(unit, createUnits);
        }
        
        /// <summary>
        /// 通知单位移除
        /// 当单位离开视野时，向指定单位发送移除单位的消息
        /// </summary>
        /// <param name="unit">接收消息的单位</param>
        /// <param name="sendUnit">需要被移除的单位</param>
        public static void NoticeUnitRemove(Unit unit, Unit sendUnit)
        {
            M2C_RemoveUnits removeUnits = M2C_RemoveUnits.Create();
            removeUnits.Units.Add(sendUnit.Id);
            MapMessageHelper.SendToClient(unit, removeUnits);
        }
        
        /// <summary>
        /// 广播消息给单位的所有观察者
        /// </summary>
        /// <param name="unit">广播源单位</param>
        /// <param name="message">要广播的消息</param>
        public static void Broadcast(Unit unit, IMessage message)
        {
            (message as MessageObject).IsFromPool = false;
            Dictionary<long, EntityRef<AOIEntity>> dict = unit.GetBeSeePlayers();
            // 网络底层做了优化，同一个消息不会多次序列化
            MessageLocationSenderOneType oneTypeMessageLocationType = unit.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.GateSession);
            foreach (AOIEntity u in dict.Values)
            {
                oneTypeMessageLocationType.Send(u.Unit.Id, message);
            }
        }
        
        /// <summary>
        /// 发送消息给客户端
        /// 通过GateSession发送消息给指定单位的客户端
        /// </summary>
        /// <param name="unit">目标单位</param>
        /// <param name="message">要发送的消息</param>
        public static void SendToClient(Unit unit, IMessage message)
        {
            unit.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.GateSession).Send(unit.Id, message);
        }
        
        /// <summary>
        /// 发送协议给Actor
        /// </summary>
        /// <param name="root">根场景</param>
        /// <param name="actorId">Actor Id</param>
        /// <param name="message">要发送的消息</param>
        public static void Send(Scene root, ActorId actorId, IMessage message)
        {
            root.GetComponent<MessageSender>().Send(actorId, message);
        }
    }
}