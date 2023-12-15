using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    /// <summary>
    /// PC
    /// </summary>
    public class InputPC : IInputPCKeyCode, IUpdata
    {
        //映射关系
        private Dictionary<IInputType, KeyCode> _keyCodeDic;    //对应按键
        private Dictionary<IInputType, Action> _inputAction;    //对饮事件
        private Coroutine coroutine;

        public void Init()
        {
            _keyCodeDic = new Dictionary<IInputType, KeyCode>();
            _inputAction = new Dictionary<IInputType, Action>();

            CoreBehaviour.Add(this);

            //KeyCodeDic.Add(IInputType.w, KeyCode.W);
            //InputAction.Add(IInputType.w, W);
        }

        public void AddKeyCode(IInputType inputType, KeyCode keyCode)
        {
            if (_keyCodeDic.ContainsKey(inputType))
            {
                Debug.Error("按键已存在");
                return;
            }
            _keyCodeDic.Add(inputType, keyCode);
        }

        public void RemoveKeyCode(IInputType inputType, KeyCode keyCode)
        {
            throw new NotImplementedException();
        }

        public void SwitchKeyCode(IInputType inputType, KeyCode keyCode)
        {
            CoreBehaviour.AddCoroutine(1,KeySet());
        }

        public void SwitchKeyCodeAction(IInputType inputType, KeyCode keyCode)
        {
        }

        public void OnUpdata()
        {
            //if (Input.GetKeyDown(KeyCodeDic[IInputType.w]))
            //{
            //    InputAction[IInputType.w]?.Invoke();
            //}
        }


        public void SwitchAction(IInputType inputType, Action action)
        {
            _inputAction[inputType] = action;
        }



        //改键
        private void Buttonw()
        {
            //coroutine = StartCoroutine(KeySet());
        }
        IEnumerator KeySet()
        {
            while (true)
            {
                UnityEngine.Debug.Log($"开始改键");
                if (Input.anyKeyDown)
                {
                    foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
                    {
                        if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
                        {
                            continue;//去除鼠标按键的影响
                        }

                        if (Input.GetKeyDown(keycode))
                        {
                            _keyCodeDic[IInputType.w] = keycode;
                            UnityEngine.Debug.Log($"改键结束");
                            CoreBehaviour.RemoveCoroutine(1);
                        }
                    }
                }
                yield return null;
            }
        }


        private void W()
        {
            UnityEngine.Debug.Log($"w");
        }

        
    }
}
