using Framework.Core;
using UnityEngine;

public class ShadeAttack : MonoBehaviour
{
    private void Start()
    {
        string animationName = this.anim[UnityEngine.Random.Range(0, 3)];
        base.GetComponent<SkeletonAnimation>().state.SetAnimation(0, animationName, false);
        R.Audio.PlayEffect(200, new Vector3?(base.transform.position));
    }

    public void Init(GameObject target)
    {
        this.atkTarget = target;
    }

    public void SendFlashAtkEvent()
    {
        EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(this.atkTarget, EnemyHurtAtkEventArgs.HurtTypeEnum.Flash);
        EGameEvent.EnemyHurtAtk.Trigger((R.Player.GameObject, args));
    }

    private GameObject atkTarget;

    private string[] anim = {
        "Down",
        "ToLeftAir",
        "ToLeftGround"
    };
}