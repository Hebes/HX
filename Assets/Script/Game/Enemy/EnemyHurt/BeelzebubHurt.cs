using UnityEngine;

/// <summary>
/// 恶魔的伤害
/// </summary>
public class BeelzebubHurt : EnemyBaseHurt
{
	protected override void Init()
	{
		this.defaultAnimName = "Hit";
		this.defaultAirAnimName = "Hit";
		this.hurtData = SingletonMono<EnemyDataPreload>.Instance.hurt[EnemyType.暴食];
	}

	protected override void PhysicAndEffect(Vector2 speed, Vector2 airSpeed, string normalAtkType, string airAtkType)
	{
		if (this.action.stateMachine.currentState == "HitQTE")
		{
			return;
		}
		base.PhysicAndEffect(speed, airSpeed, normalAtkType, airAtkType);
	}

	protected override void PlayHurtAudio()
	{
		base.PlayHurtAudio();
		if (base.PlaySpHurtAudio())
		{
			R.Audio.PlayEffect(397, new Vector3?(base.transform.position));
		}
	}

	protected override void FlashAttackHurt()
	{
		this.ExecuteDieEffect();
		R.Effect.Generate(214, null, default(Vector3), default(Vector3), default(Vector3), true);
		SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(30, 0.5f);
		R.Camera.Controller.CameraShake(0.5f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
		R.Camera.Controller.OpenMotionBlur(0.0833333358f, 1f, base.transform.position);
		this.SetHitSpeed(Vector2.zero);
		base.GenerateCritHurtNum(base.flashAttackDamage);
		this.action.AnimChangeState(BeelzebubAction.StateEnum.Hit, 0.5f);
		base.FlashAttackHurt();
	}

	public override void QTEHurt()
	{
		base.QTEHurt();
		R.Player.Action.QTEHPRecover(false);
		base.QTEZPositionRecover();
		base.QTEHpMinus();
	}

	public override void EnemyDie()
	{
	}

	protected override void ExecuteDie()
	{
		if (this.deadFlag)
		{
			return;
		}
		base.ExecuteDie();
		if (this.eAttr.rankType == EnemyAttribute.RankType.BOSS)
		{
			R.Trophy.AwardTrophy(25);
			R.Audio.PlayEffect(412, new Vector3?(base.transform.position));
			for (int i = 0; i < R.Enemy.EnemyAttributes.Count; i++)
			{
				R.Enemy.EnemyAttributes[i].GetComponent<EnemyBaseAction>().KillSelf();
			}
		}
		R.Player.Action.QTEHPRecover(this.eAttr.rankType == EnemyAttribute.RankType.BOSS);
		this.action.hurtBox.gameObject.SetActive(false);
		this.eAttr.inWeakState = false;
		this.SetHitSpeed(Vector2.zero);
	}
}
