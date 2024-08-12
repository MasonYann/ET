using System;
using System.Collections.Generic;
using System.IO;

namespace ET
{
    /// <summary>
    /// 帧缓存。
    /// </summary>
    public class FrameBuffer: Object
    {
        public int MaxFrame { get; private set; }
        /// <summary>
        /// 每帧输入列表。
        /// </summary>
        private readonly List<OneFrameInputs> frameInputs;
        /// <summary>
        /// 内存缓冲器列表。（快照同步数据）
        /// </summary>
        private readonly List<MemoryBuffer> snapshots;
        /// <summary>
        /// 哈希值列表。
        /// </summary>
        private readonly List<long> hashs;

        public FrameBuffer(int frame = 0, int capacity = LSConstValue.FrameCountPerSecond * 60)
        {
            this.MaxFrame = frame + LSConstValue.FrameCountPerSecond * 30;
            this.frameInputs = new List<OneFrameInputs>(capacity);
            this.snapshots = new List<MemoryBuffer>(capacity);
            this.hashs = new List<long>(capacity);
            
            for (int i = 0; i < this.snapshots.Capacity; ++i)
            {
                this.hashs.Add(0);
                this.frameInputs.Add(OneFrameInputs.Create());
                MemoryBuffer memoryBuffer = new(10240);
                memoryBuffer.SetLength(0);
                memoryBuffer.Seek(0, SeekOrigin.Begin);
                this.snapshots.Add(memoryBuffer);
            }
        }

        public void SetHash(int frame, long hash)
        {
            EnsureFrame(frame);
            this.hashs[frame % this.frameInputs.Capacity] = hash;
        }
        
        public long GetHash(int frame)
        {
            EnsureFrame(frame);
            return this.hashs[frame % this.frameInputs.Capacity];
        }

        public bool CheckFrame(int frame)
        {
            if (frame < 0)
            {
                return false;
            }

            if (frame > this.MaxFrame)
            {
                return false;
            }

            return true;
        }

        private void EnsureFrame(int frame)
        {
            if (!CheckFrame(frame))
            {
                throw new Exception($"frame out: {frame}, maxframe: {this.MaxFrame}");
            }
        }
        
        /// <summary>
        /// 获取某一帧的帧数据。
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public OneFrameInputs FrameInputs(int frame)
        {
            EnsureFrame(frame);
            OneFrameInputs oneFrameInputs = this.frameInputs[frame % this.frameInputs.Capacity];
            return oneFrameInputs;
        }

        public void MoveForward(int frame)
        {
            if (this.MaxFrame - frame > LSConstValue.FrameCountPerSecond) // 至少留出1秒的空间
            {
                return;
            }
            
            ++this.MaxFrame;
            
            OneFrameInputs oneFrameInputs = this.FrameInputs(this.MaxFrame);
            oneFrameInputs.Inputs.Clear();
        }

        /// <summary>
        /// 内存缓冲区中获取某一帧的快照同步数据。
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public MemoryBuffer Snapshot(int frame)
        {
            EnsureFrame(frame);
            MemoryBuffer memoryBuffer = this.snapshots[frame % this.snapshots.Capacity];
            return memoryBuffer;
        }
    }
}