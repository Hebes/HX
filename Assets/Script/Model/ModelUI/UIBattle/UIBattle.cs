using Framework.Core;

public class UIBattle : UIBase, IUIAwake
{
    public void UIAwake()
    {
        InitUIBase(EUIType.Normal, EUIMode.Normal, EUILucenyType.Pentrate);
    }
}
