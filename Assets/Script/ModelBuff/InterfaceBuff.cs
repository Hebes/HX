using System.Collections.Generic;
using Core;

/// <summary>
/// Buff生命周期，基本都要继承
/// </summary>
public interface IBuffBehaviour : IID
{
    /// <summary>
    /// 技能初始化，例如加载等
    /// </summary>
    public void BuffInit();

    /// <summary>
    /// 触发-》添加表现效果
    /// </summary>
    /// <param name="buffCarrier"></param>
    public void BuffTrigger(IBuffCarrier buffCarrier);

    /// <summary>
    /// 完毕销毁-》移除表现效果
    /// </summary>
    public void BuffOver(IBuffCarrier buffCarrier);
}

/// <summary>
/// Buff持有者，基本都要继承
/// </summary>
public interface IBuffCarrier : IID, IName
{
    /// <summary>
    /// 存放Buff的数据结构
    /// </summary>
    public List<IBuff> BuffList { get; set; }


    /// <summary>
    /// 添加技能
    /// </summary>
    /// <param name="skillCarrier">技能持有者</param>
    /// <param name="buffData">buff数据</param>
    public static void AddBuff(IBuffCarrier buffCarrier, IBuff buffData)
    {
        if (IBuffCarrier.ChackHoldBuff(buffCarrier, buffData))
            return;
        buffCarrier.BuffList.Add(buffData);
        if (buffData is IBuffBehaviour buffBehaviour)
            buffBehaviour.BuffInit();
    }

    /// <summary>
    /// 移除Buff
    /// </summary>
    public static void RemoveBuff(IBuffCarrier buffCarrier, IBuff buffData)
    {
        if (IBuffCarrier.ChackHoldBuff(buffCarrier, buffData))
            buffCarrier.BuffList.Remove(buffData);
    }

    /// <summary>
    /// 检查是否持有次技能
    /// </summary>
    /// <param name="buffCarrier"></param>
    /// <param name="buffData"></param>
    /// <returns>true 已经持有 false 未持有</returns>
    public static bool ChackHoldBuff(IBuffCarrier buffCarrier, IBuff buffData)
    {
        if (buffCarrier.BuffList.Contains(buffData))
        {
            Debug.Error($"{buffCarrier.Name}已经持有此BUFF");
            return true;
        }
        return false;
    }
}

/// <summary>
/// Buff的数据，基本都要继承
/// </summary>
public interface IBuff : IID, IName, IDescribe
{

}

/// <summary>
/// 叠加的Buff请继承这个接口
/// </summary>
public interface IBuffSuperposition : IID
{
    /// <summary>
    /// 是否可以销毁Buff
    /// </summary>
    bool DelBuff { get; set; }

    /// <summary>
    /// 移除一层buff
    /// </summary>
    public void RemoveBuff();
}