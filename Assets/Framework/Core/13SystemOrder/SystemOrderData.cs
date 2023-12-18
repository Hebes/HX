using System;
using System.Collections.Generic;

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
        private string orderName;

        /// <summary>
        /// 指令对应的代码
        /// </summary>
        private Action<List<string>> orderAction;

        public EOrderType OrderType => EOrderType.Item;
        public string OrderName { get => orderName; set => orderName = value; }
        public void OrderInit()
        {
        }
        public string TriggerOder(List<string> args)
        {
            orderAction?.Invoke(args);
            return "指令执行成功";
        }
    }
}
