using System.Collections.Generic;

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
        private List<IInput> _inputList;        //组合输入

        public void ICoreInit()
        {
            Instance = this;
            _inputList = new List<IInput>();
        }

        public static void AddInputType(IInput input)
        {
            Instance._inputList.AddNotContainElement(input);
            input.Init();
        }

        public T GetInputType<T>() where T : IInput
        {
            return Instance._inputList.GetContainElement<IInput, T>();
        }
    }
}
