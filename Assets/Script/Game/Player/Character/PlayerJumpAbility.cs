using Framework.Core;
using UnityEngine;

/// <summary>
/// 玩家跳跃能力
/// </summary>
public class PlayerJumpAbility : CharacterState
{
	public override void Update()
	{
		if (this._invincibleRecover > 0)
		{
			this._invincibleRecover--;
			if (this._invincibleRecover <= 0)
			{
				this.pab.hurt.Invincible = false;
			}
		}
	}

	public void Jump()
	{
		if (R.Player.TimeController.isPause)
		{
			return;
		}
		if ((this.stateMachine.currentState.IsInArray(PlayerJumpAbility.CanJumpSta) || (this.stateMachine.currentState == "AtkUpRising" && this.pac.canChangeAnim)) && !this._firstJumped)
		{
			this._firstJumped = true;
			this.listener.PhysicReset();
			this.StateCheck();
			this.pac.ChangeState(PlayerAction.StateEnum.Jump, 1f);
			Vector2 currentSpeed = R.Player.TimeController.GetCurrentSpeed();
			currentSpeed.y = 20f;
			R.Player.TimeController.SetSpeed(currentSpeed);
			EGameEvent.Assessment.Trigger((this,new AssessmentEventArgs(AssessmentEventArgs.EventType.CurrentComboFinish)));
			return;
		}
		if ((this.stateMachine.currentState.IsInArray(PlayerJumpAbility.SecondJumpSta) && this._firstJumped && !this._secondJumpped) || (this.stateMachine.currentState.IsInArray(PlayerJumpAbility.HurtJumpSta) && this.listener.hitJump))
		{
			if (this.listener.hitJump && this.stateMachine.currentState.IsInArray(PlayerJumpAbility.HurtJumpSta))
			{
				this.JumpEffect();
				this.listener.hitJump = false;
				this.listener.flyHitFlag = false;
				this.listener.flyHitGround = false;
				this.pab.hurt.Invincible = true;
				this._invincibleRecover = WorldTime.SecondToFrame(0.3f);
			}
			this._firstJumped = true;
			this._secondJumpped = true;
			this.listener.PhysicReset();
			this.StateCheck();
			this.pac.ChangeState(PlayerAction.StateEnum.RollJump, 1f);
			Vector2 currentSpeed2 = R.Player.TimeController.GetCurrentSpeed();
			currentSpeed2.y = 16f;
			R.Player.TimeController.SetSpeed(currentSpeed2);
			EGameEvent.Assessment.Trigger((this,new AssessmentEventArgs(AssessmentEventArgs.EventType.CurrentComboFinish)));
		}
	}

	private void StateCheck()
	{
		this.listener.checkFallDown = false;
		this.listener.isFalling = false;
		this.listener.airAtkDown = false;
	}

	private void JumpEffect()
	{
		Transform transform = R.Effect.Generate(212, null, this.pab.transform.position, default(Vector3), default(Vector3), true);
		transform.localScale = this.pab.transform.localScale;
	}

	public override void OnStateMachineStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		if (!args.lastState.IsInArray(PlayerAction.NormalSta) && args.nextState.IsInArray(PlayerAction.NormalSta))
		{
			this._firstJumped = false;
			this._secondJumpped = false;
		}
	}

	private bool _firstJumped;

	private bool _secondJumpped;

	private int _invincibleRecover;

	private static readonly string[] CanJumpSta = new string[]
	{
		"EndAtk",
		"Fall1",
		"Fall2",
		"GetUp",
		"Idle",
		"Ready",
		"Run",
		"RunSlow",
		"Atk1",
		"Atk2",
		"Atk3",
		"Atk4",
		"Atk5",
		"Atk6",
		"Atk7",
		"Atk8",
		"Atk11",
		"Atk12",
		"Atk13",
		"Atk14",
		"Atk23",
		"Atk15",
		"Atk16",
		"AtkHv1",
		"AtkHv2",
		"AtkHv3",
		"AtkHv1Push",
		"AtkUpRising",
		"RollReady",
		"Roll",
		"RollEnd",
		"Charge1Ready",
		"Charging1",
		"Charge1End",
		"AirAtk1",
		"AirAtk2",
		"AirAtk3",
		"AirAtk4",
		"AirAtk6",
		"AirAtkHv1",
		"AirAtkHv2",
		"AirAtkHv3",
		"AirAtkHv4",
		"AirAtkHv5",
		"AirAtkHv1Push",
		"AtkRollReady",
		"AtkRollEnd",
		"AirAtkRollReady",
		"AirAtkRoll"
	};

	private static readonly string[] SecondJumpSta = new string[]
	{
		"Fall1",
		"Fall2",
		"Jump"
	};

	private static readonly string[] HurtJumpSta = new string[]
	{
		"UnderAtkFlyToFall",
		"UnderAtkHitGround",
		"UnderAtkHitToFly"
	};
}
