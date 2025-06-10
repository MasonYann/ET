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
            self.View.E_CreateRoomButton.AddListener(self.Root(), self.OnCreateRoomClicked);
            self.View.E_JoinRoomButton.AddListener(self.Root(), self.OnJoinRoomClicked);
            self.View.E_MatchButton.AddListenerAsync(self.Root(), self.OnMatchClickAsync);
        }

        public static void ShowWindow(this DlgMain self, Entity contextData = null)
        {
        }


        private static void OnCreateRoomClicked(this DlgMain self)
        {
            self.Scene().GetComponent<UIComponent>().ShowWindowAsync(WindowID.WindowID_Helper).Coroutine();
        }

        private static void OnJoinRoomClicked(this DlgMain self)
        {
            //创建房间是不是要有邀请好友的按钮，和已经加入的成员的头像或者小汽车牌
        }
        
        private static async ETTask OnMatchClickAsync(this DlgMain self)
        {
            //匹配按钮的逻辑，需要判断是否已经在匹配中，如果在匹配中，就取消匹配，如果不在匹配中，就开始匹配
            await EnterMapHelper.Match(self.Fiber());
            
            
        }
    }
}