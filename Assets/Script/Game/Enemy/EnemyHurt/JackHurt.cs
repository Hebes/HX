using Framework.Core;
using UnityEngine;

public class JackHurt : EnemyBaseHurt
{
	protected override void Update()
	{
		base.Update();
		Vector2? atkFollowPos = this._atkFollowPos;
		if (atkFollowPos != null)
		{
			Vector3 position = base.player.transform.position;
			Vector2? atkFollowPos2 = this._atkFollowPos;
			Vector3 position2 = position - ((atkFollowPos2 == null) ? default(Vector3) : (Vector3)atkFollowPos2.GetValueOrDefault());
			position2.y = Mathf.Clamp(position2.y, LayerManager.YNum.GetGroundHeight(base.gameObject) + 0.2f, float.PositiveInfinity);
			position2.z = LayerManager.ZNum.MMiddleE(this.eAttr.rankType);
			base.transform.position = position2;
			this._atkFollowTime += Time.unscaledDeltaTime;
			if (this._atkFollowTime >= this._atkFollowEnd)
			{
				this._atkFollowPos = null;
			}
		}
	}

	protected override void Init()
	{
		this._anim = base.GetComponent<JackAnimEvent>();
		this.defaultAnimName = "Hit1";
		this.defaultAirAnimName = "HitToFly2";
		this.hurtData = SingletonMono<EnemyDataPreload>.Instance.hurt[EnemyType.杰克];
	}

	public override void SetHitSpeed(Vector2 speed)
	{
		if (this.playerAtkName == "UpRising" || this.playerAtkName == "AtkUpRising" || this.playerAtkName == "AtkRollEnd" || this.playerAtkName == "NewExecute2_1")
		{
			this._anim.maxFlyHeight = 4.5f;
		}
		else
		{
			this._anim.maxFlyHeight = -1f;
		}
		base.SetHitSpeed(speed);
	}

	protected override void HitIntoWeakState(Vector2 speed, Vector2 airSpeed, string normalAtkType, string airAtkType)
	{
		normalAtkType = "IdleToWeakMod";
		airAtkType = "FlyToFall";
		if (this.action.IsInWeakSta())
		{
			return;
		}
		if (this.action.stateMachine.currentState == "HitQTE")
		{
			return;
		}
		base.HitIntoWeakState(speed, airSpeed, normalAtkType, airAtkType);
	}

	protected override void SpAttack()
	{
		if (this.eAttr.currentActionInterruptPoint < this.eAttr.actionInterruptPoint)
		{
			return;
		}
		if (this.playerAtkName == "RollEnd")
		{
			Vector2? atkFollowPos = this._atkFollowPos;
			if (atkFollowPos != null)
			{
				this._atkFollowPos = null;
			}
			return;
		}
		if (this.playerAtkName == "RollGround")
		{
			Vector2? atkFollowPos2 = this._atkFollowPos;
			if (atkFollowPos2 == null)
			{
				Transform transform = base.player.GetComponentInChildren<PlayerAtk>().transform;
				this._atkFollowPos = new Vector2?((transform.position - base.transform.position) * 0.9f);
			}
			this._atkFollowTime = 0f;
			this._atkFollowEnd = 0.2f;
			return;
		}
		if (this.playerAtkName == "RollReady")
		{
			Vector2? atkFollowPos3 = this._atkFollowPos;
			if (atkFollowPos3 == null)
			{
				this._atkFollowPos = new Vector2?((base.player.transform.position - base.transform.position) * 0.7f);
			}
			this._atkFollowTime = 0f;
			this._atkFollowEnd = 0.2f;
			return;
		}
		this._atkFollowTime = 0f;
		this._atkFollowEnd = 0f;
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
		this.action.AnimChangeState(HammerAction.StateEnum.Hit1, 0.5f);
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
		if (this.eAttr.rankType == EnemyAttribute.RankType.BOSS)
		{
			R.Trophy.AwardTrophy(29);
		}
		R.Audio.PlayEffect(412, new Vector3?(base.transform.position));
		R.Player.Action.QTEHPRecover(this.eAttr.rankType == EnemyAttribute.RankType.BOSS);
		R.Audio.PlayEffect(UnityEngine.Random.Range(105, 108), new Vector3?(base.transform.position));
		this.deadFlag = true;
		this.eAttr.currentHp = 0;
		this.eAttr.inWeakState = false;
		this.eAttr.isFlyingUp = false;
		this.eAttr.checkHitGround = false;
		this.eAttr.stiffTime = 0f;
		this.eAttr.timeController.SetGravity(1f);
		base.QTEZPositionRecover();
		EGameEvent.EnemyKilled.Trigger(eAttr);
		this.action.WeakEffectDisappear("Null");
		R.Effect.Generate(91, null, base.transform.position + new Vector3(0f, 1.2f, LayerManager.ZNum.Fx), Vector3.zero, default(Vector3), true);
		R.Effect.Generate(49, base.transform, default(Vector3), default(Vector3), default(Vector3), true);
		R.Effect.Generate(14, null, base.transform.position + new Vector3(0f, 1.2f, LayerManager.ZNum.Fx), default(Vector3), default(Vector3), true);
		base.AddCoinAndExp();
		R.Effect.Generate(213, null, default(Vector3), default(Vector3), default(Vector3), true);
		R.Effect.Generate(214, null, default(Vector3), default(Vector3), default(Vector3), true);
		SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(45, 0.2f);
		R.Camera.Controller.CameraShake(0.9166667f, 0.3f, CameraController.ShakeTypeEnum.Rect, false);
		this.action.hurtBox.gameObject.SetActive(false);
		this.eAttr.inWeakState = false;
	}

	private JackAnimEvent _anim;

	private Vector2? _atkFollowPos;

	private float _atkFollowTime;

	private float _atkFollowEnd;

	[SerializeField]
	private SkeletonAnimation closeToPlayer;
}
