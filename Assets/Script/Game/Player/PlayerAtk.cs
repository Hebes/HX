using Framework.Core;
using LitJson;
using UnityEngine;

public class PlayerAtk : MonoBehaviour
{
    private void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    public void SetData(JsonData1 atkData, int atkId)
    {
        data = atkData;
        attackId = atkId;
        _hitTimes = data.Get<int>("hitTimes", 0);
        _interval = data.Get<float>("interval", 100f);
        _hitType = (HitType)data.Get<int>("hitType", 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyHurtBox"))
        {
            EventTrigger(EventArgs(other, true));
        }
        else if (other.CompareTag("EnemyBullet"))
        {
            BreakBullet(other);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (SingletonMono<WorldTime>.Instance.IsFrozen || _hitType == HitType.Once || data == null)
        {
            return;
        }

        _interval -= Time.deltaTime;
        if (_interval > 0f)
        {
            return;
        }

        if (other.CompareTag("EnemyHurtBox"))
        {
            HitType hitType = _hitType;
            if (hitType != HitType.Limited)
            {
                if (hitType == HitType.UnLimited)
                {
                    UnlimitedAttack(other);
                }
            }
            else
            {
                LimitedAttack(other);
            }
        }
    }

    private void UnlimitedAttack(Collider2D other)
    {
        _interval = data.Get<float>("interval", 0f);
        attackId = Incrementor.GetNextId();
        EventTrigger(EventArgs(other, false));
    }

    private void LimitedAttack(Collider2D other)
    {
        if (_hitTimes > 0)
        {
            _interval = data.Get<float>("interval", 0f);
            attackId = Incrementor.GetNextId();
            EventTrigger(EventArgs(other, false));
            _hitTimes--;
        }
    }

    private void BreakBullet(Collider2D bullet)
    {
        if (R.Player.Action.stateMachine.currentState.IsInArray(PlayerAtkType.CantBreakBullet))
        {
            return;
        }

        EnemyBullet component = bullet.GetComponent<EnemyBullet>();
        EnemyBulletLaucher component2 = bullet.GetComponent<EnemyBulletLaucher>();
        if (component != null)
        {
            component.beAtked = true;
            component.HitBullet();
        }
        else if (component2 != null)
        {
            component2.beAtked = true;
            component2.HitBullet();
        }

        SingletonMono<WorldTime>.Instance.TimeFrozenByFixedFrame(7);
        R.Camera.Controller.CameraShake(0.166666672f);
    }

    private Vector2 HurtPos(Bounds enemyBound)
    {
        return MathfX.Intersect2DCenter(enemyBound, _collider.bounds);
    }

    private EnemyHurtAtkEventArgs EventArgs(Collider2D enemyBody, bool firstHurt)
    {
        return new EnemyHurtAtkEventArgs(enemyBody.transform.parent.gameObject, gameObject, attackId, HurtPos(enemyBody.bounds),
            HurtCheck.BodyType.Body, new EnemyHurtAtkEventArgs.PlayerNormalAtkData(data, firstHurt));
    }

    private void EventTrigger(EnemyHurtAtkEventArgs args)
    {
        EGameEvent.EnemyHurtAtk.Trigger((gameObject, args));
    }

    public JsonData1 data;

    public int attackId;

    private float _interval;

    private int _hitTimes;

    private HitType _hitType;

    private Collider2D _collider;

    public enum HitType
    {
        Once,
        Limited,
        UnLimited
    }
}