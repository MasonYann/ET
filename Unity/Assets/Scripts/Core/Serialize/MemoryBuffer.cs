using System;
using System.Buffers;
using System.IO;

namespace ET
{
    /// <summary>
    /// 存储缓冲器。
    /// 提供了一种灵活且高效的方式来管理内存中的缓冲区，支持各种初始化场景以及对缓冲区内容的写入和管理操作。
    /// </summary>
    public class MemoryBuffer: MemoryStream, IBufferWriter<byte>
    {
        private int origin;
        
        public MemoryBuffer()
        {
        }
        
        public MemoryBuffer(int capacity): base(capacity)
        {
        }
        
        public MemoryBuffer(byte[] buffer): base(buffer)
        {
        } 
        
        public MemoryBuffer(byte[] buffer, int index, int length): base(buffer, index, length)
        {
            this.origin = index;
        }
        
        //返回一个只读内存区域，表示已写入缓冲区的部分，从 origin 到当前位置。
        public ReadOnlyMemory<byte> WrittenMemory => this.GetBuffer().AsMemory(this.origin, (int)this.Position);

        //返回一个只读 Span，表示已写入缓冲区的部分，从 origin 到当前位置。
        public ReadOnlySpan<byte> WrittenSpan => this.GetBuffer().AsSpan(this.origin, (int)this.Position);

        //更新当前位置。
        public void Advance(int count)
        {
            long newLength = this.Position + count;
            if (newLength > this.Length)
            {
                this.SetLength(newLength);
            }
            this.Position = newLength;
        }

        //返回一个可写的内存区域，从当前位置开始。
        public Memory<byte> GetMemory(int sizeHint = 0)
        {
            if (this.Length - this.Position < sizeHint)
            {
                this.SetLength(this.Position + sizeHint);
            }
            var memory = this.GetBuffer().AsMemory((int)this.Position + this.origin, (int)(this.Length - this.Position));
            return memory;
        }

        //类似于 GetMemory，但返回一个可写的 span 而不是内存区域。确保有足够的空间根据 sizeHint 进行写入。
        public Span<byte> GetSpan(int sizeHint = 0)
        {
            if (this.Length - this.Position < sizeHint)
            {
                this.SetLength(this.Position + sizeHint);
            }
            var span = this.GetBuffer().AsSpan((int)this.Position + this.origin, (int)(this.Length - this.Position));
            return span;
        }
    }
}