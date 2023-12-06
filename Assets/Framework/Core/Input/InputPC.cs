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
    public class InputPC : MonoBehaviour, IInput
    {
        //映射关系
        Dictionary<IInputType, KeyCode> KeyCodeDic = new Dictionary<IInputType, KeyCode>(); //对应按键
        Dictionary<IInputType, Action> InputAction = new Dictionary<IInputType, Action>();  //对饮事件

        public Button buttonw;
        public Text textw;

        private Coroutine coroutine;

        public void Init()
        {
        }

        public void Updata()
        {

        }


        private void Awake()
        {
            KeyCodeDic.Add(IInputType.w, KeyCode.W);
            InputAction.Add(IInputType.w, W);

            buttonw.onClick.AddListener(Buttonw);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCodeDic[IInputType.w]))
            {
                InputAction[IInputType.w]?.Invoke();
            }
        }

        public void SwitchAction(IInputType inputType, Action action)
        {
            InputAction[inputType] = action;
        }


        //改键
        private void Buttonw()
        {
            coroutine = StartCoroutine(KeySet());
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
                            textw.text = keycode.ToString();
                            KeyCodeDic[IInputType.w]= keycode;
                            UnityEngine.Debug.Log($"改键结束");
                            StopCoroutine(coroutine);
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
