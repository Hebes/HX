using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IBuffCarrier
{
    /// <summary>
    /// Buff持有者的编号
    /// </summary>
    public int ID { get; }

    /// <summary>
    /// 存放Buff的数据结构
    /// </summary>
    public List<IBuffData> BuffList { get; set; }
}
