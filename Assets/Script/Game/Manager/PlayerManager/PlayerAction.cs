using System;
using Framework.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 玩家行动
/// </summary>
[RequireComponent(typeof(StateMachine))]
[RequireComponent(typeof(PlayerAbilities))]
public class PlayerAction : MonoBehaviour
{
    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        pAttr = R.Player.Attribute;
        pab = GetComponent<PlayerAbilities>();
        weapon = GetComponent<Claymore>();
        msac = GetComponent<MultiSpineAnimationController>();
        listener = GetComponent<PlayerAnimEventListener>();
        StateInit();
    }

    private void OnEnable()
    {
        EGameEvent.EnhanceLevelup.Register(OnEnhancementLevelUp);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        main = this;
        ChangeState(StateEnum.Idle);
        AttributeInit();
        ShadowInit();
    }

    private void OnDisable()
    {
        EGameEvent.EnhanceLevelup.UnRegister(OnEnhancementLevelUp);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public Transform executeFollow => !spATK.enabled ? sword : leftHand;

    //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public event EventHandler OnPlayerTurnRound;

    public bool NotAllowPassSceneGate => stateMachine.currentState.IsInArray(HurtSta) ||
                                         stateMachine.currentState.IsInArray(ExecuteSta);

    public bool IsInNormalState()
    {
        return stateMachine.currentState.IsInArray(NormalSta);
    }

    private void StateInit()
    {
        stateMachine.AddStates(typeof(StateEnum));
        stateMachine.OnEnter += OnMyStateEnter;
        stateMachine.OnTransfer += OnStateTransfer;
    }

    private void AttributeInit()
    {
        pAttr.ResetData();
        TurnRound(1);
    }

    private void ShadowInit()
    {
        shadow = Instantiate(shadowPrefab);
        shadow.GetComponent<ShadowControl>().SetTarget(transform);
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode loadSceneMode)
    {
        shadow = Instantiate(shadowPrefab);
        shadow.GetComponent<ShadowControl>().SetTarget(transform);
        shadow.gameObject.SetActive(true);
        if (stateMachine.currentState.IsInArray(HurtSta))
        {
            ChangeState((!pAttr.isOnGround) ? StateEnum.Fall1 : StateEnum.Idle);
        }
    }

    private void Update()
    {
        if (pAttr.isDead) return;
        if (stateMachine.currentState.IsInArray(NormalSta) && !pAttr.isOnGround)
            ChangeState(StateEnum.Fall1);
    }

    private void OnStateTransfer(object sender, StateMachine.TransferEventArgs args)
    {
        canChangeAnim = false;
        listener.BoxSizeRecover();
        if (args.nextState.IsInArray(HurtSta))
        {
            weapon.AirAttackReset();
            listener.StopIEnumerator("FlashPositionSet");
            listener.isFalling = false;
            listener.airAtkDown = false;
            listener.checkFallDown = false;
        }

        if (!args.lastState.IsInArray(NormalSta) && args.nextState.IsInArray(NormalSta))
        {
            listener.PhysicReset();
            weapon.AirAttackReset();
            if (!GetComponent<Collider2D>().enabled)
            {
                GetComponent<Collider2D>().enabled = true;
            }
        }

        if (args.nextState == "UnderAtkHitToFly")
        {
            listener.hitJump = false;
            StartCoroutine(listener.HitJumpBack());
        }

        if (pab.hurt.DeadFlag)
        {
            GetComponent<ChangeSpineColor>().TurnOffAll();
        }
        else if (args.nextState.IsInArray(NormalSta) || args.nextState.IsInArray(JumpSta) ||
                 args.nextState.IsInArray(FlySta))
        {
            GetComponent<ChangeSpineColor>().TurnOnBreatheLight();
        }
        else
        {
            GetComponent<ChangeSpineColor>().TurnOnEmission();
        }
    }

    private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
    {
        if (pab.hurt.DeadFlag && !args.state.IsInArray(DieSta))
        {
            return;
        }

        string state = args.state;
        switch (state)
        {
            case "AirAtk1":
            case "AirAtk2":
            case "AirAtk3":
            case "AirAtk4":
            case "Atk1":
            case "Atk2":
            case "Atk3":
            case "Atk4":
            case "Atk5":
            case "Atk7":
            case "Atk8":
            case "Charge1Ready":
            case "Charge1End":
            case "Atk11":
            case "Atk12":
            case "Atk13":
            case "Atk14":
            case "Atk23":
            case "Atk15":
            case "AirAtk6":
            case "AirAtk7":
            case "RiderQTEHurt_3":
            case "AirAtkHv1":
            case "AirAtkHv2":
            case "AirAtkHv3":
            case "AirAtkHv5":
            case "AtkHv1":
            case "AtkHv2":
            case "AtkHv3":
            case "AtkUpRising":
            case "Atk16":
            case "AirAtkHv1Push":
            case "AtkHv1Push":
            case "NewExecute1_2":
            case "RiderQTEHurt_2":
            case "NewExecute2_1":
            case "NewExecute2_2":
            case "NewExecuteAir1_1":
            case "NewExecuteAir2_2":
            case "FlashAttack":
            case "AirFlashAttack":
            case "DahalAtkUpRising":
            case "BeelzebubQTEDie":
            case "QTEPush":
            case "AirChargeEnd":
            case "AirQTEPush":
            case "AtkHv4":
            case "QTECharge1Ready":
            case "QTECharge1End":
                msac.Play(args.state, SkeletonType.Attack, false, true, animSpeed);
                break;
            case "Charging1":
            case "QTECharging1":
                msac.Play(args.state, SkeletonType.Attack, true, false, animSpeed);
                break;
            case "AirShootReady":
            case "Flash2":
            case "RollJump":
            case "HitGround":
            case "HitGround2":
            case "ShootReady":
            case "Execute":
            case "ExecuteToIdle":
            case "Execute2":
            case "Execute2ToFall":
            case "Flash1":
            case "AirCallSkill":
            case "CallSkill":
            case "CallSkillAir":
            case "Shoot":
            case "AirShoot":
            case "FlashDown1":
            case "FlashDown2":
            case "FlashUp1":
            case "FlashUp2":
            case "RollReady":
            case "BladeStormReady":
            case "RollEnd":
            case "RollGround":
            case "NewExecute1_1":
            case "RiderQTEHurt_1":
            case "BeelzebubQTECatch":
            case "NewExecute2_0":
            case "NewExecuteAir1_2":
            case "NewExecuteAir2_1":
            case "AirAtkRoll":
            case "AirAtkRollReady":
            case "AtkRollEnd":
            case "AtkRollReady":
            case "FlashDown45_1":
            case "FlashDown45_2":
            case "FlashUp45_1":
            case "FlashUp45_2":
            case "RollEndFrame":
            case "RollFrameEnd":
            case "AtkFlashRollEnd":
            case "QTEHitGround":
            case "QTEHitGround2":
            case "QTERollEnd":
                msac.Play(args.state, SkeletonType.SpAttack, false, true, animSpeed);
                break;
            case "Roll":
            case "BladeStorm":
            case "DahalRoll":
            case "QTERoll":
                msac.Play(args.state, SkeletonType.SpAttack, true, false, animSpeed);
                break;
            case "EndAtk":
            case "Fall1":
            case "GetUp":
            case "Jump":
            case "Jump2":
            case "RunSlow":
            case "FlashGround":
            case "IdleToDefense":
            case "FallToDefenseAir":
            case "QTEEndAtk":
                msac.Play(args.state, SkeletonType.Normal, false, true, animSpeed);
                break;
            case "Fall2":
            case "Idle":
            case "Ready":
            case "Run":
            case "Defense":
            case "DefenseAir":
            case "QTEReady":
                msac.Play(args.state, SkeletonType.Normal, true, false, animSpeed);
                break;
            case "UnderAtk1":
            case "UnderAtkHitGround":
            case "UnderAtkHitToFly":
            case "UnderAtkGetUp":
            case "UnderAtkFlyToFall":
            case "UnderAtkBombKillerII":
                msac.Play(args.state, SkeletonType.Hurt, false, true, animSpeed);
                break;
            case "UnderAtkJumper":
            case "UnderAtkEat":
            case "UnderAtkHitSaw":
            case "Disappear":
                msac.Play(args.state, SkeletonType.Hurt, true, false, animSpeed);
                break;
            case "UpRising":
                msac.Play(args.state, SkeletonType.UpRising, false, true, animSpeed);
                break;
            case "Atk6":
            case "AirCombo":
            case "AirComboFlash":
                msac.Play(args.state, SkeletonType.HeavyAttack, false, true, animSpeed);
                break;
            case "DoubleFlash":
            case "DoubleFlashAir":
                msac.Play(args.state, SkeletonType.HeavyAttack, false, true, 1.5f);
                break;
            case "AirCharging":
                msac.Play(args.state, SkeletonType.HeavyAttack, true, false, animSpeed);
                break;
        }
    }

    /// <summary>
    /// 转向
    /// </summary>
    /// <param name="dir"></param>
    public void TurnRound(int dir)
    {
        Vector3 localScale = transform.localScale;
        if (OnPlayerTurnRound != null)
        {
            OnPlayerTurnRound(this, null);
        }

        if (dir == -1)
        {
            pAttr.faceDir = dir;
            localScale.x = 1f * Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }
        else if (dir == 1)
        {
            pAttr.faceDir = dir;
            localScale.x = -1f * Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }
    }

    public void ChangeState(StateEnum sta, float speed = 1f)
    {
        animSpeed = speed;
        stateMachine.SetState(State[(int)sta]);
    }

    public void ChangeState(string sta, float speed = 1f)
    {
        animSpeed = speed;
        stateMachine.SetState(sta);
    }

    /// <summary>
    /// 显示重置
    /// </summary>
    public void FlashReset()
    {
    }

    /// <summary>
    /// 吸收能量球
    /// </summary>
    public void AbsorbEnergyBall()
    {
        R.Audio.PlayEffect(43, transform.position);
        if (pAttr.isInCharging && R.Player.EnhancementSaveData.Charging >= 1)
        {
            absorbNum++;
            weapon.AddChargeLevel();
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    public static void Reborn()
    {
        EGameEvent.Assessment.Trigger((R.Player.Action, new AssessmentEventArgs(AssessmentEventArgs.EventType.ContinueGame)));
        PlayerAttribute attribute = R.Player.Attribute;
        attribute.AllAttributeRecovery();
        R.Player.Action.pab.hurt.DeadFlag = false;
        R.Player.Action.ChangeState(StateEnum.Idle);
    }

    public static void Reset()
    {
        PlayerAttribute attribute = R.Player.Attribute;
        attribute.ResetData();
        R.Player.Action.pab.hurt.DeadFlag = false;
        R.Player.Action.ChangeState(StateEnum.Idle);
    }

    public void HPRecover(int recoverNum)
    {
        pAttr.currentHP += recoverNum;
    }

    private void OnEnhancementLevelUp(object udata)
    {
        EnhanceArgs msg = (EnhanceArgs)udata;
        string name = msg.Name;
        if (name != null)
        {
            if (name == "maxHP")
            {
                pAttr.maxHP = DB.Enhancements["maxHP"].GetEnhanceEffect(msg.UpToLevel);
                pAttr.currentHP = pAttr.maxHP;
            }
        }
    }

    public void QTEHPRecover(bool fullRecover = false)
    {
        Input.Vibration.Vibrate(8);
        float num = 0f;
        int recover = R.Player.EnhancementSaveData.Recover;
        if (recover != 1)
        {
            if (recover != 2)
            {
                if (recover == 3)
                {
                    num = 0.3f;
                }
            }
            else
            {
                num = 0.25f;
            }
        }
        else
        {
            num = 0.2f;
        }

        if (fullRecover)
        {
            pAttr.currentHP = pAttr.maxHP;
        }
        else
        {
            pAttr.currentHP += (int)(pAttr.maxHP * num);
        }

        pAttr.currentEnergy = pAttr.maxEnergy;
    }

    private const int LEFT = -1;

    private const int RIGHT = 1;

    private const int STOP = 0;

    private const int CURRENT = 3;

    public bool canChangeAnim;

    public StateMachine stateMachine;

    [SerializeField] private Transform leftHand;

    [SerializeField] private Transform sword;

    [SerializeField] private MeshRenderer spATK;

    public static PlayerAction main;

    public int tempDir;

    public Transform shadow;

    private MultiSpineAnimationController msac;

    private PlayerAnimEventListener listener;

    private PlayerAttribute pAttr;

    public PlayerAbilities pab;

    private Claymore weapon;

    [SerializeField] public ParticleSystem blockPartical;

    [SerializeField] private Transform shadowPrefab;

    public Transform hurtBox;

    private float animSpeed = 1f;

    public int absorbNum;

    private static readonly string[] State =
    {
        "EndAtk",
        "Fall1",
        "Fall2",
        "GetUp",
        "Idle",
        "Jump",
        "Jump2",
        "Ready",
        "Run",
        "RunSlow",
        "AirAtk1",
        "AirAtk2",
        "AirAtk3",
        "Atk1",
        "Atk2",
        "Atk3",
        "Atk4",
        "Atk5",
        "AirShoot",
        "AirShootReady",
        "Flash1",
        "Flash2",
        "HitGround",
        "HitGround2",
        "Shoot",
        "ShootReady",
        "UnderAtk1",
        "UnderAtkFlyToFall",
        "UnderAtkHitGround",
        "UnderAtkHitToFly",
        "Atk6",
        "Atk7",
        "Atk8",
        "UpRising",
        "UnderAtkGetUp",
        "Execute",
        "AirAtk4",
        "Charge1Ready",
        "Charging1",
        "Charge1End",
        "UnderAtkJumper",
        "UnderAtkEat",
        "RollJump",
        "UnderAtkHitSaw",
        "AirCallSkill",
        "CallSkill",
        "CallSkillAir",
        "Atk11",
        "Atk12",
        "Atk13",
        "Atk14",
        "Atk23",
        "Atk15",
        "FlashDown1",
        "FlashDown2",
        "FlashUp1",
        "FlashUp2",
        "IdleToDefense",
        "Defense",
        "FallToDefenseAir",
        "DefenseAir",
        "AirAtk6",
        "AirAtkHv1",
        "AirAtkHv2",
        "AirAtkHv3",
        "AirAtkHv4",
        "AirAtkHv5",
        "AtkHv1",
        "AtkHv2",
        "AtkHv3",
        "AtkUpRising",
        "Atk16",
        "AtkHv1Push",
        "AirAtkHv1Push",
        "Execute2",
        "RollReady",
        "Roll",
        "RollEnd",
        "RollGround",
        "ExecuteToIdle",
        "Execute2ToFall",
        "FlashGround",
        "NewExecute1_1",
        "NewExecute1_2",
        "NewExecute2_1",
        "NewExecute2_2",
        "NewExecuteAir1_1",
        "NewExecuteAir1_2",
        "NewExecuteAir2_1",
        "NewExecuteAir2_2",
        "AtkRollReady",
        "AtkRollEnd",
        "AirAtkRollReady",
        "AirAtkRoll",
        "FlashAttack",
        "AirComboFlash",
        "AirCombo",
        "AirFlashAttack",
        "NewExecute2_0",
        "AirAtk7",
        "Disappear",
        "FlashDown45_1",
        "FlashDown45_2",
        "FlashUp45_1",
        "FlashUp45_2",
        "DahalAtkUpRising",
        "DahalRoll",
        "BeelzebubQTECatch",
        "BeelzebubQTEDie",
        "QTEPush",
        "RiderQTEHurt_1",
        "RiderQTEHurt_2",
        "RiderQTEHurt_3",
        "BladeStormReady",
        "BladeStorm",
        "AirCharging",
        "AirChargeEnd",
        "DoubleFlash",
        "DoubleFlashAir",
        "RollEndFrame",
        "RollFrameEnd",
        "AtkFlashRollEnd",
        "UnderAtkBombKillerII",
        "QTEHitGround",
        "QTEHitGround2",
        "QTERoll",
        "QTERollEnd",
        "AirQTEPush",
        "AtkHv4",
        "QTECharge1Ready",
        "QTECharging1",
        "QTECharge1End",
        "QTEEndAtk",
        "QTEReady"
    };

    public static readonly string[] AttackSta =
    {
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
        "AtkHv1",
        "AtkHv2",
        "AtkHv3",
        "Atk16",
        "AtkHv1Push",
        "AtkRollReady",
        "AtkRollEnd",
        "DoubleFlash"
    };

    public static readonly string[] FlySta =
    {
        "Fall1",
        "Fall2",
        "Flash2",
        "FlashDown2",
        "FlashUp2"
    };

    public static readonly string[] UpRisingSta =
    {
        "UpRising",
        "AtkUpRising"
    };

    public static readonly string[] JumpSta =
    {
        "Jump",
        "Jump2",
        "RollJump"
    };

    public static readonly string[] AirAttackSta =
    {
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
        "AirAtkRollReady",
        "AirAtkRoll",
        "DoubleFlashAir"
    };

    public static readonly string[] AirLightAttackSta =
    {
        "AirAtk1",
        "AirAtk2",
        "AirAtk6"
    };

    public static readonly string[] NormalSta =
    {
        "EndAtk",
        "GetUp",
        "Idle",
        "Ready",
        "Run",
        "RunSlow"
    };

    public static readonly string[] HitGroundSta =
    {
        "HitGround",
        "HitGround2",
        "RollReady",
        "Roll",
        "RollEnd",
        "RollEndFrame",
        "RollFrameEnd"
    };

    public static readonly string[] FlashAttackSta =
    {
        "Flash1",
        "FlashDown1",
        "FlashUp1",
        "FlashGround",
        "FlashDown45_1",
        "FlashDown45_2",
        "FlashUp45_1",
        "FlashUp45_2"
    };

    public static readonly string[] ChargeSta =
    {
        "Charge1Ready",
        "Charging1",
        "Charge1End",
        "AirCharging",
        "AirChargeEnd"
    };

    public static readonly string[] HurtSta =
    {
        "UnderAtk1",
        "UnderAtkFlyToFall",
        "UnderAtkHitGround",
        "UnderAtkHitToFly",
        "UnderAtkGetUp",
        "UnderAtkJumper",
        "UnderAtkEat",
        "UnderAtkHitSaw",
        "UnderAtkBombKillerII"
    };

    public static readonly string[] SpHurtSta =
    {
        "UnderAtkJumper",
        "UnderAtkEat",
        "UnderAtkHitSaw",
        "UnderAtkBombKillerII"
    };

    public static readonly string[] DieSta =
    {
        "UnderAtkFlyToFall",
        "UnderAtkHitGround",
        "UnderAtkHitToFly"
    };

    public static readonly string[] ExecuteSta =
    {
        "Execute",
        "Execute2",
        "NewExecute1_1",
        "NewExecute1_2",
        "NewExecute2_1",
        "NewExecute2_2",
        "NewExecuteAir1_1",
        "NewExecuteAir1_2",
        "NewExecuteAir2_1",
        "NewExecuteAir2_2",
        "NewExecute2_0",
        "DahalAtkUpRising",
        "DahalRoll",
        "BeelzebubQTECatch",
        "BeelzebubQTEDie",
        "QTEPush",
        "RiderQTEHurt_1",
        "RiderQTEHurt_2",
        "RiderQTEHurt_3",
        "QTEHitGround",
        "QTEHitGround2",
        "QTERoll",
        "QTERollEnd",
        "AirQTEPush",
        "AtkHv4",
        "QTECharge1Ready",
        "QTECharging1",
        "QTECharge1End",
        "QTEEndAtk",
        "QTEReady"
    };

    public static readonly string CanRunSlow = "Run";

    public enum SkeletonType
    {
        Normal,
        Attack,
        SpAttack,
        Hurt,
        UpRising,
        HeavyAttack
    }

    public enum StateEnum
    {
        EndAtk,
        Fall1,
        Fall2,
        GetUp,
        Idle,
        Jump,
        Jump2,
        Ready,
        Run,
        RunSlow,
        AirAtk1,
        AirAtk2,
        AirAtk3,
        Atk1,
        Atk2,
        Atk3,
        Atk4,
        Atk5,
        AirShoot,
        AirShootReady,
        Flash1,
        Flash2,
        HitGround,
        HitGround2,
        Shoot,
        ShootReady,
        UnderAtk1,
        UnderAtkFlyToFall,
        UnderAtkHitGround,
        UnderAtkHitToFly,
        Atk6,
        Atk7,
        Atk8,
        UpRising,
        UnderAtkGetUp,
        Execute,
        AirAtk4,
        Charge1Ready,
        Charging1,
        Charge1End,
        UnderAtkJumper,
        UnderAtkEat,
        RollJump,
        UnderAtkHitSaw,
        AirCallSkill,
        CallSkill,
        CallSkillAir,
        Atk11,
        Atk12,
        Atk13,
        Atk14,
        Atk23,
        Atk15,
        FlashDown1,
        FlashDown2,
        FlashUp1,
        FlashUp2,
        IdleToDefense,
        Defense,
        FallToDefenseAir,
        DefenseAir,
        AirAtk6,
        AirAtkHv1,
        AirAtkHv2,
        AirAtkHv3,
        AirAtkHv4,
        AirAtkHv5,
        AtkHv1,
        AtkHv2,
        AtkHv3,
        AtkUpRising,
        Atk16,
        AtkHv1Push,
        AirAtkHv1Push,
        Execute2,
        RollReady,
        Roll,
        RollEnd,
        RollGround,
        ExecuteToIdle,
        Execute2ToFall,
        FlashGround,
        NewExecute1_1,
        NewExecute1_2,
        NewExecute2_1,
        NewExecute2_2,
        NewExecuteAir1_1,
        NewExecuteAir1_2,
        NewExecuteAir2_1,
        NewExecuteAir2_2,
        AtkRollReady,
        AtkRollEnd,
        AirAtkRollReady,
        AirAtkRoll,
        FlashAttack,
        AirComboFlash,
        AirCombo,
        AirFlashAttack,
        NewExecute2_0,
        AirAtk7,
        Disappear,
        FlashDown45_1,
        FlashDown45_2,
        FlashUp45_1,
        FlashUp45_2,
        DahalAtkUpRising,
        DahalRoll,
        BeelzebubQTECatch,
        BeelzebubQTEDie,
        QTEPush,
        RiderQTEHurt_1,
        RiderQTEHurt_2,
        RiderQTEHurt_3,
        BladeStormReady,
        BladeStorm,
        AirCharging,
        AirChargeEnd,
        DoubleFlash,
        DoubleFlashAir,
        RollEndFrame,
        RollFrameEnd,
        AtkFlashRollEnd,
        UnderAtkBombKillerII,
        QTEHitGround,
        QTEHitGround2,
        QTERoll,
        QTERollEnd,
        AirQTEPush,
        AtkHv4,
        QTECharge1Ready,
        QTECharging1,
        QTECharge1End,
        QTEEndAtk,
        QTEReady
    }
}