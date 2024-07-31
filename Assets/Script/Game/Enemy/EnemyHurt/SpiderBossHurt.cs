using Framework.Core;
using UnityEngine;

/// <summary>
/// 蜘蛛Boss伤害
/// </summary>
public class SpiderBossHurt : EnemyBaseHurt
{
	protected override void Init()
	{
		defaultAnimName = "Hit1";
		defaultAirAnimName = "NoStiff";
		hurtData = SingletonMono<EnemyDataPreload>.Instance.hurt[EnemyType.愚笨蜘蛛];
	}

	protected override void PlayHurtAudio()
	{
		R.Audio.PlayEffect(255, transform.position);
		if (PlaySpHurtAudio())
		{
			R.Audio.PlayEffect(401, transform.position);
		}
	}

	protected override void PhysicAndEffect(Vector2 speed, Vector2 airSpeed, string normalAtkType, string airAtkType)
	{
		if (action.IsInWeakSta())
		{
			return;
		}
		if (action.stateMachine.currentState == "Hit2")
		{
			return;
		}
		if (action.stateMachine.currentState == "HitQTE")
		{
			return;
		}
		base.PhysicAndEffect(speed, airSpeed, normalAtkType, airAtkType);
	}

	protected override void HitIntoWeakState(Vector2 speed, Vector2 airSpeed, string normalAtkType, string airAtkType)
	{
		normalAtkType = "IdleToWeakMod";
		airAtkType = "IdleToWeakMod";
		base.HitIntoWeakState(speed, airSpeed, normalAtkType, airAtkType);
	}

	protected override void FlashAttackHurt()
	{
		ExecuteDieEffect();
		R.Effect.Generate(214);
		SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(30, 0.5f);
		R.Camera.Controller.CameraShake(0.5f);
		R.Camera.Controller.OpenMotionBlur(0.0833333358f, 1f, transform.position);
		R.Audio.PlayEffect(255, transform.position);
		SetHitSpeed(Vector2.zero);
		GenerateCritHurtNum(flashAttackDamage);
		action.AnimChangeState(HammerAction.StateEnum.Hit1, 0.5f);
		base.FlashAttackHurt();
	}

	public override void QTEHurt()
	{
		base.QTEHurt();
		R.Player.Action.QTEHPRecover();
		QTEZPositionRecover();
		QTEHpMinus();
	}

	public override void EnemyDie()
	{
	}

	protected override void ExecuteDie()
	{
		if (deadFlag)
		{
			return;
		}
		if (eAttr.rankType == EnemyAttribute.RankType.BOSS)
		{
			R.Trophy.AwardTrophy(27);
		}
		R.Player.Action.QTEHPRecover(eAttr.rankType == EnemyAttribute.RankType.BOSS);
		R.Audio.PlayEffect(412, transform.position);
		R.Audio.PlayEffect(Random.Range(105, 108), transform.position);
		deadFlag = true;
		eAttr.currentHp = 0;
		eAttr.inWeakState = false;
		eAttr.isFlyingUp = false;
		eAttr.checkHitGround = false;
		eAttr.stiffTime = 0f;
		eAttr.timeController.SetGravity(1f);
		R.Audio.PlayEffect(255, transform.position);
		EGameEvent.EnemyKilled.Trigger(eAttr);
		action.WeakEffectDisappear("Null");
		R.Effect.Generate(91, null, transform.position + new Vector3(0f, 1.2f, LayerManager.ZNum.Fx), Vector3.zero);
		R.Effect.Generate(49, transform);
		R.Effect.Generate(14, null, transform.position + new Vector3(0f, 1.2f, LayerManager.ZNum.Fx));
		AddCoinAndExp();
		R.Effect.Generate(213);
		R.Effect.Generate(214);
		SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(45, 0.2f);
		R.Camera.Controller.CameraShake(0.9166667f, 0.3f);
		action.hurtBox.gameObject.SetActive(false);
		eAttr.inWeakState = false;
		DieEffect();
		for (int i = 0; i < R.Enemy.EnemyAttributes.Count; i++)
		{
			R.Enemy.EnemyAttributes[i].GetComponent<EnemyBaseAction>().KillSelf();
		}
	}

	private void DieEffect()
	{
		Transform transform = Instantiate(corePrefab, center.position, Quaternion.identity);
		transform.GetComponent<Rigidbody2D>().velocity = new Vector2(-2 * eAttr.faceDir, 10f);
		transform.GetComponent<EnemyArm>().SetAngularSpeed(100 * eAttr.faceDir);
		Transform transform2 = Instantiate(headPrefab, headPos.position, Quaternion.identity);
		transform2.GetComponent<Rigidbody2D>().velocity = new Vector2(2 * eAttr.faceDir, 10f);
		transform2.GetComponent<EnemyArm>().SetAngularSpeed(100 * -(float)eAttr.faceDir);
	}

	[SerializeField]
	private Transform headPos;

	[SerializeField]
	private Transform headPrefab;

	[SerializeField]
	private Transform corePrefab;
}
