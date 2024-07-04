using Framework.Core;
using System.Collections;
using System.Collections.Generic;

/*--------脚本描述-----------

描述:
    库存管理系统

-----------------------*/

/// <summary>
/// 库存管理
/// </summary>
public class ManagerInventory : IModel
{
    public static ManagerInventory Instance;
    private Dictionary<int, List<InventoryItem>> _itemDic; //物品字典

    public void Init()
    {
        Instance = this;
        _itemDic = new Dictionary<int, List<InventoryItem>>();
    }

    public IEnumerator AsyncEnter()
    {
        yield return null;
    }

    public IEnumerator Exit()
    {
        yield return null;
    }

    /// <summary>
    /// 创建新的背包数据
    /// </summary>
    /// <param name="key"></param>
    public static void CreatData(int key)
    {
        if (Instance._itemDic.ContainsKey(key))
        {
            EDebug.Error("该背包数据已经存在!");
            return;
        }

        Instance._itemDic.Add(key, new List<InventoryItem>());
    }

    /// <summary>
    /// 创建物品管理列表
    /// </summary>
    public static void CreatData(int key, List<InventoryItem> inventoryItems)
    {
        if (Instance._itemDic.ContainsKey(key))
            EDebug.Error("该背包数据已经存在!");
        Instance._itemDic.Add(key, inventoryItems);
        Instance.RefreshInventoryItemList(key);
    }

    /// <summary>
    /// 添加物品数据
    /// </summary>
    /// <param name="key"></param>
    public static void AddItem(int key, InventoryItem value)
    {
        if (Instance._itemDic.TryGetValue(key, out List<InventoryItem> valueList))
        {
            valueList.Add(value);
            Instance.RefreshInventoryItemList(key);
            return;
        }

        EDebug.Error("背包数据不存在");
    }

    /// <summary>
    /// 删除物品
    /// </summary>
    /// <param name="key"></param>
    public static void RemoveItem(int key, int id, int amount)
    {
        if (Instance._itemDic.TryGetValue(key, out List<InventoryItem> valueList))
        {
            for (int i = 0; i < valueList.Count; i++)
            {
                if (valueList[i].itemID == id)
                {
                    Instance.ChackItemAmount(valueList[i], amount);
                    break;
                }
            }

            Instance.RefreshInventoryItemList(key);
            return;
        }

        EDebug.Log($"当前没有{key},请检查");
    }

    /// <summary>
    /// 交换物品
    /// </summary>
    private void ChangeItem(int oldKey, int oldID, int newKey, int newID)
    {
        //老的物品数据
        int oldIndex = 0;
        if (Instance._itemDic.TryGetValue(oldKey, out List<InventoryItem> oldValueList))
        {
            oldIndex = ChackItem(oldKey, oldID);
        }

        //新的物品数据
        int newIndex = 0;
        if (Instance._itemDic.TryGetValue(newKey, out List<InventoryItem> newValueList))
        {
            newIndex = ChackItem(newKey, newID);
        }

        //交换数据
        InventoryItem inventoryItemTemp = oldValueList[oldIndex];
        oldValueList[oldIndex] = newValueList[newIndex];
        newValueList[newIndex] = inventoryItemTemp;

        //合并重复物体
        MergeDuplicatItem(oldKey);
        MergeDuplicatItem(newKey);

        //刷新数据
        RefreshInventoryItemList(oldKey);
        RefreshInventoryItemList(newKey);
    }

    /// <summary>
    /// 检查物品是否存在
    /// </summary>
    /// <param name="key">库存的关键词对应的物品数据列表</param>
    /// <param name="id">物品的id</param>
    /// <returns>-1表示没有空位置,0表示这个格子是空的,其表示有物品</returns>
    private int ChackItem(int key, int id = 0)
    {
        Instance._itemDic.TryGetValue(key, out List<InventoryItem> valueList);
        for (int i = 0; i < valueList?.Count; i++)
        {
            if (valueList[i].itemID == id)
                return i;
        }

        return -1;
    }

    /// <summary>
    /// 获取列表
    /// </summary>
    public List<InventoryItem> GetInventoryItemList(int key)
    {
        if (Instance._itemDic.TryGetValue(key, out List<InventoryItem> valueList))
            return valueList;
        EDebug.Error("背包数据不存在");
        return null;
    }

    /// <summary>
    /// 刷新物品所在的UI数据
    /// </summary>
    /// <param name="key">库存的关键词对应的物品数据列表</param>
    private void RefreshInventoryItemList(int key)
    {
        CoreEvent.EventTrigger(key, _itemDic[key]); //这里的在比如背包页面那边开启的时候监听
    }

    /// <summary>
    /// 减少数量
    /// </summary>
    /// <param name="inventoryItem"></param>
    /// <param name="amount"></param>
    private void ChackItemAmount(InventoryItem inventoryItem, int amount)
    {
        if (inventoryItem.count >= amount)
        {
            inventoryItem.count -= amount;
            return;
        }

        EDebug.Log("当前的物品数量不足，请检查");
    }

    /// <summary>
    /// 合并重复物体
    /// </summary>
    private void MergeDuplicatItem(int key)
    {
        if (Instance._itemDic.TryGetValue(key, out List<InventoryItem> valueList))
        {
            return;
        }
    }
}