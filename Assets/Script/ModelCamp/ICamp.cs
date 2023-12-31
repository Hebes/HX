using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*--------脚本描述-----------

描述:
	阵营接口

-----------------------*/

public interface ICamp
{
    public ECamp enumsCamp { get; set; }

    /// <summary>
    /// 获取NPC编号
    /// </summary>
    public abstract void GetID();
}
