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

    private Dictionary<uint, IEffect> _effectDic;

    public void Init()
    {
        Instance = this;
        _effectDic = new Dictionary<uint, IEffect>();
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



    public static IEffect GetEffect(uint id)
    {
        if (Instance._effectDic.ContainsKey(id))
        {
            return Instance._effectDic[id];
        }
        Debug.Error($"没有找到该特效{id}");
        return default(IEffect);
    }
}



