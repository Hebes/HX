using UnityEngine;

public class JumperAction : EnemyBaseAction
{
	protected override void Start()
	{
		stateMachine.AddStates(typeof(StateEnum));
		stateMachine.OnEnter += OnMyStateEnter;
		stateMachine.OnTransfer += OnStateTransfer;
		AnimChangeState(StateEnum.Ready);
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
		if (catchFollow)
		{
			player.position = new Vector3(catachPos.position.x, catachPos.position.y, player.position.z);
			Vector3 eulerAngles = catachPos.localRotation.eulerAngles;
			player.localRotation = Quaternion.Euler(new Vector3(eulerAngles.x, eulerAngles.y, eAttr.faceDir * -eulerAngles.z));
		}
		if (IsInDefenceState() && Time.time - startDefenceTime >= defenceTime)
		{
			AnimChangeState(StateEnum.DefenseToIdle);
		}
		if (stateMachine.currentState == "FlyToFall")
		{
			Vector2 currentSpeed = eAttr.timeController.GetCurrentSpeed();
			if (currentSpeed.y > 0f)
			{
				currentSpeed.y = 0f;
				eAttr.timeController.SetSpeed(currentSpeed);
			}
		}
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		string state = args.state;
		switch (state)
		{
		case "Atk1":
		case "Atk2":
		case "Atk3":
		case "Atk3Fail":
		case "Atk3Success":
		case "Atk4":
		case "Atk5":
		case "Atk5_1":
		case "Atk5_2":
			spineAnim.Play(args.state, false, true, eAttr.atkSpeed);
			break;
		case "Die":
		case "AirDie":
		case "AirDiePre":
		case "FlyToFall":
		case "GetUp":
		case "Hit1":
		case "Hit2":
		case "HitGround":
		case "HitToFly":
		case "Execute":
		case "ExecuteDie":
		case "Die2":
		case "Die3":
		case "DefenseToIdle":
			spineAnim.Play(args.state, false, true);
			break;
		case "Move":
		case "Ready":
		case "Fall":
		case "DieFall":
		case "HitToFly2":
		case "WeakMod":
		case "Defense":
		case "HitFall":
			spineAnim.Play(args.state, true);
			break;
		}
	}

	private void OnStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		GetComponent<EnemyBaseHurt>().StopFollowLeftHand();
		if (!args.nextState.IsInArray(AttackSta))
		{
			if (catchFollow)
			{
				catchResult = true;
				GetComponent<JumperAnimListener>().Atk3Success();
			}
			else
			{
				catchResult = false;
			}
		}
		if (ExitAtkSta(args.lastState, args.nextState))
		{
			eAttr.paBody = false;
			atkBox.localScale = Vector3.zero;
		}
		if (EnterAtkSta(args.lastState, args.nextState))
		{
			GetComponentInChildren<EnemyAtk>().atkId = Incrementor.GetNextId();
		}
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
	}

	public void CatchSuccess()
	{
		GameObject prefab = CameraEffectProxyPrefabData.GetPrefab(14);
		Instantiate(prefab, player.position, Quaternion.identity);
		catchFollow = true;
	}

	public override bool IsInNormalState()
	{
		return stateMachine.currentState.IsInArray(NormalSta) && base.IsInNormalState();
	}

	public override bool IsInDeadState(string state)
	{
		return state.IsInArray(DieSta);
	}

	public override void AnimReady()
	{
		AnimChangeState(StateEnum.Ready);
	}

	public override void AnimMove()
	{
		AnimChangeState(StateEnum.Move);
	}

	public override bool IsInAttackState()
	{
		return stateMachine.currentState.IsInArray(AttackSta);
	}

	protected override bool EnterAtkSta(string lastState, string nextState)
	{
		return !lastState.IsInArray(AttackSta) && nextState.IsInArray(AttackSta);
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return lastState.IsInArray(AttackSta) && !nextState.IsInArray(AttackSta);
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
		AnimChangeState(StateEnum.Atk1);
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

	public override void Attack3(int dir)
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
		AnimChangeState(StateEnum.Atk3);
	}

	public override void Attack4(int dir)
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
		AnimChangeState(StateEnum.Atk4);
	}

	public override void Attack5(int dir)
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
		AnimChangeState(StateEnum.Atk5_1);
	}

	public override void CounterAttack(int dir)
	{
		if (eAttr.isDead)
		{
			return;
		}
		if (IsInAttackState())
		{
			return;
		}
		R.Effect.Generate(128, transform, Vector3.up * 1.5f, Vector3.zero);
		ChangeFace(dir);
		AnimChangeState(StateEnum.Atk3);
	}

	public override void Defence()
	{
		if (eAttr.isDead)
		{
			return;
		}
		if (IsInDefenceState())
		{
			return;
		}
		if (IsInWeakSta())
		{
			return;
		}
		if (!eAttr.isOnGround)
		{
			return;
		}
		base.Defence();
		eAttr.timeController.SetSpeed(Vector2.zero);
		startDefenceTime = Time.time;
		defenceTime = Random.Range(2f, 4f);
		AnimChangeState(StateEnum.Defense);
	}

	public override void DefenceSuccess()
	{
		if (eAttr.isDead)
		{
			return;
		}
		if (!eAttr.isOnGround)
		{
			return;
		}
		if (IsInWeakSta())
		{
			return;
		}
		R.Audio.PlayEffect(406, transform.position);
		AnimChangeState(StateEnum.Defense);
	}

	public override bool IsInWeakSta()
	{
		return eAttr.inWeakState;
	}

	public override bool IsInDefenceState()
	{
		return stateMachine.currentState.IsInArray(DefenceSta);
	}

	[SerializeField]
	private Transform catachPos;

	public bool catchFollow;

	public bool catchResult;

	private static readonly string[] NormalSta = {
		"Atk3Fail",
		"Move",
		"Ready"
	};

	public static readonly string[] AttackSta = {
		"Atk1",
		"Atk2",
		"Atk3",
		"Atk4",
		"Atk5",
		"Atk3Success",
		"Atk3Fail",
		"Atk5_1",
		"Atk5_2"
	};

	public static readonly string[] HurtSta = {
		"FlyToFall",
		"GetUp",
		"Hit1",
		"Hit2",
		"Fall",
		"HitGround",
		"HitToFly",
		"HitToFly2",
		"HitFall"
	};

	private static readonly string[] DieSta = {
		"Die",
		"AirDiePre",
		"DieFall",
		"AirDie",
		"ExecuteDie",
		"Die2",
		"Die3"
	};

	private static readonly string[] DefenceSta = {
		"Defense",
		"DefenseHit"
	};

	public float airFric = 8f;

	public enum StateEnum
	{
		Atk1,
		Atk2,
		Atk3,
		Atk4,
		Atk5,
		Atk3Fail,
		Atk3Success,
		Die,
		FlyToFall,
		GetUp,
		Hit1,
		Hit2,
		HitGround,
		HitToFly,
		Move,
		Ready,
		Fall,
		HitToFly2,
		AirDiePre,
		DieFall,
		AirDie,
		WeakMod,
		Execute,
		ExecuteDie,
		Atk5_1,
		Atk5_2,
		Die2,
		Die3,
		Defense,
		DefenseHit,
		DefenseToIdle,
		HitFall
	}
}
