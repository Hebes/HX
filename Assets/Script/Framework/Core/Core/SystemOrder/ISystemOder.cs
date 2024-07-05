using System.Collections.Generic;

namespace Framework.Core
{
    public interface ISystemOder
    {
        /// <summary>
        /// 指令名称
        /// </summary>
        public string OrderName { get; set; }

        /// <summary>
        /// 指令类型
        /// </summary>
        public EOrderType OrderType { get; }

        /// <summary>
        /// 指令初始化
        /// </summary>
        public void OrderInit();

        /// <summary>
        /// 指令执行
        /// </summary>
        /// <param name="args"></param>
        public string TriggerOder(List<string> args);
    }
}
