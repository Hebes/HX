using UnityEngine;

/// <summary>
/// 炸弹杀手动作
/// </summary>
public class BombKillerAction : EnemyBaseAction
{
	protected override void Start()
	{
		stateMachine.AddStates(typeof(StateEnum));
		stateMachine.OnEnter += OnMyStateEnter;
		stateMachine.OnTransfer += OnStateTransfer;
		AnimChangeState(StateEnum.Idle);
	}

	private void FixedUpdate()
	{
		if (stateMachine.currentState == "FlyToFall")
		{
			Vector2 currentSpeed = eAttr.timeController.GetCurrentSpeed();
			float f = currentSpeed.x;
			f = Mathf.Clamp(Mathf.Abs(f) - airFric * Time.fixedDeltaTime, 0f, float.MaxValue) * Mathf.Sign(f);
			currentSpeed.x = Mathf.Clamp(Mathf.Abs(f) - airFric * Time.fixedDeltaTime, 0f, float.PositiveInfinity) * Mathf.Sign(f);
			eAttr.timeController.SetSpeed(currentSpeed);
		}
	}

	protected override void Update()
	{
		base.Update();
		if (atk1Success)
		{
			Vector3 position = player.position;
			position.x = transform.position.x;
			player.position = position;
		}
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		StateEnum stateEnum = EnumTools.ToEnum<StateEnum>(args.state);
		switch (stateEnum)
		{
		case StateEnum.Atk1Ready:
		case StateEnum.Atk1Success:
		case StateEnum.Atk1Fail:
		case StateEnum.Atk2:
		case StateEnum.Die:
		case StateEnum.ExecuteDie:
		case StateEnum.FlyToFall:
		case StateEnum.GetUp:
		case StateEnum.Hit1:
		case StateEnum.Hit2:
		case StateEnum.HitGround:
		case StateEnum.HitToFly1:
		case StateEnum.AirDie:
		case StateEnum.AirDieFlyToFall:
		case StateEnum.AirDieHitGround:
		case StateEnum.Null:
			spineAnim.Play(stateEnum, false, true);
			break;
		case StateEnum.Execute:
		case StateEnum.Fall:
		case StateEnum.HitToFly2:
		case StateEnum.Idle:
		case StateEnum.Move:
		case StateEnum.AirDieFall:
		case StateEnum.HitFall:
			spineAnim.Play(stateEnum, true);
			break;
		}
	}

	private void OnStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		GetComponent<EnemyBaseHurt>().StopFollowLeftHand();
		if (args.nextState == "FlyToFall")
		{
			Vector2 currentSpeed = eAttr.timeController.GetCurrentSpeed();
			currentSpeed.y = 0f;
			eAttr.timeController.SetSpeed(currentSpeed);
			eAttr.timeController.SetGravity(0f);
		}
		if (args.lastState == "FlyToFall" && !eAttr.willBeExecute)
		{
			eAttr.timeController.SetGravity(1f);
		}
		if (ExitAtkSta(args.lastState, args.nextState) && atk1Success)
		{
			atk1Success = false;
			GetComponent<BombKillerAnimEvent>().GenerateExplosion_Atk1();
		}
	}

	public override void Attack1(int dir)
	{
		if (eAttr.isDead)
		{
			return;
		}
		if (!IsInNormalState())
		{
			return;
		}
		ChangeFace(dir);
		AnimChangeState(StateEnum.Atk1Ready);
	}

	public void Atk1Success()
	{
		Vector3 position = transform.position;
		position.z = player.position.z - 0.01f;
		transform.position = position;
		atk1Success = true;
	}

	public override void Attack2(int dir)
	{
		if (eAttr.isDead)
		{
			return;
		}
		if (!IsInNormalState())
		{
			return;
		}
		ChangeFace(dir);
		AnimChangeState(StateEnum.Atk2);
	}

	public override bool IsInIdle()
	{
		return stateMachine.currentState == "Idle";
	}

	public override bool IsInNormalState()
	{
		return EnumTools.IsInEnum<NormalSta>(stateMachine.currentState) && base.IsInNormalState();
	}

	public override void AnimMove()
	{
		AnimChangeState(StateEnum.Move);
	}

	public override void AnimReady()
	{
		AnimChangeState(StateEnum.Idle);
	}

	public override bool IsInAttackState()
	{
		return EnumTools.IsInEnum<AttackSta>(stateMachine.currentState);
	}

	public override bool IsInDeadState(string state)
	{
		return EnumTools.IsInEnum<DieSta>(state);
	}

	public override bool IsInWeakSta()
	{
		return eAttr.inWeakState;
	}

	protected override bool EnterAtkSta(string lastState, string nextState)
	{
		return EnumTools.IsInEnum<AttackSta>(nextState) && !EnumTools.IsInEnum<AttackSta>(lastState);
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return !EnumTools.IsInEnum<AttackSta>(nextState) && EnumTools.IsInEnum<AttackSta>(lastState);
	}

	public bool atk1Success;

	public float airFric = 8f;

	public enum StateEnum
	{
		Atk1Ready,
		Atk1Success,
		Atk1Fail,
		Atk2,
		Die,
		Execute,
		ExecuteDie,
		Fall,
		FlyToFall,
		GetUp,
		Hit1,
		Hit2,
		HitGround,
		HitToFly1,
		HitToFly2,
		Idle,
		Move,
		AirDie,
		AirDieFlyToFall,
		AirDieFall,
		AirDieHitGround,
		HitFall,
		Null
	}

	public enum AttackSta
	{
		Atk1Ready,
		Atk1Success,
		Atk1Fail,
		Atk2
	}

	public enum DieSta
	{
		Die,
		Execute,
		ExecuteDie,
		AirDie,
		AirFlyToFall,
		AirDieFall,
		AirDieHitGround,
		Null
	}

	public enum HurtSta
	{
		Fall,
		FlyToFall,
		GetUp,
		Hit1,
		Hit2,
		HitGround,
		HitToFly1,
		HitToFly2,
		HitFall
	}

	public enum NormalSta
	{
		Idle,
		Move
	}
}
