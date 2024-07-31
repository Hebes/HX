using UnityEngine;

/// <summary>
/// 玩家属性
/// </summary>
public class PlayerAbilities : MonoBehaviour
{
    private void Awake()
    {
        stateMachine = R.Player.StateMachine;
        stateMachine.OnEnter += OnStateMachineStateEnter;
        stateMachine.OnExit += OnStateMachineStateExit;
        stateMachine.OnTransfer += OnStateMachineStateTransfer;
        states = new CharacterState[13];
        states[0] = move;
        states[1] = attack;
        states[2] = jump;
        states[3] = charge;
        states[4] = execute;
        states[5] = hitGround;
        states[6] = upRising;
        states[7] = jumpDown;
        states[8] = flash;
        states[9] = skill;
        states[10] = flashAttack;
        states[11] = hurt;
        states[12] = chase;
        for (int i = 0; i < states.Length; i++)
            states[i].Init();
    }

    private void Start()
    {
        for (var i = 0; i < states.Length; i++)
            states[i].Start();
    }

    private void Update()
    {
        for (var i = 0; i < states.Length; i++)
            states[i].Update();
    }

    private void OnEnable()
    {
        for (var i = 0; i < states.Length; i++)
            states[i].OnEnable();
    }

    private void OnDisable()
    {
        for (var i = 0; i < states.Length; i++)
            states[i].OnDisable();
    }

    private void OnDestroy()
    {
        for (var i = 0; i < states.Length; i++)
            states[i].OnDestroy();
    }

    private void FixedUpdate()
    {
        for (var i = 0; i < states.Length; i++)
            states[i].FixedUpdate();
    }

    public virtual void OnStateMachineStateTransfer(object sender, StateMachine.TransferEventArgs args)
    {
        for (var i = 0; i < states.Length; i++)
            states[i].OnStateMachineStateTransfer(sender, args);
    }

    public virtual void OnStateMachineStateEnter(object sender, StateMachine.StateEventArgs args)
    {
        for (var i = 0; i < states.Length; i++)
            states[i].OnStateMachineStateEnter(sender, args);
    }

    public virtual void OnStateMachineStateExit(object sender, StateMachine.StateEventArgs args)
    {
        for (var i = 0; i < states.Length; i++)
            states[i].OnStateMachineStateExit(sender, args);
    }

    public PlayerMoveAbility move = new PlayerMoveAbility();
    public PlayerAttackAbility attack = new PlayerAttackAbility();
    public PlayerJumpAbility jump = new PlayerJumpAbility();
    public PlayerChargingAbility charge = new PlayerChargingAbility();
    public PlayerExecuteAbility execute = new PlayerExecuteAbility();
    public PlayerHitGroundAbility hitGround = new PlayerHitGroundAbility();
    public PlayerUpRisingAbility upRising = new PlayerUpRisingAbility();
    public PlayerJumpDownAbility jumpDown = new PlayerJumpDownAbility();
    public PlayerFlashAbility flash = new PlayerFlashAbility();
    public PlayerSkillAbility skill = new PlayerSkillAbility();
    public PlayerFlashAttackAbility flashAttack = new PlayerFlashAttackAbility();
    public PlayerHurtAbility hurt = new PlayerHurtAbility();
    public PlayerChaseAbility chase = new PlayerChaseAbility();
    protected StateMachine stateMachine;
    private CharacterState[] states;
}