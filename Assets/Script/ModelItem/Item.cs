using Core;
using UnityEngine;
using Debug = Core.Debug;

[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour, IPool, IItem
{
    public long _itemID;
    public string _name;
    public string _des;

    private SpriteRenderer spriteRenderer;

    public long ID { get => _itemID; set => _itemID = value; }
    public string Name { get => _name; set => _name = value; }
    public string Des { get => _des; set => _des = value; }

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// 刷新物品
    /// </summary>
    /// <param name="itemCodeParam"></param>
    public void RefreshItem(long itemCodeParam)
    {
        if (itemCodeParam <= 0)
        {
            Debug.Error($"物品ID不对{itemCodeParam}");
            return;
        }
        _itemID = itemCodeParam;
        //ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(ItemCode);
        //spriteRenderer.sprite = itemDetails.itemSprite;
        //if (itemDetails.itemType == ItemType.Reapable_scenary)
        //    gameObject.AddComponent<ItemNudge>();
    }

    public void GetAfter()
    {
        gameObject.SetActive(true);
        RefreshItem(_itemID);
    }

    public void PushBefore()
    {
        gameObject.SetActive(false);
    }
}

