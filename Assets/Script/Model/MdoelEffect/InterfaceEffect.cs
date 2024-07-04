using Framework.Core;
using UnityEngine;
/// <summary>
/// 特效接口
/// </summary>
public interface IEffect : IID
{
    /// <summary>
    /// 特效预制体
    /// </summary>
    GameObject EffectGO { get; set; }
}

public interface IEffectBehaviour : IEffect
{
    /// <summary>
    /// 特效进入
    /// </summary>
    void EffectEnter();

    /// <summary>
    /// 特效离开
    /// </summary>
    void EffectExit();

    /// <summary>
    /// 触发特效
    /// </summary>
    /// <param name="effectBehaviour"></param>
    public static void TriggerEffect(IEffectBehaviour effectBehaviour)
    {
        effectBehaviour.EffectEnter();
    }
}
