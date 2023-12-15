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
        /// <summary> 指令 </summary>
        Oder,
    }

    public class CoreInput : ICore
    {
        public static CoreInput Instance { get; private set; }
        private IInput input;           //输入类型

        public void ICoreInit()
        {
            Instance = this;
            input = new InputPC();
            input.Init();
        }

        //public static void AddKeyCode()
        //{
        //    if (Instance.input is IInputPCKeyCode)
        //    {
        //        Instance.input.add
        //    }
        //}

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
