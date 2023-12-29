using System.Collections.Generic;
using Core;

/// <summary>
/// Buff生命周期，基本都要继承
/// </summary>
public interface IBuffBehaviour
{
    /// <summary>
    /// 触发-》添加表现效果
    /// </summary>
    /// <param name="buffCarrier"></param>
    public void Trigger(IBuffCarrier buffCarrier);

    /// <summary>
    /// 销毁-》移除表现效果
    /// </summary>
    public void OnDestroy(IBuffCarrier buffCarrier);
}

/// <summary>
/// Buff持有者，基本都要继承
/// </summary>
public interface IBuffCarrier : IID, IName
{
    /// <summary>
    /// 存放Buff的数据结构
    /// </summary>
    public List<IBuffData> BuffList { get; set; }


    /// <summary>
    /// 添加技能
    /// </summary>
    /// <param name="skillCarrier">技能持有者</param>
    /// <param name="buffData">buff数据</param>
    public static void AddBuff(IBuffCarrier buffCarrier, IBuffData buffData)
    {
        if (buffCarrier.ChackHoldBuff(buffCarrier, buffData))
            return;
        buffCarrier.BuffList.Add(buffData);

        if (buffData is IBuffBehaviour buffBehaviour)
            buffBehaviour.Trigger(buffCarrier);
    }

    /// <summary>
    /// 检查是否持有次技能
    /// </summary>
    /// <param name="buffCarrier"></param>
    /// <param name="buffData"></param>
    /// <returns>true 已经持有 false 未持有</returns>
    public bool ChackHoldBuff(IBuffCarrier buffCarrier, IBuffData buffData)
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
public interface IBuffData : IID, IName, IDescribe
{

}

/// <summary>
/// 叠加的Buff请继承这个接口
/// </summary>
public interface IBuffSuperposition
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