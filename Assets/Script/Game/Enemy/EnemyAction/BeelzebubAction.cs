using UnityEngine;

/// <summary>
/// 恶魔行动
/// </summary>
public class BeelzebubAction : EnemyBaseAction
{
	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(BeelzebubAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.stateMachine.OnTransfer += this.OnStateTransfer;
		base.AnimChangeState(BeelzebubAction.StateEnum.Idle, 1f);
		this._beelzebubAnimListener = base.GetComponent<BeelzebubAnimListener>();
	}

	protected override void Update()
	{
		base.Update();
		if (this.EatSuccess)
		{
			base.player.position = new Vector3(base.transform.position.x, base.transform.position.y, base.player.position.z);
		}
		if (this.SawSuccess)
		{
			base.player.position = new Vector3(this.sawPos.position.x, this.sawPos.position.y, base.player.position.z) - Vector3.up;
		}
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		string state = args.state;
		switch (state)
		{
		case "Atk1":
		case "Atk1Eat":
		case "Atk1Fail":
		case "Atk1Success":
		case "Atk1SuccessEnd":
		case "Atk2End":
		case "Atk2Ready":
		case "Atk3":
		case "Atk4":
		case "Angry":
			this.spineAnim.Play(args.state, false, true, this.eAttr.atkSpeed);
			break;
		case "Die":
		case "DieQTE":
		case "Hit":
		case "HitQTE":
		case "Die2":
		case "CallEnemy":
		case "Idle2":
			this.spineAnim.Play(args.state, false, true, 1f);
			break;
		case "Idle":
		case "Move":
		case "WeakMod":
		case "Atk2Hit":
		case "Atk2":
			this.spineAnim.Play(args.state, true, false, 1f);
			break;
		}
	}

	private void OnStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		base.GetComponent<EnemyBaseHurt>().StopFollowLeftHand();
		if (args.nextState == "Atk2End" || args.nextState == "Die" || args.nextState == "Atk2Hit" || args.nextState == "Idle" || args.nextState == "Hit")
		{
			this._beelzebubAnimListener.LSawUpper2.gameObject.SetActive(false);
		}
		if (args.nextState == "Atk2Hit")
		{
			this.LArm.localPosition = new Vector3(0f, 0f, -1f);
		}
		if (args.lastState == "Atk2Hit")
		{
			this.LArm.localPosition = Vector3.zero;
			this._beelzebubAnimListener.BeelzebubATK2.gameObject.SetActive(false);
		}
		if (!args.nextState.IsInArray(BeelzebubAction.AttackSta))
		{
			if (this.EatSuccess || this.SawSuccess)
			{
				this.Angry = true;
			}
			else
			{
				this.Angry = false;
			}
			if (this.EatSuccess)
			{
				this._beelzebubAnimListener.EatAtkSuccess();
			}
			if (this.SawSuccess)
			{
				this._beelzebubAnimListener.SawAttackFinish();
			}
		}
		if (this.ExitAtkSta(args.lastState, args.nextState))
		{
			this.eAttr.paBody = false;
			this.atkBox.localScale = Vector3.zero;
		}
		if (this.EnterAtkSta(args.lastState, args.nextState))
		{
			base.GetComponentInChildren<EnemyAtk>().atkId = Incrementor.GetNextId();
		}
	}

	public void EatAttack()
	{
		this.EatSuccess = true;
		GameObject prefab = CameraEffectProxyPrefabData.GetPrefab(7);
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, base.transform.position + Vector3.up * 2f, Quaternion.identity);
		gameObject.transform.parent = base.transform;
	}

	public void SawAttack()
	{
		this.SawSuccess = true;
		base.AnimChangeState(BeelzebubAction.StateEnum.Atk2Hit, 1f);
		R.Camera.Controller.CameraShake(2f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
		R.Camera.Controller.OpenMotionBlur(2f, 0.1f, base.transform.position + Vector3.up * 2f);
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
		this.eAttr.paBody = true;
		base.AnimChangeState(BeelzebubAction.StateEnum.Atk1, 1f);
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
		this._beelzebubAnimListener.Atk2MoveLoopTime = 1;
		base.AnimChangeState(BeelzebubAction.StateEnum.Atk2Ready, 1f);
	}

	public override void Attack4(int dir)
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
		this.eAttr.paBody = true;
		base.AnimChangeState(BeelzebubAction.StateEnum.Atk4, 1f);
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
		this.eAttr.paBody = true;
		base.AnimChangeState(BeelzebubAction.StateEnum.Angry, 1f);
	}

	public override void CounterAttack(int dir)
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (this.IsInAttackState())
		{
			return;
		}
		if (this.stateMachine.currentState.IsInArray(BeelzebubAction.QTESta) || this.IsInWeakSta())
		{
			return;
		}
		R.Effect.Generate(128, base.transform, Vector3.up * 4f, Vector3.zero, default(Vector3), true);
		base.ChangeFace(dir);
		this.eAttr.paBody = true;
		int num = UnityEngine.Random.Range(0, 100);
		base.AnimChangeState((num > 60) ? BeelzebubAction.StateEnum.Angry : BeelzebubAction.StateEnum.Atk2Ready, 1f);
	}

	public override void KillSelf()
	{
		if (this.eAttr.rankType == EnemyAttribute.RankType.BOSS)
		{
			return;
		}
		SingletonMono<WorldTime>.Instance.TimeFrozenByFixedFrame(25, WorldTime.FrozenArgs.FrozenType.Enemy, true);
		R.Camera.Controller.CameraShake(0.416666657f, 0.3f, CameraController.ShakeTypeEnum.Rect, false);
		base.GetComponent<BeelzebubHurt>().NormalKill();
		this.eAttr.currentHp = 0;
		BeelzebubAction.StateEnum stateEnum = (UnityEngine.Random.Range(0, 2) != 0) ? BeelzebubAction.StateEnum.Die2 : BeelzebubAction.StateEnum.Die;
		base.AnimChangeState(stateEnum, 1f);
	}

	public override void Idle1()
	{
		if (!this.IsInNormalState())
		{
			return;
		}
		base.AnimChangeState(BeelzebubAction.StateEnum.Idle, 1f);
	}

	public override bool IsInNormalState()
	{
		return this.stateMachine.currentState.IsInArray(BeelzebubAction.NormalSta) && base.IsInNormalState();
	}

	public override bool IsInDeadState(string state)
	{
		return state.IsInArray(BeelzebubAction.DieSta);
	}

	public override void AnimReady()
	{
		base.AnimChangeState(BeelzebubAction.StateEnum.Idle, 1f);
	}

	public override void AnimMove()
	{
		base.AnimChangeState(BeelzebubAction.StateEnum.Move, 1f);
	}

	public override void Idle2()
	{
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		base.AnimChangeState(BeelzebubAction.StateEnum.Idle2, 1f);
	}

	public override bool IsInWeakSta()
	{
		return this.eAttr.inWeakState;
	}

	public override bool IsInAttackState()
	{
		return this.stateMachine.currentState.IsInArray(BeelzebubAction.AttackSta);
	}

	protected override bool EnterAtkSta(string lastState, string nextState)
	{
		return nextState.IsInArray(BeelzebubAction.AttackSta) && !lastState.IsInArray(BeelzebubAction.AttackSta);
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return !nextState.IsInArray(BeelzebubAction.AttackSta) && lastState.IsInArray(BeelzebubAction.AttackSta);
	}

	public override void AnimQTEHurt()
	{
		base.AnimQTEHurt();
		int dir = (base.player.transform.localScale.x >= 0f) ? 1 : -1;
		base.ChangeFace(dir);
		base.ExitWeakState(true);
		Vector3 position = base.transform.position;
		position.z = LayerManager.ZNum.TempEnemy;
		base.transform.position = position;
		base.AnimChangeState(BeelzebubAction.StateEnum.HitQTE, 1f);
		Transform transform = R.Effect.Generate(179, null, position, default(Vector3), default(Vector3), true);
		transform.localScale = base.transform.localScale;
	}

	public override void AnimExecute()
	{
		base.AnimExecute();
		int dir = (base.player.transform.localScale.x >= 0f) ? 1 : -1;
		base.ChangeFace(dir);
		base.ExitWeakState(true);
		Vector3 position = base.transform.position;
		position.z = LayerManager.ZNum.TempEnemy;
		base.transform.position = position;
		base.AnimChangeState(BeelzebubAction.StateEnum.DieQTE, 1f);
	}

	public override bool IsInIdle()
	{
		return this.stateMachine.currentState == "Idle";
	}

	public static readonly string[] AttackSta = new string[]
	{
		"Atk1",
		"Atk1Eat",
		"Atk1Fail",
		"Atk1Success",
		"Atk1SuccessEnd",
		"Atk2",
		"Atk2End",
		"Atk2Ready",
		"Atk3",
		"Atk2Hit",
		"Atk4",
		"Angry"
	};

	public static readonly string[] HurtSta = new string[]
	{
		"Hit",
		"HItQTE"
	};

	private static readonly string[] DieSta = new string[]
	{
		"Die",
		"Die2",
		"DieQTE"
	};

	private static readonly string[] NormalSta = new string[]
	{
		"Idle",
		"Move",
		"CallEnemy",
		"Idle2"
	};

	private static readonly string[] QTESta = new string[]
	{
		"HItQTE",
		"DieQTE"
	};

	public bool EatSuccess;

	public bool SawSuccess;

	public bool Angry;

	private BeelzebubAnimListener _beelzebubAnimListener;

	[SerializeField]
	private Transform sawPos;

	[SerializeField]
	private Transform LArm;

	public enum StateEnum
	{
		Atk1,
		Atk1Eat,
		Atk1Fail,
		Atk1Success,
		Atk1SuccessEnd,
		Atk2,
		Atk2End,
		Atk2Ready,
		Atk3,
		Atk4,
		Die,
		Hit,
		Idle,
		Move,
		WeakMod,
		Atk2Hit,
		Die2,
		Angry,
		CallEnemy,
		Idle2,
		DieQTE,
		HitQTE
	}
}
