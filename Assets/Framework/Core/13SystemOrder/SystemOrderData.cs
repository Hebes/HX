using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// 一条指令数据
    /// </summary>
    public class SystemOrderData : ISystemOder
    {
        /// <summary>
        /// 指令名称
        /// </summary>
        public string orderName;

        /// <summary>
        /// 指令对应的代码
        /// </summary>
        public Action orderAction;

        string ISystemOder.OrderName { get => orderName; set => orderName = value; }
    }
}
