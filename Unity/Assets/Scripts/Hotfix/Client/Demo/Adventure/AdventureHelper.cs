using System;

namespace ET.Client
{
    public static class AdventureHelper
    {
        /// <summary>
        /// 请求进入关卡开始战斗。
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public static async ETTask<int> RequestStartGameLevel(Scene scene, int levelId)
        {
            C2M_StartGameLevel c2MStartGameLevel = C2M_StartGameLevel.Create();
            c2MStartGameLevel.LevelId = levelId;
            M2C_StartGameLevel m2CStartGameLevel = null;
            try
            {
                m2CStartGameLevel = (M2C_StartGameLevel)await scene.Root().GetComponent<ClientSenderComponent>().Call(c2MStartGameLevel);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }

            if (m2CStartGameLevel.Error != ErrorCode.ERR_Success)
            {
                Log.Error(m2CStartGameLevel.Error.ToString());
                return m2CStartGameLevel.Error;
            }

            await ETTask.CompletedTask;
            return ErrorCode.ERR_Success;
        }
    }
}