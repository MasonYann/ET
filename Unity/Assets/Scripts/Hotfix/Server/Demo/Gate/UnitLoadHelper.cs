namespace ET.Server
{
    public static class UnitLoadHelper
    {
        /// <summary>
        /// 加载玩家的数据缓存服数据。
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static async ETTask<(bool, Unit)> LoadUnit(Player player)
        {
            //添加 GateMap 组件
            GateMapComponent gateMapComponent = player.AddComponent<GateMapComponent>();
            //创建地图服务器的 scene 实体
            gateMapComponent.Scene = await GateMapFactory.Create(gateMapComponent, player.Id, IdGenerater.Instance.GenerateInstanceId(), "GateMap");

            //查找地图服务器身上挂载的关于玩家的 id 的 unit 游戏数据
            //Player 的 Root() 返回 Player 的 Scene == Gate 网关 Scene
            Unit unit = await UnitCacheHelper.GetUnitCache(player.Root(), gateMapComponent.Scene, player.UnitId);

            //如果 unit 为空，说明玩家是新玩家
            bool isNewUnit = unit == null;

            //如果是新玩家，就创建一个 unit 数据
            if (isNewUnit)
            {
                //创建 unit 数据（player.UnitId 和 player.Id 是一样的，都是 player 的角色ID）
                unit = UnitFactory.Create(gateMapComponent.Scene, player.UnitId, UnitType.Player);
                unit.AddComponent<UnitDBSaveComponent>();

                // //把角色信息添加到 unit 数据
                // var roleInfos = await DBManagerComponent.Instance.GetZoneDB(player.DomainZone()).Query<RoleInfo>(d => d.Id == player.UnitId);
                // unit.AddComponent(roleInfos[0]);

                //将游戏数据添加到游戏缓存服
                UnitCacheHelper.AddOrUpdateUnitAllCache(unit);
            }
            else
            {
                if (unit.GetComponent<UnitDBSaveComponent>() == null)
                {
                    unit.AddComponent<UnitDBSaveComponent>();
                }
            }

            return (isNewUnit, unit);
        }
    }
}