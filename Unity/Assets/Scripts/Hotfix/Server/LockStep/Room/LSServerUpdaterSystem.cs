using System;
using System.Collections.Generic;

namespace ET.Server
{
    [EntitySystemOf(typeof(LSServerUpdater))]
    [FriendOf(typeof(LSServerUpdater))]
    public static partial class LSServerUpdaterSystem
    {
        [EntitySystem]
        private static void Awake(this LSServerUpdater self)
        {

        }
        
        [EntitySystem]
        private static void Update(this LSServerUpdater self)
        {
            Room room = self.GetParent<Room>();
            long timeNow = TimeInfo.Instance.ServerFrameTime();


            int frame = room.AuthorityFrame + 1;
            if (timeNow < room.FixedTimeCounter.FrameTime(frame))
            {
                return;
            }

            OneFrameInputs oneFrameInputs = self.GetOneFrameMessage(frame);
            ++room.AuthorityFrame;

            //复制获取到的输入信息
            OneFrameInputs sendInput = OneFrameInputs.Create();
            oneFrameInputs.CopyTo(sendInput);
            //广播给客户端
            RoomMessageHelper.BroadCast(room, sendInput);

            room.Update(oneFrameInputs);
        }

        /// <summary>
        /// 获取当前一帧的输入信息。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="frame"></param>
        /// <returns></returns>
        private static OneFrameInputs GetOneFrameMessage(this LSServerUpdater self, int frame)
        {
            Room room = self.GetParent<Room>();
            FrameBuffer frameBuffer = room.FrameBuffer;
            OneFrameInputs oneFrameInputs = frameBuffer.FrameInputs(frame);
            frameBuffer.MoveForward(frame);

            //如果所有玩家的输入信息都获取到了就返回帧输入信息
            if (oneFrameInputs.Inputs.Count == LSConstValue.MatchCount)
            {
                return oneFrameInputs;
            }

            //获取上一帧的输入信息
            OneFrameInputs preFrameInputs = null;
            if (frameBuffer.CheckFrame(frame - 1))
            {
                preFrameInputs = frameBuffer.FrameInputs(frame - 1);
            }

            // 有人输入的消息没过来，给他使用上一帧的操作
            foreach (long playerId in room.PlayerIds)
            {
                if (oneFrameInputs.Inputs.ContainsKey(playerId))
                {
                    continue;
                }

                if (preFrameInputs != null && preFrameInputs.Inputs.TryGetValue(playerId, out LSInput input))
                {
                    // 使用上一帧的输入
                    oneFrameInputs.Inputs[playerId] = input;
                }
                else
                {
                    oneFrameInputs.Inputs[playerId] = new LSInput();
                }
            }

            return oneFrameInputs;
        }
    }
}