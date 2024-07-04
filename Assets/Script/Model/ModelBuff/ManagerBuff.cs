//using Core;
//using System.Collections;
//using System.Collections.Generic;

///// <summary>
///// buff管理器
///// </summary>
//public class ManagerBuff : IModel
//{
//    public static ManagerBuff Instance;
//    private List<IBuffList> _buffCarrierList;

//    public IEnumerator Enter()
//    {
//        Instance = this;
//        _buffCarrierList = new List<IBuffList>();
//        yield return null;
//    }

//    public IEnumerator Exit()
//    {
//        yield return null;
//    }



//    /// <summary>
//    /// 添加Buff
//    /// </summary>
//    /// <typeparam name="T"></typeparam>
//    /// <param name="t">buff携带者</param>
//    /// <param name="eBuffType">Buff类型</param>
//    /// <param name="buffData">buff效果</param>
//    //public void AddBuff(IBuffList buffCarrier, BuffData buffData)
//    //{
//    //    if (!buffCarrier.BuffList.Contains(buffData))
//    //        buffCarrier.BuffList.Add(buffData);
//    //    if (buffData is IBuffBehaviour)
//    //        (buffData as IBuffBehaviour).BuffTrigger(buffCarrier);
//    //    AddBuffCarrier(buffCarrier);
//    //}

//    /// <summary>
//    /// 移除buff
//    /// </summary>
//    //public void RemoveBuff(IBuffList buffCarrier, BuffData buffData)
//    //{
//    //    if (buffData is IBuffSuperposition superposition)
//    //    {
//    //        if (superposition.DelBuff)
//    //            buffCarrier.BuffList.Remove(buffData);
//    //        else
//    //            superposition.RemoveBuff();
//    //    }
//    //    else
//    //    {
//    //        if (buffData is IBuffBehaviour buff)
//    //            buff.BuffOver(buffCarrier);
//    //        buffCarrier.BuffList.Remove(buffData);
//    //    }
//    //}

//    /// <summary>
//    /// 添加到管理类
//    /// </summary>
//    /// <param name="buffCarrier"></param>
//    //private void AddBuffCarrier(IBuffList buffCarrier)
//    //{
//    //    if (_buffCarrierList.Contains(buffCarrier))
//    //        return;
//    //    _buffCarrierList.Add(buffCarrier);
//    //}
//}

//public static class HelperBuff
//{
//    /// <summary>
//    /// 添加技能
//    /// </summary>
//    /// <param name="skillCarrier">技能持有者</param>
//    /// <param name="buffData">buff数据</param>
//    //public static void AddBuff(IBuffList buffCarrier, BuffData buffData)
//    //{
//    //    if (buffCarrier.BuffList == null)
//    //        buffCarrier.BuffList = new List<BuffData>();

//    //    if (IBuffList.ChackHoldBuff(buffCarrier, buffData))
//    //        return;
//    //    buffCarrier.BuffList.Add(buffData);
//    //    if (buffData is IBuffBehaviour buffBehaviour)
//    //        buffBehaviour.BuffInit();
//    //}

//    /// <summary>
//    /// 移除Buff
//    /// </summary>
//    //public static void RemoveBuff(IBuffList buffCarrier, BuffData buffData)
//    //{
//    //    if (IBuffList.ChackHoldBuff(buffCarrier, buffData))
//    //        buffCarrier.BuffList.Remove(buffData);
//    //}

//    /// <summary>
//    /// 检查是否持有次技能
//    /// </summary>
//    /// <param name="buffCarrier"></param>
//    /// <param name="buffData"></param>
//    /// <returns>true 已经持有 false 未持有</returns>
//    //public static bool ChackHoldBuff(IBuffList buffCarrier, BuffData buffData)
//    //{
//    //    if (buffCarrier.BuffList.Contains(buffData))
//    //    {
//    //        Debug.Error($"{buffCarrier.Name}已经持有此BUFF");
//    //        return true;
//    //    }
//    //    return false;
//    //}
//}
