using System.Buffers.Text;
using UnityEngine;

/*--------脚本描述-----------

描述:
    窗口基类

-----------------------*/

namespace Core
{
    public class UIBase : MonoBehaviour
    {
        public bool IsClearStack;                           //是否清空“栈集合”
        public EUIType type = EUIType.Normal;               //窗口的位置
        public EUIMode mode = EUIMode.Normal;               //窗口显示类型
        public EUILucenyType lucenyType = EUILucenyType.Lucency;   //窗口的透明度
        public string UIName { get; set; }                        //UI的名称

        /// <summary>初始化方法</summary>
        /// <param name="type">窗口的位置</param>
        /// <param name="mod">窗口显示类型</param>
        /// <param name="lucenyType">窗口的透明度</param>
        /// <param name="isClearStack">是否清空“栈集合”</param>
        public void InitUIBase(EUIType type, EUIMode mod, EUILucenyType lucenyType, bool isClearStack = false)
        {
            this.type = type;
            this.mode = mod;
            this.lucenyType = lucenyType;
            //this.name = gameObject.name.Replace("(Clone)", "");// this.GetType().ToString();
            IsClearStack = isClearStack;
        }

        public virtual void UIAwake() { }
        public virtual void UIOnEnable()
        {
            this.gameObject.SetActive(true);
            //设置模态窗体调用(必须是弹出窗体)
            if (type == EUIType.PopUp)
                UIMaskMgr.Instance.SetMaskWindow(gameObject, lucenyType);
        }
        public virtual void UIOnDisable()
        {
            gameObject.SetActive(false);
            //取消模态窗体调用
            if (type == EUIType.PopUp)
                UIMaskMgr.Instance.CancelMaskWindow();
        }
        public virtual void UIOnDestroy() { }
        public virtual void Freeze()
        {
            //冻结状态（即：窗体显示在其他窗体下面）
            gameObject.SetActive(true);
        }


        protected void OpenUIForm<T>(string uiFormName) where T : UIBase => CoreUI.ShwoUIPanel<T>(uiFormName);
        protected T GetUIForm<T>(string uiFormName) where T : UIBase => CoreUI.GetUIPanl<T>(uiFormName);
        protected void CloseUIForm() => CoreUI.CloseUIForms(UIName);
        protected void CloseOtherUIForm(string uiFormName) => CoreUI.CloseUIForms(uiFormName);
    }
}
