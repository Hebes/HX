using System.Collections.Generic;
using UnityEngine;

public class JackAction : EnemyBaseAction
{
    static Dictionary<string, int> _003C_003Ef__switch_0024map6;
	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(JackAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.stateMachine.OnTransfer += this.OnStateTransfer;
		base.AnimChangeState(JackAction.StateEnum.Idle, 1f);
	}

	protected override void Update()
	{
		base.Update();
		this.UpdateHp();
	}

	private void FixedUpdate()
	{
		if (this.stateMachine.currentState != "FlyToFall")
		{
			return;
		}
		Vector2 currentSpeed = this.eAttr.timeController.GetCurrentSpeed();
		float f = currentSpeed.x;
		f = Mathf.Clamp(Mathf.Abs(f) - this._airFric * Time.fixedDeltaTime, 0f, float.MaxValue) * Mathf.Sign(f);
		currentSpeed.x = Mathf.Clamp(Mathf.Abs(f) - this._airFric * Time.fixedDeltaTime, 0f, float.PositiveInfinity) * Mathf.Sign(f);
		this.eAttr.timeController.SetSpeed(currentSpeed);
	}

	private void UpdateHp()
	{
		switch (R.GameData.Difficulty)
		{
		case 0:
		case 1:
			this.NormalModeHp();
			break;
		case 2:
		case 3:
			this.HardModeHp();
			break;
		}
	}

	private void NormalModeHp()
	{
		if (this.eAttr.currentHp < this.eAttr.maxHp * 65 / 100 && this._first65)
		{
			this._first65 = false;
			this.UseAtk5 = true;
		}
		if (this.eAttr.currentHp < this.eAttr.maxHp * 35 / 100 && this._first35)
		{
			this._first35 = false;
			this.UseAtk5 = true;
		}
	}

	private void HardModeHp()
	{
		if (this.eAttr.currentHp < this.eAttr.maxHp * 75 / 100 && this._first75)
		{
			this._first75 = false;
			this.UseAtk5 = true;
		}
		if (this.eAttr.currentHp < this.eAttr.maxHp * 55 / 100 && this._first55)
		{
			this._first55 = false;
			this.UseAtk5 = true;
		}
		if (this.eAttr.currentHp < this.eAttr.maxHp * 25 / 100 && this._first25)
		{
			this._first25 = false;
			this.UseAtk5 = true;
		}
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		string state = args.state;
		if (state != null)
		{
			if (JackAction._003C_003Ef__switch_0024map6 == null)
			{
				JackAction._003C_003Ef__switch_0024map6 = new Dictionary<string, int>(25)
				{
					{
						"Atk1",
						0
					},
					{
						"Atk2",
						0
					},
					{
						"Atk3",
						0
					},
					{
						"Atk4",
						0
					},
					{
						"Atk5Aim",
						0
					},
					{
						"Atk5End",
						0
					},
					{
						"Atk5Ready",
						0
					},
					{
						"Atk5Shoot",
						0
					},
					{
						"DieQTE",
						0
					},
					{
						"FallHitGround",
						0
					},
					{
						"FlyToFall",
						0
					},
					{
						"GetUp",
						0
					},
					{
						"Hit1",
						0
					},
					{
						"Hit2",
						0
					},
					{
						"HitQTE",
						0
					},
					{
						"HitToFly1",
						0
					},
					{
						"IdleToWeakMod",
						0
					},
					{
						"Jump",
						0
					},
					{
						"MoveAway",
						0
					},
					{
						"WeakModToIdle",
						0
					},
					{
						"Fall",
						1
					},
					{
						"HitToFly2",
						1
					},
					{
						"Idle",
						1
					},
					{
						"Move",
						1
					},
					{
						"WeakMod",
						1
					}
				};
			}
			int num;
			if (JackAction._003C_003Ef__switch_0024map6.TryGetValue(state, out num))
			{
				if (num != 0)
				{
					if (num == 1)
					{
						this.spineAnim.Play(args.state, true, false, 1f);
					}
				}
				else
				{
					this.spineAnim.Play(args.state, false, true, 1f);
				}
			}
		}
	}

	private void OnStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		if (args.nextState == "FlyToFall")
		{
			Vector2 currentSpeed = this.eAttr.timeController.GetCurrentSpeed();
			currentSpeed.y = 0f;
			this.eAttr.timeController.SetSpeed(currentSpeed);
			this.eAttr.timeController.SetGravity(0f);
		}
		if (args.lastState == "FlyToFall" || args.lastState == "Atk2" || args.lastState == "Atk3" || args.lastState == "Atk4" || args.lastState == "Jump")
		{
			this.eAttr.timeController.SetGravity(1f);
		}
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
		base.AnimChangeState(JackAction.StateEnum.Atk1, 1f);
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
		this.eAttr.timeController.SetGravity(0f);
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		base.AnimChangeState(JackAction.StateEnum.Atk2, 1f);
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
		this.eAttr.timeController.SetGravity(0f);
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		base.AnimChangeState(JackAction.StateEnum.Atk3, 1f);
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
		this.eAttr.timeController.SetGravity(0f);
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		base.AnimChangeState(JackAction.StateEnum.Atk4, 1f);
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
		base.AnimChangeState(JackAction.StateEnum.Atk5Ready, 1f);
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
		if (this.stateMachine.currentState.IsInArray(JackAction.QTESta) || this.IsInWeakSta())
		{
			return;
		}
		R.Effect.Generate(128, base.transform, Vector3.up * 2.5f, Vector3.zero, default(Vector3), true);
		base.ChangeFace(dir);
		this.eAttr.timeController.SetGravity(0f);
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		base.AnimChangeState(JackAction.StateEnum.Atk3, 1f);
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
		base.FaceToPlayer();
		this.eAttr.timeController.SetGravity(0f);
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		base.AnimChangeState(JackAction.StateEnum.Jump, 1f);
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
		base.AnimChangeState(JackAction.StateEnum.MoveAway, 1f);
	}

	public override void AnimMove()
	{
		base.AnimChangeState(JackAction.StateEnum.Move, 1f);
	}

	public override void AnimReady()
	{
		base.AnimChangeState(JackAction.StateEnum.Idle, 1f);
	}

	public override bool IsInNormalState()
	{
		return this.stateMachine.currentState.IsInArray(JackAction.NormalSta) && base.IsInNormalState();
	}

	public override bool IsInAttackState()
	{
		return this.stateMachine.currentState.IsInArray(JackAction.AttackSta);
	}

	public override bool IsInDeadState(string state)
	{
		return state == "DieQTE";
	}

	public override bool IsInWeakSta()
	{
		return this.eAttr.inWeakState;
	}

	public override bool IsInIdle()
	{
		return this.stateMachine.currentState == "Idle";
	}

	protected override bool EnterAtkSta(string lastState, string nextState)
	{
		return nextState.IsInArray(JackAction.AttackSta) && !lastState.IsInArray(JackAction.AttackSta);
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return !nextState.IsInArray(JackAction.AttackSta) && lastState.IsInArray(JackAction.AttackSta);
	}

	public override void AnimQTEHurt()
	{
		int dir = (base.player.transform.localScale.x >= 0f) ? 1 : -1;
		base.ChangeFace(dir);
		base.ExitWeakState(true);
		this.eAttr.isFlyingUp = false;
		this.eAttr.checkHitGround = false;
		Vector3 position = base.transform.position;
		position.y = LayerManager.YNum.GetGroundHeight(base.gameObject);
		position.z = LayerManager.ZNum.TempEnemy;
		base.transform.position = position;
		base.AnimChangeState(JackAction.StateEnum.HitQTE, 1f);
	}

	public override void AnimExecute()
	{
		int dir = (base.player.transform.localScale.x >= 0f) ? 1 : -1;
		base.ChangeFace(dir);
		base.ExitWeakState(true);
		this.eAttr.isFlyingUp = false;
		this.eAttr.checkHitGround = false;
		Vector3 position = base.transform.position;
		position.y = LayerManager.YNum.GetGroundHeight(base.gameObject);
		position.z = LayerManager.ZNum.TempEnemy;
		base.transform.position = position;
		base.AnimChangeState(JackAction.StateEnum.DieQTE, 1f);
	}

	public bool UseAtk5;

	private bool _first75 = true;

	private bool _first65 = true;

	private bool _first55 = true;

	private bool _first35 = true;

	private bool _first25 = true;

	private static readonly string[] AttackSta = new string[]
	{
		"Atk1",
		"Atk2",
		"Atk3",
		"Atk4",
		"Atk5Aim",
		"Atk5End",
		"Atk5Ready",
		"Atk5Shoot"
	};

	public static readonly string[] NormalSta = new string[]
	{
		"Idle",
		"Move"
	};

	public static readonly string[] HurtSta = new string[]
	{
		"Fall",
		"FallHitGround",
		"FlyToFall",
		"GetUp",
		"Hit1",
		"Hit2",
		"HitQTE",
		"HitToFly1",
		"HitToFly2"
	};

	public static readonly string[] QTESta = new string[]
	{
		"DieQTE",
		"HitQTE"
	};

	private readonly float _airFric = 8f;

	public enum StateEnum
	{
		Idle,
		Atk1,
		Atk2,
		Atk3,
		Atk4,
		Atk5Aim,
		Atk5End,
		Atk5Ready,
		Atk5Shoot,
		DieQTE,
		Fall,
		FallHitGround,
		FlyToFall,
		GetUp,
		Hit1,
		Hit2,
		HitQTE,
		HitToFly1,
		HitToFly2,
		IdleToWeakMod,
		Jump,
		Move,
		MoveAway,
		WeakMod,
		WeakModToIdle
	}
}
