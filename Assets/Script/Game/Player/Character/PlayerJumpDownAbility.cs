
/// <summary>
/// 玩家跳下来技能
/// </summary>
public class PlayerJumpDownAbility : CharacterState
{
    public override void Start()
    {
        this._platform = this.pac.GetComponent<PlatformMovement>();
    }

    public void JumpDown()
    {
        if (!this.stateMachine.currentState.IsInArray(PlayerAction.NormalSta))
        {
            return;
        }
        if (this._platform.IgnoreOnOneWayGround())
        {
            this.pac.ChangeState(PlayerAction.StateEnum.Fall1, 1f);
        }
    }

    private PlatformMovement _platform;
}