using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IBuffManager
{
    /// <summary>
    /// NPCID或者自己
    /// </summary>
    public int id { get; set; }

    /// <summary>
    /// BUFF数据结构
    /// </summary>
    public List<IBuffData> buffDataList { get; set; }
}
