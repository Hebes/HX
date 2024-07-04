using UnityEngine;
using UnityEngine.UI;

/*--------脚本描述-----------

描述:
    多语言组件

-----------------------*/

[RequireComponent(typeof(Text))]
public class LanguageComponent : MonoBehaviour
{
    public Text text; //组件
    public string key; //关键字

    private void Awake() => CoreLanguage.AddLanguageComponent(this);
    public void Change() => text.text = CoreLanguage.Get(key);
}