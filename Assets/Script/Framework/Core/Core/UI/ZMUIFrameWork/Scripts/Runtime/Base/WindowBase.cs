using System;
using DG.Tweening;
using UnityEngine;

namespace Framework.Core
{
    public class WindowBase : MonoBehaviour
    {
        public GameObject gameObject { get; set; } //当前窗口物体
        public Transform transform { get; set; } //代表自己
        public Canvas Canvas { get; set; }
        public string Name { get; set; }
        public bool Visible { get; set; }
        public bool PopStack { get; set; } //是否是通过堆栈系统弹出的弹窗

        /// <summary>
        /// 全屏窗口标志(在窗口Awake接口中进行设置,智能显隐开启后当全屏弹窗弹出时，被遮挡的窗口都会通过伪隐藏隐藏掉，从而提升性能)
        /// </summary>
        public bool FullScreenWindow { get; set; }

        public Action<WindowBase> PopStackListener { get; set; }


        private CanvasGroup mUIMaskCanvasGroup;
        private CanvasGroup mCanvasGroup;
        protected Transform mUIContent;
        protected bool mDisableAnim = false; //禁用动画

        /// <summary>
        /// 与Mono Awake调用时机和次数保持一致
        /// </summary>
        public virtual void OnAwake()
        {
            mCanvasGroup = transform.GetComponent<CanvasGroup>();
            mUIMaskCanvasGroup = transform.Find("UIMask").GetComponent<CanvasGroup>();
            mUIContent = transform.Find("UIContent").transform;
        }

        /// <summary>
        /// 与MonoOnEnable一致
        /// </summary>
        public virtual void OnShow()
        {
        }

        protected virtual void OnUpdate()
        {
        }

        /// <summary>
        /// 与Mono OnDisable 一致
        /// </summary>
        public virtual void OnHide()
        {
        }

        /// <summary>
        /// 在当前界面被销毁时调用一次
        /// </summary>
        public virtual void OnDestroy()
        {
        }

        /// <summary>
        /// 设置物体的可见性
        /// </summary>
        /// <param name="isVisble"></param>
        public virtual void SetVisible(bool isVisble)
        {
            if (mCanvasGroup == null)
            {
                Debug.LogError("CanvasGroup is Null!" + Name);
                return;
            }

            Visible = mCanvasGroup.interactable = mCanvasGroup.blocksRaycasts = isVisble;
            mCanvasGroup.alpha = isVisble ? 1 : 0;
            if (isVisble && PopStack)
            {
                gameObject.SetActive(false);
                gameObject.SetActive(true);
            }
        }

        public virtual void PseudoHidden(int value)
        {
            mUIMaskCanvasGroup.alpha = mCanvasGroup.alpha = value;
            mUIMaskCanvasGroup.interactable = mCanvasGroup.interactable = value == 1 ? true : false;
            mUIMaskCanvasGroup.blocksRaycasts = mCanvasGroup.blocksRaycasts = value == 1 ? true : false;
        }


        #region 动画管理

        public void ShowAnimation()
        {
            //基础弹窗不需要动画
            if (Canvas.sortingOrder > 90 && mDisableAnim == false)
            {
                //Mask动画
                mUIMaskCanvasGroup.alpha = 0;
                mUIMaskCanvasGroup.DOFade(1, 0.2f);
                //缩放动画
                mUIContent.localScale = Vector3.one * 0.8f;
                mUIContent.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
            }
        }

        public void HideAnimation()
        {
            if (Canvas.sortingOrder > 90 && mDisableAnim == false)
            {
                mUIContent.DOScale(Vector3.one * 1.1f, 0.2f).SetEase(Ease.OutBack).OnComplete(() => { CoreUI.Instance.HideWindow(Name); });
            }
            else
            {
                CoreUI.Instance.HideWindow(Name);
            }
        }

        #endregion


        public void SetMaskVisible(bool isVisible)
        {
            if (!CoreUI.Instance.SINGMASK_SYSTEM) return;
            mUIMaskCanvasGroup.alpha = isVisible ? 1 : 0;
            mUIMaskCanvasGroup.blocksRaycasts = isVisible;
            //特殊情况下进行窗口同层级重绘渲染
            if (isVisible && PopStack)
            {
                mUIMaskCanvasGroup.gameObject.SetActive(false);
                mUIMaskCanvasGroup.gameObject.SetActive(true);
            }
        }

        protected void HideWindow()
        {
            //HideAnimation();
            CoreUI.Instance.HideWindow(Name);
        }

        protected T PopUpWindow<T>() where T : WindowBase
        {
            return CoreUI.Instance.PopUpWindow<T>();
        }

        protected T GetWindow<T>() where T : WindowBase
        {
            return CoreUI.Instance.GetWindow<T>();
        }
    }
}