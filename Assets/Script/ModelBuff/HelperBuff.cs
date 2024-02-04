using Core;
using System.Collections.Generic;


/// <summary>
/// Buff持有者，基本都要继承
/// </summary>
public interface IBuffList : IID
{
    /// <summary>
    /// 存放Buff的数据结构
    /// </summary>
    public List<BuffData> BuffList { get; set; }
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



