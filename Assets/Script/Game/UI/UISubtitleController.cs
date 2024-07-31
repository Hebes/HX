using System.Text;
using DG.Tweening;
using UnityEngine;

public class UISubtitleController : MonoBehaviour
{
    private bool IsShow => this.label.alpha > 0f;

    private void Awake()
    {
        R.Ui.UISubtitle = this;
    }

    private void Start()
    {
        this.label.alpha = 0f;
    }

    public void Show(string subtitle)
    {
        subtitle = UISubtitleController.Pretreatment(subtitle);
        //this.label.text = subtitle;
        this.label.alpha = 1f;
    }

    public void Hide()
    {
        this.label.alpha = 0f;
    }

    public YieldInstruction FadeIn(string subtitle)
    {
        if (this._fadeIn != null && this._fadeIn.IsPlaying())
        {
            this._fadeIn.Kill(false);
            this._fadeIn = null;
        }
        if (this._fadeOut != null && this._fadeOut.IsPlaying())
        {
            this._fadeOut.Kill(false);
            this._fadeOut = null;
        }
        subtitle = UISubtitleController.Pretreatment(subtitle);
        //this.label.text = subtitle;
        this._fadeIn = this.label.DOFade(0.8f, 1.5f);
        this._fadeIn.OnComplete(delegate
        {
            this._fadeIn = null;
        });
        return this._fadeIn.WaitForCompletion();
    }

    public YieldInstruction FadeOut()
    {
        if (this._fadeIn != null)
        {
            this._fadeIn.Kill(false);
            this._fadeIn = null;
        }
        if (this._fadeOut != null || !this.IsShow)
        {
            return null;
        }
        this._fadeOut = this.label.DOFade(0f, 1.5f);
        this._fadeOut.OnComplete(delegate
        {
            this._fadeOut = null;
        });
        return this._fadeOut.WaitForCompletion();
    }

    private static string Pretreatment(string str)
    {
        return new StringBuilder(str).Insert(0, "[-]").Replace("[-]", "[FFFFFF]").Replace("\\n", "\n").ToString();
    }

    
    //private UILabel label;
    [SerializeField]private CanvasGroup label;

    private Tweener _fadeIn;

    private Tweener _fadeOut;
}