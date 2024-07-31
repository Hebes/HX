using Framework.Core;
using UnityEngine;

/// <summary>
/// 审判者伤害
/// </summary>
public class JudgesHurt : EnemyBaseHurt
{
	protected override void Init()
	{
		this.defaultAnimName = "NoStiff";
		this.defaultAirAnimName = "NoStiff";
		this.hurtData = SingletonMono<EnemyDataPreload>.Instance.hurt[EnemyType.犹大];
	}

	protected override void HitIntoWeakState(Vector2 speed, Vector2 airSpeed, string normalAtkType, string airAtkType)
	{
		normalAtkType = "IdleToWeak";
		airAtkType = "IdleToWeak";
		base.ChaseEnd();
		this.action.EnterWeakState();
		this.eAttr.stiffTime = 1f;
		R.Audio.PlayEffect(405, new Vector3?(base.transform.position));
		base.PlayHurtAnim(normalAtkType, airAtkType, speed, airSpeed);
		if (!base.IsInvoking("ExitWeak"))
		{
			base.Invoke("ExitWeak", 5f);
		}
	}

	protected override void FlashAttackHurt()
	{
		R.Effect.Generate(156, null, base.transform.position + Vector3.up, default(Vector3), default(Vector3), true);
		R.Effect.Generate(214, null, default(Vector3), default(Vector3), default(Vector3), true);
		SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(30, 0.5f);
		R.Camera.Controller.CameraShake(0.5f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
		R.Camera.Controller.OpenMotionBlur(0.0833333358f, 1f, base.transform.position);
		R.Audio.PlayEffect(405, new Vector3?(base.transform.position));
		this.SetHitSpeed(Vector2.zero);
		base.GenerateCritHurtNum(base.flashAttackDamage);
		base.FlashAttackHurt();
	}

	public override void QTEHurt()
	{
		base.QTEHurt();
		R.Audio.PlayEffect(405, new Vector3?(base.transform.position));
		R.Player.Action.QTEHPRecover(false);
		base.QTEZPositionRecover();
		base.QTEHpMinus();
	}

	protected override void ExecuteDieEffect()
	{
	}

	public override void EnemyDie()
	{
	}

	protected override void ExecuteFollow()
	{
	}

	protected override void ExecuteDie()
	{
		if (this.deadFlag)
		{
			return;
		}
		R.Trophy.AwardTrophy(33);
		R.Audio.PlayEffect(412, new Vector3?(base.transform.position));
		R.Player.Action.QTEHPRecover(this.eAttr.rankType == EnemyAttribute.RankType.BOSS);
		R.Audio.PlayEffect(UnityEngine.Random.Range(105, 108), new Vector3?(base.transform.position));
		R.Audio.PlayEffect(404, new Vector3?(base.transform.position));
		this.deadFlag = true;
		this.eAttr.currentHp = 0;
		this.eAttr.inWeakState = false;
		this.eAttr.isFlyingUp = false;
		this.eAttr.checkHitGround = false;
		this.eAttr.stiffTime = 0f;
		this.eAttr.timeController.SetGravity(1f);
		EGameEvent.EnemyKilled.Trigger(eAttr);
		this.action.WeakEffectDisappear("Null");
		R.Effect.Generate(91, null, base.transform.position + new Vector3(0f, 3f, LayerManager.ZNum.Fx), Vector3.zero, default(Vector3), true);
		R.Effect.Generate(49, null, base.transform.position + new Vector3(0f, 3f, LayerManager.ZNum.Fx), default(Vector3), default(Vector3), true);
		R.Effect.Generate(14, null, base.transform.position + new Vector3(0f, 3f, LayerManager.ZNum.Fx), default(Vector3), default(Vector3), true);
		base.AddCoinAndExp();
		R.Effect.Generate(213, null, default(Vector3), default(Vector3), default(Vector3), true);
		R.Effect.Generate(214, null, default(Vector3), default(Vector3), default(Vector3), true);
		SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(45, 0.2f);
		R.Camera.Controller.CameraShake(0.9166667f, 0.3f, CameraController.ShakeTypeEnum.Rect, false);
		this.action.hurtBox.gameObject.SetActive(false);
		this.eAttr.inWeakState = false;
		for (var i = 0; i < R.Enemy.EnemyAttributes.Count; i++)
		{
			R.Enemy.EnemyAttributes[i].GetComponent<EnemyBaseAction>().KillSelf();
		}
	}
}
