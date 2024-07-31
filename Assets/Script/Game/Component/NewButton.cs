using UnityEngine.UI;

/// <summary>
/// 新按钮
/// </summary>
public class NewButton : Button
{
    
    /// <summary>
    /// 是否按下
    /// </summary>
    public new bool IsPressed => currentSelectionState == SelectionState.Pressed;
    
    /// <summary>
    /// 是否可见
    /// </summary>
    public bool IsActive
    {
        get => gameObject.activeSelf;
        set => gameObject.SetActive(value);
    }
    
    public string Text
    {
        get => textValue == null ? string.Empty : textValue.text;
        set
        {
            if (textValue != null)
                textValue.text = value;
        }
    }

    public Text textValue;
}