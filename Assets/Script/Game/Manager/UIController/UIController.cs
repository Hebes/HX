using System;
using System.Collections;
using DG.Tweening;
using Framework.Core;
using UnityEngine;

public class UIController : SingletonMono<UIController>
{
	private void Awake()
	{
		Camera = CameraGO.GetComponent<Camera>();
	}

	public void Reset()
	{
		// if (bossHpBar.Visible)
		// {
		// 	bossHpBar.Disappear();
		// }
		// R.Ui.HitsGrade.HideHitNumAndBar();
	}

	public YieldInstruction ShowUI(bool immediately = false)
	{
		// if (immediately)
		// {
		// 	for (int i = 0; i < _uiNeedHide.Length; i++)
		// 	{
		// 		_uiNeedHide[i].alpha = 1f;
		// 	}
		// 	IsHide = false;
		// 	return null;
		// }
		return IFadeInAnim().StartCoroutine();
	}

	public YieldInstruction HideUI(bool immediately = false)
	{
		// if (immediately)
		// {
		// 	for (int i = 0; i < _uiNeedHide.Length; i++)
		// 	{
		// 		_uiNeedHide[i].alpha = 0f;
		// 	}
		// 	IsHide = true;
		// 	return null;
		// }
		return StartCoroutine(IFadeOutAnim());
	}

	public GameObject CreateEnemyPoint(EnemyAttribute enemy)
	{
		GameObject gameObject = Instantiate(enemyPoint);
		if (gameObject != null)
		{
			UIEnemyPointController component = gameObject.GetComponent<UIEnemyPointController>();
			component.enemy = enemy;
		}
		return enemyPoint;
	}

	public void EnterMovieMode()
	{
		// UIWidget component = movieModePlayAnimation.GetComponent<UIWidget>();
		// if (component.alpha < 1f)
		// {
		// 	component.alpha = 1f;
		// 	movieModePlayAnimation.Play(true);
		// }
	}

	public void ExitMovieMode()
	{
		// UIWidget widget = movieModePlayAnimation.GetComponent<UIWidget>();
		// if (widget.alpha < 1f)
		// {
		// 	return;
		// }
		// EventDelegate.Add(movieModePlayAnimation.onFinished, delegate()
		// {
		// 	widget.alpha = 0f;
		// }, true);
		// movieModePlayAnimation.Play(false);
	}

	private IEnumerator IFadeInAnim()
	{
		// for (int i = 0; i < _uiNeedHide.Length; i++)
		// {
		// 	FadeTo(_uiNeedHide[i], 1f, 0.5f);
		// }
		yield return WorldTime.WaitForSecondsIgnoreTimeScale(0.5f);
		IsHide = false;
	}

	private IEnumerator IFadeOutAnim()
	{
		// IsHide = true;
		// for (int i = 0; i < _uiNeedHide.Length; i++)
		// {
		// 	FadeTo(_uiNeedHide[i], 0f, 0.5f);
		// }
		yield return WorldTime.WaitForSecondsIgnoreTimeScale(0.5f);
	}

	// private void FadeTo(UIRect widget, float endValue, float duration)
	// {
	// 	DOTween.To(() => widget.alpha, delegate(float alpha)
	// 	{
	// 		widget.alpha = alpha;
	// 	}, endValue, duration);
	// }

	public bool IsHide;

	// [SerializeField]
	 //public UIWidget RootWidget;

	[SerializeField]
	public GameObject CameraGO;

	[NonSerialized]
	public Camera Camera;

	// [SerializeField]
	// private UIRect[] _uiNeedHide;
	
	[HideInInspector]
	public UISubtitleController UISubtitle;
	
	[SerializeField]
	public UIBlackSceneController BlackScene;
	
	// [SerializeField]
	// public UIHitsGradeController HitsGrade;
	//
	// [SerializeField]
	// public UIEnhancementController Enhancement;
	//
	// [SerializeField]
	// public UINotifacationController uiNotifacation;
	
	[SerializeField]
	public UIBossHpBarController bossHpBar;
	
	// [SerializeField]
	// public UIToast Toast;
	//
	// [SerializeField]
	// public UISaveProgressCircleController SaveProgressCircle;
	//
	// [SerializeField]
	// public UITerminalController Terminal;
	//
	// [SerializeField]
	// public UILevelSelectController LevelSelect;
	//
	// [SerializeField]
	// public UITutorialController Tutorial;
	//
	[SerializeField]
	public UIPauseController Pause;

	// [SerializeField]
	// public UILevelNameController LevelName;
	//
	// [SerializeField]
	// public UIVolumeController Volume;
	//
	// [SerializeField]
	// public UIEndTitleController EndTitle;
	
	[SerializeField]
	public UIFlashController Flash;
	
	// [SerializeField]
	// public UIBloodPalaceController BloodPalace;

	[SerializeField]
	public UITrophyNotificationController TrophyNotification;

	[SerializeField]
	private GameObject enemyPoint;

	// [SerializeField]
	// private UIPlayAnimation movieModePlayAnimation;
}
