using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Framework.Core;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// BOSS血条控制器
/// </summary>
public class UIBossHpBarController : MonoBehaviour
{
	private void Awake()
	{
		// cursorTweenAlpha = cursorSprite.GetComponent<TweenAlpha>();
		// StartCoroutine(HPValueHistoryRecorder());
		// StartCoroutine(HPChangeAnim());
	}

	private void Update()
	{
		if (boss == null)
		{
			return;
		}
		BindData();
		if (bossData.FullCurrentHp <= 0)
		{
			Disappear();
		}
	}

	private IEnumerator HPChangeAnim()
	{
		// for (;;)
		// {
		// 	if (play)
		// 	{
		// 		play = false;
		// 		cursorTweenAlpha.enabled = true;
		// 		cursorSprite.enabled = true;
		// 		float start = hp2Bar.value;
		// 		float targe = hp1Bar.value;
		// 		for (float x = 0f; x < 1f; x += 0.02f)
		// 		{
		// 			float current = start + (targe - start) * x;
		// 			hp2Bar.value = current;
		// 			yield return new WaitForSeconds(0.02f);
		// 		}
		// 		cursorTweenAlpha.enabled = false;
		// 		cursorSprite.alpha = 0f;
		// 	}
		// 	yield return null;
		// }
		yield break;
	}

	private IEnumerator HPValueHistoryRecorder()
	{
		// for (;;)
		// {
		// 	hpValueHistory.Enqueue(hp1Bar.value);
		// 	if (hpValueHistory.Count > 10)
		// 	{
		// 		hpValueHistory.Dequeue();
		// 	}
		// 	yield return new WaitForSeconds(0.1f);
		// }
		yield break;
	}

	private void BindData()
	{
		// bossData.Update();
		// hp1Bar.value = bossData.CurrentAppearHp / Math.Max(1f, bossData.MaxHps[bossData.CurrentHpBarIndex]);
		// hp1Bar.value = bossData.CurrentAppearHp / (float)Mathf.Max(bossData.MaxHps[bossData.CurrentHpBarIndex], 1);
		// if (hpValueHistory.Count >= 10 && Mathf.Abs(hp1Bar.value - hpValueHistory.Peek()) < 0.0001 && Mathf.Abs(hp1Bar.value - hpValueWhenLastPlayAnim) > 0.0001)
		// {
		// 	hpValueWhenLastPlayAnim = hp1Bar.value;
		// 	play = true;
		// }
		// if (Math.Abs(hp1Bar.value - 1f) < 0.0001)
		// {
		// 	hp2Bar.value = 1f;
		// 	cursorTweenAlpha.enabled = false;
		// 	cursorSprite.alpha = 0f;
		// 	hpValueWhenLastPlayAnim = 1f;
		// }
		// int num = bossData.HpBarCount - bossData.CurrentHpBarIndex;
		// _numLabel.alpha = (float)((num != 1) ? 1 : 0);
		// _numLabel.text = StringTools.Int2String[bossData.HpBarCount - bossData.CurrentHpBarIndex];
	}

	public void Create(EnemyAttribute enemy, List<int> phaseHp)
	{
		if (!Visible)
		{
			boss = enemy;
			bossData = new BossHpBarData(phaseHp);
			FadeTo(1f, 1f, delegate
			{
				Visible = true;
			});
		}
		else
		{
			"重复生成Boss血条".Error();
		}
	}

	public void Disappear()
	{
		if (Visible)
		{
			Visible = false;
			FadeTo(0f, 1f);
			boss = null;
			StopCoroutine(HPValueHistoryRecorder());
			StopCoroutine(HPChangeAnim());
		}
		else
		{
			"重复隐藏Boss血条".Error();
		}
	}

	public YieldInstruction FadeTo(float endValue, float duration)
	{
		return DOTween.To(() => _widget.alpha, delegate(float alpha)
		{
			_widget.alpha = alpha;
		}, endValue, duration).WaitForCompletion();
	}

	public YieldInstruction FadeTo(float endValue, float duration, TweenCallback onComplete)
	{
		return DOTween.To(() => _widget.alpha, delegate(float alpha)
		{
			_widget.alpha = alpha;
		}, endValue, duration).OnComplete(onComplete).WaitForCompletion();
	}

	private static EnemyAttribute boss;

	private BossHpBarData bossData;

	[SerializeField]
	private CanvasGroup _widget;

	[SerializeField]
	private Text _numLabel;

	// [SerializeField]
	// private UIProgressBar hp1Bar;
	//
	// [SerializeField]
	// private UIProgressBar hp2Bar;

	[SerializeField]
	private CanvasGroup cursorSprite;

	// private TweenAlpha cursorTweenAlpha;

	private bool play;

	public bool Visible;

	private readonly Queue<float> hpValueHistory = new Queue<float>();

	private float hpValueWhenLastPlayAnim = 1f;

	private class BossHpBarData
	{
		public BossHpBarData(List<int> phaseHp)
		{
			MaxHps = phaseHp;
			HpBarCount = phaseHp.Count;
		}

		public int FullMaxHp
		{
			get
			{
				return boss.maxHp;
			}
		}

		public int FullCurrentHp
		{
			get
			{
				return boss.currentHp;
			}
		}

		public void Update()
		{
			int num = 0;
			for (int i = HpBarCount - 1; i >= 0; i--)
			{
				num += MaxHps[i];
				if (FullCurrentHp <= num)
				{
					CurrentHpBarIndex = i;
					CurrentAppearHp = FullCurrentHp - (num - MaxHps[i]);
					break;
				}
			}
		}

		public readonly int HpBarCount;

		public readonly List<int> MaxHps;

		public int CurrentHpBarIndex;

		public int CurrentAppearHp;
	}
}
