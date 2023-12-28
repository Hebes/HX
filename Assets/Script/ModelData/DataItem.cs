using Core;

/// <summary>
/// 实际使用的数据
/// </summary>
public class DataItem : IData
{
    public int itemID;
    public string name;
    public int itemType;
    public string itemIconPackage;
    public string itemIcon;
    public string itemOnWorldPackage;
    public string itemOnWorldSprite;
    public string itemDescription;
    public int itemUseRadiue;
    public bool canPickedup;
    public bool canDropped;
    public bool canCarried;
    public int itemPrice;
    public float sellPercentage;

    public int GetId()
    {
        return itemID;
    }
}
