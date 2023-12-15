using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

namespace Core
{

    public enum BTN_TYPE
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,

        FIRE,
        JUMP,
    }

    /// <summary>
    /// 新输入系统
    /// 
    /// Project 页面中 Create -> Input Actions 来创建一个新的 Input Action Assets。
    /// 
    /// 接着打开 Edit -> Project Settings，
    /// 在 Player 选项页中的 Other Settings -> Configuration 中
    /// 设置 Active Input Handling 为 Input System 以禁用旧输入系统，
    /// 以防止两个输入系统的冲突和杜绝隐患。
    /// 
    /// 一个绑定可能有以下几种合成类型（Composite Type）：
    /// 1D Axis 单轴输入, 
    /// 2D Vector 双轴向量输入, 
    /// Button 按钮输入, 
    /// Button with modifier 带有条件的按钮输入, 
    /// Custom composite 自定义输入数据类型。
    /// 
    /// https://zhuanlan.zhihu.com/p/106396562
    /// </summary>
    public class InputNewInputPC : MonoBehaviour, IInput
    {
        public Button buttonw;
        public Text textw;

        private Operation operation;
        /// <summary> 输入的字符串 </summary>
        private InputActionAsset inputActions;

        private Dictionary<BTN_TYPE, string> btnDic = new Dictionary<BTN_TYPE, string>()
        {
            {BTN_TYPE.JUMP,"<Keyboard>/space" },
        };
        //记录当前改哪一个键
        private BTN_TYPE nowType;

        public void Init()
        {

        }

        public void Updata()
        {

        }
        private void Awake()
        {
            operation = new Operation();
            inputActions = operation.asset;
            string str = inputActions.ToJson();
            UnityEngine.Debug.Log(str);
            operation.Enable();

            operation.Player.jump.performed += Jump;
            //buttonw.onClick.AddListener(() =>
            //{
            //    ChangeBtn(BTN_TYPE.UP);
            //});
        }

        private void Jump(InputAction.CallbackContext context)
        {
            UnityEngine.Debug.Log($"跳跃");
        }


        /// <summary>
        /// 打开输入
        /// </summary>
        public void OpenInput()
        {
            inputActions.Enable();
        }


        private void ChangeBtn(BTN_TYPE type)
        {
            //nowType = type;
            ////得到一次任意键输入
            //InputSystem.onAnyButtonPress.CallOnce(ChangeBtnReally);
        }
    }
}
