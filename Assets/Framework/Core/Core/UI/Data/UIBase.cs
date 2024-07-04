using System.Buffers.Text;
using UnityEngine;

/*--------脚本描述-----------

描述:
    窗口基类

-----------------------*/

namespace Framework.Core
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIBase : MonoBehaviour, IUI
    {
        /// <summary>
        /// 是否清空“栈集合”
        /// </summary>
        [SerializeField] private bool IsClearStack;

        /// <summary>
        /// UI界面
        /// </summary>
        [SerializeField] private GameObject go;

        /// <summary>
        /// UI名称
        /// </summary>
        [SerializeField] private string uiName;

        /// <summary>
        /// 窗口的位置
        /// </summary>
        [SerializeField] private EUIType type = EUIType.Normal;

        /// <summary>
        /// 窗口显示类型
        /// </summary>
        [SerializeField] private EUIMode mode = EUIMode.Normal;

        /// <summary>
        /// 窗口的透明度
        /// </summary>
        [SerializeField] private EUILucenyType lucenyType = EUILucenyType.Lucency;


        public GameObject GameObject { get => go; set => go = value; }
        public EUIType UIType { get => type; set => type = value; }
        public EUIMode UIMode { get => mode; set => mode = value; }
        public EUILucenyType UILucenyType { get => lucenyType; set => lucenyType = value; }
        public string UIName { get => uiName; set => uiName = value; }


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
            IsClearStack = isClearStack;
        }

        protected void OpenUIForm<T>(string uiFormName) where T : UIBase => CoreUI.ShwoUIPanel<T>(uiFormName);
        protected T GetUIForm<T>(string uiFormName) where T : UIBase => CoreUI.GetUIPanl<T>(uiFormName);
        protected void CloseUIForm() => CoreUI.CloseUIForms(uiName);
        protected void CloseOtherUIForm(string uiFormName) => CoreUI.CloseUIForms(uiFormName);
        
        
    }
}
