using UnityEngine;

namespace Core
{
    public interface IInputPCKeyCode : IInput
    {
        /// <summary>
        /// 添加按键
        /// </summary>
        /// <param name="inputType"></param>
        /// <param name="keyCode"></param>
        public void AddKeyCode(IInputType inputType, KeyCode keyCode);

        public void RemoveKeyCode(IInputType inputType, KeyCode keyCode);

        public void SwitchKeyCode(IInputType inputType, KeyCode keyCode);

        public void SwitchKeyCodeAction(IInputType inputType, KeyCode keyCode);
    }
}
