using Framework.Core;
using System;
using UnityEngine;

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
    public DateTime Pushtime { get ; set ; }

    public float DesMilliseconds => 1000;

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
            EDebug.Error($"物品ID不对{itemCodeParam}");
            return;
        }
        _itemID = itemCodeParam;
        //ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(ItemCode);
        //spriteRenderer.sprite = itemDetails.itemSprite;
        //if (itemDetails.itemType == ItemType.Reapable_scenary)
        //    gameObject.AddComponent<ItemNudge>();
    }

    public void Get()
    {
        gameObject.SetActive(true);
        RefreshItem(_itemID);
    }

    public void Push()
    {
        gameObject.SetActive(false);
    }
}

