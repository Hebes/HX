using UnityEngine;

public class DaoPaoAction : EnemyBaseAction
{
	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(DaoPaoAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.stateMachine.OnTransfer += this.OnStateTransfer;
		base.AnimChangeState(DaoPaoAction.StateEnum.Idle, 1f);
	}

	private void FixedUpdate()
	{
		if (this.stateMachine.currentState == "HitToFly3")
		{
			Vector2 currentSpeed = this.eAttr.timeController.GetCurrentSpeed();
			float f = currentSpeed.x;
			f = Mathf.Clamp(Mathf.Abs(f) - this._airFric * Time.fixedDeltaTime, 0f, float.MaxValue) * Mathf.Sign(f);
			currentSpeed.x = Mathf.Clamp(Mathf.Abs(f) - this._airFric * Time.fixedDeltaTime, 0f, float.PositiveInfinity) * Mathf.Sign(f);
			this.eAttr.timeController.SetSpeed(currentSpeed);
		}
	}

	protected override void Update()
	{
		base.Update();
		if (this.stateMachine.currentState == "HitToFly3")
		{
			Vector2 currentSpeed = this.eAttr.timeController.GetCurrentSpeed();
			if (currentSpeed.y > 0f)
			{
				currentSpeed.y = 0f;
				this.eAttr.timeController.SetSpeed(currentSpeed);
			}
		}
	}

	private void OnStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		base.GetComponent<EnemyBaseHurt>().StopFollowLeftHand();
		this.atkBox.GetComponent<Collider2D>().enabled = true;
		if (this.ExitAtkSta(args.lastState, args.nextState))
		{
			this.eAttr.paBody = false;
			this.atkBox.localScale = Vector3.zero;
		}
		if (args.nextState == "DaoAtk2")
		{
			base.GetComponent<DaoPaoAnimEvent>().Atk2Start();
		}
		if (this.EnterAtkSta(args.lastState, args.nextState))
		{
			base.GetComponentInChildren<EnemyAtk>().atkId = Incrementor.GetNextId();
		}
		if (args.nextState == "HitToFly3")
		{
			Vector2 currentSpeed = this.eAttr.timeController.GetCurrentSpeed();
			currentSpeed.y = 0f;
			this.eAttr.timeController.SetSpeed(currentSpeed);
			this.eAttr.timeController.SetGravity(0f);
		}
		if (args.lastState == "HitToFly3" && !this.eAttr.willBeExecute)
		{
			this.eAttr.timeController.SetGravity(1f);
		}
		if (args.nextState.IsInArray(DaoPaoAction.NormalSta))
		{
			this.hurtBox.localScale = Vector3.one;
			this.eAttr.timeController.SetGravity(1f);
		}
		if (args.nextState == "Jump" || args.nextState == "PaoAtk3")
		{
			this.eAttr.timeController.SetSpeed(Vector2.zero);
			this.eAttr.timeController.SetGravity(0f);
			this.hurtBox.localScale = Vector3.one;
		}
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		string state = args.state;
		switch (state)
		{
		case "Atk1":
		case "Atk1Ready":
		case "DaoAtk1":
		case "DaoAtk6_1":
		case "DaoAtk2Ready":
		case "DaoAtk6_2Ready":
		case "PaoAtk1":
		case "PaoAtk2":
		case "DaoAtk3":
		case "DaoAtk4":
		case "DaoAtk5":
		case "PaoAtk3":
		case "DaoPaoAtk1":
			this.spineAnim.Play(args.state, false, true, this.eAttr.atkSpeed);
			break;
		case "DaoAtk2":
		case "DaoAtk6_2":
			this.spineAnim.Play(args.state, true, false, this.eAttr.atkSpeed);
			break;
		case "FallHitGround":
		case "GetUp":
		case "Hit1":
		case "HitToFly1":
		case "HitToFly3":
		case "AirDie":
		case "HitGround":
		case "AirFly":
		case "AirFly2":
		case "HitWall":
		case "Idle2":
		case "Idle3":
		case "PaoAtk1ToIdle":
		case "Execute":
		case "Jump":
		case "JumpBack":
		case "RunAway":
		case "RunAwayReady":
		case "RunAwayEnd":
		case "MoveSlow":
			this.spineAnim.Play(args.state, false, true, 1f);
			break;
		case "Die":
		case "ExecuteDie":
		case "Die2":
		case "Null":
			this.spineAnim.Play(args.state, false, true, 1f);
			break;
		case "EMP":
		case "Fall":
		case "HitFall":
		case "AirDiePre":
		case "Idle":
		case "Move":
		case "DaoAtk2HitBack":
		case "HitToFly2":
			this.spineAnim.Play(args.state, true, false, 1f);
			break;
		}
	}

	public override bool IsInNormalState()
	{
		return this.stateMachine.currentState.IsInArray(DaoPaoAction.NormalSta) && base.IsInNormalState();
	}

	public override bool IsInDeadState(string state)
	{
		return state.IsInArray(DaoPaoAction.DeadSta);
	}

	public void RollToIdle()
	{
		base.AnimChangeState("DaoAtk2HitBack", 1f);
		base.GetComponent<EnemyBaseHurt>().SetHitSpeed(new Vector2((float)(this.eAttr.faceDir * -4), 15f));
	}

	public void Jump()
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		base.AnimChangeState(DaoPaoAction.StateEnum.Jump, 1f);
	}

	public void JumpBack()
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		this.eAttr.timeController.SetGravity(0f);
		base.AnimChangeState(DaoPaoAction.StateEnum.JumpBack, 1f);
	}

	public override void Idle2()
	{
		if (!this.IsInNormalState())
		{
			return;
		}
		base.AnimChangeState(DaoPaoAction.StateEnum.Idle2, 1f);
	}

	public override void Idle3()
	{
		if (!this.IsInNormalState())
		{
			return;
		}
		base.AnimChangeState(DaoPaoAction.StateEnum.Idle3, 1f);
	}

	public override void Attack1(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		base.ChangeFace(dir);
		base.AnimChangeState(DaoPaoAction.StateEnum.DaoAtk1, 1f);
	}

	public override void Attack2(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		base.ChangeFace(dir);
		base.AnimChangeState(DaoPaoAction.StateEnum.DaoAtk2Ready, 1f);
	}

	public override void Attack3(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		base.ChangeFace(dir);
		base.AnimChangeState(DaoPaoAction.StateEnum.PaoAtk1, 1f);
	}

	public override void Attack5(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		base.ChangeFace(dir);
		base.AnimChangeState(DaoPaoAction.StateEnum.PaoAtk2, 1f);
	}

	public override void Attack8(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		base.ChangeFace(dir);
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		this.eAttr.timeController.SetGravity(0f);
		base.AnimChangeState(DaoPaoAction.StateEnum.DaoAtk5, 1f);
	}

	public override void Attack11(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		base.ChangeFace(dir);
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		this.eAttr.timeController.SetGravity(0f);
		base.AnimChangeState(DaoPaoAction.StateEnum.DaoPaoAtk1, 1f);
	}

	public override void AnimMove()
	{
		base.AnimChangeState(DaoPaoAction.StateEnum.Move, 1f);
	}

	public override void AnimReady()
	{
		base.AnimChangeState(DaoPaoAction.StateEnum.Idle, 1f);
	}

	public override void SideStep()
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (this.IsInSideStepState())
		{
			return;
		}
		if (!this.eAttr.isOnGround)
		{
			return;
		}
		base.SideStep();
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		base.AnimChangeState(DaoPaoAction.StateEnum.JumpBack, 1f);
	}

	protected override bool EnterAtkSta(string lastState, string nextState)
	{
		return nextState.IsInArray(DaoPaoAction.AttackSta) && !lastState.IsInArray(DaoPaoAction.AttackSta);
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return !nextState.IsInArray(DaoPaoAction.AttackSta) && lastState.IsInArray(DaoPaoAction.AttackSta);
	}

	public override bool IsInAttackState()
	{
		return this.stateMachine.currentState.IsInArray(DaoPaoAction.AttackSta);
	}

	public override bool IsInWeakSta()
	{
		return this.eAttr.inWeakState;
	}

	public override bool IsInIdle()
	{
		return this.stateMachine.currentState == "Idle" || this.stateMachine.currentState == "Idle2" || this.stateMachine.currentState == "Idle3";
	}

	public static readonly string[] AttackSta = new string[]
	{
		"DaoAtk1",
		"DaoAtk2",
		"DaoAtk2Ready",
		"PaoAtk1",
		"Atk1",
		"Atk1Ready",
		"PaoAtk2",
		"DaoAtk3",
		"DaoAtk4",
		"DaoAtk5",
		"PaoAtk3",
		"DaoAtk6_1",
		"DaoAtk6_2",
		"DaoAtk6_2Ready",
		"DaoPaoAtk1"
	};

	public static readonly string[] DeadSta = new string[]
	{
		"AirDiePre",
		"AirDie",
		"Die",
		"ExecuteDie",
		"Die2",
		"Null"
	};

	public static readonly string[] NormalSta = new string[]
	{
		"Idle",
		"IDle2",
		"Idle3",
		"Move",
		"MoveSlow",
		"Jump"
	};

	private readonly float _airFric = 8f;

	[SerializeField]
	private SkeletonUtilityBone gun;

	public enum StateEnum
	{
		DaoAtk1,
		DaoAtk2,
		DaoAtk2Ready,
		DaoAtk2HitBack,
		PaoAtk1,
		Die,
		EMP,
		Fall,
		FallHitGround,
		GetUp,
		Hit1,
		HitToFly1,
		HitToFly3,
		Idle,
		Idle2,
		Idle3,
		Move,
		HitGround,
		AirFly,
		AirFly2,
		HitWall,
		Atk1,
		Atk1Ready,
		AirDiePre,
		AirDie,
		PaoAtk1ToIdle,
		HitToFly2,
		Execute,
		ExecuteDie,
		Die2,
		Null,
		Jump,
		JumpBack,
		RunAway,
		RunAwayReady,
		RunAwayEnd,
		PaoAtk2,
		HitFall,
		DaoAtk3,
		DaoAtk4,
		DaoAtk5,
		MoveSlow,
		PaoAtk3,
		DaoAtk6_1,
		DaoAtk6_2,
		DaoAtk6_2Ready,
		DaoPaoAtk1
	}
}
