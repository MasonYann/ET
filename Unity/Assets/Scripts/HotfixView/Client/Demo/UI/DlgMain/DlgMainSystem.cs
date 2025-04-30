using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
	[FriendOf(typeof(DlgMain))]
	public static  class DlgMainSystem
	{

		public static void RegisterUIEvent(this DlgMain self)
		{
		  self.View.E_PlayButton.AddListener(self.Root(), self.OnPlayClicked);
		}

		public static void ShowWindow(this DlgMain self, Entity contextData = null)
		{
		}

		private static void OnPlayClicked(this DlgMain self)
		{
			
		}

		 

	}
}
