using System.Collections.Generic;
using UnityEngine;

public class JumperFooterAction : EnemyBaseAction
{
    static Dictionary<string, int> _003C_003Ef__switch_0024map11;

    protected override void Start()
    {
        this.stateMachine.AddStates(typeof(JumperFooterAction.StateEnum));
        this.stateMachine.OnEnter += this.OnMyStateEnter;
        this.stateMachine.OnTransfer += this.OnStateTransfer;
        base.AnimChangeState(JumperFooterAction.StateEnum.Idle, 1f);
    }

    protected override void Update()
    {
        base.Update();

        if (this.Atk2Success)
        {
            Vector3 position = base.transform.position;
            position.z = LayerManager.ZNum.TempEnemy;
            base.transform.position = position;
            base.player.position = new Vector3(this.catachPos.position.x, this.catachPos.position.y, base.player.position.z);
            Vector3 eulerAngles = this.catachPos.localRotation.eulerAngles;
            eulerAngles.z *= (float)(-(float)this.eAttr.faceDir);
            base.player.localRotation = Quaternion.Euler(eulerAngles);
        }

        if (this.IsInDefenceState() && Time.time - this.startDefenceTime >= this.defenceTime)
        {
            base.AnimChangeState(JumperFooterAction.StateEnum.DefenseStateToIdle, 1f);
        }
    }

    private void OnMyStateEnter(object sender, StateMachine.StateEventArgs args)
    {
        string state = args.state;
        if (state != null)
        {
            if (JumperFooterAction._003C_003Ef__switch_0024map11 == null)
            {
                JumperFooterAction._003C_003Ef__switch_0024map11 = new Dictionary<string, int>(20)
                {
                    {
                        "Atk1",
                        0
                    },
                    {
                        "Atk2Fail",
                        0
                    },
                    {
                        "Atk2Ready",
                        0
                    },
                    {
                        "Atk2Success",
                        0
                    },
                    {
                        "Atk3",
                        0
                    },
                    {
                        "Defense",
                        0
                    },
                    {
                        "DefenseStateToIdle",
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
                        "HitQTE",
                        0
                    },
                    {
                        "IdleToDefenseState",
                        0
                    },
                    {
                        "Jump",
                        0
                    },
                    {
                        "HitToWeakMod",
                        0
                    },
                    {
                        "WeakModToIdle",
                        0
                    },
                    {
                        "Die",
                        0
                    },
                    {
                        "Idle",
                        1
                    },
                    {
                        "DefenseState",
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
            if (JumperFooterAction._003C_003Ef__switch_0024map11.TryGetValue(state, out num))
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
        base.GetComponent<EnemyBaseHurt>().StopFollowLeftHand();
        if (!args.nextState.IsInArray(JumperFooterAction.AttackSta) && this.Atk2Success)
        {
            base.GetComponent<JumperFooterAnimEvent>().Atk2Release();
        }

        if (this.ExitAtkSta(args.lastState, args.nextState))
        {
            this.eAttr.paBody = false;
            this.atkBox.localScale = Vector3.zero;
        }

        if (args.lastState == "Atk3" || args.lastState == "Jump")
        {
            this.eAttr.timeController.SetGravity(1f);
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
        base.AnimChangeState(JumperFooterAction.StateEnum.Atk1, 1f);
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
        base.AnimChangeState(JumperFooterAction.StateEnum.Atk2Ready, 1f);
    }

    public void Attack2Success()
    {
        this.Atk2Success = true;
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
        base.AnimChangeState(JumperFooterAction.StateEnum.Atk3, 1f);
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

        if (this.stateMachine.currentState.IsInArray(JumperFooterAction.QTESta) || this.IsInWeakSta())
        {
            return;
        }

        R.Effect.Generate(128, base.transform, Vector3.up * 3.3f, Vector3.zero, default(Vector3), true);
        base.ChangeFace(dir);
        base.AnimChangeState(JumperFooterAction.StateEnum.Atk2Ready, 1f);
    }

    public override void KillSelf()
    {
        SingletonMono<WorldTime>.Instance.TimeFrozenByFixedFrame(25, WorldTime.FrozenArgs.FrozenType.Enemy, true);
        R.Camera.Controller.CameraShake(0.416666657f, 0.3f, CameraController.ShakeTypeEnum.Rect, false);
        base.GetComponent<JumperFooterHurt>().NormalKill();
        this.eAttr.currentHp = 0;
        base.AnimChangeState(JumperFooterAction.StateEnum.Die, 1f);
    }

    public override void AnimMove()
    {
        base.AnimChangeState(JumperFooterAction.StateEnum.Move, 1f);
    }

    public override void AnimReady()
    {
        base.AnimChangeState(JumperFooterAction.StateEnum.Idle, 1f);
    }

    public override void Defence()
    {
        if (this.eAttr.isDead)
        {
            return;
        }

        if (this.IsInDefenceState())
        {
            return;
        }

        if (this.IsInWeakSta())
        {
            return;
        }

        if (!this.eAttr.isOnGround)
        {
            return;
        }

        if (this.stateMachine.currentState == "DieQTE" || this.stateMachine.currentState == "HitQTE")
        {
            return;
        }

        base.Defence();
        this.eAttr.timeController.SetSpeed(Vector2.zero);
        this.startDefenceTime = Time.time;
        this.defenceTime = UnityEngine.Random.Range(2f, 4f);
        base.AnimChangeState(JumperFooterAction.StateEnum.DefenseState, 1f);
    }

    public override void DefenceSuccess()
    {
        if (this.eAttr.isDead)
        {
            return;
        }

        if (!this.eAttr.isOnGround)
        {
            return;
        }

        if (this.IsInWeakSta())
        {
            return;
        }

        R.Audio.PlayEffect(406, new Vector3?(base.transform.position));
        base.AnimChangeState(JumperFooterAction.StateEnum.Defense, 1f);
    }

    public void Jump()
    {
        if (this.eAttr.isDead)
        {
            return;
        }

        if (!this.IsInNormalState())
        {
            return;
        }

        this.eAttr.timeController.SetGravity(0f);
        this.eAttr.timeController.SetSpeed(Vector2.zero);
        base.AnimChangeState(JumperFooterAction.StateEnum.Jump, 1f);
    }

    public override bool IsInNormalState()
    {
        return this.stateMachine.currentState.IsInArray(JumperFooterAction.NormalSta) && base.IsInNormalState();
    }

    public override bool IsInAttackState()
    {
        return this.stateMachine.currentState.IsInArray(JumperFooterAction.AttackSta);
    }

    public override bool IsInDeadState(string state)
    {
        return state.IsInArray(JumperFooterAction.DieSta);
    }

    public override bool IsInWeakSta()
    {
        return this.eAttr.inWeakState;
    }

    protected override bool EnterAtkSta(string lastState, string nextState)
    {
        return nextState.IsInArray(JumperFooterAction.AttackSta) && !lastState.IsInArray(JumperFooterAction.AttackSta);
    }

    protected override bool ExitAtkSta(string lastState, string nextState)
    {
        return !nextState.IsInArray(JumperFooterAction.AttackSta) && lastState.IsInArray(JumperFooterAction.AttackSta);
    }

    public override bool IsInDefenceState()
    {
        return this.stateMachine.currentState.IsInArray(JumperFooterAction.DefenseSta);
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
        base.AnimChangeState(JumperFooterAction.StateEnum.HitQTE, 1f);
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
        base.AnimChangeState(JumperFooterAction.StateEnum.DieQTE, 1f);
    }

    [SerializeField] private Transform catachPos;

    public bool Atk2Success;

    public bool Atk2Result;

    private static readonly string[] AttackSta = new string[]
    {
        "Atk1",
        "Atk2Ready",
        "Atk2Success",
        "Atk2Fail",
        "Atk3"
    };

    public static readonly string[] HurtSta = new string[]
    {
        "Hit1",
        "Hit2",
        "HitQTE"
    };

    private static readonly string[] NormalSta = new string[]
    {
        "Idle",
        "Move",
        "Jump"
    };

    private static readonly string[] DefenseSta = new string[]
    {
        "Defense",
        "DefenseState",
        "IdleToDefenseState"
    };

    private static readonly string[] DieSta = new string[]
    {
        "Die",
        "DieQTE"
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
        Atk2Success,
        Atk2Fail,
        Atk3,
        Defense,
        DefenseState,
        DefenseStateToIdle,
        DieQTE,
        Hit1,
        Hit2,
        IdleToDefenseState,
        Jump,
        HitQTE,
        WeakMod,
        HitToWeakMod,
        WeakModToIdle,
        Die
    }
}