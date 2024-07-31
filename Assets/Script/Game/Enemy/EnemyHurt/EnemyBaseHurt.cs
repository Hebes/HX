using System.Collections;
using System.Collections.Generic;
using Framework.Core;
using LitJson;
using UnityEngine;

/// <summary>
/// 敌人基础伤害
/// </summary>
[RequireComponent(typeof(EnemyAttribute))]
[RequireComponent(typeof(EnemyArmor))]
[RequireComponent(typeof(EnemyBaseAction))]
[RequireComponent(typeof(Pivot))]
public class EnemyBaseHurt : MonoBehaviour
{
    /// <summary>
    /// 显示攻击伤害
    /// </summary>
    protected int flashAttackDamage
    {
        get
        {
            if (R.Player.EnhancementSaveData.FlashAttack == 0) return 0;
            return (int)(eAttr.maxHp * 0.05f * Random.Range(0.95f, 1.05f));
        }
    }

    /// <summary>
    /// 显示百分比
    /// </summary>
    protected bool flashPercent
    {
        get
        {
            int num = Random.Range(1, 100);
            int flashAttack = R.Player.EnhancementSaveData.FlashAttack;
            if (flashAttack == 1) return num < 30;
            if (flashAttack != 2) return flashAttack == 3;
            return num < 60;
        }
    }

    /// <summary>
    /// 玩家
    /// </summary>
    protected GameObject player => R.Player.GameObject;

    /// <summary>
    /// 玩家属性
    /// </summary>
    protected PlayerAttribute pAttr => R.Player.Attribute;

    /// <summary>
    /// 相对的HP
    /// </summary>
    public List<int> phaseHp
    {
        get
        {
            if (_phaseHp != null && _phaseHp.Count != 0) return _phaseHp;
            _phaseHp = new List<int>();
            if (hpPercent.Length == 0)
            {
                maxPhase = 1;
                _phaseHp.Add(eAttr.maxHp);
                return _phaseHp;
            }

            maxPhase = hpPercent.Length;
            if (eAttr.rankType != EnemyAttribute.RankType.Normal)
            {
                for (var i = 0; i < hpPercent.Length; i++)
                    _phaseHp.Add(eAttr.maxHp * hpPercent[i] / 100);
            }

            return _phaseHp;
        }
    }

    /// <summary>
    /// 速度
    /// </summary>
    private float speedDir => Mathf.Sign(transform.localScale.x);

    protected void Awake()
    {
        eAttr = GetComponent<EnemyAttribute>();
        armor = GetComponent<EnemyArmor>();
        action = GetComponent<EnemyBaseAction>();
        Pivot = GetComponent<Pivot>();
    }

    protected void Start()
    {
        frameShakeOffset = m_frameShakeOffset;
        spHurtAudio = 6;
        chaseEnd = 0f;
        Init();
    }

    protected void OnEnable()
    {
        EGameEvent.EnemyHurtAtk.Register(EnemyHurtAtk);
    }

    protected void OnDisable()
    {
        EGameEvent.EnemyHurtAtk.UnRegister(EnemyHurtAtk);
    }

    private void OnDestroy()
    {
        QTECameraFinish();
    }

    protected virtual void Update()
    {
        UpdateEnemyDie();
        UpdateExecuteFollow();
        if (deadFlag)
        {
            return;
        }

        UpdateChase();
    }

    private void UpdateEnemyDie()
    {
        if (eAttr.isDead && !deadFlag && eAttr.rankType == EnemyAttribute.RankType.Normal)
        {
            EnemyDie();
        }
    }

    private void UpdateExecuteFollow()
    {
        if (eAttr.followLeftHand)
        {
            Vector3 position = R.Player.Action.executeFollow.position + new Vector3(eAttr.faceDir * centerOffset.x, centerOffset.y, centerOffset.z);
            position.z = LayerManager.ZNum.Fx;
            transform.position = position;
        }
    }

    private void UpdateChase()
    {
        if (eAttr.canBeChased)
        {
            chaseEnd += Time.unscaledDeltaTime;
            if (chaseEnd >= 1.3f)
            {
                ChaseEnd();
            }
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    protected virtual void Init()
    {
    }

    /// <summary>
    /// 敌人受到伤害
    /// </summary>
    /// <param name="udata"></param>
    private void EnemyHurtAtk(object udata)
    {
        var temp = (EnemyHurtAtkEventArgs)udata;
        EnemyHurt(string.Empty, null, temp);
    }

    /// <summary>
    /// QTEZ位置恢复
    /// </summary>
    protected void QTEZPositionRecover()
    {
        Vector3 position = transform.position;
        position.z = LayerManager.ZNum.MMiddleE(eAttr.rankType);
        transform.position = position;
    }

    /// <summary>
    /// 敌人收到伤害
    /// </summary>
    /// <param name="eventName">事件名称</param>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    private bool EnemyHurt(string eventName, object sender, EnemyHurtAtkEventArgs args)
    {
        if (args.hurted != gameObject) return false;
        switch (args.hurtType)
        {
            case EnemyHurtAtkEventArgs.HurtTypeEnum.Normal:
                NormalHurt(args.attackData, args.attackId, args.body, args.hurtPos);
                break;
            case EnemyHurtAtkEventArgs.HurtTypeEnum.ExecuteFollow:
                ExecuteFollow();
                break;
            case EnemyHurtAtkEventArgs.HurtTypeEnum.Execute:
                Execute(args.attackData.atkName);
                break;
            case EnemyHurtAtkEventArgs.HurtTypeEnum.QTEHurt:
                QTEHurt();
                break;
            case EnemyHurtAtkEventArgs.HurtTypeEnum.Flash:
                FlashAttackHurt();
                break;
        }

        return true;
    }

    public IEnumerator ClipShake(int frame)
    {
        for (int i = 0; i < frame; i++)
        {
            if (i % 2 == 0)
                frameShakeBody.localPosition += Vector3.right * frameShakeOffset;
            else
                frameShakeBody.localPosition -= Vector3.right * frameShakeOffset;
            yield return null;
        }

        frameShakeBody.localPosition = Vector3.zero;
    }

    /// <summary>
    /// 时间冻结和相机抖动
    /// </summary>
    /// <param name="frozenFrame"></param>
    /// <param name="frameShakeFrame"></param>
    /// <param name="shakeType"></param>
    /// <param name="shakeFrame"></param>
    /// <param name="shakeOffset"></param>
    protected void TimeFrozenAndCameraShake(int frozenFrame, int frameShakeFrame, int shakeType, int shakeFrame, float shakeOffset)
    {
        WorldTime.FrozenArgs.FrozenType type = playerAtkName != "HitGround"
            ? WorldTime.FrozenArgs.FrozenType.All
            : WorldTime.FrozenArgs.FrozenType.Enemy;
        WorldTime.Instance.TimeFrozenByFixedFrame(frozenFrame, type, true);
        StopCoroutine("ClipShake");
        StartCoroutine(ClipShake(frameShakeFrame));
        if (shakeType != 0)
        {
            if (shakeType != 1)
            {
                if (shakeType == 2)
                {
                    R.Camera.Controller.CameraShake(shakeFrame / 60f, shakeOffset, CameraController.ShakeTypeEnum.Vertical, false);
                }
            }
            else
            {
                R.Camera.Controller.CameraShake(shakeFrame / 60f, shakeOffset, CameraController.ShakeTypeEnum.Horizon, false);
            }
        }
        else
        {
            R.Camera.Controller.CameraShake(shakeFrame / 60f, shakeOffset, CameraController.ShakeTypeEnum.Rect, false);
        }
    }

    public virtual void SetHitSpeed(Vector2 speed)
    {
        if (eAttr.isDead && speed.y > 0f)
        {
            return;
        }

        eAttr.timeController.SetSpeed(speed);
    }

    protected void PlayHurtAnim(string normalSta, string airSta, Vector2 speed, Vector2 airSpeed)
    {
        if (eAttr.isDead)
        {
            return;
        }

        if (!eAttr.iCanFly)
        {
            eAttr.timeController.SetGravity(1f);
        }

        eAttr.isFlyingUp = false;
        eAttr.checkHitGround = false;
        action.AnimChangeState((!eAttr.isOnGround) ? airSta : normalSta);
        SetHitSpeed((!eAttr.isOnGround) ? airSpeed : speed);
    }

    /// <summary>
    /// 伤害效果
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="atkName"></param>
    public void HitEffect(Vector3 pos, string atkName = "Atk1")
    {
        R.Effect.Generate(1, transform, pos, new Vector3(0f, 0f, 0f), default(Vector3), true);
        R.Effect.Generate(71, transform, pos + Vector3.right * pAttr.faceDir * 0.5f,
            new Vector3(0f, (pAttr.faceDir <= 0) ? 0 : 180, Random.Range(-90f, 0f)), default(Vector3), true);
        R.Effect.Generate(151, transform, pos, new Vector3(0f, 0f, 0f), default(Vector3), true);
        if (atkName.IsInArray(PlayerAtkType.HeavyEffectAttack))
            R.Effect.Generate(156, transform, pos, default(Vector3), default(Vector3), true);
    }

    private void HitIntoWeakEffect(Vector3 pos, string atkName)
    {
        SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(48, 0.5f);
        HitEffect(pos, atkName);
    }

    /// <summary>
    /// 播放伤害音频
    /// </summary>
    protected virtual void PlayHurtAudio()
    {
        R.Audio.PlayEffect(Random.Range(129, 132), new Vector3?(transform.position));
    }

    /// <summary>
    /// 播放Sp受伤音频
    /// </summary>
    /// <returns></returns>
    protected bool PlaySpHurtAudio()
    {
        if (!SoundJudge())
        {
            return true;
        }

        bool flag = Random.Range(0, 100) < spHurtAudio;
        if (flag)
        {
            spHurtAudio = 6;
            return true;
        }

        spHurtAudio += 6;
        return false;
    }

    /// <summary>
    /// 声音判断
    /// </summary>
    /// <returns></returns>
    private bool SoundJudge()
    {
        int enemyType = (int)eAttr.baseData.enemyType;
        switch (enemyType)
        {
            case 1:
            case 2:
            case 3:
                break;
            default:
                switch (enemyType)
                {
                    case 22:
                    case 23:
                    case 26:
                        break;
                    default:
                        switch (enemyType)
                        {
                            case 15:
                            case 18:
                                break;
                            default:
                                if (enemyType != 33 && enemyType != 37)
                                {
                                    return false;
                                }

                                break;
                        }

                        break;
                }

                break;
        }

        return true;
    }

    /// <summary>
    /// 物理与效果
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="airSpeed"></param>
    /// <param name="normalAtkType"></param>
    /// <param name="airAtkType"></param>
    protected virtual void PhysicAndEffect(Vector2 speed, Vector2 airSpeed, string normalAtkType, string airAtkType)
    {
        if (playerAtkName != "Charge1EndLevel1" && !playerAtkName.IsInArray(PlayerAction.ExecuteSta))
        {
            if (eAttr.paBody) return;
            if (eAttr.currentActionInterruptPoint < eAttr.actionInterruptPoint && !defecnceBreak) return;
        }

        defecnceBreak = false;
        if (eAttr.isOnGround && normalAtkType != "NoStiff")
        {
            eAttr.stiffTime = 1f;
            PlayHurtAnim(normalAtkType, airAtkType, speed, airSpeed);
        }
        else if (!eAttr.isOnGround && airAtkType != "NoStiff")
        {
            eAttr.stiffTime = 1f;
            PlayHurtAnim(normalAtkType, airAtkType, speed, airSpeed);
        }
        else
        {
            eAttr.stiffTime = 0f;
        }
    }

    public void QTECameraStart()
    {
        if (!qteCameraEffectOn)
            qteCameraEffectOn = true;
    }

    public void QTECameraFinish()
    {
        if (qteCameraEffectOn)
            qteCameraEffectOn = false;
    }

    protected bool BloodWeak()
    {
        return HpInWeak() && !eAttr.isDead && !action.IsInWeakSta() && eAttr.accpectExecute;
    }

    protected bool HpInWeak()
    {
        int num = 0;
        for (int i = 0; i < currentPhase; i++)
        {
            num += GetPhaseHp(i);
        }

        num += (int)(GetPhaseHp(currentPhase) * ((eAttr.rankType != EnemyAttribute.RankType.Normal) ? 0.9f : 0.7f));
        return eAttr.currentHp < eAttr.maxHp - num;
    }

    public void GenerateCritHurtNum(int damage)
    {
        if (damage == 0)
        {
            return;
        }

        HpMinus(damage);
    }

    protected virtual void HpMinus(int num)
    {
        if (eAttr.rankType != EnemyAttribute.RankType.Normal)
        {
            eAttr.currentHp = Mathf.Clamp(eAttr.currentHp - num, MinLockedHp(), int.MaxValue);
            return;
        }

        eAttr.currentHp -= num;
    }

    protected void QTEHpMinus()
    {
        int num = 0;
        for (int i = 0; i < currentPhase; i++)
        {
            num += GetPhaseHp(i);
        }

        eAttr.currentHp = eAttr.maxHp - num;
    }

    protected int MinLockedHp()
    {
        int num = 0;
        for (int i = 0; i < currentPhase + 1; i++)
        {
            int phaseHp = GetPhaseHp(i);
            num += phaseHp;
        }

        return eAttr.maxHp - num + 1;
    }

    protected void HpRecover()
    {
        int num = 0;
        for (int i = 0; i < currentPhase; i++)
        {
            num += GetPhaseHp(i);
        }

        num += (int)(GetPhaseHp(currentPhase) * 0.7f);
        eAttr.currentHp = eAttr.maxHp - num;
    }

    /// <summary>
    /// 反击
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="groundOnly"></param>
    /// <returns></returns>
    protected virtual bool Counterattack(int damage, bool groundOnly)
    {
        return HurtDataTools.Counterattack(damage, groundOnly, ref actionInterrupt, ref eAttr, ref action);
    }

    protected void AddActionInterruptPoint(int damage, string atkName)
    {
        HurtDataTools.AddActionInterruptPoint(damage, atkName, ref eAttr, ref actionInterrupt);
    }

    protected bool CalculateMonsterDefence(int damage)
    {
        return HurtDataTools.CalculateMonsterDefence(damage, ref defenceTrigger, ref action, ref eAttr);
    }

    protected bool CalculateMonsterSideStep(int damage)
    {
        return HurtDataTools.CalculateMonsterSideStep(damage, ref sideStepTrigger, ref action, ref eAttr);
    }

    protected int GetPhaseHp(int phase)
    {
        if (phase < 0 || phase > phaseHp.Count)
        {
            return 0;
        }

        return phaseHp[phase];
    }

    public void StopFollowLeftHand()
    {
        eAttr.followLeftHand = false;
    }

    protected virtual void HitIntoWeakState(Vector2 speed, Vector2 airSpeed, string normalAtkType, string airAtkType)
    {
        ChaseEnd();
        action.EnterWeakState();
        eAttr.stiffTime = 1f;
        if (eAttr.isOnGround && normalAtkType != "NoStiff")
        {
            action.FaceToPlayer();
            PlayHurtAnim(normalAtkType, airAtkType, speed, airSpeed);
        }
        else if (!eAttr.isOnGround && airAtkType != "NoStiff")
        {
            action.FaceToPlayer();
            PlayHurtAnim(normalAtkType, airAtkType, speed, airSpeed);
        }

        if (!IsInvoking("ExitWeak"))
        {
            exitWeakPhase = currentPhase;
            Invoke("ExitWeak", 5f);
        }
    }

    protected virtual void ExitWeak()
    {
        if (!action.IsInWeakSta() || exitWeakPhase != currentPhase || eAttr.willBeExecute)
        {
            return;
        }

        HpRecover();
        action.ExitWeakState();
    }

    /// <summary>
    /// 正常的伤害
    /// </summary>
    /// <param name="atkData"></param>
    /// <param name="atkId"></param>
    /// <param name="body"></param>
    /// <param name="hurtPos"></param>
    public virtual void NormalHurt(EnemyHurtAtkEventArgs.PlayerNormalAtkData atkData, int atkId, HurtCheck.BodyType body, Vector2 hurtPos)
    {
        if (hurtId >= atkId || !hurtData.Contains(atkData.atkName))return;
        GetHurt(atkId);
        HurtAttribute hurtAttribute = new HurtAttribute(hurtData[atkData.atkName], defaultAnimName, defaultAirAnimName);
        SpeedAdjust(atkData.atkName, ref hurtAttribute);
        playerAtkName = atkData.atkName;
        Vector2 speed = new Vector2(hurtAttribute.xSpeed, hurtAttribute.ySpeed);
        Vector2 airSpeed = new Vector2(hurtAttribute.airXSpeed, hurtAttribute.airYSpeed);
        Vector3 pos = (Vector3)hurtPos - transform.position;
        int finalDamage = PlayerDamageCalculate.GetFinalDamage(atkData.damagePercent, HurtDataTools.GetAtkLevel(playerAtkName), playerAtkName);
        if (eAttr.isArmorBroken)
        {
            AddActionInterruptPoint(finalDamage, playerAtkName);
            bool flag = CalDefense(finalDamage);
            bool flag2 = CalSideStep(finalDamage);
            canCounterattack = Counterattack(finalDamage, eAttr.isOnGround);
            if (atkData.joystickShakeNum > 0)
            {
                Input.Vibration.Vibrate(atkData.joystickShakeNum);
            }

            if (canCounterattack)
            {
                PlayHurtAudio();
                DoCounterAttack(atkData, pos);
                GenerateCritHurtNum(finalDamage);
                DoWeak(pos, speed, airSpeed, hurtAttribute, atkData);
                return;
            }

            CanDefenseOrSideStep(ref flag, ref flag2, ref finalDamage);
            if (InDefense(flag, atkData, pos))
            {
                return;
            }

            flag = false;
            NotInDefense();
            speed.x *= speedDir;
            airSpeed.x *= speedDir;
            if (!eAttr.isDead)
            {
                GenerateCritHurtNum(finalDamage);
            }

            if (DoWeak(pos, speed, airSpeed, hurtAttribute, atkData))
            {
                return;
            }

            if (!flag2)
            {
                HitEffect(pos, atkData.atkName);
            }

            if (!flag && !flag2)
            {
                DoHurt(atkData, speed, airSpeed, hurtAttribute);
            }
        }
        else
        {
            armor.HitArmor(finalDamage, atkData.atkName);
        }
    }

    private void GetHurt(int atkId)
    {
        hurtId = atkId;
        if (atkBox != null)
        {
            atkBox.localScale = Vector3.zero;
        }
    }

    private void SpeedAdjust(string name, ref HurtAttribute hurtAttr)
    {
        if ((name == "UpRising" || name == "AtkUpRising") && hurtAttr.airYSpeed > 0f)
        {
            float num = Mathf.Clamp(Physics2D.Raycast(transform.position, -Vector2.up, 100f, LayerManager.GroundMask).distance, 0f,
                float.PositiveInfinity);
            hurtAttr.airYSpeed = Mathf.Lerp(hurtAttr.ySpeed, 6f, num / 4f);
        }
    }

    private bool CalDefense(int hitNumber)
    {
        bool flag = CalculateMonsterDefence(hitNumber);
        if (flag)
        {
            eAttr.dynamicDefence = (int)(eAttr.baseDefence * Random.Range(0.7f, 1.3f));
            defenceTrigger = 0f;
        }

        return flag;
    }

    private bool CalSideStep(int hitNumber)
    {
        bool flag = CalculateMonsterSideStep(hitNumber);
        if (flag)
        {
            eAttr.dynamicSideStep = (int)(eAttr.baseSideStep * Random.Range(0.7f, 1.3f));
            sideStepTrigger = 0f;
        }

        return flag;
    }

    private void DoCounterAttack(EnemyHurtAtkEventArgs.PlayerNormalAtkData atkData, Vector3 pos)
    {
        TimeFrozenAndCameraShake(atkData.frozenFrame, atkData.shakeFrame, atkData.shakeType, atkData.camShakeFrame, atkData.shakeStrength);
        HitEffect(pos, atkData.atkName);
        eAttr.currentDefence = 0;
        eAttr.currentSideStep = 0;
        int dir = InputSetting.JudgeDir(transform.position.x, player.transform.position.x);
        action.CounterAttack(dir);
    }

    private void CanDefenseOrSideStep(ref bool defence, ref bool sideStep, ref int hitNumber)
    {
        if (defence || sideStep)
        {
            if (defence && sideStep)
            {
                if (Random.Range(0, 2) == 0)
                {
                    eAttr.currentSideStep /= 2;
                    action.Defence();
                    sideStep = false;
                }
                else
                {
                    eAttr.currentDefence /= 2;
                    action.SideStep();
                    defence = false;
                    hitNumber = 0;
                }
            }
            else if (defence)
            {
                action.Defence();
            }
            else
            {
                action.SideStep();
                hitNumber = 0;
            }
        }
    }

    private bool InDefense(bool defence, EnemyHurtAtkEventArgs.PlayerNormalAtkData atkData, Vector3 pos)
    {
        if (defence)
        {
            DoDefenseSucces(atkData, pos);
            return true;
        }

        if (!action.IsInDefenceState())
        {
            return false;
        }

        if (!playerAtkName.IsInArray(PlayerAtkType.BreakDefense))
        {
            DoDefenseSucces(atkData, pos);
            return true;
        }

        DoFefenseBreak();
        return false;
    }

    private void DoDefenseSucces(EnemyHurtAtkEventArgs.PlayerNormalAtkData atkData, Vector3 pos)
    {
        R.Effect.Generate(157, transform.Find("HurtBox"), default(Vector3), default(Vector3), default(Vector3), true);
        action.FaceToPlayer();
        action.DefenceSuccess();
        HitEffect(pos, atkData.atkName);
    }

    private void NotInDefense()
    {
        if (canChangeFace && !eAttr.paBody && eAttr.currentActionInterruptPoint >= eAttr.actionInterruptPoint)
        {
            int dir = (R.Player.Attribute.faceDir != 1) ? 1 : -1;
            action.ChangeFace(dir);
        }
    }

    private void DoFefenseBreak()
    {
        R.Audio.PlayEffect(192, new Vector3?(transform.position));
        R.Effect.Generate(160, transform.Find("HurtBox"), default(Vector3), default(Vector3), default(Vector3), true);
        Input.Vibration.Vibrate(4);
        eAttr.currentActionInterruptPoint = 0;
        defecnceBreak = true;
        eAttr.paBody = false;
    }

    private bool DoWeak(Vector3 pos, Vector2 speed, Vector2 airSpeed, HurtAttribute hurtAttr, EnemyHurtAtkEventArgs.PlayerNormalAtkData atkData)
    {
        if (BloodWeak())
        {
            HitIntoWeakState(speed, airSpeed, hurtAttr.normalAtkType, hurtAttr.airAtkType);
            HitIntoWeakEffect(pos, atkData.atkName);
            return true;
        }

        return false;
    }

    private void DoHurt(EnemyHurtAtkEventArgs.PlayerNormalAtkData atkData, Vector2 speed, Vector2 airSpeed, HurtAttribute hurtAttr)
    {
        PlayHurtAudio();
        if (!eAttr.isDead && HurtDataTools.ChaseAttack())
        {
            currentChaseTime = 0;
            ChaseStart();
        }

        SpAttack();
        PhysicAndEffect(speed, airSpeed, hurtAttr.normalAtkType, hurtAttr.airAtkType);
        TimeFrozenAndCameraShake(atkData.frozenFrame, atkData.shakeFrame, atkData.shakeType, atkData.camShakeFrame, atkData.shakeStrength);
    }

    protected virtual void FlashAttackHurt()
    {
        HurtDataTools.FlashHPRecover();
        if (eAttr.rankType == EnemyAttribute.RankType.Normal)
        {
            Execute("NewExecute1_2", false);
        }
        else if (flashPercent)
        {
            R.Effect.Generate(127, transform, default(Vector3), default(Vector3), default(Vector3), true);
        }

        Input.Vibration.Vibrate(4);
        if (BloodWeak())
        {
            action.EnterWeakState();
            if (!IsInvoking("ExitWeak"))
            {
                Invoke("ExitWeak", 5f);
            }
        }
    }

    public virtual void QTEHurt()
    {
        if (eAttr.rankType == EnemyAttribute.RankType.Normal)
        {
            return;
        }

        eAttr.willBeExecute = false;
        eAttr.inWeakState = false;
        eAttr.isFlyingUp = false;
        eAttr.checkHitGround = false;
        eAttr.timeController.SetGravity(1f);
        SetHitSpeed(Vector2.zero);
        action.hurtBox.gameObject.SetActive(true);
        currentPhase = Mathf.Clamp(currentPhase + 1, 0, maxPhase);
        ExecuteDieEffect();
        SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(30, 0.2f);
        R.Camera.Controller.CameraShake(0.5f, 0.3f, CameraController.ShakeTypeEnum.Rect, false);
        R.Effect.Generate(127, transform, default(Vector3), default(Vector3), default(Vector3), true);
    }

    protected virtual void ExecuteFollow()
    {
        int dir = (player.transform.localScale.x >= 0f) ? 1 : -1;
        action.ChangeFace(dir);
        action.WeakEffectDisappear("RollEnd");
        eAttr.followLeftHand = true;
        eAttr.willBeExecute = true;
        eAttr.checkHitGround = false;
        eAttr.flyToFall = false;
        eAttr.isFlyingUp = false;
        SetHitSpeed(Vector2.zero);
        eAttr.timeController.SetGravity(0f);
        StartCoroutine(ClipShake(17));
    }

    protected virtual void SpAttack()
    {
        if (eAttr.currentActionInterruptPoint < eAttr.actionInterruptPoint)
        {
        }
    }

    public void ChaseStart()
    {
        if (action.IsInWeakSta())
        {
            return;
        }

        if (player.GetComponent<PlayerAbilities>().flashAttack.CheckEnemy(gameObject))
        {
            return;
        }

        if (currentChaseTime > 2)
        {
            return;
        }

        currentChaseTime++;
        chaseEnd = 0f;
        eAttr.canBeChased = true;
        if (_chaseCoroutine != null)
        {
            StopCoroutine(_chaseCoroutine);
        }

        _chaseCoroutine = StartCoroutine(ChaseCoroutine());
    }

    private IEnumerator ChaseCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        ChaseAttack();
        yield return new WaitForSeconds(0.3f);
        ChaseAttack();
        ChaseEnd();
    }

    private void ChaseAttack()
    {
        Vector3 position = this.transform.position;
        R.Audio.PlayEffect(Random.Range(18, 21), new Vector3?(this.transform.position));
        position.y = Mathf.Clamp(position.y, LayerManager.YNum.GetGroundHeight(gameObject), float.MaxValue);
        Transform transform = R.Effect.Generate(182, null, position, default(Vector3), default(Vector3), true);
        Vector3 localScale = transform.localScale;
        localScale.x = (Random.Range(0, 2) != 0) ? -1 : 1;
        transform.localScale = localScale;
    }

    public void ChaseEnd()
    {
        chaseEnd = 0f;
        currentChaseTime = 0;
        eAttr.canBeChased = false;
    }

    protected void Execute(string playerState, bool chargeUp = true)
    {
        eAttr.willBeExecute = false;
        eAttr.inWeakState = false;
        action.hurtBox.gameObject.SetActive(true);
        playerAtkName = playerState;
        ExecuteDie();
        if (chargeUp || flashPercent)
        {
            R.Effect.Generate(127, transform, default(Vector3), default(Vector3), default(Vector3), true);
        }
    }

    /// <summary>
    /// 死亡完成
    /// </summary>
    protected virtual void ExecuteDie()
    {
        R.Audio.PlayEffect(Random.Range(105, 108), new Vector3?(transform.position));
        deadFlag = true;
        eAttr.currentHp = 0;
        eAttr.inWeakState = false;
        eAttr.isFlyingUp = false;
        eAttr.checkHitGround = false;
        eAttr.stiffTime = 0f;
        eAttr.timeController.SetGravity(1f);
        EGameEvent.EnemyKilled.Trigger(eAttr);
        action.WeakEffectDisappear("Null");
        R.Effect.Generate(91, null, transform.position + new Vector3(0f, 1.2f, LayerManager.ZNum.Fx), Vector3.zero, default(Vector3), true);
        R.Effect.Generate(49, transform, default(Vector3), default(Vector3), default(Vector3), true);
        R.Effect.Generate(14, null, transform.position + new Vector3(0f, 1.2f, LayerManager.ZNum.Fx), default(Vector3), default(Vector3), true);
        AddCoinAndExp();
        ExecuteDieEffect();
        R.Effect.Generate(213, null, default(Vector3), default(Vector3), default(Vector3), true);
        R.Effect.Generate(214, null, default(Vector3), default(Vector3), default(Vector3), true);
        SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(45, 0.2f);
        R.Camera.Controller.CameraShake(0.9166667f, 0.3f, CameraController.ShakeTypeEnum.Rect, false);
        R.Camera.Controller.OpenMotionBlur(0.13333334f, 1f, transform.position);
    }

    public virtual void EnemyDie()
    {
        if (eAttr.rankType != EnemyAttribute.RankType.Normal) return;
        R.Audio.PlayEffect(Random.Range(105, 108), new Vector3?(transform.position));
        deadFlag = true;
        eAttr.inWeakState = false;
        eAttr.isFlyingUp = false;
        eAttr.checkHitGround = false;
        eAttr.stiffTime = 0f;
        eAttr.timeController.SetGravity(1f);
        EGameEvent.EnemyKilled.Trigger(eAttr);
        action.WeakEffectDisappear("Null");
        StartCoroutine(GenerateEnergyBall());
        AddCoinAndExp();
    }

    private IEnumerator GenerateEnergyBall()
    {
        yield return new WaitForSeconds(0.8f);
        R.Effect.Generate(91, null, transform.position + new Vector3(0f, 1.2f, LayerManager.ZNum.Fx), Vector3.zero, default(Vector3), true);
    }

    /// <summary>
    /// 增加金币和经验值
    /// </summary>
    protected void AddCoinAndExp()
    {
        R.Equipment.CoinNum += eAttr.dropCoins;
    }

    protected void DieTimeControl()
    {
        SingletonMono<WorldTime>.Instance.TimeFrozenByFixedFrame(25, WorldTime.FrozenArgs.FrozenType.Enemy, true);
        R.Camera.Controller.CameraShake(0.416666657f, 0.3f, CameraController.ShakeTypeEnum.Rect, false);
        if (gameObject.activeSelf)
        {
            StartCoroutine(ClipShake(12));
        }
    }

    public void NormalKill()
    {
        R.Audio.PlayEffect(24, new Vector3?(transform.position));
        R.Camera.Controller.CameraBloom(0.25f, 0f);
        R.Effect.Generate(49, transform, new Vector3(0f, 1.2f, LayerManager.ZNum.Fx), Vector3.zero, default(Vector3), true);
        R.Effect.Generate(9, transform, new Vector3(0f, 1.2f, LayerManager.ZNum.Fx), new Vector3(0f, 0f, 0f), default(Vector3), true);
        R.Effect.Generate(14, transform, new Vector3(0f, 1.2f, -0.1f), new Vector3(0f, 0f, 0f), default(Vector3), true);
    }

    protected virtual void ExecuteDieEffect()
    {
        R.Effect.Generate(156, null, center.position, default(Vector3), default(Vector3), true);
    }

    protected virtual IEnumerator DeathIEnumerator()
    {
        bool deadFly = true;
        bool deadFall = false;
        while (eAttr.isDead)
        {
            if (deadFly && eAttr.timeController.GetCurrentSpeed().y <= 0f)
            {
                deadFly = false;
                deadFall = true;
                action.AnimChangeState(airDieAnimName);
            }

            if (deadFall && eAttr.isOnGround)
            {
                deadFall = false;
                action.AnimChangeState(airDieHitGroundAnimName);
            }

            yield return null;
        }
    }

    public bool canCounterattack;

    public Transform center;

    public int currentPhase;

    public const int maxChaseTime = 2;

    public int currentChaseTime;

    protected int hurtId;

    protected bool deadFlag;

    protected string defaultAnimName;

    protected string defaultAirAnimName;

    protected string airDieAnimName;

    protected string airDieHitGroundAnimName;

    protected string flyToFallAnimName;

    protected Pivot Pivot;

    protected EnemyAttribute eAttr;

    protected EnemyBaseAction action;

    protected JsonData1 hurtData;

    /// <summary>
    /// 玩家攻击的名称
    /// </summary>
    protected string playerAtkName;

    protected bool actionInterrupt;

    protected bool defecnceBreak;

    /// <summary>
    /// sp伤害音频
    /// </summary>
    protected int spHurtAudio;

    [SerializeField] private List<int> _phaseHp;

    protected int maxPhase;

    [SerializeField] private Transform atkBox;

    [SerializeField] private bool canChangeFace = true;

    /// <summary>
    /// 框架振动体
    /// </summary>
    [SerializeField] private Transform frameShakeBody;

    private EnemyArmor armor;

    [SerializeField] private Vector3 centerOffset;

    [SerializeField] [Header("左右抖动幅度")] private float m_frameShakeOffset;

    [SerializeField] private int[] hpPercent;

    /// <summary>
    /// 左右抖动幅度
    /// </summary>
    private float frameShakeOffset;

    /// <summary>
    /// 防御触发
    /// </summary>
    private float defenceTrigger;

    /// <summary>
    /// 闪避触发
    /// </summary>
    private float sideStepTrigger;

    /// <summary>
    /// 追逐结束
    /// </summary>
    private float chaseEnd;

    /// <summary>
    /// 弱相位
    /// </summary>
    private int exitWeakPhase;

    /// <summary>
    /// 相机效果是否启动
    /// </summary>
    private bool qteCameraEffectOn;

    private Coroutine _chaseCoroutine;

    /// <summary>
    /// 伤害属性
    /// </summary>
    private class HurtAttribute
    {
        public HurtAttribute(JsonData1 hurt, string defaultAnimName, string defaultAirAnimName)
        {
            xSpeed = hurt.Get<float>("xSpeed", 0f);
            ySpeed = hurt.Get<float>("ySpeed", 0f);
            airXSpeed = hurt.Get<float>("airXSpeed", 0f);
            airYSpeed = hurt.Get<float>("airYSpeed", 0f);
            normalAtkType = hurt.Get<string>("normalAtkType", defaultAnimName);
            airAtkType = hurt.Get<string>("airAtkType", defaultAirAnimName);
        }

        /// <summary>
        /// X速度
        /// </summary>
        public float xSpeed;

        /// <summary>
        /// Y速度
        /// </summary>
        public float ySpeed;

        /// <summary>
        /// 空中X速度
        /// </summary>
        public float airXSpeed;

        /// <summary>
        /// 空中Y速度
        /// </summary>
        public float airYSpeed;

        /// <summary>
        /// 正常攻击类型
        /// </summary>
        public string normalAtkType;

        /// <summary>
        /// 空中攻击类型
        /// </summary>
        public string airAtkType;
    }
}