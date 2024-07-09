using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Framework.Core
{
    /// <summary>
    /// PC
    /// </summary>
    public class InputPC : IInput, IUpdate
    {
        public List<KeyBoardData> KeyBoardUpdateList;
        private Coroutine _changeKeyCodeCoroutine;

        public void Init()
        {
            CoreBehaviour.Add(this);
        }

        private void Update()
        {
            foreach (var value in KeyBoardUpdateList)
                value.Action?.Invoke(value);
        }

        /// <summary>
        /// 添加按键监听
        /// </summary>
        /// <param name="keyBoard"></param>
        /// <exception cref="Exception"></exception>
        private void Add(KeyBoardData keyBoard)
        {
            foreach (var keyBoardData in KeyBoardUpdateList)
            {
                if (keyBoardData.KeyCode != keyBoard.KeyCode) continue;
                throw new Exception($"当前已有按键{keyBoard.KeyCode.ToString()}");
            }

            KeyBoardUpdateList.Add(keyBoard);
        }

        /// <summary>
        /// 移除按键
        /// </summary>
        /// <param name="keyCode"></param>
        public void Remove(KeyCode keyCode)
        {
            for (var i = KeyBoardUpdateList.Count - 1; i >= 0; i--)
            {
                if (KeyBoardUpdateList[i].KeyCode != keyCode) continue;
                KeyBoardUpdateList.RemoveAt(i);
                break;
            }
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        public string GetStr(EInputKeyBoardType value)
        {
            switch (value)
            {
                case EInputKeyBoardType.Attack:
                    return "攻击";
                case EInputKeyBoardType.MoveUp:
                    return "上移动";
                case EInputKeyBoardType.MoveDown:
                    return "下移动";
                case EInputKeyBoardType.MoveLeft:
                    return "左移动";
                case EInputKeyBoardType.MoveRight:
                    return "右移动";
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }


        /// <summary>
        /// 直接改键
        /// </summary>
        /// <param name="value"></param>
        /// <param name="keyCode"></param>
        /// <param name="???"></param>
        private void ChangeKeyCodeDirect(EInputKeyBoardType value, KeyCode keyCode)
        {
            foreach (var keyBoardData in KeyBoardUpdateList)
            {
                if (keyBoardData.InputKeyBoardType != value) continue;
                keyBoardData.KeyCode = keyCode;
                break;
            }
        }

        /// <summary>
        /// 切换按键执行方法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="action"></param>
        public void ChangeAction(EInputKeyBoardType value, UnityAction<KeyBoardData> action)
        {
            foreach (var keyBoardData in KeyBoardUpdateList)
            {
                if (keyBoardData.InputKeyBoardType != value) continue;
                keyBoardData.Action = action;
                break;
            }
        }

        public void ChangeAction(EInputKeyBoardType value, UnityAction value1, UnityAction value2, UnityAction value3)
        {
            foreach (var keyBoardData in KeyBoardUpdateList)
            {
                if (keyBoardData.InputKeyBoardType != value) continue;
                keyBoardData.KeyBoardListener.Action1 = value1;
                keyBoardData.KeyBoardListener.Action2 = value2;
                keyBoardData.KeyBoardListener.Action3 = value3;
                break;
            }
        }

        /// <summary>
        /// 改键(需要按键)
        /// </summary>
        /// <param name="keyCode">需要改的按键</param>
        /// <param name="changeOverAction"></param>
        /// <param name="funcAction"></param>
        public void ChangeKeyCode(KeyCode keyCode, UnityAction changeOverAction, UnityAction funcAction)
        {
            _changeKeyCodeCoroutine = CoreBehaviour.AddCoroutine(KeySet(keyCode));
            return;

            IEnumerator KeySet(KeyCode keyCodeValue)
            {
                var keyCodeList = Enum.GetValues(typeof(KeyCode));
                while (true)
                {
                    if (!Input.anyKey)
                    {
                        yield return null;
                        continue;
                    }

                    foreach (KeyCode keycode in keyCodeList)
                    {
                        //去除鼠标按键的影响
                        if (!Input.GetKeyDown(keycode)) continue;
                        if (Input.GetMouseButton(0)) continue;
                        if (Input.GetMouseButton(1)) continue;
                        if (Input.GetMouseButton(2)) continue;

                        foreach (var value in KeyBoardUpdateList)
                        {
                            if (value.KeyCode != keyCodeValue) continue;

                            //检查是否有相同的按键
                            bool canSwitch = true;
                            foreach (var keycodeTemp in KeyBoardUpdateList)
                            {
                                if (keycodeTemp.KeyCode != keycode) continue;
                                //if (keyCodeValue == keyCode) continue;
                                funcAction?.Invoke();
                                canSwitch = false;
                                break;
                            }

                            if (canSwitch) value.KeyCode = keycode;
                            break;
                        }

                        CoreBehaviour.StopCoroutine(_changeKeyCodeCoroutine);
                        changeOverAction?.Invoke();
                    }

                    yield return null;
                }
            }
        }

        public void CoreUpdate()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 按键数据
    /// </summary>
    public class KeyBoardData
    {
        public EInputKeyBoardType InputKeyBoardType;
        public KeyCode KeyCode;
        public UnityAction<KeyBoardData> Action;

        public KeyBoardData(KeyCode keyCode,
            EInputKeyBoardType inputKeyBoardTypeValue,
            UnityAction<KeyBoardData> action)
        {
            KeyCode = keyCode;
            InputKeyBoardType = inputKeyBoardTypeValue;
            Action = action;
        }

        /****************************************拓展*************************************************/
        public KeyBoardListener KeyBoardListener;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="inputKeyBoardTypeValue"></param>
        /// <param name="value1">短按</param>
        /// <param name="value2">长按</param>
        public KeyBoardData(KeyCode keyCode,
            EInputKeyBoardType inputKeyBoardTypeValue)
        {
            KeyCode = keyCode;
            InputKeyBoardType = inputKeyBoardTypeValue;
            KeyBoardListener = new();
            Action = KeyBoardListener.Update;
        }
    }

    /// <summary>
    /// 拓展键盘监听
    /// </summary>
    public class KeyBoardListener
    {
        /// <summary>
        /// 键盘是否按下
        /// </summary>
        private bool isKeyPressed;

        /// <summary>
        /// 按下开始的时间
        /// </summary>
        private float pressStartTime;

        /// <summary>
        /// 长按持续时间（秒）
        /// </summary>
        private const float longPressDuration = 0.5f;


        /// <summary>
        /// 短按事件
        /// </summary>
        public UnityAction Action1;

        /// <summary>
        /// 长按事件
        /// </summary>
        public UnityAction Action2;

        /// <summary>
        /// 抬起事件
        /// </summary>
        public UnityAction Action3;

        public void Update(KeyBoardData data)
        {
            if (Input.GetKey(data.KeyCode))
            {
                pressStartTime += Time.deltaTime;
                switch (pressStartTime)
                {
                    case < longPressDuration:
                        Action1?.Invoke();
                        break;
                    case >= longPressDuration:
                        Action2?.Invoke();
                        break;
                }
            }

            // 检测键盘抬起事件
            if (Input.GetKeyUp(data.KeyCode))
            {
                pressStartTime = 0;
                Action3?.Invoke();
            }
        }
    }
}