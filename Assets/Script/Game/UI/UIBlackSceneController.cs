using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIBlackSceneController : MonoBehaviour
{
    public float Alpha
    {
        get => _panel.alpha;
        set => _panel.alpha = value;
    }

    private void Awake()
    {
        _panel = GetComponent<CanvasGroup>();
        _panel.alpha = 0f;
    }

    public YieldInstruction FadeTransparent(float during = 0.3f, bool ignoreTimeScale = false)
    {
        return FadeTo(0f, during, ignoreTimeScale);
    }

    public YieldInstruction FadeBlack(float during = 0.3f, bool ignoreTimeScale = false)
    {
        return FadeTo(1f, during, ignoreTimeScale);
    }

    public YieldInstruction FadeTo(float endValue, float during, bool ignoreTimeScale = false)
    {
        return DOTween.To(() => _panel.alpha, delegate(float alpha)
        {
            _panel.alpha = alpha;
        }, endValue, during).SetUpdate(ignoreTimeScale).WaitForCompletion();
    }

    public void Kill()
    {
        _panel.DOKill(false);
    }

    public CanvasGroup _panel;
}