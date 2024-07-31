using System.Collections.Generic;
using Framework.Core;
using LitJson;
using UnityEngine;

public class PlayerHurtAbility : CharacterState
{
    private float DamageCutOff
    {
        get
        {
            switch (R.Player.EnhancementSaveData.MaxEnergy)
            {
                case 1:
                    return 0.7f;
                case 2:
                    return 0.6f;
                case 3:
                    return 0.5f;
                default:
                    return 1f;
            }
        }
    }

    private float ShieldRecoverTime => (R.GameData.Difficulty > 1) ? 10f : 5f;

    public override void Start()
    {
        _spHurt = new PlayerSpHurt();
    }

    public override void OnEnable()
    {
        EGameEvent.PlayerHurtAtk.Register(PlayeAllHurt);
    }

    public override void OnDisable()
    {
        EGameEvent.PlayerHurtAtk.UnRegister(PlayeAllHurt);
    }

    public override void Update()
    {
        UpdateHurtId();
        UpdatePlayerDeath();
        UpdateShield();
        UpdateHurtGetUp();
        if (_hurtLimit > 0f)
        {
            _hurtLimit = Mathf.Clamp(_hurtLimit - Time.deltaTime, 0f, float.MaxValue);
        }
    }

    private void UpdateHurtId()
    {
        _clearRate -= 1f;
        if (_clearRate <= 0f)
        {
            _clearRate = 600f;
            hurtId.Clear();
        }
    }

    private void UpdatePlayerDeath()
    {
        if (pAttr.isDead && !DeadFlag)
        {
            PlayerDie();
        }
    }

    private void UpdateShield()
    {
        if (pAttr.currentEnergy == 0)
        {
            _energyRecover += Time.deltaTime;
            if (_energyRecover >= ShieldRecoverTime)
            {
                pAttr.currentEnergy = pAttr.maxEnergy;
                _energyRecover = 0f;
            }
        }
    }


    private void PlayeAllHurt(object udata)
    {
        PlayerHurtAtkEventArgs args = (PlayerHurtAtkEventArgs)udata;
        if (pAttr.isDead) return;
        if (pAttr.flashFlag && R.Player.EnhancementSaveData.FlashAttack != 0)
        {
            hurtId.Add(args.atkId);
            pab.flashAttack.FlashAttack(args.origin);
            EGameEvent.Assessment.Trigger((this, new AssessmentEventArgs(AssessmentEventArgs.EventType.AddFlashTime)));
            return;
        }

        if (Invincible) return;
        HurtFeedback(args.sender.transform, args.damage, args.atkId, args.data, args.forceHurt);
    }

    private void HurtFeedback(Transform enemy, int damage, int atkId, JsonData1 atkData, bool forceHurt)
    {
        if (HurtFilter(atkId)) return;
        if (stateMachine.currentState.IsInArray(PlayerAction.FlashAttackSta))
        {
            EGameEvent.Assessment.Trigger((this, new AssessmentEventArgs(AssessmentEventArgs.EventType.AddFlashTime)));
            return;
        }

        if (atkData == null)
        {
            Debug.Log(enemy.name + "  " + enemy.GetComponent<EnemyBaseAction>().stateMachine.currentState);
            return;
        }

        _broken = false;
        hurtId.Add(atkId);
        SingletonMono<WorldTime>.Instance.TimeFrozenByFixedFrame(6, WorldTime.FrozenArgs.FrozenType.Player);
        SingletonMono<CameraController>.Instance.CameraShake(0.2f);
        damage = (int)((float)damage * atkData.Get<float>("damagePercent", 1f));
        _energyRecover = 0f;
        int dir = JudgeDir(enemy);
        if (pAttr.currentEnergy > 0 && !pac.stateMachine.currentState.IsInArray(PlayerAction.SpHurtSta) &&
            HurtInDefense(enemy, atkData.Get<int>("shieldDamage", 3), ref damage))
        {
            return;
        }

        if (_spHurt.DaoRoll(enemy))
        {
            if (enemy.GetComponent<DaoAction>() != null)
            {
                enemy.GetComponent<DaoAction>().RollToIdle();
            }

            if (enemy.GetComponent<DaoPaoAction>() != null)
            {
                enemy.GetComponent<DaoPaoAction>().RollToIdle();
            }
        }

        if (_spHurt.CanBeJumperCatach(enemy))
        {
            HurtStiffInit(dir);
            HurtState("UnderAtkJumper");
            enemy.GetComponent<JumperAction>().CatchSuccess();
            return;
        }

        if (_spHurt.CanBeJumperFooterCatch(enemy))
        {
            HurtStiffInit(dir);
            HurtState("UnderAtkJumper");
            enemy.GetComponent<JumperFooterAction>().Attack2Success();
            return;
        }

        if (_spHurt.CanBeBeelzebubEat(enemy))
        {
            HurtStiffInit(dir);
            HurtState("UnderAtkEat");
            enemy.GetComponent<BeelzebubAction>().EatAttack();
            return;
        }

        if (_spHurt.CanEatingBossEat(enemy))
        {
            HurtStiffInit(dir);
            HurtState("UnderAtkEat");
            enemy.GetComponent<EatingBossAction>().Attack2Success();
            return;
        }

        if (_spHurt.CanBeBeelzebubSaw(enemy))
        {
            HurtStiffInit(dir);
            HurtState("UnderAtkHitSaw");
            enemy.GetComponent<BeelzebubAction>().SawAttack();
            return;
        }

        if (_spHurt.CanBombKillerCatch(enemy))
        {
            HurtStiffInit(dir);
            HurtState("UnderAtkBombKillerII");
            enemy.GetComponent<BombKillerAction>().Atk1Success();
            return;
        }

        if (_spHurt.JumperCatachSuccess(enemy, forceHurt) || _spHurt.BombKillerAtkSuccess(enemy, forceHurt) ||
            _spHurt.JumperFooterCatchSuccess(enemy, forceHurt) || _spHurt.BeelzebubSpAttackSuccess(enemy, forceHurt) ||
            _spHurt.EatingBossEatSuccess(enemy, forceHurt))
        {
            HurtStiff(dir, atkData, enemy);
        }

        HpMinus(damage);
        if (pAttr.isDead)
        {
            return;
        }

        if (pac.stateMachine.currentState.IsInArray(PlayerAction.SpHurtSta))
        {
            return;
        }

        HurtStiff(dir, atkData, enemy);
    }

    private void HurtStiff(int dir, JsonData1 atkData, Transform enemy)
    {
        R.Player.Rigidbody2D.gravityScale = 1f;
        float num = 0f;
        float y = 0f;
        float num2 = 3f;
        float y2 = 5f;
        string text = "UnderAtk1";
        string text2 = "UnderAtkHitToFly";
        if (atkData != null)
        {
            num = (float)atkData.Get<int>("xSpeed", 0);
            y = (float)atkData.Get<int>("ySpeed", 0);
            num2 = (float)atkData.Get<int>("airXSpeed", 3);
            y2 = (float)atkData.Get<int>("airYSpeed", 5);
            text = atkData.Get<string>("animName", "UnderAtk1");
        }

        if (_broken || (_hurtTimes >= 2 && _hurtLimit > 0f))
        {
            text = text2;
            num = 3f;
            y = 20f;
            num2 = 3f;
            y2 = 15f;
        }

        R.Effect.Generate(216);
        HurtStiffInit(dir);
        Vector2 vector = new Vector2(num * pac.transform.localScale.x, y);
        Vector2 vector2 = new Vector2(num2 * pac.transform.localScale.x, y2);
        R.Player.TimeController.SetSpeed((!pAttr.isOnGround) ? vector2 : vector);
        Input.Vibration.Vibrate(1);
        HurtState((!pAttr.isOnGround) ? text2 : text);
        HitEffect(dir, enemy);
    }

    private void PlayerDie()
    {
        DeadFlag = true;
        R.Player.Rigidbody2D.gravityScale = 1f;
        R.Camera.Controller.CameraBloom(0.5f, 1f);
        Vector2 speed = new Vector2(pAttr.faceDir * -7, 15f);
        R.Player.TimeController.SetSpeed(speed);
        pac.ChangeState(PlayerAction.StateEnum.UnderAtkHitToFly);
        StartCoroutine(listener.PlayerDieEnumerator());
    }

    private void ShadowControl(bool show)
    {
        if (!pac.shadow)
        {
            return;
        }

        if (pac.shadow.gameObject.activeSelf != show)
        {
            pac.shadow.gameObject.SetActive(show);
        }
    }

    private bool HurtFilter(int atkId)
    {
        return hurtId.Contains(atkId) || pac.stateMachine.currentState.IsInArray(PlayerAction.ExecuteSta);
    }

    private void HurtStiffInit(int dir)
    {
        listener.checkFallDown = false;
        listener.isFalling = false;
        listener.airAtkDown = false;
        listener.checkHitGround = false;
        listener.flyHitFlag = false;
        listener.flyHitGround = false;
        listener.StopIEnumerator("FlashPositionSet");
        pac.TurnRound(dir);
        pac.pab.charge.CancelCharge();
    }

    private int JudgeDir(Transform enemy)
    {
        if (enemy == null)
        {
            return pAttr.faceDir;
        }

        EnemyAttribute component = enemy.GetComponent<EnemyAttribute>();
        if (component != null)
        {
            return component.faceDir * -1;
        }

        Rigidbody2D component2 = enemy.GetComponent<Rigidbody2D>();
        if (component2 != null)
        {
            return (component2.velocity.x >= 0f) ? -1 : 1;
        }

        return (enemy.position.x - pac.transform.position.x <= 0f) ? -1 : 1;
    }

    private void HpMinus(int damage)
    {
        if (damage == 0)
        {
            return;
        }

        pAttr.currentHP -= damage;
    }

    private bool HurtInDefense(Transform enemy, int shieldDamage, ref int damage)
    {
        shieldDamage *= ((R.GameData.Difficulty > 1) ? 2 : 1);
        if (R.GameData.Difficulty == 3)
        {
            shieldDamage *= 100;
        }

        damage = Mathf.Clamp((int)(damage * DamageCutOff), 1, int.MaxValue);
        pAttr.currentEnergy = pAttr.currentEnergy - shieldDamage;
        HitEffect(pAttr.faceDir, enemy);
        if (pAttr.currentEnergy > 0)
        {
            HitShidleEffect(158, 186);
            HpMinus(damage);
            Input.Vibration.Vibrate(1);
            return true;
        }

        HitShidleEffect(161, 191);
        Input.Vibration.Vibrate(3);
        _broken = true;
        return false;
    }

    private void HitShidleEffect(int effectId, int soundId)
    {
        Vector3 position = pac.transform.position - pAttr.bounds.center;
        position.Set(position.x, position.y + 2f, position.z);
        R.Effect.Generate(effectId, pac.transform, position);
        R.Audio.PlayEffect(soundId, pac.transform.position);
    }

    private void HitEffect(int dir, Transform enemy)
    {
        R.Effect.Generate((enemy.position.x - pac.transform.position.x <= 0f) ? 76 : 80, null,
            pac.transform.position + new Vector3(dir * 0.2f, 1.3f, 0f), Vector3.zero);
        R.Effect.Generate(70, null, pac.transform.position + new Vector3(dir * 0.2f, 1.3f, 0f), Vector3.zero);
    }

    private void HurtState(string sta)
    {
        EGameEvent.PlayerHurt.Trigger(pac.gameObject);
        pac.ChangeState(sta);
    }

    public override void OnStateMachineStateTransfer(object sender, StateMachine.TransferEventArgs args)
    {
        if (args.lastState == "Disappear" || args.lastState == "UnderAtkEat")
        {
            ShadowControl(true);
        }

        if (args.nextState == "Disappear" || args.nextState == "UnderAtkEat")
        {
            ShadowControl(false);
        }

        if (args.lastState == "UnderAtkGetUp")
        {
            Invincible = true;
            _getUpInvincibleTime = WorldTime.SecondToFrame(0.2f);
        }

        if (args.nextState == "UnderAtkHitToFly")
        {
            _hurtTimes = 0;
            _hurtLimit = 0f;
        }

        if (args.nextState == "UnderAtk1")
        {
            _hurtTimes++;
            _hurtLimit = 0.5f;
        }
    }

    private void UpdateHurtGetUp()
    {
        if (_getUpInvincibleTime > 0)
        {
            _getUpInvincibleTime--;
            if (_getUpInvincibleTime <= 0)
            {
                Invincible = false;
            }
        }
    }

    public bool DeadFlag;

    private List<int> hurtId = new List<int>();

    private float _clearRate;

    private PlayerSpHurt _spHurt;

    private bool _broken;

    private float _energyRecover;

    private int _hurtTimes;

    private float _hurtLimit;

    public bool Invincible;

    private int _getUpInvincibleTime;
}