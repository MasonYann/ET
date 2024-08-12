using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
	public enum MotionType
	{
		None,
		Idle,
		Run,
	}

	[ComponentOf]
	public class AnimatorComponent : Entity, IAwake, IUpdate, IDestroy
	{
		public Dictionary<string, AnimationClip> animationClips = new();
		public HashSet<string> Parameter = new();

		public MotionType MotionType;//运动类型
		public float MontionSpeed;	//运动速度
		public bool isStop;			//是否停止
		public float stopSpeed;		//停止速度
		public Animator Animator;
	}
}