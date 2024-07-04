using System.Collections.Generic;
using System.Collections;
using Framework.Core;

/*--------脚本描述-----------

描述:
    特效管理

-----------------------*/
[ModelCreat(typeof(ManagerEffect), 1)]
public class ManagerEffect : IModel
{
    public static ManagerEffect Instance;

    private Dictionary<long, IEffect> _effectDic;

    public void Init()
    {
    }

    public IEnumerator AsyncEnter()
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
        if (Instance._effectDic.TryGetValue(id, out var effect))
            return effect;
        EDebug.Error($"没有找到该特效{id}");
        return default(IEffect);
    }
}