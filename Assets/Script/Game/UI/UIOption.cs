using Game.UI;
using UnityEngine.UI;
using ExpansionUnity;
using Framework.Core;
using UnityEngine.EventSystems;

public class UIOption : WindowBase
{
    public override void OnAwake()
    {
        base.OnAwake();

        UIComponent uiComponent = GetComponent<UIComponent>();
        var T_Audio_optionGameObject = uiComponent.Get("T_Audio_option");
        var T_Language_optionGameObject = uiComponent.Get("T_Language_option");
        var T_ResetGameObject = uiComponent.Get("T_Reset");
        var T_BackGameObject = uiComponent.Get("T_Back");


        T_Audio_optionGameObject.AddEventTrigger(EventTriggerType.PointerClick, Audio_option);
        T_BackGameObject.AddEventTrigger(EventTriggerType.PointerClick, Back);
    }

    

    public override void OnShow()
    {
        base.OnShow();
    }

    public override void OnHide()
    {
        base.OnHide();
    }

    private void Audio_option(PointerEventData obj)
    {
    }
    
    private void Back(PointerEventData obj)
    {
        HideWindow();
        PopUpWindow<UIStart>();
    }
}