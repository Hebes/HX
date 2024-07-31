using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 暴食BOSS动作
/// </summary>
public class EatingBossAction : EnemyBaseAction
{
    static Dictionary<string, int> _003C_003Ef__switch_0024map2;
	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(EatingBossAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.stateMachine.OnTransfer += this.OnStateTransfer;
		base.AnimChangeState(EatingBossAction.StateEnum.Idle, 1f);
		this.summonRate = (float)((R.GameData.Difficulty > 1) ? 45 : 15);
	}

	protected override void Update()
	{
		if (this.attack2Success)
		{
			Vector3 position = base.transform.position;
			R.Player.Transform.position = position.SetZ(LayerManager.ZNum.MMiddle_P);
		}
		this.summonRate += Time.deltaTime;
		this.UpdateHp();
		base.Update();
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
		if (this.eAttr.currentHp < this.eAttr.maxHp * 75 / 100 && this.first75)
		{
			this.first75 = false;
			this.useAtk5 = true;
		}
		if (this.eAttr.currentHp >= this.eAttr.maxHp * 30 / 100 || !this.first30)
		{
			return;
		}
		this.first30 = false;
		this.useAtk5 = true;
	}

	private void HardModeHp()
	{
		if (this.eAttr.currentHp < this.eAttr.maxHp * 75 / 100 && this.first75)
		{
			this.first75 = false;
			this.useAtk5 = true;
		}
		if (this.eAttr.currentHp < this.eAttr.maxHp * 50 / 100 && this.first50)
		{
			this.first50 = false;
			this.useAtk5 = true;
		}
		if (this.eAttr.currentHp >= this.eAttr.maxHp * 25 / 100 || !this.first25)
		{
			return;
		}
		this.first25 = false;
		this.useAtk5 = true;
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		string state = args.state;
		if (state != null)
		{
			if (EatingBossAction._003C_003Ef__switch_0024map2 == null)
			{
				EatingBossAction._003C_003Ef__switch_0024map2 = new Dictionary<string, int>(20)
				{
					{
						"Atk1",
						0
					},
					{
						"Atk2Ready",
						0
					},
					{
						"Atk2",
						0
					},
					{
						"Atk2SuccessEnd",
						0
					},
					{
						"Atk2End",
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
						"DieQTE",
						0
					},
					{
						"IdleToWeakMod",
						0
					},
					{
						"WeakModToIdle",
						0
					},
					{
						"Atk5Ready",
						0
					},
					{
						"Atk5End",
						0
					},
					{
						"Atk2Success",
						1
					},
					{
						"Atk5",
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
			if (EatingBossAction._003C_003Ef__switch_0024map2.TryGetValue(state, out num))
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
		if (args.lastState == "Atk4" || args.lastState == "Atk5")
		{
			this.eAttr.timeController.SetGravity(1f);
		}
		if (args.nextState == "WeakMod")
		{
			R.Audio.PlayEffect(254, new Vector3?(base.transform.position));
		}
		if (this.ExitAtkSta(args.lastState, args.nextState) && this.attack2Success)
		{
			base.GetComponent<EatingBossAnimEvent>().Attack2Release();
		}
	}

	public override void Attack1(int dir)
	{
		this.summonRate = 0f;
		if (this.eAttr.isDead)
		{
			return;
		}
		if (!this.IsInNormalState())
		{
			return;
		}
		if (R.Enemy.Count >= 3)
		{
			return;
		}
		base.AnimChangeState(EatingBossAction.StateEnum.Atk1, 1f);
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
		base.AnimChangeState(EatingBossAction.StateEnum.Atk2Ready, 1f);
	}

	public void Attack2Success()
	{
		this.attack2Success = true;
		base.AnimChangeState(EatingBossAction.StateEnum.Atk2Success, 1f);
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
		base.AnimChangeState(EatingBossAction.StateEnum.Atk3, 1f);
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
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		this.eAttr.timeController.SetGravity(0f);
		base.AnimChangeState(EatingBossAction.StateEnum.Atk4, 1f);
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
		this.eAttr.timeController.SetSpeed(Vector2.zero);
		this.eAttr.timeController.SetGravity(0f);
		base.AnimChangeState(EatingBossAction.StateEnum.Atk5Ready, 1f);
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
		if (this.stateMachine.currentState.IsInArray(EatingBossAction.QTESta) || this.IsInWeakSta())
		{
			return;
		}
		R.Effect.Generate(128, base.transform, Vector3.up * 3.3f, Vector3.zero, default(Vector3), true);
		base.ChangeFace(dir);
		base.AnimChangeState(EatingBossAction.StateEnum.Atk3, 1f);
	}

	public override void AnimMove()
	{
		base.AnimChangeState(EatingBossAction.StateEnum.Move, 1f);
	}

	public override void AnimReady()
	{
		base.AnimChangeState(EatingBossAction.StateEnum.Idle, 1f);
	}

	public override bool IsInNormalState()
	{
		return this.stateMachine.currentState.IsInArray(EatingBossAction.NormalSta) && base.IsInNormalState();
	}

	public override bool IsInAttackState()
	{
		return this.stateMachine.currentState.IsInArray(EatingBossAction.AttackSta);
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
		return nextState.IsInArray(EatingBossAction.AttackSta) && !lastState.IsInArray(EatingBossAction.AttackSta);
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return !nextState.IsInArray(EatingBossAction.AttackSta) && lastState.IsInArray(EatingBossAction.AttackSta);
	}

	public override void AnimQTEHurt()
	{
		int dir = (base.player.transform.localScale.x >= 0f) ? 1 : -1;
		base.ChangeFace(dir);
		base.ExitWeakState(true);
		Vector3 position = base.transform.position;
		position.y = LayerManager.YNum.GetGroundHeight(base.gameObject);
		position.z = LayerManager.ZNum.TempEnemy;
		base.transform.position = position;
		base.AnimChangeState(EatingBossAction.StateEnum.HitQTE, 1f);
	}

	public override void AnimExecute()
	{
		int dir = (base.player.transform.localScale.x >= 0f) ? 1 : -1;
		base.ChangeFace(dir);
		base.ExitWeakState(true);
		Vector3 position = base.transform.position;
		position.y = LayerManager.YNum.GetGroundHeight(base.gameObject);
		position.z = LayerManager.ZNum.TempEnemy;
		base.transform.position = position;
		base.AnimChangeState(EatingBossAction.StateEnum.DieQTE, 1f);
	}

	public bool useAtk5;

	private bool first75 = true;

	private bool first50 = true;

	private bool first30 = true;

	private bool first25 = true;

	public bool attack2Success;

	public float summonRate;

	private static readonly string[] NormalSta = new string[]
	{
		"Idle",
		"Move"
	};

	private static readonly string[] AttackSta = new string[]
	{
		"Atk1",
		"Atk2Ready",
		"Atk2",
		"Atk2Success",
		"Atk2SuccessEnd",
		"Atk2End",
		"Atk3",
		"Atk4",
		"Atk5Ready",
		"Atk5",
		"Atk5End"
	};

	private static readonly string[] QTESta = new string[]
	{
		"DieQTE",
		"HitQTE"
	};

	public enum StateEnum
	{
		Idle,
		Move,
		Atk1,
		Atk2Ready,
		Atk2,
		Atk2Success,
		Atk2SuccessEnd,
		Atk2End,
		Atk3,
		Atk4,
		DieQTE,
		Hit1,
		Hit2,
		HitQTE,
		IdleToWeakMod,
		WeakMod,
		WeakModToIdle,
		Atk5Ready,
		Atk5,
		Atk5End
	}

	public enum HurtSta
	{
		Hit1,
		Hit2,
		HitQTE
	}
}
