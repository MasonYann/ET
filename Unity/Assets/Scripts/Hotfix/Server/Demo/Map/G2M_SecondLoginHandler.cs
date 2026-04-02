namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class G2M_SecondLoginHandler : MessageLocationHandler<Unit, G2M_SecondLogin, M2G_SecondLogin>
    {
        protected override async ETTask Run(Unit unit, G2M_SecondLogin request, M2G_SecondLogin response)
        {
            M2C_StartSceneChange m2CStartSceneChange = M2C_StartSceneChange.Create();
            m2CStartSceneChange.SceneInstanceId = unit.Scene().InstanceId;
            m2CStartSceneChange.SceneName = unit.Scene().Name;
            MapMessageHelper.SendToClient(unit, m2CStartSceneChange);

            M2C_CreateMyUnit m2CCreateMyUnit = M2C_CreateMyUnit.Create();
            m2CCreateMyUnit.Unit = UnitHelper.CreateUnitInfo(unit);
            MapMessageHelper.SendToClient(unit, m2CCreateMyUnit);

            AOIEntity aoiEntity = unit.GetComponent<AOIEntity>();
            if (aoiEntity != null)
            {
                M2C_CreateUnits m2CCreateUnits = M2C_CreateUnits.Create();
                foreach (AOIEntity beSeePlayer in unit.GetBeSeePlayers().Values)
                {
                    Unit otherUnit = beSeePlayer.GetParent<Unit>();
                    if (otherUnit == null || otherUnit.Id == unit.Id)
                    {
                        continue;
                    }

                    m2CCreateUnits.Units.Add(UnitHelper.CreateUnitInfo(otherUnit));
                }

                if (m2CCreateUnits.Units.Count > 0)
                {
                    MapMessageHelper.SendToClient(unit, m2CCreateUnits);
                }
            }

            await ETTask.CompletedTask;
        }
    }
}
