using UnityEngine;

namespace Framework.Core
{
    public interface IUI
    {
        /// <summary>
        /// UI界面的物体
        /// </summary>
        public GameObject GameObject { get; set; }

        /// <summary>
        /// 界面名称
        /// </summary>
        public string UIName { get; set; }

        /// <summary>
        /// 窗口的位置
        /// </summary>
        public EUIType UIType { get; set; }

        /// <summary>
        /// 窗口显示类型
        /// </summary>
        public EUIMode UIMode { get; set; }

        /// <summary>
        /// 窗口的透明度
        /// </summary>
        public EUILucenyType UILucenyType { get; set; }
    }

    public interface IUIAwake
    {
        /// <summary>
        /// 界面初始化
        /// </summary>
        void UIAwake();
    }

    public interface IUIUpdata
    {
        /// <summary>
        /// 界面轮询
        /// </summary>
        void UIUpdata();
    }

    public interface IUIOnEnable
    {
        /// <summary>
        /// 界面开启
        /// </summary>
        void UIOnEnable();
    }

    public interface IUIOnDisable
    {
        /// <summary>
        /// 界面关闭
        /// </summary>
        void UIOnDisable();
    }

    public interface IUIOnDestroy
    {
        /// <summary>
        /// 界面销毁
        /// </summary>
        void UIOnDestroy();
    }
}
