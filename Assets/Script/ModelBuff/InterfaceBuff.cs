using System.Collections.Generic;

/// <summary>
/// Buff数据类，基本都要继承
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
public interface IBuffCarrier : IID
{
    /// <summary>
    /// 存放Buff的数据结构
    /// </summary>
    public List<IBuffData> BuffList { get; set; }
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