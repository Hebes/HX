using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 锤击作用
/// </summary>
public class HammerAction : EnemyBaseAction
{
    static Dictionary<string, int> _003C_003Ef__switch_0024map5;
	protected override void Start()
	{
		this.stateMachine.AddStates(typeof(HammerAction.StateEnum));
		this.stateMachine.OnEnter += this.OnMyStateEnter;
		this.stateMachine.OnTransfer += this.OnStateTransfer;
		base.AnimChangeState(HammerAction.StateEnum.Idle, 1f);
	}

	private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
	{
		string state = args.state;
		if (state != null)
		{
			if (HammerAction._003C_003Ef__switch_0024map5 == null)
			{
				HammerAction._003C_003Ef__switch_0024map5 = new Dictionary<string, int>(13)
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
						"DieQTE",
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
						"IdleToWeakMod",
						0
					},
					{
						"Jump",
						0
					},
					{
						"WeakModToIdle",
						0
					},
					{
						"HitQTE",
						0
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
			if (HammerAction._003C_003Ef__switch_0024map5.TryGetValue(state, out num))
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
		if (args.lastState == "Jump" || args.lastState == "Atk3")
		{
			this.eAttr.timeController.SetGravity(1f);
		}
		if (args.nextState == "WeakMod")
		{
			R.Audio.PlayEffect(405, new Vector3?(base.transform.position));
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
		base.AnimChangeState(HammerAction.StateEnum.Atk1, 1f);
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
		base.AnimChangeState(HammerAction.StateEnum.Atk2, 1f);
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
		base.AnimChangeState(HammerAction.StateEnum.Atk3, 1f);
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
		if (this.stateMachine.currentState.IsInArray(HammerAction.QTESta) || this.IsInWeakSta())
		{
			return;
		}
		R.Effect.Generate(128, base.transform, Vector3.up * 3.3f, Vector3.zero, default(Vector3), true);
		base.ChangeFace(dir);
		base.AnimChangeState(HammerAction.StateEnum.Atk1, 1f);
	}

	public override void AnimMove()
	{
		base.AnimChangeState(HammerAction.StateEnum.Move, 1f);
	}

	public override void AnimReady()
	{
		base.AnimChangeState(HammerAction.StateEnum.Idle, 1f);
	}

	public override bool IsInNormalState()
	{
		return this.stateMachine.currentState.IsInArray(HammerAction.NormalSta) && base.IsInNormalState();
	}

	public override bool IsInAttackState()
	{
		return this.stateMachine.currentState.IsInArray(HammerAction.AttackSta);
	}

	public override bool IsInDeadState(string state)
	{
		return state == "DieQTE";
	}

	public override bool IsInWeakSta()
	{
		return this.eAttr.inWeakState;
	}

	protected override bool EnterAtkSta(string lastState, string nextState)
	{
		return nextState.IsInArray(HammerAction.AttackSta) && !lastState.IsInArray(HammerAction.AttackSta);
	}

	protected override bool ExitAtkSta(string lastState, string nextState)
	{
		return !nextState.IsInArray(HammerAction.AttackSta) && lastState.IsInArray(HammerAction.AttackSta);
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
		base.AnimChangeState(HammerAction.StateEnum.HitQTE, 1f);
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
		base.AnimChangeState(HammerAction.StateEnum.DieQTE, 1f);
	}

	private static readonly string[] NormalSta = new string[]
	{
		"Idle",
		"Move"
	};

	private static readonly string[] AttackSta = new string[]
	{
		"Atk1",
		"Atk2",
		"Atk3"
	};

	public static readonly string[] HurtSta = new string[]
	{
		"Hit1",
		"Hit2",
		"HitQTE"
	};

	public static readonly string[] QTESta = new string[]
	{
		"HitQTE",
		"DieQTE"
	};

	public enum StateEnum
	{
		Idle,
		Move,
		Atk1,
		Atk2,
		DieQTE,
		Hit1,
		Hit2,
		IdleToWeakMod,
		Jump,
		WeakMod,
		WeakModToIdle,
		Atk3,
		HitQTE
	}
}
