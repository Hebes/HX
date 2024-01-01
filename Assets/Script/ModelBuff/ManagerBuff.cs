using Core;
using System.Collections.Generic;

/// <summary>
/// buff管理器
/// </summary>
public class ManagerBuff : ICore
{
    public static ManagerBuff Instance;
    private List<IBuffCarrier> _buffCarrierList;
    public void ICoreInit()
    {
        Instance = this;
        _buffCarrierList = new List<IBuffCarrier>();
    }


    /// <summary>
    /// 添加Buff
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t">buff携带者</param>
    /// <param name="eBuffType">Buff类型</param>
    /// <param name="buffData">buff效果</param>
    public void AddBuff(IBuffCarrier buffCarrier, IBuff buffData)
    {
        if (!buffCarrier.BuffList.Contains(buffData))
            buffCarrier.BuffList.Add(buffData);
        if (buffData is IBuffBehaviour)
            (buffData as IBuffBehaviour).Trigger(buffCarrier);
        AddBuffCarrier(buffCarrier);
    }

    /// <summary>
    /// 移除buff
    /// </summary>
    public void RemoveBuff(IBuffCarrier buffCarrier, IBuff buffData)
    {
        if (buffData is IBuffSuperposition superposition)
        {
            if (superposition.DelBuff)
                buffCarrier.BuffList.Remove(buffData);
            else
                superposition.RemoveBuff();
        }
        else
        {
            if (buffData is IBuffBehaviour buff)
                buff.OnDestroy(buffCarrier);
            buffCarrier.BuffList.Remove(buffData);
        }
    }

    /// <summary>
    /// 添加到管理类
    /// </summary>
    /// <param name="buffCarrier"></param>
    private void AddBuffCarrier(IBuffCarrier buffCarrier)
    {
        if (_buffCarrierList.Contains(buffCarrier))
            return;
        _buffCarrierList.Add(buffCarrier);
    }
}
