namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class AIDevModeRuntimeComponent: Entity, IAwake
    {
        public bool HasRedirectedToTestScene { get; set; }
    }
}
