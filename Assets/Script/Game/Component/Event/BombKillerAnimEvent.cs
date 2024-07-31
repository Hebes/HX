using Framework.Core;
using LitJson;
using UnityEngine;

/// <summary>
/// 炸弹杀手事件
/// </summary>
public class BombKillerAnimEvent : MonoBehaviour
{
    private void Awake()
    {
        _action = GetComponent<BombKillerAction>();
        _eAttr = GetComponent<EnemyAttribute>();
    }

    private void Start()
    {
        _jsonData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.半身自爆];
    }

    private void Update()
    {
        if (_eAttr.isFlyingUp)
        {
            bool flag = maxFlyHeight > 0f && _eAttr.height >= maxFlyHeight;
            if (flag)
            {
                Vector2 currentSpeed = _eAttr.timeController.GetCurrentSpeed();
                currentSpeed.y = 0f;
                _eAttr.timeController.SetSpeed(currentSpeed);
            }

            if (_eAttr.timeController.GetCurrentSpeed().y <= 0f)
            {
                _eAttr.isFlyingUp = false;
                _action.AnimChangeState(BombKillerAction.StateEnum.FlyToFall);
            }
        }

        if (_eAttr.checkHitGround && _eAttr.isOnGround)
        {
            _eAttr.checkHitGround = false;
            R.Effect.Generate(6, transform);
            if (_action.stateMachine.currentState == "HitFall")
            {
                maxFlyHeight = 4f;
                _eAttr.timeController.SetSpeed(Vector2.up * 25f);
                _action.AnimChangeState(BombKillerAction.StateEnum.HitToFly1);
            }
            else
            {
                _action.AnimChangeState(BombKillerAction.StateEnum.HitGround);
            }
        }
    }

    public void ChangeState(BombKillerAction.StateEnum sta)
    {
        _action.AnimChangeState(sta);
    }

    public void HitGround()
    {
        _eAttr.checkHitGround = true;
    }

    public void FlyUp()
    {
        _eAttr.isFlyingUp = true;
    }

    public void GetUp()
    {
        if (_eAttr.isDead)
        {
            DieBlock();
            DestroySelf();
        }
        else
        {
            _action.AnimChangeState(BombKillerAction.StateEnum.GetUp);
        }
    }

    public void BackToIdle()
    {
        if (_action.IsInWeakSta())
        {
            _eAttr.enterWeakMod = false;
        }

        _action.AnimChangeState(BombKillerAction.StateEnum.Idle);
    }

    public void PlaySound(int id)
    {
        R.Audio.PlayEffect(id, transform.position);
    }

    public void DieBlock()
    {
        R.Effect.Generate(163, null, new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f), Vector3.zero);
    }

    /// <summary>
    /// 攻击1死亡消息
    /// </summary>
    public void Atk1DieMessage()
    {
        EGameEvent.EnemyKilled.Trigger(_eAttr);
    }

    public void DestroySelf()
    {
        Invoke(nameof(RealDestroy), Time.deltaTime);
        gameObject.SetActive(false);
    }

    private void RealDestroy()
    {
        Destroy(gameObject);
    }

    public void SetAtkData()
    {
        GetComponentInChildren<EnemyAtk>().atkId = Incrementor.GetNextId();
        GetComponentInChildren<EnemyAtk>().atkData = _jsonData[_action.stateMachine.currentState];
    }

    public void Atk1Finish()
    {
        if (_action.atk1Success)
        {
            _action.AnimChangeState(BombKillerAction.StateEnum.Atk1Success);
        }
        else
        {
            _action.AnimChangeState(BombKillerAction.StateEnum.Atk1Fail);
        }
    }

    public void Atk1FollowOver()
    {
        _action.atk1Success = false;
    }

    public void GenerateExplosion_Atk1()
    {
        PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(R.Player.GameObject, gameObject, 
            gameObject, _eAttr.atk, Incrementor.GetNextId(), _jsonData["Atk1Ready"], true);
        EGameEvent.PlayerHurtAtk.Trigger((transform, args));
    }

    public void GenerateExplosionEffectNoDamage()
    {
        Transform transform = R.Effect.Generate(162, this.transform, Vector2.up * 1.2f, Vector3.zero, Vector3.one * 0.4f);
        Destroy(transform.GetChild(1).gameObject);
    }

    public void GenerateExplosionEffect()
    {
        Transform transform = R.Effect.Generate(162, this.transform, Vector2.up * 1.2f);
        EnemyBullet componentInChildren = transform.GetComponentInChildren<EnemyBullet>();
        componentInChildren.SetAtkData(_jsonData[_action.stateMachine.currentState]);
        componentInChildren.damage = _eAttr.atk;
        componentInChildren.origin = null;
    }

    private BombKillerAction _action;

    private EnemyAttribute _eAttr;

    public float maxFlyHeight;

    private JsonData1 _jsonData;
}