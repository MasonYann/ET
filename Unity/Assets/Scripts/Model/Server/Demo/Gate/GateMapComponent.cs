namespace ET.Server
{
    /// <summary>
    /// Gate 网关上的 Map Scene 管理组件。
    /// 负责传送 Player 到 Map Scene。
    /// </summary>
    [ComponentOf(typeof(Player))]
    public class GateMapComponent: Entity, IAwake
    {
        private EntityRef<Scene> scene;

        public Scene Scene
        {
            get
            {
                return this.scene;
            }
            set
            {
                this.scene = value;
            }
        }
    }
}