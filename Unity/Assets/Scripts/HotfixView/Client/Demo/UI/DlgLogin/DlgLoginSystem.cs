using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [FriendOf(typeof(DlgLogin))]
    public static class DlgLoginSystem
    {
        public static void RegisterUIEvent(this DlgLogin self)
        {
            self.View.ELoginButton.AddListener(self.Root(), self.OnLoginClickHandler);
        }

        public static void ShowWindow(this DlgLogin self, Entity contextData = null)
        {
            self.View.EAccountInputField.text = "Zy123456";
            self.View.EPasswordInputField.text = "Zy123456";
        }

        public static void OnLoginClickHandler(this DlgLogin self)
        {
            try
            {
                //调用登录方法
                LoginHelper.Login(self.Root(),
                    self.View.EAccountInputField.text,
                    self.View.EPasswordInputField.text).Coroutine();
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }
    }
}