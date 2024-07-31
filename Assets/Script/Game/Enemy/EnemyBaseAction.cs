using System;
using Framework.Core;
using UnityEngine;

/// <summary>
/// 敌人基础行动
/// </summary>
public abstract class EnemyBaseAction : MonoBehaviour
{
    public Transform player => R.Player.Transform;

    private bool inStiff => eAttr.stiffTime > 0f;

    protected virtual void Awake()
    {
        spineAnim = GetComponent<SpineAnimationController>();
        eAttr = GetComponent<EnemyAttribute>();
        stateMachine = GetComponent<StateMachine>();
        if (shadowPrefab)
        {
            shadow = Instantiate(shadowPrefab);
            shadow.GetComponent<ShadowControl>().SetTarget(transform);
        }
    }

    protected abstract void Start();

    protected virtual void Update()
    {
        eAttr.stiffTime = Mathf.Clamp(eAttr.stiffTime - Time.deltaTime, 0f, float.PositiveInfinity);
        if (QTE)
        {
            QTE.transform.localScale = ((transform.localScale.x >= 0f)
                ? new Vector3(1f, 1f, 1f)
                : new Vector3(-1f, 1f, 1f));
        }

        if (IsInWeakSta() && weakEffectShow)
        {
            QTEAnim((!CurrentCanBeExecute()) ? "Null" : "Show");
        }

        if (!IsInNormalState())
        {
            isAutoMoveing = false;
        }

        if (!isAutoMoveing || SingletonMono<WorldTime>.Instance.IsFrozen)
        {
            return;
        }

        float y = 0f;
        if (eAttr.iCanFly)
        {
            float distance = Physics2D.Raycast(transform.position + Vector3.right * eAttr.faceDir,
                -Vector2.up, 1000f, LayerManager.GroundMask).distance;
            if (Mathf.Abs(distance - eAttr.flyHeight) > 0.1f)
            {
                float num = Mathf.Abs(distance - eAttr.flyHeight) / (1f / eAttr.moveSpeed);
                y = num * Time.deltaTime * Mathf.Sign(eAttr.flyHeight - distance);
            }
        }

        float num2 = Mathf.Abs(eAttr.moveSpeed * Time.deltaTime);
        transform.position += new Vector3(num2 * eAttr.faceDir, y, 0f);
        if (!eAttr.iCanFly)
        {
            SetToGroundPos();
        }
    }

    public void AnimChangeState(string state, float speed = 1f)
    {
        if (eAttr.isDead && !IsInDeadState(state))
        {
            return;
        }

        animSpeed = speed;
        stateMachine.SetState(state);
    }

    public void AnimChangeState(Enum nextState, float speed = 1f)
    {
        AnimChangeState(nextState.ToString(), speed);
    }

    public virtual bool IsInNormalState()
    {
        return !eAttr.isDead && !inStiff && !IsInWeakSta();
    }

    public bool CurrentCanBeExecute()
    {
        bool flag = IsInWeakSta() && (eAttr.rankType == EnemyAttribute.RankType.Normal ||
                                           (eAttr.rankType != EnemyAttribute.RankType.Normal &&
                                            eAttr.isOnGround));
        bool flag2;
        if (R.Player.Attribute.isOnGround)
        {
            flag2 = (eAttr.accpectExecute &&
                     (eAttr.isOnGround ||
                      (!eAttr.isOnGround && eAttr.timeController.GetCurrentSpeed().y < 0f)) &&
                     !eAttr.isDead);
        }
        else
        {
            flag2 = (eAttr.accpectAirExecute && !eAttr.isDead);
        }

        bool flag3 = Vector2.Distance(transform.position, R.Player.Transform.position) <= 4f;
        return flag && flag2 && flag3;
    }

    public abstract bool IsInDeadState(string state);

    public void ChangeFace(int dir)
    {
        if (eAttr.faceDir == dir)
        {
            return;
        }

        eAttr.faceDir = dir;
        if (dir != -1)
        {
            if (dir == 1)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x),
                    transform.localScale.y, 1f);
            }
        }
        else
        {
            transform.localScale =
                new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1f);
        }
    }

    public void FaceToPlayer()
    {
        int dir = InputSetting.JudgeDir(transform.position, player.position);
        ChangeFace(dir);
    }

    public bool TurnRound(int dir)
    {
        if (!IsInNormalState())
        {
            return false;
        }

        ChangeFace(dir);
        return true;
    }

    public virtual void KillSelf()
    {
        if (eAttr.rankType == EnemyAttribute.RankType.Normal)
        {
            eAttr.currentHp = 0;
        }
    }

    public void SetToGroundPos()
    {
        if (eAttr.isOnGround)
        {
            return;
        }

        transform.position =
            transform.position.SetY(LayerManager.YNum.GetGroundHeight(gameObject) + 0.2f);
    }

    public bool AutoMove()
    {
        if (isAutoMoveing)
        {
            return true;
        }

        if (IsInNormalState())
        {
            isAutoMoveing = true;
            AnimMove();
            return true;
        }

        return false;
    }

    public bool StopMoveToIdle()
    {
        isAutoMoveing = false;
        if (IsInNormalState())
        {
            AnimReady();
            return true;
        }

        return false;
    }

    public void AppearAtPosition(Vector2 pos)
    {
        transform.position = new Vector3(pos.x, pos.y, LayerManager.ZNum.MMiddleE(eAttr.rankType));
        FaceToPlayer();
    }

    public void AppearEffect(Vector2 pos)
    {
        float num = 3.18f;
        R.Effect.Generate(0, null, new Vector3(pos.x, pos.y + num, LayerManager.ZNum.Fx), Vector3.zero);
        R.Audio.PlayEffect(123, transform.position);
    }

    public abstract void AnimReady();

    public abstract void AnimMove();

    public virtual void Attack1(int dir)
    {
        if (eAttr.isDead)
        {
            return;
        }

        if (!IsInNormalState())
        {
            return;
        }

        ChangeFace(dir);
    }

    public virtual void Attack2(int dir)
    {
        if (eAttr.isDead)
        {
            return;
        }

        if (!IsInNormalState())
        {
            return;
        }

        ChangeFace(dir);
    }

    public virtual void Attack3(int dir)
    {
        if (eAttr.isDead)
        {
            return;
        }

        if (!IsInNormalState())
        {
            return;
        }

        ChangeFace(dir);
    }

    public virtual void Attack4(int dir)
    {
        if (eAttr.isDead)
        {
            return;
        }

        if (!IsInNormalState())
        {
            return;
        }

        ChangeFace(dir);
    }

    public virtual void Attack5(int dir)
    {
        if (eAttr.isDead)
        {
            return;
        }

        if (!IsInNormalState())
        {
            return;
        }

        ChangeFace(dir);
    }

    public virtual void Attack6(int dir)
    {
        if (eAttr.isDead)
        {
            return;
        }

        if (!IsInNormalState())
        {
            return;
        }

        ChangeFace(dir);
    }

    public virtual void Attack7(int dir)
    {
        if (eAttr.isDead)
        {
            return;
        }

        if (!IsInNormalState())
        {
            return;
        }

        ChangeFace(dir);
    }

    public virtual void Attack8(int dir)
    {
        if (eAttr.isDead)
        {
            return;
        }

        if (!IsInNormalState())
        {
            return;
        }

        ChangeFace(dir);
    }

    public virtual void Attack9(int dir)
    {
        if (eAttr.isDead)
        {
            return;
        }

        if (!IsInNormalState())
        {
            return;
        }

        ChangeFace(dir);
    }

    public virtual void Attack10(int dir)
    {
        if (eAttr.isDead)
        {
            return;
        }

        if (!IsInNormalState())
        {
            return;
        }

        ChangeFace(dir);
    }

    public virtual void Attack11(int dir)
    {
        if (eAttr.isDead)
        {
            return;
        }

        if (!IsInNormalState())
        {
            return;
        }

        ChangeFace(dir);
    }

    public virtual void Attack12(int dir)
    {
        if (eAttr.isDead)
        {
            return;
        }

        if (!IsInNormalState())
        {
            return;
        }

        ChangeFace(dir);
    }

    public virtual void Attack13(int dir)
    {
        if (eAttr.isDead)
        {
            return;
        }

        if (!IsInNormalState())
        {
            return;
        }

        ChangeFace(dir);
    }

    public virtual void Attack14(int dir)
    {
        if (eAttr.isDead)
        {
            return;
        }

        if (!IsInNormalState())
        {
            return;
        }

        ChangeFace(dir);
    }

    public virtual void Attack15(int dir)
    {
        if (eAttr.isDead)
        {
            return;
        }

        if (!IsInNormalState())
        {
            return;
        }

        ChangeFace(dir);
    }

    public virtual void CounterAttack(int dir)
    {
    }

    public virtual void Idle1()
    {
        throw new NotImplementedException();
    }

    public virtual void Idle2()
    {
        Idle1();
    }

    public virtual void Idle3()
    {
        Idle1();
    }

    public virtual void Idle4()
    {
        Idle1();
    }

    public virtual void Idle5()
    {
        Idle1();
    }

    public virtual void Defence()
    {
        eAttr.timeController.SetSpeed(Vector2.zero);
        eAttr.currentDefence = 0;
        eAttr.startDefence = false;
    }

    public virtual void DefenceSuccess()
    {
    }

    public virtual void SideStep()
    {
        FaceToPlayer();
        eAttr.currentSideStep = 0;
    }

    public void QTEAnim(string anim)
    {
        QTESetSkin();
        if (!QTE || QTE.AnimationName == anim)
        {
            return;
        }

        QTESetText(anim);
        QTE.state.SetAnimation(0, anim, true);
        QTE.skeleton.SetToSetupPose();
    }

    private void QTESetSkin()
    {
        if (QTE)
        {
            QTE.skeleton.SetSkin("Mobile");
        }   
    }

    private void QTESetText(string anim)
    {
    }

    public void ExitWeakState(bool forceQuit = false)
    {
        if (!eAttr.accpectExecute)
        {
            return;
        }

        if (eAttr.isDead)
        {
            return;
        }

        if (!eAttr.willBeExecute || forceQuit)
        {
            eAttr.inWeakState = false;
            WeakEffectDisappear("RollEnd");
        }
    }

    public void WeakEffectDisappear(string disappear)
    {
        if (!eAttr.accpectExecute)
        {
            return;
        }

        if (weakThunder)
        {
            weakThunder.SetActive(false);
        }

        if (weakEffect)
        {
            weakEffect.state.SetAnimation(0, disappear, false);
            weakEffect.skeleton.SetSlotsToSetupPose();
            weakEffect.Update(0f);
        }

        weakEffectShow = false;
        QTEAnim("Null");
    }

    public virtual void EnterWeakState()
    {
        if (!eAttr.accpectExecute)
        {
            return;
        }

        eAttr.inWeakState = true;
        WeakEffectAppear();
    }

    protected virtual void WeakEffectAppear()
    {
        if (!eAttr.accpectExecute)
        {
            return;
        }

        if (eAttr.isDead)
        {
            return;
        }

        R.Audio.PlayEffect(104, transform.position);
        if (weakThunder)
        {
            weakThunder.SetActive(true);
        }

        if (weakEffect)
        {
            weakEffect.state.SetAnimation(0, "Roll", true);
            weakEffect.skeleton.SetSlotsToSetupPose();
            weakEffect.Update(0f);
        }

        weakEffectShow = true;
        QTEAnim("Show");
    }

    public void PlayEffect(int effectId)
    {
        if (!R.Effect.fxData.ContainsKey(effectId))
        {
            string.Format("敌人 {0} 尝试播放第{1}效果, 但是不存在", name, effectId).Log();
            return;
        }

        EnemyAtkEffector component = R.Effect.fxData[effectId].effect.GetComponent<EnemyAtkEffector>();
        Vector3 position = new Vector3(component.pos.x * transform.localScale.x, component.pos.y, component.pos.z);
        R.Effect.Generate(effectId, transform, position);
    }

    public virtual bool IsInDefenceState()
    {
        return false;
    }

    public virtual bool IsInSideStepState()
    {
        return false;
    }

    public abstract bool IsInWeakSta();

    public abstract bool IsInAttackState();

    protected abstract bool EnterAtkSta(string lastState, string nextState);

    protected abstract bool ExitAtkSta(string lastState, string nextState);

    public virtual bool IsInIdle()
    {
        return false;
    }

    public virtual void AnimExecute()
    {
        Vector3 position = transform.position;
        position.y = LayerManager.YNum.GetGroundHeight(gameObject);
        transform.position = position;
    }

    public virtual void AnimQTEHurt()
    {
        Vector3 position = transform.position;
        position.y = LayerManager.YNum.GetGroundHeight(gameObject);
        transform.position = position;
    }

    public const int LEFT = -1;

    public const int RIGHT = 1;

    public const int STOP = 0;

    [HideInInspector] public StateMachine stateMachine;

    [SerializeField] public Transform hurtBox;

    [HideInInspector] public EnemyAttribute eAttr;

    [SerializeField] protected Transform atkBox;

    [SerializeField] protected SkeletonAnimation weakEffect;

    [SerializeField] protected GameObject weakThunder;

    [SerializeField] protected SkeletonAnimation QTE;

    [SerializeField] private TextMesh _qteTextMesh;

    protected SpineAnimationController spineAnim;

    protected float animSpeed;

    protected float startDefenceTime;

    protected float defenceTime;

    private bool weakEffectShow;

    public bool isAutoMoveing;

    [SerializeField] private Transform shadowPrefab;

    private Transform shadow;
}