using Framework.Core;
using UnityEngine;

/// <summary>
/// 玩家移动能力
/// </summary>
public class PlayerMoveAbility : CharacterState
{
    public override void Update()
    {
        moveStopCounter--;
        if (moveStopCounter == 0)
            Move(0);
    }

    public override void FixedUpdate()
    {
        if ((stateMachine.currentState.IsInArray(PlayerAction.JumpSta) || stateMachine.currentState.IsInArray(PlayerAction.FlySta)) &&
            !pAttr.isOnGround)
            DealAirFric(pAttr.moveSpeed, true);
        if (stateMachine.currentState.IsInArray(PlayerAction.UpRisingSta) && !pAttr.isOnGround)
            DealAirFric(pAttr.moveSpeed / 2f, false);
        if (stateMachine.currentState == "BladeStorm")
            DealAirFric(pAttr.moveSpeed / 4f, false);
    }

    private void DealAirFric(float maxSpeed, bool canTurn)
    {
        Vector2 currentSpeed = R.Player.TimeController.GetCurrentSpeed();
        float num = currentSpeed.x;
        num = Mathf.Clamp(Mathf.Abs(num) - airFric * Time.fixedDeltaTime, 0f, float.MaxValue) * Mathf.Sign(num);
        if (Input.Game.MoveLeft.Pressed || Input.Game.MoveRight.Pressed)
        {
            int num2 = (!Input.Game.MoveLeft.Pressed) ? 1 : -1;
            int num3 = (!Input.Game.MoveLeft.Pressed) ? 1 : -1;
            if (num2 != pAttr.faceDir && canTurn)
                pac.TurnRound(num2);
            num += num3 * extraFric * Time.fixedDeltaTime;
        }

        currentSpeed.x = Mathf.Clamp(Mathf.Abs(num) - airFric * Time.fixedDeltaTime, 0f, maxSpeed) * Mathf.Sign(num);
        if (!pAttr.isOnGround)
        {
            Collider2D[] array = Physics2D.OverlapAreaAll(pac.transform.position + new Vector3(0.5f * pAttr.faceDir, 0f, 0f),
                pac.transform.position + new Vector3(0.6f * pAttr.faceDir, 2.2f, 0f), LayerManager.WallMask);
            int num4 = 0;
            for (var i = 0; i < array.Length; i++)
            {
                if (array[i].gameObject.layer == LayerManager.WallLayerID || array[i].gameObject.layer == LayerManager.GroundLayerID)
                    num4++;
            }

            bool flag = num4 > 0;
            if (flag) 
                currentSpeed.x = Mathf.Sign(currentSpeed.x) * -1f * 0.1f;
            num4 = 0;
            for (int j = 0; j < array.Length; j++)
            {
                if (array[j].gameObject.layer == LayerManager.CeilingLayerID)
                    num4++;
            }

            bool flag2 = num4 > 0;
            if (flag2 && currentSpeed.y > 0f)
            {
                currentSpeed.y = 0f;
            }
        }

        R.Player.TimeController.SetSpeed(currentSpeed);
    }

    public void Move(int dir)
    {
        if (pAttr.isDead) return;
        if (dir == 0)
        {
            pac.tempDir = 3;
            if (stateMachine.currentState == PlayerAction.CanRunSlow)
            {
                R.Player.TimeController.SetSpeed(Vector2.zero);
                pac.ChangeState(PlayerAction.StateEnum.RunSlow);
            }

            if (stateMachine.currentState.IsInArray(PlayerAction.AttackSta))
                R.Player.TimeController.SetSpeed(Vector2.zero);
        }
        else
        {
            pac.tempDir = dir;
            if (stateMachine.currentState.IsInArray(PlayerAction.NormalSta) || pac.canChangeAnim)
                PlayerMove(dir == -1, pAttr.moveSpeed, Vector2.zero);
            if (stateMachine.currentState.IsInArray(AttackSta) && dir == pAttr.faceDir)
                PlayerMove(dir == -1, pAttr.moveSpeed / 4f, addSpeed, false);
            if ((stateMachine.currentState.IsInArray(PlayerAction.AirLightAttackSta) || stateMachine.currentState == "AirAtkHv1" ||
                 stateMachine.currentState == "AirAtkHv2") && dir == pAttr.faceDir)
                PlayerMove(dir == -1, pAttr.moveSpeed / 4f, addSpeed, false);
            if (stateMachine.currentState == "BladeStorm")
                PlayerMove(dir == -1, pAttr.moveSpeed / 2f, Vector2.zero, false);
        }
    }

    private void PlayerMove(bool isLeft, float walkSpeed, Vector2 aSpeed, bool playRun = true)
    {
        if (R.Player.TimeController.isPause)
        {
            return;
        }

        walkSpeed = AirWallCheck(walkSpeed);
        int num = !isLeft ? 1 : -1;
        Vector2 vector = new Vector2(walkSpeed * num, R.Player.TimeController.GetCurrentSpeed().y);
        vector += aSpeed;
        vector = EdgeCheck(vector);
        vector = SlopeCheck(vector);
        R.Player.TimeController.SetSpeed(vector);
        if (playRun)
        {
            pac.TurnRound((!isLeft) ? 1 : -1);
            pac.ChangeState(PlayerAction.StateEnum.Run);
        }

        moveStopCounter = 4;
    }

    private float AirWallCheck(float walkSpeed)
    {
        if (!pAttr.isOnGround && Physics2D.OverlapAreaAll(pac.transform.position + new Vector3(0.5f * pAttr.faceDir, 0f, 0f),
                pac.transform.position + new Vector3(0.6f * pAttr.faceDir, 2.2f, 0f), LayerManager.GroundMask).Length > 0)
            walkSpeed = 0f;
        return walkSpeed;
    }

    private Vector2 EdgeCheck(Vector2 speed)
    {
        Vector3 position = pac.transform.position;
        if (position.x >= GameArea.PlayerRange.max.x - pAttr.bounds.size.x / 2f)
            speed.x = speed.x <= 0f ? speed.x : 0f;
        if (position.x <= GameArea.PlayerRange.min.x + pAttr.bounds.size.x / 2f)
            speed.x = speed.x >= 0f ? speed.x : 0f;
        return speed;
    }

    private Vector2 SlopeCheck(Vector2 speed)
    {
        if (pac.IsInNormalState())
        {
            PlatformMovement component = pac.GetComponent<PlatformMovement>();
            Vector2 groundNormal = component.GetGroundNormal();
            Vector2 vector = Vector3.ProjectOnPlane(speed, groundNormal);
            float d = Mathf.Clamp(Mathf.Abs(vector.x), Mathf.Abs(speed.x / 2f), Mathf.Abs(speed.x));
            speed = speed.y > 0f ? vector.normalized * d : vector;
        }

        return speed;
    }

    public override void OnStateMachineStateTransfer(object sender, StateMachine.TransferEventArgs args)
    {
        if (args.nextState.IsInArray(PlayerAction.NormalSta) && !args.lastState.IsInArray(PlayerAction.NormalSta))
            EGameEvent.Assessment.Trigger((this, new AssessmentEventArgs(AssessmentEventArgs.EventType.CurrentComboFinish)));
    }

    private static readonly string[] AttackSta =
    {
        "Atk1",
        "Atk2",
        "Atk5",
        "Atk6",
        "Atk7",
        "Atk8",
        "Atk11",
        "Atk12",
        "Atk13",
        "Atk14",
        "Atk15",
        "AtkHv1",
        "AtkHv2",
        "AtkHv3",
        "AtkHv1Push",
        "AtkRollReady",
        "AtkRollEnd"
    };

    private const int maxMoveStopCount = 4;

    private int moveStopCounter;

    private float airFric = 8f;

    private float extraFric = 60f;

    public Vector2 addSpeed = Vector2.zero;
}