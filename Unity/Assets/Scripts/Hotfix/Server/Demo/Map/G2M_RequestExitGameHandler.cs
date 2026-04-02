namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class G2M_RequestExitGameHandler : MessageLocationHandler<Unit, G2M_RequestExitGame, M2G_RequestExitGame>
    {
        protected override async ETTask Run(Unit unit, G2M_RequestExitGame request, M2G_RequestExitGame response)
        {
            M2C_RemoveUnits m2CRemoveUnits = M2C_RemoveUnits.Create();
            m2CRemoveUnits.Units.Add(unit.Id);
            MapMessageHelper.Broadcast(unit, m2CRemoveUnits);

            UnitCacheHelper.AddOrUpdateUnitAllCache(unit);
            await unit.RemoveLocation(LocationType.Unit);
            unit.Dispose();
        }
    }
}
