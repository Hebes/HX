using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Core;

/// <summary>
/// Buff数据
/// </summary>
public class BuffData : IID, IName, IDescribe
{
    public long ID { get; set; }
    public string Name { get; set; }
    public string Des { get ; set ; }

    /// <summary>
    /// 技能初始化，例如加载等
    /// </summary>
    public virtual void BuffInit()
    {

    }

    /// <summary>
    /// 触发-》添加表现效果
    /// </summary>
    /// <param name="buffCarrier"></param>
    public virtual void BuffTrigger(IBuffList buffCarrier)
    {

    }

    /// <summary>
    /// 完毕销毁-》移除表现效果
    /// </summary>
    public virtual void BuffOver(IBuffList buffCarrier)
    {

    }
}
