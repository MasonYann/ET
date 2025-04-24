using System;

namespace ET.Server
{
    [Event(SceneType.All)]
    //对发生改变的数据组件类型进行记录并触发回调
    public class UnitGetComponent_GetComponent : AEvent<Scene, UnitGetComponent>
    {
        protected override async ETTask Run(Scene scene, UnitGetComponent args)
        {
            Unit unit = args.Unit;
            Type type = args.Type;
            
            unit.GetComponent<UnitDBSaveComponent>()?.AddChange(type);
            
            //判定 Unit 身上是否存在需要获取的数值
            if (unit.Components.ContainsKey(type.TypeHandle.Value.ToInt64()))
            {
                return;
            }

            UnitDBSaveComponent unitDBSaveComponent = unit.GetComponent<UnitDBSaveComponent>();
            if (unitDBSaveComponent == null)
            {
                return;
            }
            //Unit身上不存在需要挂在的组件，这个时候就从字节数组容器中获取，并进行反序列化挂在到Unit身上
            if (!unit.GetComponent<UnitDBSaveComponent>().Bytes.TryGetValue(type, out byte[] bs))
            {
                return;
            }
            
            //这里的意图就是延迟组件的反序列化的时机，玩家有用到对应组件再对需要的组件进行反序列化操作，抹平CPU消耗尖峰
            Entity t = MongoHelper.Deserialize(type,bs) as Entity;
            unit.AddComponent(t);
            
            await ETTask.CompletedTask;
        }
    }
}