using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [FriendOf(typeof(DlgMain))]
    public static class DlgMainSystem
    {
        public static void RegisterUIEvent(this DlgMain self)
        {
            self.View.E_BattleButton.AddListener(self.Root(), self.OnBattleButtonClickHandler);
        }

        public static void ShowWindow(this DlgMain self, Entity contextData = null)
        {
        }

        public static void OnBattleButtonClickHandler(this DlgMain self)
        {
            // self.Root().GetComponent<UIComponent>().ShowWindow(WindowID.WindowID_Adventure);

            AdventureHelper.RequestStartGameLevel(self.Root(), 1).Coroutine();
        }
    }
}