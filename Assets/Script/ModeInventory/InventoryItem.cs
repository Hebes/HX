using Core;
using System.Collections.Generic;
/// <summary>
/// 物品数据结构
/// </summary>
public struct InventoryItem
{
    /// <summary>
    /// 物体ID
    /// </summary>
    public int itemID;

    /// <summary>
    /// 物品数量
    /// </summary>
    public int count;

    /// <summary>
    /// 描述
    /// </summary>
    public string Describe;
}

/// <summary>
/// 库存管理系统的持有者
/// </summary>
public interface IInventoryCarrier : IID
{
    List<InventoryItem> InventoryItemList { get; set; }

    /// <summary>
    /// 添加物品
    /// </summary>
    public static void AddItem(IInventoryCarrier inventoryCarrier, InventoryItem inventoryItem)
    {
        if (inventoryCarrier.InventoryItemList == null)
            inventoryCarrier.InventoryItemList = new List<InventoryItem>(GameSetting.InventoryItemCount);//默认10个格子
        inventoryCarrier.InventoryItemList.Add(inventoryItem);
    }

    public static void RemoveItem(IInventoryCarrier inventoryCarrier, InventoryItem inventoryItem)
    {
        if (inventoryCarrier.InventoryItemList == null)
            inventoryCarrier.InventoryItemList = new List<InventoryItem>();
        inventoryCarrier.InventoryItemList.Add(inventoryItem);
    }

    /// <summary>
    /// 检查物品是否存在
    /// </summary>
    public static bool CheckItemExist(IInventoryCarrier inventoryCarrier, InventoryItem inventoryItem)
    {
        return inventoryCarrier.InventoryItemList.Contains(inventoryItem);
    }
}

public static class HelperInventory
{
    /// <summary>
    /// 添加物品
    /// </summary>
    /// <param name="inventoryCarrier"></param>
    /// <param name="inventoryItem"></param>
    public static void AddItem(this IInventoryCarrier inventoryCarrier, InventoryItem inventoryItem)
    {
        IInventoryCarrier.AddItem(inventoryCarrier, inventoryItem);
    }

    public static void RemoveItem(this IInventoryCarrier inventoryCarrier, InventoryItem inventoryItem)
    {
        IInventoryCarrier.RemoveItem(inventoryCarrier, inventoryItem);
    }

    public static void CheckItemExist(this IInventoryCarrier inventoryCarrier, InventoryItem inventoryItem)
    {
        IInventoryCarrier.CheckItemExist(inventoryCarrier, inventoryItem);
    }
}