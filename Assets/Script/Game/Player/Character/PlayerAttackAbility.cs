using System;
using UnityEngine;

public class PlayerAttackAbility : CharacterState
{
	public override void Update()
	{
		if (this.listener.airAtkDown && this.pAttr.isOnGround)
		{
			this.listener.airAtkDown = false;
			this.listener.PhysicReset();
			R.Player.TimeController.SetSpeed(Vector2.zero);
			this.pac.ChangeState(PlayerAction.StateEnum.AirAtkHv5, 1f);
		}
	}

	public void PlayerAttack(int attackDir, bool cirtAtk)
	{
		if (this.stateMachine.currentState.IsInArray(PlayerAction.AttackSta))
		{
			if (this.weapon.canChangeAtkAnim)
			{
				this.weapon.cirtAttack = cirtAtk;
				this.weapon.CanPlayNextAttack();
			}
			else
			{
				this.weapon.HandleAttack(cirtAtk);
			}
			return;
		}
		if (this.stateMachine.currentState.IsInArray(PlayerAction.AirAttackSta))
		{
			if (this.weapon.canChangeAirAtkAnim)
			{
				this.weapon.cirtAttack = cirtAtk;
				this.weapon.CanPlayNextAirAttack();
			}
			else
			{
				this.weapon.HandleAirAttack(cirtAtk);
			}
			return;
		}
		if (R.Player.TimeController.isPause)
		{
			return;
		}
		if (this.stateMachine.currentState.IsInArray(PlayerAttackAbility.CanAttackSta) || this.pac.canChangeAnim)
		{
			if (attackDir != 3)
			{
				this.pac.TurnRound(attackDir);
			}
			this.listener.PhysicReset();
			if (this.pAttr.isOnGround)
			{
				R.Player.TimeController.SetSpeed(Vector2.zero);
				this.weapon.HandleAttack(cirtAtk);
			}
			else
			{
				this.listener.checkFallDown = false;
				this.weapon.HandleAirAttack(cirtAtk);
			}
		}
	}

	public void PlayerCirtPressAttack(int attackDir)
	{
		if (R.Player.TimeController.isPause)
		{
			return;
		}
		if (this.stateMachine.currentState.IsInArray(PlayerAttackAbility.CanAttackSta) && this._pressRelease)
		{
			if (attackDir != 3)
			{
				this.pac.TurnRound(attackDir);
			}
			this.listener.PhysicReset();
			R.Player.TimeController.SetSpeed(Vector2.zero);
			if (this.stateMachine.currentState.IsInArray(PlayerAction.NormalSta))
			{
				R.Player.TimeController.SetSpeed(Vector2.zero);
				this.weapon.CirtAttackHold();
			}
			else
			{
				this.weapon.AirCirtAttackHold();
			}
			this._pressRelease = false;
		}
	}

	public void PlayerCirtPressAttackReleasd()
	{
		this._pressRelease = true;
	}

	public override void OnStateMachineStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		this.listener.atkBox.localScale = new Vector3(0f, 0f, 1f);
		this.listener.atkBox.localPosition = Vector3.zero;
		if (args.lastState.IsInArray(PlayerAction.AirAttackSta) && !args.nextState.IsInArray(PlayerAction.AirAttackSta))
		{
			this.weapon.AirAttackRecover();
		}
	}

	private bool _pressRelease = true;

	private static readonly string[] CanAttackSta = new string[]
	{
		"EndAtk",
		"GetUp",
		"Idle",
		"Ready",
		"Run",
		"RunSlow",
		"Jump",
		"Fall1",
		"Fall2",
		"Flash2",
		"FlashDown2",
		"FlashUp2",
		"FlashGround"
	};
}
