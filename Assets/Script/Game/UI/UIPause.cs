using System.Collections;
using DG.Tweening;
using ExpansionUnity;
using Framework.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

/// <summary>
/// 暂停
/// </summary>
public class UIPause : WindowBase
{

    public override void OnAwake()
    {
        base.OnAwake();

        UIComponent uiComponent = GetComponent<UIComponent>();
    }
    
    /// <summary>
    /// 游戏暂停
    /// </summary>
    public bool Enabled
    {
        get => Input.UI.Pause.IsOpen;
        set
        {
            Input.UI.Pause.IsOpen = value;
            MobileInputPlayer.Instance.OptionsVisible = value;
        }
    }
}