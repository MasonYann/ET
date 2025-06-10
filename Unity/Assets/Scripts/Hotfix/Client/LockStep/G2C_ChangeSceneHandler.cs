using System.Diagnostics;

namespace ET.Client
{
    /// <summary>
    /// 客户端接收 Match 服务器发送到 Gate 服务器匹配成功消息后，Gate  服务器会向所有玩家发送场景切换消息。
    /// </summary>
    // [MessageHandler(SceneType.LockStep)]
    [MessageHandler(SceneType.Demo)]
    public class Match2G_NotifyMatchSuccessHandler : MessageHandler<Scene, Match2G_NotifyMatchSuccess>
    {
        protected override async ETTask Run(Scene root, Match2G_NotifyMatchSuccess message)
        {
            // await LSSceneChangeHelper.SceneChangeTo(root, "Map1", message.ActorId.InstanceId);
            Log.Debug("匹配成功！");
            await LSSceneChangeHelper.SceneChangeTo(root, "Room1", message.ActorId.InstanceId);
        }
    }
}