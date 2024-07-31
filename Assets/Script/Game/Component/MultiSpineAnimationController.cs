using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

/// <summary>
/// 多脊柱动画控制器
/// </summary>
public class MultiSpineAnimationController : MonoBehaviour
{
	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event EventHandler<EffectArgs> OnAnimChange;

	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event EventHandler<EffectArgs> OnAnimSpeedChange;

	public float timeScale => SwitchSkeletonByWeaponType(currentSkeleton).timeScale;

	private void Awake()
	{
		timeController = GetComponent<PlayerTimeController>();
		_skeletonMeshRenderers = new List<MeshRenderer>
		{
			atkSkeleton.GetComponent<MeshRenderer>(),
			restAtkSkeleton.GetComponent<MeshRenderer>(),
			normalSkeleton.GetComponent<MeshRenderer>(),
			hurtSkeleton.GetComponent<MeshRenderer>(),
			upRisingkeleton.GetComponent<MeshRenderer>(),
			heavyAtkkeleton.GetComponent<MeshRenderer>()
		};
		m_animation = GetComponent<Animation>();
		currentAnim = string.Empty;
		//animData = JsonMapper.ToObject(animMapping.text);
	}

	public void Play(string animName, PlayerAction.SkeletonType skeletonType, bool loop = false, bool forceChange = false, float animSpeed = 1f)
	{
		if (m_animation.GetClip(animName) == null)
		{
			Debug.LogError("动画 " + animName + " 不存在");
			return;
		}
		pauseTimeScale = animSpeed;
		animSpeed = !timeController.isPause ? animSpeed : 0f;
		SwitchSkeletonByWeaponType(skeletonType).timeScale = animSpeed;
		m_animation[animName].speed = animSpeed;
		if (OnAnimSpeedChange != null)
		{
			OnAnimSpeedChange(this, new EffectArgs(animSpeed));
		}
		if (forceChange)
		{
			m_animation.Stop();
			m_animation[animName].wrapMode = ((!loop) ? WrapMode.Default : WrapMode.Loop);
			if (OnAnimChange != null)
			{
				OnAnimChange(this, new EffectArgs(animData.Get<string>(animName, animName), loop));
			}
			m_animation.Play(animName, PlayMode.StopAll);
			SwitchSkeletonByWeaponType(skeletonType).state.SetAnimation(0, animData.Get<string>(animName, animName), loop);
			SwitchSkeletonByWeaponType(skeletonType).skeleton.SetToSetupPose();
			SwitchSkeletonByWeaponType(skeletonType).Update(0f);
			currentAnim = animName;
		}
		else if (currentAnim != animName)
		{
			m_animation.Stop();
			m_animation[animName].wrapMode = ((!loop) ? WrapMode.Default : WrapMode.Loop);
			m_animation.Play(animName, PlayMode.StopAll);
			if (OnAnimChange != null)
			{
				OnAnimChange(this, new EffectArgs(animData.Get<string>(animName, animName), loop));
			}
			SwitchSkeletonByWeaponType(skeletonType).state.SetAnimation(0, animData.Get<string>(animName, animName), loop);
			SwitchSkeletonByWeaponType(skeletonType).skeleton.SetToSetupPose();
			SwitchSkeletonByWeaponType(skeletonType).Update(0f);
			currentAnim = animName;
		}
		animLoop = loop;
		animForceChange = forceChange;
	}

	public void AddAnim(string animName, PlayerAction.SkeletonType weaponType, bool loop)
	{
		if (m_animation[animName] == null)
		{
			Debug.LogError("动画 " + animName + " 不存在");
			return;
		}
		m_animation[animName].wrapMode = !loop ? WrapMode.Default : WrapMode.Loop;
		m_animation.PlayQueued(animName, QueueMode.CompleteOthers);
		SwitchSkeletonByWeaponType(weaponType).state.AddAnimation(0, animData.Get<string>(animName, animName), loop, 0f);
		SwitchSkeletonByWeaponType(weaponType).Update(0.01f);
		currentAnim = animName;
		animLoop = loop;
	}

	public void Pause()
	{
		if (m_animation[currentAnim] != null)
			m_animation[currentAnim].speed = 0f;
		SwitchSkeletonByWeaponType(currentSkeleton).timeScale = 0f;
	}

	public void Resume()
	{
		if (pauseTimeScale == 0f)
			pauseTimeScale = 1f;
		if (m_animation[currentAnim] != null)
			m_animation[currentAnim].speed = pauseTimeScale;
		SwitchSkeletonByWeaponType(currentSkeleton).timeScale = pauseTimeScale;
	}

	public void Resume(float animSpeed)
	{
		if (m_animation[currentAnim] != null)
			m_animation[currentAnim].speed = animSpeed;
		SwitchSkeletonByWeaponType(currentSkeleton).timeScale = animSpeed;
	}

	public void ChangeTimeScale(float animSpeed)
	{
		if (m_animation[currentAnim] != null)
			m_animation[currentAnim].speed = animSpeed;
		if (OnAnimSpeedChange != null)
			OnAnimSpeedChange(this, new EffectArgs(animSpeed));
		SwitchSkeletonByWeaponType(currentSkeleton).timeScale = animSpeed;
	}

	public float GetAnimNormalizedTime()
	{
		return m_animation[currentAnim].normalizedTime;
	}

	public float GetAnimLength()
	{
		return m_animation[currentAnim].length;
	}

	public string GetCurrentAnimName()
	{
		return m_animation[currentAnim].name;
	}

	private SkeletonAnimation SwitchSkeletonByWeaponType(PlayerAction.SkeletonType skeleton)
	{
		currentSkeleton = skeleton;
		switch (skeleton)
		{
		case PlayerAction.SkeletonType.Normal:
			HideSkeleton(2);
			return normalSkeleton;
		case PlayerAction.SkeletonType.Attack:
			HideSkeleton(0);
			return atkSkeleton;
		case PlayerAction.SkeletonType.SpAttack:
			HideSkeleton(1);
			return restAtkSkeleton;
		case PlayerAction.SkeletonType.Hurt:
			HideSkeleton(3);
			return hurtSkeleton;
		case PlayerAction.SkeletonType.UpRising:
			HideSkeleton(4);
			return upRisingkeleton;
		case PlayerAction.SkeletonType.HeavyAttack:
			HideSkeleton(5);
			return heavyAtkkeleton;
		default:
			return null;
		}
	}

	private void HideSkeleton(int index)
	{
		for (int i = 0; i < _skeletonMeshRenderers.Count; i++)
		{
			_skeletonMeshRenderers[i].enabled = (i == index);
		}
	}

	private float pauseTimeScale;

	private PlayerTimeController timeController;

	private List<MeshRenderer> _skeletonMeshRenderers;

	private Animation m_animation;

	private PlayerAction.SkeletonType currentSkeleton;

	private JsonData1 animData;

	[SerializeField]
	private TextAsset animMapping;

	[SerializeField]
	private SkeletonAnimation atkSkeleton;

	[SerializeField]
	private SkeletonAnimation restAtkSkeleton;

	[SerializeField]
	private SkeletonAnimation normalSkeleton;

	[SerializeField]
	private SkeletonAnimation hurtSkeleton;

	[SerializeField]
	private SkeletonAnimation upRisingkeleton;

	[SerializeField]
	private SkeletonAnimation heavyAtkkeleton;

	public string currentAnim;

	private bool animLoop;

	private bool animForceChange;

	public class EffectArgs : EventArgs
	{
		public EffectArgs(string _effectName, bool _loop)
		{
			effectName = _effectName;
			loop = _loop;
		}

		public EffectArgs(float _animSpeed)
		{
			animSpeed = _animSpeed;
		}

		public string effectName;

		public bool loop;

		public float animSpeed;
	}
}
