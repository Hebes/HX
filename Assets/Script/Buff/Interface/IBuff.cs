using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IBuff
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
