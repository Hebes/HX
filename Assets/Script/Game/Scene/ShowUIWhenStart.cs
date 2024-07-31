using UnityEngine;

/// <summary>
/// 启动时显示UI
/// </summary>
public class ShowUIWhenStart : MonoBehaviour
{
    private void Start()
    {
        if (R.Ui.IsHide)
            R.Ui.ShowUI(false);
    }
}