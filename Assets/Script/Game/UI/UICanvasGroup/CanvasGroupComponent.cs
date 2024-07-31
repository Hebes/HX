using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupComponent : MonoBehaviour
{
    private Tweener _tweener;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    
    // private void Update()
    // {
    //     if (this._lastLayer != null && Core.Input.UI.Cancel.OnClick)
    //     {
    //         this.OnCancelClick();
    //     }
    // }
    //
    // public void OnCancelClick()
    // {
    //     this._lastLayer.Appear();
    //     this.KillTweenerIfPlaying();
    //     this.Disappear();
    //     UIKeyInput.LoadHoveredObject();
    // }
    
    /// <summary>
    /// 显示
    /// </summary>
    public void Appear()
    {
        gameObject.SetActive(true);
        KillTweenerIfPlaying();
        _canvasGroup.alpha = 0f;
        _tweener = _canvasGroup.DOFade(1f, 0.2f);
    }
    
    /// <summary>
    /// 隐藏
    /// </summary>
    public void Disappear()
    {
        KillTweenerIfPlaying();
        _canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 关闭Tweener
    /// </summary>
    private void KillTweenerIfPlaying()
    {
        if (_tweener == null) return;
        if (!_tweener.IsPlaying()) return;
        _tweener.Kill(false);
    }
}