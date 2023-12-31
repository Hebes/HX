using UnityEngine;
using UnityEngine.UI;

/*--------脚本描述-----------

描述:
	多语言组件

-----------------------*/

[RequireComponent(typeof(Text))]
public class LanguageComponent : MonoBehaviour
{
    public string key = "错误";
    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }
    private void OnEnable()
    {
        OnSwitchLanguage();
        ManagerLanguage.Instance.languageChangeEvt += OnSwitchLanguage;
    }
    private void OnDisable()
    {
        ManagerLanguage.Instance.languageChangeEvt -= OnSwitchLanguage;
    }

    private void OnSwitchLanguage()
    {
        if (_text == null) return;
        _text.text = ManagerLanguage.GetText(key);
        if (ManagerLanguage.Instance._font is null) return;
        _text.font = ManagerLanguage.Instance._font;
    }

    /// <summary>
    /// 设置Key和切换语言
    /// </summary>
    /// <param name="key">关键词</param>
    public void SetKeyAndChange(string key)
    {
        this.key = key;
        OnSwitchLanguage();
    }
}
