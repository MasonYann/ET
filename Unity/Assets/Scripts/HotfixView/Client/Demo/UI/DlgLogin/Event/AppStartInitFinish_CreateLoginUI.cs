using System;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class AppStartInitFinish_CreateLoginUI: AEvent<Scene, AppStartInitFinish>
    {
        protected override async ETTask Run(Scene root, AppStartInitFinish args)
        {
            await root.GetComponent<UIComponent>().PreLoadWindowAsync(WindowID.WindowID_Loading);

            if (AIDevModeHelper.IsEnabled(root))
            {
                if (root.GetComponent<AIDevModeRuntimeComponent>() == null)
                {
                    root.AddComponent<AIDevModeRuntimeComponent>();
                }

                LoginHelper.Login(root, AIDevModeHelper.TestAccount, AIDevModeHelper.TestPassword).Coroutine();
                return;
            }

            await root.GetComponent<UIComponent>().ShowWindowAsync(WindowID.WindowID_Login);
        }
    }

    public static class AIDevModeHelper
    {
        public const string TestAccount = "AI_Tester_01";
        public const string TestPassword = "123456";
        public const string TestSceneName = "Map2";

        public static bool IsEnabled(Scene root)
        {
            GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
            return globalComponent != null && (globalComponent.GlobalConfig.IsAIDevMode || HasLaunchArg("isAIDevMode"));
        }

        private static bool HasLaunchArg(string argName)
        {
            foreach (string arg in Environment.GetCommandLineArgs())
            {
                if (string.IsNullOrWhiteSpace(arg))
                {
                    continue;
                }

                string normalized = arg.Trim().TrimStart('-', '/');
                if (string.Equals(normalized, argName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
