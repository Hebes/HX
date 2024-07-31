using Framework.Core;
using UnityEngine;
using Random = UnityEngine.Random;

public class JumperFooterHurt : EnemyBaseHurt
{
	protected override void Init()
	{
		this.defaultAnimName = "Hit1";
		this.defaultAirAnimName = "NoStiff";
		this.hurtData = SingletonMono<EnemyDataPreload>.Instance.hurt[EnemyType.跳拳大脚组合];
	}

	protected override void PhysicAndEffect(Vector2 speed, Vector2 aieSpeed, string normalAtkType, string airAtkType)
	{
		if (this.action.stateMachine.currentState == "Jump" || this.action.stateMachine.currentState == "Atk3" || this.action.stateMachine.currentState == "HitQTE")
		{
			return;
		}
		base.PhysicAndEffect(speed, aieSpeed, normalAtkType, airAtkType);
	}

	protected override void HitIntoWeakState(Vector2 speed, Vector2 airSpeed, string normalAtkType, string airAtkType)
	{
		R.Audio.PlayEffect(257, new Vector3?(base.transform.position));
		base.HitIntoWeakState(speed, airSpeed, normalAtkType, airAtkType);
	}

	protected override void PlayHurtAudio()
	{
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
		R.Audio.PlayEffect(257, new Vector3?(base.transform.position));
		this.SetHitSpeed(Vector2.zero);
		base.GenerateCritHurtNum(base.flashAttackDamage);
		this.action.AnimChangeState(JumperFooterAction.StateEnum.Hit1, 0.5f);
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
		if (deadFlag)
		{
			return;
		}
		R.Player.Action.QTEHPRecover();
		R.Audio.PlayEffect(Random.Range(105, 108), transform.position);
		R.Audio.PlayEffect(484, transform.position);
		R.Audio.PlayEffect(257, transform.position);
		deadFlag = true;
		eAttr.currentHp = 0;
		eAttr.inWeakState = false;
		eAttr.isFlyingUp = false;
		eAttr.checkHitGround = false;
		eAttr.stiffTime = 0f;
		eAttr.timeController.SetGravity(1f);
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
	}
}
