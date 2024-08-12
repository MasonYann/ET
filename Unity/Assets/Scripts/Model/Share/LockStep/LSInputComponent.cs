using MemoryPack;

namespace ET
{
    /// <summary>
    /// 帧同步输入组件。
    /// </summary>
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class LSInputComponent: LSEntity, ILSUpdate, IAwake, ISerializeToEntity
    {
        public LSInput LSInput { get; set; }
    }
}