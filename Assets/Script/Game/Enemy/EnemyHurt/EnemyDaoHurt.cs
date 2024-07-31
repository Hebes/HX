using System.Collections;
using UnityEngine;

public class EnemyDaoHurt : EnemyBaseHurt
{
    protected override void Update()
    {
        base.Update();
        Vector2? vector = atkFollowPos;
        if (vector != null)
        {
            Vector3 position = player.transform.position;
            Vector2? vector2 = atkFollowPos;
            Vector3 position2 = position - ((vector2 == null) ? default(Vector3) : vector2.GetValueOrDefault());
            position2.y = Mathf.Clamp(position2.y, LayerManager.YNum.GetGroundHeight(gameObject) + 1f, float.PositiveInfinity);
            position2.z = LayerManager.ZNum.MMiddleE(eAttr.rankType);
            transform.position = position2;
            atkFollowTime += Time.unscaledDeltaTime;
            if (atkFollowTime >= atkFollowEnd)
            {
                atkFollowPos = null;
            }
        }
    }

    protected override void Init()
    {
        _dao = GetComponent<DaoEnemyAnimListener>();
        defaultAnimName = "Hit1";
        defaultAirAnimName = "HitToFly1";
        airDieAnimName = "Fall";
        airDieHitGroundAnimName = "AirDie";
        flyToFallAnimName = "AirDiePre";
        hurtData = SingletonMono<EnemyDataPreload>.Instance.hurt[EnemyType.斩轮式一型];
    }

    public override void SetHitSpeed(Vector2 speed)
    {
        if (playerAtkName == "UpRising" || playerAtkName == "AtkUpRising" || playerAtkName == "AtkRollEnd" || playerAtkName == "NewExecute2_1")
        {
            _dao.maxFlyHeight = 4.5f;
        }
        else
        {
            _dao.maxFlyHeight = -1f;
        }

        base.SetHitSpeed(speed);
    }

    public override void NormalHurt(EnemyHurtAtkEventArgs.PlayerNormalAtkData atkData, int atkId, HurtCheck.BodyType body, Vector2 hurtPos)
    {
        if (action.stateMachine.currentState.IsInArray(DaoAction.SideStepSta))
        {
            return;
        }

        base.NormalHurt(atkData, atkId, body, hurtPos);
    }

    private void RollHurt()
    {
        Vector2 vector = new Vector2(eAttr.faceDir * -8, 12f);
        PlayHurtAnim("HitToFly1", "HitToFly1", vector, vector);
    }

    protected override void SpAttack()
    {
        if (eAttr.currentActionInterruptPoint < eAttr.actionInterruptPoint)
        {
            return;
        }

        if (playerAtkName == "RollEnd")
        {
            Vector2? vector = atkFollowPos;
            if (vector != null)
            {
                atkFollowPos = null;
            }

            return;
        }

        if (playerAtkName == "RollGround")
        {
            Vector2? vector2 = atkFollowPos;
            if (vector2 == null)
            {
                Transform transform = player.GetComponentInChildren<PlayerAtk>().transform;
                atkFollowPos = (transform.position - this.transform.position) * 0.9f;
            }

            atkFollowTime = 0f;
            atkFollowEnd = 0.2f;
            return;
        }

        if (playerAtkName == "RollReady" || playerAtkName == "BladeStormReady")
        {
            Vector2? vector3 = atkFollowPos;
            if (vector3 == null)
            {
                atkFollowPos = (player.transform.position - transform.position) * 0.7f;
            }

            atkFollowTime = 0f;
            atkFollowEnd = 0.2f;
            return;
        }

        if (playerAtkName == "Atk4")
        {
            StartCoroutine(CloseToPlayer());
            return;
        }

        atkFollowTime = 0f;
        atkFollowEnd = 0f;
    }

    private IEnumerator CloseToPlayer()
    {
        closeToPlayer.state.SetAnimation(0, "Show", false);
        closeToPlayer.skeleton.SetToSetupPose();
        Vector3 endPos = player.transform.position + Vector3.right * (pAttr.faceDir * Random.Range(0.8f, 1.5f));
        endPos.z = LayerManager.ZNum.MMiddleE(eAttr.rankType);
        Vector3 startPos = transform.position;
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForFixedUpdate();
        }

        for (int j = 0; j < 6; j++)
        {
            transform.position = Vector3.Lerp(startPos, endPos, j / 5f);
            yield return new WaitForFixedUpdate();
        }
    }

    protected override void ExecuteFollow()
    {
        action.AnimChangeState(DaoAction.StateEnum.Execute);
        base.ExecuteFollow();
    }

    protected override void PlayHurtAudio()
    {
        R.Audio.PlayEffect(401, transform.position);
        if (PlaySpHurtAudio())
        {
            R.Audio.PlayEffect(Random.Range(57, 59), transform.position);
        }
    }

    protected override void ExecuteDie()
    {
        base.ExecuteDie();
        eAttr.timeController.SetGravity(1f);
        SetHitSpeed(Vector2.zero);
        SpDieEffect();
    }

    public override void EnemyDie()
    {
        if (deadFlag)
        {
            return;
        }

        base.EnemyDie();
        if (eAttr.isOnGround)
        {
            SetHitSpeed(Vector2.zero);
            NormalKill();
            action.AnimChangeState("Die");
            Invoke("DieTimeControl", 0.12f);
        }
        else
        {
            NormalKill();
            DieTimeControl();
            StartCoroutine(DeathIEnumerator());
        }
    }

    private void SpDieEffect()
    {
        DaoAction daoAction = (DaoAction)action;
        Transform transform = R.Effect.Generate(219, null, center.position, Vector3.zero);
        transform.localScale = this.transform.localScale;
        transform.GetComponent<SkeletonAnimation>().skeleton.SetSkin((!daoAction.isPao) ? "DaoBrother" : "PaoSister");
        Transform transform2 = R.Effect.Generate(220, null, center.position, Vector3.zero);
        transform2.localScale = this.transform.localScale;
        action.AnimChangeState(DaoAction.StateEnum.Null);
        string playerAtkName = this.playerAtkName;
        if (playerAtkName != null)
        {
            if (!(playerAtkName == "NewExecute1_2") && !(playerAtkName == "NewExecute2_2") && !(playerAtkName == "NewExecuteAir2_2"))
            {
                if (playerAtkName == "NewExecuteAir1_2")
                {
                    transform.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(this.transform.localScale.x) * 2f, 15f);
                    transform2.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(this.transform.localScale.x) * -2f, 15f);
                }
            }
            else
            {
                transform.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(this.transform.localScale.x) * 2f, 15f);
                transform2.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(this.transform.localScale.x) * -2f, -5f);
            }
        }
    }

    protected override IEnumerator DeathIEnumerator()
    {
        bool deadFly = true;
        bool deadFall = false;
        while (eAttr.isDead)
        {
            if (deadFly && eAttr.timeController.GetCurrentSpeed().y <= 0f)
            {
                deadFly = false;
                deadFall = true;
                action.AnimChangeState(flyToFallAnimName);
            }

            if (deadFall && eAttr.isOnGround)
            {
                deadFall = false;
                action.AnimChangeState(airDieHitGroundAnimName);
                Invoke("RealDie", 0.5f);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void RealDie()
    {
        GetComponent<DaoEnemyAnimListener>().GetUp();
    }

    private DaoEnemyAnimListener _dao;

    private Vector2? atkFollowPos;

    private float atkFollowTime;

    private float atkFollowEnd;

    [SerializeField] private SkeletonAnimation closeToPlayer;
}