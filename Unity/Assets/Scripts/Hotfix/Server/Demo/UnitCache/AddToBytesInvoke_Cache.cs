namespace ET.Server
{
    [Invoke((long)SceneType.UnitCache)]
    public class AddToBytesInvoke_Cache : AInvokeHandler<AddToBytes>
    {
        /// <summary>
        /// 把组件数据转换为实体组件并挂载到 Unit 实体上。
        /// </summary>
        /// <param name="args"></param>
        public override void Handle(AddToBytes args)
        {
            Unit unit = args.Unit;
            unit?.GetComponent<UnitDBSaveComponent>().AddToBytes(args.Type, args.Bytes);
        }
    }
}