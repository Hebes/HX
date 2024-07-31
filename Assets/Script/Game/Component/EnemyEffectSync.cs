using LitJson;
using UnityEngine;

/// <summary>
/// 敌人效果同步
/// </summary>
public class EnemyEffectSync : MonoBehaviour
{
    private void Awake()
    {
        data = null;// JsonMapper.ToObject(effectConfigure.text);
    }

    private void Update()
    {
        m_skeletonAnimation.timeScale = animController.TimeScale;
    }

    private void OnEnable()
    {
        animController.OnAnimChange += SyncEffect;
    }

    private void OnDisable()
    {
        if (animController)
        {
            animController.OnAnimChange -= SyncEffect;
        }
    }

    private void OnDestroy()
    {
        if (animController)
        {
            animController.OnAnimChange -= SyncEffect;
        }
    }

    private void SyncEffect(object obj, SpineAnimationController.EffectArgs e)
    {
        string effectName = e.EffectName;
        if (data.Contains(effectName))
        {
            m_skeletonAnimation.state.SetAnimation(0, data[effectName].ToString(), e.Loop);
            m_skeletonAnimation.skeleton.SetToSetupPose();
            m_skeletonAnimation.Update(0f);
        }
        else
        {
            m_skeletonAnimation.state.SetAnimation(0, "Null", true);
            m_skeletonAnimation.skeleton.SetToSetupPose();
            m_skeletonAnimation.Update(0f);
        }
    }

    [SerializeField]
    private SkeletonAnimation m_skeletonAnimation;

    [SerializeField]
    private SpineAnimationController animController;

    [SerializeField]
    private TextAsset effectConfigure;

    private JsonData1 data;
}