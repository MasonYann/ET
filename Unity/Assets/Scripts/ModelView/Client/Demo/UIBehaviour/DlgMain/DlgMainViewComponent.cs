
using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ComponentOf(typeof(DlgMain))]
	[EnableMethod]
	public  class DlgMainViewComponent : Entity,IAwake,IDestroy 
	{
		public UnityEngine.UI.Button E_CreateRoomButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CreateRoomButton == null )
     			{
		    		this.m_E_CreateRoomButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"Group_Bottom/E_CreateRoom");
     			}
     			return this.m_E_CreateRoomButton;
     		}
     	}

		public UnityEngine.UI.Image E_CreateRoomImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_CreateRoomImage == null )
     			{
		    		this.m_E_CreateRoomImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"Group_Bottom/E_CreateRoom");
     			}
     			return this.m_E_CreateRoomImage;
     		}
     	}

		public UnityEngine.UI.Button E_JoinRoomButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_JoinRoomButton == null )
     			{
		    		this.m_E_JoinRoomButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"Group_Bottom/E_JoinRoom");
     			}
     			return this.m_E_JoinRoomButton;
     		}
     	}

		public UnityEngine.UI.Image E_JoinRoomImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_JoinRoomImage == null )
     			{
		    		this.m_E_JoinRoomImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"Group_Bottom/E_JoinRoom");
     			}
     			return this.m_E_JoinRoomImage;
     		}
     	}

		public UnityEngine.UI.Button E_MatchButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_MatchButton == null )
     			{
		    		this.m_E_MatchButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"Group_Bottom/E_Match");
     			}
     			return this.m_E_MatchButton;
     		}
     	}

		public UnityEngine.UI.Image E_MatchImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_MatchImage == null )
     			{
		    		this.m_E_MatchImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"Group_Bottom/E_Match");
     			}
     			return this.m_E_MatchImage;
     		}
     	}

		public UnityEngine.UI.Text E_RoleLevelText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_RoleLevelText == null )
     			{
		    		this.m_E_RoleLevelText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"Group_Top/Group_Head/E_RoleLevel");
     			}
     			return this.m_E_RoleLevelText;
     		}
     	}

		public UnityEngine.UI.Text E_GoldText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_GoldText == null )
     			{
		    		this.m_E_GoldText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"Group_Top/Group_GoldTip/E_Gold");
     			}
     			return this.m_E_GoldText;
     		}
     	}

		public UnityEngine.UI.Text E_ExpText
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_ExpText == null )
     			{
		    		this.m_E_ExpText = UIFindHelper.FindDeepChild<UnityEngine.UI.Text>(this.uiTransform.gameObject,"Group_Top/Group_ExpTip/E_Exp");
     			}
     			return this.m_E_ExpText;
     		}
     	}

		public UnityEngine.UI.Button E_RankButton
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_RankButton == null )
     			{
		    		this.m_E_RankButton = UIFindHelper.FindDeepChild<UnityEngine.UI.Button>(this.uiTransform.gameObject,"E_Rank");
     			}
     			return this.m_E_RankButton;
     		}
     	}

		public UnityEngine.UI.Image E_RankImage
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_E_RankImage == null )
     			{
		    		this.m_E_RankImage = UIFindHelper.FindDeepChild<UnityEngine.UI.Image>(this.uiTransform.gameObject,"E_Rank");
     			}
     			return this.m_E_RankImage;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_E_CreateRoomButton = null;
			this.m_E_CreateRoomImage = null;
			this.m_E_JoinRoomButton = null;
			this.m_E_JoinRoomImage = null;
			this.m_E_MatchButton = null;
			this.m_E_MatchImage = null;
			this.m_E_RoleLevelText = null;
			this.m_E_GoldText = null;
			this.m_E_ExpText = null;
			this.m_E_RankButton = null;
			this.m_E_RankImage = null;
			this.uiTransform = null;
		}

		private UnityEngine.UI.Button m_E_CreateRoomButton = null;
		private UnityEngine.UI.Image m_E_CreateRoomImage = null;
		private UnityEngine.UI.Button m_E_JoinRoomButton = null;
		private UnityEngine.UI.Image m_E_JoinRoomImage = null;
		private UnityEngine.UI.Button m_E_MatchButton = null;
		private UnityEngine.UI.Image m_E_MatchImage = null;
		private UnityEngine.UI.Text m_E_RoleLevelText = null;
		private UnityEngine.UI.Text m_E_GoldText = null;
		private UnityEngine.UI.Text m_E_ExpText = null;
		private UnityEngine.UI.Button m_E_RankButton = null;
		private UnityEngine.UI.Image m_E_RankImage = null;
		public Transform uiTransform = null;
	}
}
