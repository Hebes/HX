using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*--------脚本描述-----------

描述:
	叠加的Buff请继承这个接口

-----------------------*/

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
