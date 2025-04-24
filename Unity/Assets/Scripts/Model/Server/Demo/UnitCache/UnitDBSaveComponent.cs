using System;
using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// Unit 数据库保存组件。
    /// </summary>
    [ComponentOf(typeof(Unit))]
    public class UnitDBSaveComponent : Entity, IAwake,IDestroy
    {
        public long Timer;
        /// <summary>
        /// 缓存变化的组件类型。
        /// </summary>
        public HashSet<Type> EntityChangeTypeSet { get; } = new HashSet<Type>();
        /// <summary>
        /// 缓存变化的组件类型和组件数据。
        /// </summary>
        public Dictionary<Type, byte[]> Bytes { get; } = new Dictionary<Type, byte[]>();
    }
}