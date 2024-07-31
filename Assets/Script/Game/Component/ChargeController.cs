using System;
using UnityEngine;

/// <summary>
/// 充电控制器
/// </summary>
public class ChargeController : MonoBehaviour
{
    public void ChargeZeroToOne()
    {
        this.ChargeOver("Lv0ToLv1", -1, -1);
    }

    public void ChargeOneOver()
    {
        this.ChargeOver("Lv1ToLv2", 34, 121);
    }

    public void ChargeOneOverAir()
    {
        this.ChargeOver("Lv1ToLv2Air", 34, 181);
    }

    private void ChargeOver(string chargeLevel, int audioID, int effectID)
    {
        this.CheckActive();
        this.m_skeletonAnim.state.SetAnimation(0, chargeLevel, true);
        this.m_skeletonAnim.skeleton.SetToSetupPose();
        this.m_skeletonAnim.Update(0f);
        if (effectID != -1)
        {
            R.Effect.Generate(effectID, R.Player.Transform, Vector3.zero, default(Vector3), default(Vector3), true);
        }
        if (audioID != -1)
        {
            R.Audio.PlayEffect(audioID, new Vector3?(base.transform.position));
        }
    }

    public void CheckActive()
    {
        if (!base.gameObject.activeSelf)
        {
            base.gameObject.SetActive(true);
        }
    }

    [SerializeField]
    private SkeletonAnimation m_skeletonAnim;

    [SerializeField]
    private SkeletonAnimation baseSkeletonAnim;
}