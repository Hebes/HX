using System;
using Framework.Core;
using UnityEngine;

/// <summary>
/// 玩家闪现能力
/// </summary>
public class PlayerFlashAbility : CharacterState
{
    private bool IsOnObstacle
    {
        get
        {
            RaycastHit2D hit = Physics2D.Raycast(this.pac.transform.position + new Vector3(-0.45f, 0.4f, 0f), Vector2.down, 0.6f,
                LayerManager.ObstacleMask);
            RaycastHit2D hit2 = Physics2D.Raycast(this.pac.transform.position + new Vector3(0.45f, 0.4f, 0f), Vector2.down, 0.6f,
                LayerManager.ObstacleMask);
            return hit || hit2;
        }
    }

    private int CoolDown
    {
        get
        {
            if (!R.Mode.CheckMode(Mode.AllMode.Battle))
            {
                return WorldTime.SecondToFrame(0.5f);
            }

            switch (R.GameData.Difficulty)
            {
                case 0:
                    return WorldTime.SecondToFrame(1.5f);
                case 1:
                    return WorldTime.SecondToFrame(2.5f);
                case 2:
                    return WorldTime.SecondToFrame(3f);
                default:
                    return WorldTime.SecondToFrame(1.5f);
            }
        }
    }

    public override void Update()
    {
        if (this.pAttr.isDead)
        {
            return;
        }

        this.UpdateFlash();
    }

    public void FlashFace()
    {
        this.Swipe(R.Player.Attribute.faceDir);
    }

    public void FlashRight()
    {
        this.Swipe(1);
    }

    public void FlashLeft()
    {
        this.Swipe(-1);
    }

    public void FlashUp()
    {
        this.Swipe(2);
    }

    public void FlashDown()
    {
        if (this.FlashOnObstacle())
        {
            this.Swipe(-2);
            return;
        }

        if (this.pAttr.isOnGround)
        {
            return;
        }

        this.Swipe(-2);
    }

    public void FlashRightUp()
    {
        this.Swipe(4);
    }

    public void FlashRightDown()
    {
        this.Swipe((!this.pAttr.isOnGround) ? -4 : 1);
    }

    public void FlashLeftUp()
    {
        this.Swipe(5);
    }

    public void FlashLeftDown()
    {
        this.Swipe((!this.pAttr.isOnGround) ? -5 : -1);
    }

    private bool FlashLevelCheck()
    {
        return this.pAttr.currentFlashTimes > 0;
    }

    private bool FlashOnObstacle()
    {
        bool flag = R.Player.Attribute.flashLevel == 3;
        return flag && this.IsOnObstacle;
    }

    private void Swipe(int dir)
    {
        if (R.Player.TimeController.isPause)
        {
            return;
        }

        if ((this.stateMachine.currentState.IsInArray(PlayerFlashAbility.CanFlashSta) || this.pac.canChangeAnim) && this.FlashLevelCheck())
        {
            this.listener.StopIEnumerator("FlashPositionSet");
            if (dir == 1 || dir == -4 || dir == 4)
            {
                this.pac.TurnRound(1);
            }

            if (dir == -1 || dir == -5 || dir == 5)
            {
                this.pac.TurnRound(-1);
            }

            this.listener.flashDir = dir;
            switch (dir + 5)
            {
                case 0:
                case 1:
                    this.pac.ChangeState(PlayerAction.StateEnum.FlashDown45_1, 1f);
                    break;
                case 3:
                    this.pac.ChangeState(PlayerAction.StateEnum.FlashDown1, 1f);
                    break;
                case 4:
                case 6:
                    this.pac.ChangeState(PlayerAction.StateEnum.Flash1, 1f);
                    break;
                case 7:
                    this.pac.ChangeState(PlayerAction.StateEnum.FlashUp1, 1f);
                    break;
                case 9:
                case 10:
                    this.pac.ChangeState(PlayerAction.StateEnum.FlashUp45_1, 1f);
                    break;
            }

            EGameEvent.Assessment.Trigger((this,new AssessmentEventArgs(AssessmentEventArgs.EventType.CurrentComboFinish)));
            this.StateInit();
            this.listener.FlashStart();
            this.pAttr.currentFlashTimes = Mathf.Clamp(this.pAttr.currentFlashTimes - 1, 0, this.pAttr.flashTimes);
            R.Ui.Flash.OnFlash(this.pAttr.currentFlashTimes);
        }
    }

    private void StateInit()
    {
        this.listener.isFalling = false;
        this.listener.airAtkDown = false;
        this.listener.checkFallDown = false;
        this.listener.checkHitGround = false;
        R.Player.TimeController.SetSpeed(Vector2.zero);
        this.listener.AirPhysic(0f);
    }

    public override void OnStateMachineStateTransfer(object sender, StateMachine.TransferEventArgs args)
    {
        this.pAttr.flashFlag = false;
        ParticleSystem.EmissionModule emission = this.pac.blockPartical.emission;
        if (args.nextState.IsInArray(PlayerAction.FlashAttackSta) && Math.Abs(emission.rateOverDistance.constant - 10f) > 1.401298E-45f)
        {
            emission.rateOverDistance = 10f;
        }

        if (!args.nextState.IsInArray(PlayerAction.FlashAttackSta) && Math.Abs(emission.rateOverDistance.constant) > 1.401298E-45f)
        {
            emission.rateOverDistance = 0f;
        }
    }

    public void FlashOnce()
    {
        this.listener.StopIEnumerator("FlashPositionSet");
        this.listener.flashDir = this.pAttr.faceDir;
        this.pac.ChangeState(PlayerAction.StateEnum.Flash1, 1f);
        this.StateInit();
    }

    private void UpdateFlash()
    {
        if (this.pAttr.currentFlashTimes < this.pAttr.flashTimes)
        {
            this.pAttr.FlashCd++;
            if (this.pAttr.FlashCd >= this.CoolDown)
            {
                this.pAttr.FlashCd = 0;
                this.pAttr.currentFlashTimes = Mathf.Clamp(this.pAttr.currentFlashTimes + 1, 0, this.pAttr.flashTimes);
                bool isFilled = this.pAttr.currentFlashTimes == this.pAttr.flashTimes;
                R.Ui.Flash.OnRecover(this.pAttr.currentFlashTimes - 1, isFilled);
            }
        }
    }

    private static readonly string[] CanFlashSta = new string[]
    {
        "EndAtk",
        "Fall1",
        "Fall2",
        "Idle",
        "Jump",
        "Jump2",
        "RollJump",
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
        "UpRising",
        "AtkUpRising",
        "HitGround",
        "HitGround2",
        "RollReady",
        "Roll",
        "RollEnd",
        "Flash2",
        "FlashDown2",
        "FlashUp2",
        "Charge1Ready",
        "Charging1",
        "Charge1End",
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
        "ExecuteToIdle",
        "Execute2ToFall",
        "FlashGround",
        "AtkRollReady",
        "AtkRollEnd",
        "AirAtkRollReady",
        "AirAtkRoll"
    };
}