using Core;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour, IPool
{
    /// <summary>
    /// 物品ID
    /// </summary>
    public int itemID;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Init(int itemCodeParam)
    {
        if (itemCodeParam != 0)
        {
            itemID = itemCodeParam;
            //获取数据
            //ItemDetails itemDetails = InventoryManager.Instance.GetItemDetails(ItemCode);

            //spriteRenderer.sprite = itemDetails.itemSprite;

            //if (itemDetails.itemType == ItemType.Reapable_scenary)
            //{
            //    gameObject.AddComponent<ItemNudge>();
            //}
        }
    }

    public void GetAfter()
    {
        gameObject.SetActive(true);
        Init(itemID);
    }

    public void PushBefore()
    {
        gameObject.SetActive(false);
    }
}

