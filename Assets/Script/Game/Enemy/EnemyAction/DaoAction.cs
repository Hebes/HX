using UnityEngine;

public class DaoAction : EnemyBaseAction
{
	protected override void Start()
	{
		this.stateMachine.AddStates(DaoAction.State.StateArray);
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.stateMachine.OnTransfer += this.OnStateTransfer;
		base.AnimChangeState(DaoAction.StateEnum.Idle, 1f);
	}

	private void FixedUpdate()
	{
		if (this.stateMachine.currentState == DaoAction.State.HitToFly3)
		{
			Vector2 currentSpeed = this.eAttr.timeController.GetCurrentSpeed();
			float f = currentSpeed.x;
			f = Mathf.Clamp(Mathf.Abs(f) - this.airFric * Time.fixedDeltaTime, 0f, float.MaxValue) * Mathf.Sign(f);
			currentSpeed.x = Mathf.Clamp(Mathf.Abs(f) - this.airFric * Time.fixedDeltaTime, 0f, float.PositiveInfinity) * Mathf.Sign(f);
			this.eAttr.timeController.SetSpeed(currentSpeed);
		}
	}

	protected override void Update()
	{
		base.Update();
		if (this.stateMachine.currentState == DaoAction.State.HitToFly3)
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
		if (args.nextState != DaoAction.State.PaoAtk1ToIdle && this.isPao && this.gun != null)
		{
			this.gun.mode = SkeletonUtilityBone.Mode.Follow;
		}
		if (this.ExitAtkSta(args.lastState, args.nextState))
		{
			this.eAttr.paBody = false;
			this.atkBox.localScale = Vector3.zero;
		}
		if (args.nextState == "DaoAtk2")
		{
			base.GetComponent<DaoEnemyAnimListener>().Atk2Start();
		}
		if (this.EnterAtkSta(args.lastState, args.nextState))
		{
			base.GetComponentInChildren<EnemyAtk>().atkId = Incrementor.GetNextId();
		}
		if (args.nextState == DaoAction.State.HitToFly3)
		{
			Vector2 currentSpeed = this.eAttr.timeController.GetCurrentSpeed();
			currentSpeed.y = 0f;
			this.eAttr.timeController.SetSpeed(currentSpeed);
			this.eAttr.timeController.SetGravity(0f);
		}
		if (args.lastState == DaoAction.State.HitToFly3 && !this.eAttr.willBeExecute)
		{
			this.eAttr.timeController.SetGravity(1f);
		}
		if (args.nextState.IsInArray(DaoAction.NormalSta))
		{
			this.eAttr.timeController.SetGravity(1f);
		}
		if (args.nextState == DaoAction.State.Jump || args.nextState == DaoAction.State.PaoAtk3)
		{
			this.eAttr.timeController.SetSpeed(Vector2.zero);
			this.eAttr.timeController.SetGravity(0f);
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
			this.spineAnim.Play(state, false, true, this.eAttr.atkSpeed);
			break;
		case "DaoAtk2":
		case "DaoAtk6_2":
			this.spineAnim.Play(state, true, false, this.eAttr.atkSpeed);
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
			this.spineAnim.Play(state, false, true, 1f);
			break;
		case "Die":
		case "ExecuteDie":
		case "Die2":
		case "Null":
			this.spineAnim.Play(state, false, true, 1f);
			break;
		case "EMP":
		case "Fall":
		case "HitFall":
		case "AirDiePre":
		case "Idle":
		case "Move":
		case "DaoAtk2HitBack":
		case "HitToFly2":
			this.spineAnim.Play(state, true, false, 1f);
			break;
		}
	}

	public override bool IsInNormalState()
	{
		return this.stateMachine.currentState.IsInArray(DaoAction.NormalSta) && base.IsInNormalState();
	}

	public override bool IsInDeadState(string state)
	{
		return state.IsInArray(DaoAction.DeadSta);
	}

	public void RollToIdle()
	{
		base.AnimChangeState(DaoAction.StateEnum.DaoAtk2HitBack, 1f);
		base.GetComponent<EnemyDaoHurt>().SetHitSpeed(new Vector2((float)(this.eAttr.faceDir * -4), 15f));
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
		base.AnimChangeState(DaoAction.StateEnum.Jump, 1f);
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
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		this.eAttr.timeController.SetGravity(0f);
		base.AnimChangeState(DaoAction.StateEnum.JumpBack, 1f);
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
		base.AnimChangeState(DaoAction.StateEnum.DaoAtk1, 1f);
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
		base.AnimChangeState(DaoAction.StateEnum.DaoAtk2Ready, 1f);
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
		base.AnimChangeState(DaoAction.StateEnum.PaoAtk1, 1f);
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
		base.AnimChangeState(DaoAction.StateEnum.PaoAtk2, 1f);
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
		base.AnimChangeState(DaoAction.StateEnum.DaoAtk5, 1f);
	}

	public override void Attack9(int dir)
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
		base.AnimChangeState(DaoAction.StateEnum.PaoAtk3, 1f);
	}

	public override void Attack10(int dir)
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
		base.AnimChangeState(DaoAction.StateEnum.DaoAtk6_1, 1f);
	}

	public override void Idle1()
	{
		if (!this.IsInNormalState())
		{
			return;
		}
		base.AnimChangeState(DaoAction.StateEnum.Idle, 1f);
	}

	public override void Idle2()
	{
		if (!this.IsInNormalState())
		{
			return;
		}
		base.AnimChangeState(DaoAction.StateEnum.Idle2, 1f);
	}

	public override void Idle3()
	{
		if (!this.IsInNormalState())
		{
			return;
		}
		base.AnimChangeState(DaoAction.StateEnum.Idle3, 1f);
	}

	public override void AnimReady()
	{
		base.AnimChangeState(DaoAction.StateEnum.Idle, 1f);
	}

	public override void AnimMove()
	{
		base.AnimChangeState(DaoAction.StateEnum.Move, 1f);
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
		base.AnimChangeState((!this.isPao) ? DaoAction.StateEnum.JumpBack : DaoAction.StateEnum.RunAwayReady, 1f);
	}

	protected override bool EnterAtkSta(string lastState, string nextState)
	{
		return nextState.IsInArray(DaoAction.AttackSta) && !lastState.IsInArray(DaoAction.AttackSta);
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return !nextState.IsInArray(DaoAction.AttackSta) && lastState.IsInArray(DaoAction.AttackSta);
	}

	public override bool IsInAttackState()
	{
		return this.stateMachine.currentState.IsInArray(DaoAction.AttackSta);
	}

	public override bool IsInWeakSta()
	{
		return this.eAttr.inWeakState;
	}

	public override bool IsInIdle()
	{
		return this.stateMachine.currentState == "Idle" || this.stateMachine.currentState == "Idle2" || this.stateMachine.currentState == "Idle3";
	}

	public static string[] AttackSta = new string[]
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
		"DaoAtk6_2Ready"
	};

	public static string[] DeadSta = new string[]
	{
		"AirDiePre",
		"AirDie",
		"Die",
		"ExecuteDie",
		"Die2",
		"Null"
	};

	public static string[] HurtSta = new string[]
	{
		"DaoAtk2HitBack",
		"Fall",
		"FallHitGround",
		"GetUp",
		"Hit1",
		"HitToFly1",
		"HitToFly3",
		"AirFly",
		"AirFly2",
		"HitWall",
		"HitToFly2",
		"HitFall"
	};

	public static string[] NormalSta = new string[]
	{
		"Idle",
		"IDle2",
		"Idle3",
		"Move",
		"MoveSlow",
		"Jump"
	};

	public static string[] SideStepSta = new string[]
	{
		"RunAway",
		"RunAwayReady",
		"RunAwayEnd",
		"JumpBack"
	};

	[SerializeField]
	private SkeletonUtilityBone gun;

	public bool isPao;

	public float airFric = 8f;

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
		DaoAtk6_2Ready
	}

	public static class State
	{
		public static string[] StateArray = new string[]
		{
			"DaoAtk1",
			"DaoAtk2",
			"DaoAtk2Ready",
			"DaoAtk2HitBack",
			"PaoAtk1",
			"Die",
			"EMP",
			"Fall",
			"FallHitGround",
			"GetUp",
			"Hit1",
			"HitToFly1",
			"HitToFly3",
			"Idle",
			"Idle2",
			"Idle3",
			"Move",
			"HitGround",
			"AirFly",
			"AirFly2",
			"HitWall",
			"Atk1",
			"Atk1Ready",
			"AirDiePre",
			"AirDie",
			"PaoAtk1ToIdle",
			"HitToFly2",
			"Execute",
			"ExecuteDie",
			"Die2",
			"Null",
			"Jump",
			"JumpBack",
			"RunAway",
			"RunAwayReady",
			"RunAwayEnd",
			"PaoAtk2",
			"HitFall",
			"DaoAtk3",
			"DaoAtk4",
			"DaoAtk5",
			"MoveSlow",
			"PaoAtk3",
			"DaoAtk6_1",
			"DaoAtk6_2",
			"DaoAtk6_2Ready"
		};

		public static string DaoAtk1 = DaoAction.State.StateArray[0];

		public static string DaoAtk2 = DaoAction.State.StateArray[1];

		public static string DaoAtk2Ready = DaoAction.State.StateArray[2];

		public static string DaoAtk2HitBack = DaoAction.State.StateArray[3];

		public static string PaoAtk1 = DaoAction.State.StateArray[4];

		public static string Die = DaoAction.State.StateArray[5];

		public static string EMP = DaoAction.State.StateArray[6];

		public static string Fall = DaoAction.State.StateArray[7];

		public static string FallHitGround = DaoAction.State.StateArray[8];

		public static string GetUp = DaoAction.State.StateArray[9];

		public static string Hit1 = DaoAction.State.StateArray[10];

		public static string HitToFly1 = DaoAction.State.StateArray[11];

		public static string HitToFly3 = DaoAction.State.StateArray[12];

		public static string Idle = DaoAction.State.StateArray[13];

		public static string Idle2 = DaoAction.State.StateArray[14];

		public static string Idle3 = DaoAction.State.StateArray[15];

		public static string Move = DaoAction.State.StateArray[16];

		public static string HitGround = DaoAction.State.StateArray[17];

		public static string AirFly = DaoAction.State.StateArray[18];

		public static string AirFly2 = DaoAction.State.StateArray[19];

		public static string HitWall = DaoAction.State.StateArray[20];

		public static string Atk1 = DaoAction.State.StateArray[21];

		public static string Atk1Ready = DaoAction.State.StateArray[22];

		public static string AirDiePre = DaoAction.State.StateArray[23];

		public static string AirDie = DaoAction.State.StateArray[24];

		public static string PaoAtk1ToIdle = DaoAction.State.StateArray[25];

		public static string HitToFly2 = DaoAction.State.StateArray[26];

		public static string Execute = DaoAction.State.StateArray[27];

		public static string ExecuteDie = DaoAction.State.StateArray[28];

		public static string Die2 = DaoAction.State.StateArray[29];

		public static string Null = DaoAction.State.StateArray[30];

		public static string Jump = DaoAction.State.StateArray[31];

		public static string JumpBack = DaoAction.State.StateArray[32];

		public static string RunAway = DaoAction.State.StateArray[33];

		public static string RunAwayReady = DaoAction.State.StateArray[34];

		public static string RunAwayEnd = DaoAction.State.StateArray[35];

		public static string PaoAtk2 = DaoAction.State.StateArray[36];

		public static string HitFall = DaoAction.State.StateArray[37];

		public static string DaoAtk3 = DaoAction.State.StateArray[38];

		public static string DaoAtk4 = DaoAction.State.StateArray[39];

		public static string DaoAtk5 = DaoAction.State.StateArray[40];

		public static string MoveSlow = DaoAction.State.StateArray[41];

		public static string PaoAtk3 = DaoAction.State.StateArray[42];

		public static string DaoAtk6_1 = DaoAction.State.StateArray[43];

		public static string DaoAtk6_2 = DaoAction.State.StateArray[44];

		public static string DaoAtk6_2Ready = DaoAction.State.StateArray[45];
	}
}
