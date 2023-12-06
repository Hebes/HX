using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    /// <summary>
    /// 按键事件
    /// </summary>
    public enum IInputType
    {
        w,
        s,
        a,
        d,
    }

    public class CoreInput : ICore
    {
        private IInput input;           //输入类型

        public void ICoreInit()
        {
            input = new InputPC();
            input.Init();
        }

        /// <summary>
        /// 切换按键
        /// </summary>
        /// <param name="inputType"></param>
        public void SwitchKeyCode(IInputType inputType)
        {

        }

        /// <summary>
        /// 切换按键监听事件
        /// </summary>
        public void SwitchKeyCodeAction(IInputType inputType)
        {

        }
    }
}
