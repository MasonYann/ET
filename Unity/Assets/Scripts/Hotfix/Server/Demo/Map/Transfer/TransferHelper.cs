using System.Collections.Generic;
using MongoDB.Bson;

namespace ET.Server
{
    public static partial class TransferHelper
    {
        /// <summary>
        /// Gate 网关服务器传送到 Map 服务器。
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="sceneInstanceId"></param>
        /// <param name="sceneName"></param>
        public static async ETTask TransferAtFrameFinish(Unit unit, ActorId sceneInstanceId, string sceneName)
        {
            await unit.Fiber().WaitFrameFinish();

            await TransferHelper.Transfer(unit, sceneInstanceId, sceneName);
        }
        

        public static async ETTask Transfer(Unit unit, ActorId sceneInstanceId, string sceneName)
        {
            Scene root = unit.Root();
            
            // location加锁
            long unitId = unit.Id;
            
            M2M_UnitTransferRequest request = M2M_UnitTransferRequest.Create();
            request.OldActorId = unit.GetActorId();
            request.Unit = unit.ToBson();
            foreach (Entity entity in unit.Components.Values)
            {
                if (entity is ITransfer)
                {
                    request.Entitys.Add(entity.ToBson());
                }
            }
            unit.Dispose();
            
            //锁住 location，不再接收发给Unit的消息
            await root.GetComponent<LocationProxyComponent>().Lock(LocationType.Unit, unitId, request.OldActorId);
            await root.GetComponent<MessageSender>().Call(sceneInstanceId, request);
        }
    }
}