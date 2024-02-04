
/*--------脚本描述-----------

描述:
	事件接口

-----------------------*/

using System.Collections.Generic;

namespace Core
{
    public interface IEvent : IID
    {
        /// <summary>
        /// 方法名称
        /// </summary>
        public string MethodName { get; set; }
    }
}

