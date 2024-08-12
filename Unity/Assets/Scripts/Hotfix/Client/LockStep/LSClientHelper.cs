using System.IO;

namespace ET.Client
{
    public static partial class LSClientHelper
    {
        //组件回滚
        public static void RunLSRollbackSystem(Entity entity)
        {
            if (entity is LSEntity)
            {
                return;
            }
            
            LSEntitySystemSingleton.Instance.LSRollback(entity);
            
            if (entity.ComponentsCount() > 0)
            {
                foreach (var kv in entity.Components)
                {
                    RunLSRollbackSystem(kv.Value);
                }
            }

            if (entity.ChildrenCount() > 0)
            {
                foreach (var kv in entity.Children)
                {
                    RunLSRollbackSystem(kv.Value);
                }
            }
        }
        
        // 回滚
        public static void Rollback(Room room, int frame)
        {
            room.LSWorld.Dispose();
            FrameBuffer frameBuffer = room.FrameBuffer;
            
            // 回滚
            room.LSWorld = room.GetLSWorld(SceneType.LockStepClient, frame);
            OneFrameInputs authorityFrameInput = frameBuffer.FrameInputs(frame);
            // 执行AuthorityFrame
            room.Update(authorityFrameInput);
            room.SendHash(frame);

            
            // 重新执行预测的帧
            for (int i = room.AuthorityFrame + 1; i <= room.PredictionFrame; ++i)
            {
                OneFrameInputs oneFrameInputs = frameBuffer.FrameInputs(i);
                LSClientHelper.CopyOtherInputsTo(room, authorityFrameInput, oneFrameInputs); // 重新预测消息
                room.Update(oneFrameInputs);
            }
            
            //Room 身上挂载的组件回滚
            RunLSRollbackSystem(room);
        }
        
        public static void SendHash(this Room self, int frame)
        {
            if (frame > self.AuthorityFrame)
            {
                return;
            }
            long hash = self.FrameBuffer.GetHash(frame);
            //发送哈希值而不是整个数据集可以节省网络带宽。
            //只有当哈希值不匹配时，才需要发送完整的数据更新。
            //计算哈希值通常比直接发送大量数据更快。
            //客户端可以通过快速比较哈希值来确定是否需要请求最新数据。
            C2Room_CheckHash c2RoomCheckHash = C2Room_CheckHash.Create();
            c2RoomCheckHash.Frame = frame;
            c2RoomCheckHash.Hash = hash;
            self.Root().GetComponent<ClientSenderComponent>().Send(c2RoomCheckHash);
        }
        
        // 重新调整预测消息，只需要调整其他玩家的输入
        public static void CopyOtherInputsTo(Room room, OneFrameInputs from, OneFrameInputs to)
        {
            long myId = room.GetComponent<LSClientUpdater>().MyId;
            foreach (var kv in from.Inputs)
            {
                if (kv.Key == myId)
                {
                    continue;
                }
                to.Inputs[kv.Key] = kv.Value;
            }
        }

        public static void SaveReplay(Room room, string path)
        {
            if (room.IsReplay)
            {
                return;
            }
            
            Log.Debug($"save replay: {path} frame: {room.Replay.FrameInputs.Count}");
            byte[] bytes = MemoryPackHelper.Serialize(room.Replay);
            File.WriteAllBytes(path, bytes);
        }
        
        public static void JumpReplay(Room room, int frame)
        {
            if (!room.IsReplay)
            {
                return;
            }

            if (frame >= room.Replay.FrameInputs.Count)
            {
                frame = room.Replay.FrameInputs.Count - 1;
            }
            
            int snapshotIndex = frame / LSConstValue.SaveLSWorldFrameCount;
            Log.Debug($"jump replay start {room.AuthorityFrame} {frame} {snapshotIndex}");
            if (snapshotIndex != room.AuthorityFrame / LSConstValue.SaveLSWorldFrameCount || frame < room.AuthorityFrame)
            {
                room.LSWorld.Dispose();
                // 回滚
                byte[] memoryBuffer = room.Replay.Snapshots[snapshotIndex];
                LSWorld lsWorld = MemoryPackHelper.Deserialize(typeof (LSWorld), memoryBuffer, 0, memoryBuffer.Length) as LSWorld;
                room.LSWorld = lsWorld;
                room.AuthorityFrame = snapshotIndex * LSConstValue.SaveLSWorldFrameCount;
                RunLSRollbackSystem(room);
            }
            
            room.FixedTimeCounter.Reset(TimeInfo.Instance.ServerFrameTime() - frame * LSConstValue.UpdateInterval, 0);

            Log.Debug($"jump replay finish {frame}");
        }
    }
}