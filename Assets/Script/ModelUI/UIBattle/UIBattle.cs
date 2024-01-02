using Core;

public class UIBattle : UIBase
{
    public override void UIAwake()
    {
        base.UIAwake();
        InitUIBase(EUIType.Normal, EUIMode.Normal, EUILucenyType.Pentrate);
    }
}
