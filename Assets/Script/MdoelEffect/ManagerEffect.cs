using System.Collections.Generic;
using UnityEngine;
using Debug = Core.Debug;
using Core;


/*--------脚本描述-----------

描述:
	特效管理

-----------------------*/

public class ManagerEffect : IModelInit
{
    public static ManagerEffect Instance;

    private Dictionary<int, IEffect> _effectDic;

    public void Init()
    {
        Instance = this;
        _effectDic = new Dictionary<int, IEffect>();
    }

    public static void AddEffect(IEffect effect)
    {
        if (Instance._effectDic.ContainsKey(effect.ID))
            Instance._effectDic[effect.ID] = effect;
        else
            Instance._effectDic.Add(effect.ID, effect);
    }

    public static void RemoveEffect(IEffect effect)
    {

    }



    public static IEffect GetEffect(int id)
    {
        if (Instance._effectDic.ContainsKey(id))
        {
            return Instance._effectDic[id];
        }
        Debug.Error($"没有找到该特效{id}");
        return default(IEffect);
    }
}

/// <summary>
/// 特效接口
/// </summary>
public interface IEffect
{
    /// <summary>
    /// 特效ID
    /// </summary>
    int ID { get; set; }

    /// <summary>
    /// 特效预制体
    /// </summary>
    GameObject EffectGO { get; set; }
}

public interface IEffectBehaviour
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

