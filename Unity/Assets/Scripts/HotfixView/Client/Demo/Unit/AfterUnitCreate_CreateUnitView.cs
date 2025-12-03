using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class AfterUnitCreate_CreateUnitView : AEvent<Scene, AfterUnitCreate>
    {
        protected override async ETTask Run(Scene scene, AfterUnitCreate args)
        {
            Unit unit = args.Unit;
            // Unit View层
            //string assetsName = $"Assets/Bundles/Unit/Unit.prefab";
            UnitConfig unitConfig = UnitConfigCategory.Instance.Get(unit.ConfigId);
            string assetsName = unitConfig.PrefabName;

            GameObject bundleGameObject = await scene.GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>($"Assets/Bundles/Unit/{assetsName}");

            GlobalComponent globalComponent = scene.Root().GetComponent<GlobalComponent>();
            GameObject go = UnityEngine.Object.Instantiate(bundleGameObject, globalComponent.Unit, true);
            unit.AddComponent<GameObjectComponent>().GameObject = go;
            unit.GetComponent<GameObjectComponent>().SpriteRenderer = go.GetComponent<SpriteRenderer>();
            unit.AddComponent<AnimatorComponent>();

            unit.Position = unit.Type() == UnitType.Player ? new Vector3(-1.5f, 0, 0) : new Vector3(1.5f, RandomGenerator.RandomNumber(-1, 1), 0);
            Log.Debug("Unit 创建成功！！！");

            await ETTask.CompletedTask;
        }
    }
}