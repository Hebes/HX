using System;
using DG.Tweening;
using Framework.Core;
using UnityEngine;

/// <summary>
/// 窗口基类
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Canvas))]
public class WindowBase : MonoBehaviour
{
    private UIComponent _uiComponent;//组件
    private CanvasGroup _uIMask;//UI遮罩
    private CanvasGroup _canvasGroup;//组件自带
    private Transform _uiContent;
    private bool _disableAnim = false;//禁用动画
    public Canvas Canvas { get; private set; }
    public Action<WindowBase> PopStackListener { get; set; }//弹出堆栈监听器
    public bool PopStack { get; set; }//是否是通过堆栈系统弹出的弹窗
    public bool Visible { get; private set; }//是否可见
    
    private void InitBaseCompont()
    {
        _uiComponent = GetComponent<UIComponent>();
        Canvas = GetComponent<Canvas>();
        _canvasGroup = transform.GetComponent<CanvasGroup>();
        _uIMask = _uiComponent.Get<GameObject>("T_UIMask").GetComponent<CanvasGroup>();
    }
    
    #region 声明周期
    public virtual void OnAwake() { InitBaseCompont();} //只会在物体创建时执行一次 ，与Mono Awake调用时机和次数保持一致
    public virtual void OnShow() {ShowAnimation(); }  //在物体显示时执行一次，与MonoOnEnable一致
    public virtual void OnUpdate() { }//轮询
    public virtual void OnHide() { } //在物体隐藏时执行一次，与Mono OnDisable 一致
    public virtual void OnDes() { } //在当前界面被销毁时调用一次
    #endregion
    
    #region 动画管理
    private void ShowAnimation()
    {
        if (_disableAnim)return;
        transform.localScale = Vector3.one * 0.8f;
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    }
    
    private void HideAnimation() {
        UIModule.Instance.HideWindow(name);
    }
    #endregion
    
    public void HideWindow() {
        HideAnimation();
    }
    
    /// <summary>
    /// 设置物体的可见性
    /// </summary>
    /// <param name="isVisble"></param>
    public virtual void SetVisible(bool isVisble)
    {
        _canvasGroup.alpha = isVisble ? 1 : 0;
        _canvasGroup.blocksRaycasts = isVisble;
        Visible = isVisble;
    }  
    
    /// <summary>
    /// 设置Mask可见度
    /// </summary>
    /// <param name="isVisble"></param>
    public void SetMaskVisible(bool isVisble) {
        if (!UIModule.Instance.SINGMASK_SYSTEM) {
            return;
        }
        _uIMask.alpha = isVisble ? 1 : 0;
    }
}