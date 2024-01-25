using System.Collections.Generic;
using UnityEngine;
using Debug = Core.Debug;
using Core;
using System.Collections;


/*--------脚本描述-----------

描述:
	特效管理

-----------------------*/

public class ManagerEffect : IModel
{
    public static ManagerEffect Instance;

    private Dictionary<long, IEffect> _effectDic;

    public IEnumerator Enter()
    {
        Instance = this;
        _effectDic = new Dictionary<long, IEffect>();
        yield return null;
    }
    public IEnumerator Exit()
    {
        yield return null;
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



    public static IEffect GetEffect(long id)
    {
        if (Instance._effectDic.ContainsKey(id))
        {
            return Instance._effectDic[id];
        }
        Debug.Error($"没有找到该特效{id}");
        return default(IEffect);
    }

   
}



