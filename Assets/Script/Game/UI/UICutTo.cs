using DG.Tweening;
using Framework.Core;
using UnityEngine;

/// <summary>
/// 转场
/// </summary>
public class UICutTo : WindowBase
{
    private CanvasGroupComponent _canvasGroupComponent;
    private CanvasGroup _canvasGroup;
    
    public override void OnAwake()
    {
        base.OnAwake();
        _canvasGroupComponent = GetComponent<CanvasGroupComponent>();
        
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
    }

    public override void OnShow()
    {
        base.OnShow();
        //_canvasGroupComponent.Appear();
    }
    
    
    public float Alpha
    {
        get => _canvasGroup.alpha;
        set => _canvasGroup.alpha = value;
    }

    
    /// <summary>
    /// 透明消失
    /// </summary>
    /// <param name="during"></param>
    /// <param name="ignoreTimeScale"></param>
    /// <returns></returns>
    public YieldInstruction FadeTransparent(float during = 0.3f, bool ignoreTimeScale = false)
    {
        return this.FadeTo(0f, during, ignoreTimeScale);
    }

    /// <summary>
    /// 黑色消失
    /// </summary>
    /// <param name="during"></param>
    /// <param name="ignoreTimeScale"></param>
    /// <returns></returns>
    public YieldInstruction FadeBlack(float during = 0.3f, bool ignoreTimeScale = false)
    {
        return this.FadeTo(1f, during, ignoreTimeScale);
    }

    /// <summary>
    /// 逐渐消失
    /// </summary>
    /// <param name="endValue"></param>
    /// <param name="during"></param>
    /// <param name="ignoreTimeScale"></param>
    /// <returns></returns>
    public YieldInstruction FadeTo(float endValue, float during, bool ignoreTimeScale = false)
    {
        return DOTween.To(() => _canvasGroup.alpha, delegate(float alpha)
        {
            _canvasGroup.alpha = alpha;
        }, endValue, during).SetUpdate(ignoreTimeScale).WaitForCompletion();
    }

    public void Kill()
    {
        _canvasGroup.DOKill(false);
    }
    
}