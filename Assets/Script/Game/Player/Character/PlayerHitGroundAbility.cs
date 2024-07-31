using System.Collections;
using UnityEngine;

/// <summary>
/// 玩家击地技能
/// </summary>
public class PlayerHitGroundAbility : CharacterState
{
	public override void Start()
	{
		_atkBox = pac.GetComponentInChildren<PlayerAtk>().transform;
		_platform = R.Player.GetComponent<PlatformMovement>();
	}

	public override void Update()
	{
		if (pAttr.isDead)
		{
			return;
		}
		if (R.Player.TimeController.isPause)
		{
			return;
		}
		if (listener.checkHitGround && pAttr.isOnGround)
		{
			R.Player.TimeController.SetSpeed(Vector2.zero);
			listener.PhysicReset();
			listener.checkHitGround = false;
			Vector3 position = new Vector3(0f, _platform.GetDistanceToGround(), 0f);
			R.Effect.Generate(22, pac.transform, position, Vector3.zero);
			string currentState = stateMachine.currentState;
			switch (currentState)
			{
			case "Roll":
			case "DahalRoll":
				_atkBox.localScale = Vector2.zero;
				pac.ChangeState(PlayerAction.StateEnum.RollEnd);
				break;
			case "HitGround":
				pac.ChangeState(PlayerAction.StateEnum.HitGround2);
				break;
			case "RollEndFrame":
				pac.ChangeState(PlayerAction.StateEnum.RollFrameEnd);
				break;
			case "QTEHitGround":
				pac.ChangeState(PlayerAction.StateEnum.QTEHitGround2);
				break;
			case "NewExecuteAir1_2":
				SingletonMono<CameraController>.Instance.KillTweening();
				SingletonMono<CameraController>.Instance.CloseMotionBlur();
				StartCoroutine(ExecuteHitGround());
				break;
			case "QTERoll":
				pac.ChangeState(PlayerAction.StateEnum.QTERollEnd);
				break;
			}
		}
	}

	private IEnumerator ExecuteHitGround()
	{
		yield return new WaitForSeconds(0.05f);
		listener.StartExecute();
	}

	public void HitGround()
	{
		if (R.Player.TimeController.isPause)
		{
			return;
		}
		if (!pAttr.isOnGround && stateMachine.currentState.IsInArray(CanHitGroundSta) && R.Player.EnhancementSaveData.HitGround != 0)
		{
			listener.StopIEnumerator("FlashPositionSet");
			if (pac.tempDir != 3)
			{
				pac.TurnRound(pac.tempDir);
			}
			weapon.HandleHitGround();
		}
	}

	public override void OnStateMachineStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
		if (args.nextState == "Roll")
		{
			Vector2 speed = new Vector2(4 * pAttr.faceDir, 0f);
			listener.AirPhysic(0.8f);
			R.Player.TimeController.SetSpeed(speed);
		}
	}

	private static readonly string[] CanHitGroundSta = {
		"Fall1",
		"Fall2",
		"Flash2",
		"FlashDown2",
		"FlashUp2",
		"Flash1",
		"FlashDown1",
		"FlashUp1",
		"UpRising",
		"Jump",
		"Jump2",
		"RollJump",
		"IdleToDefense",
		"Defense",
		"FallToDefenseAir",
		"DefenseAir",
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
		"RollReady",
		"Roll",
		"Execute2ToFall"
	};

	private Transform _atkBox;

	private PlatformMovement _platform;
}
