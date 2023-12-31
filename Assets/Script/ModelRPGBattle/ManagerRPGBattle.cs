using Core;
using System;
using System.Collections.Generic;

public class ManagerRPGBattle : IModelInit, IUpdata
{
    public static ManagerRPGBattle Instance;

    /// <summary>
    /// 所有的战斗列表->int 是NPC的ID合并起来
    /// </summary>
    private Dictionary<uint, IOneBattle> _battleDic;


    public void Init()
    {
        Instance = this;
        _battleDic = new Dictionary<uint, OneBattleNPC>();
        CoreBehaviour.Add(this);
    }


    public void OnUpdata()
    {
        foreach (OneBattleNPC item in _battleDic.Values)
            item.Updata();
    }



    /// <summary>
    /// 添加一场战斗
    /// </summary>
    public static void AddOneBattle(uint battleID, OneBattleNPC oneBattle)
    {
        //oneBattle.Init(battleID);
        //if (!Instance._battleDic.TryAdd(battleID, oneBattle))
        //    Debug.Error("战斗添加失败,已存在");
    }

    /// <summary>
    /// 添加一场战斗
    /// </summary>
    public static void AddOneBattle(IOneBattle oneBattle)
    {
        if (Instance._battleDic.TryAdd(oneBattle.BattleId, oneBattle))
            oneBattle.Init();
        else
            Debug.Error("战斗添加失败,已存在");
    }

    /// <summary>
    /// 移除一场
    /// </summary>
    public static void RemoveOneBattle(uint battleID)
    {
        if (Instance._battleDic.TryGetValue(battleID, out IOneBattle oneBattle))
        {
            Instance._battleDic.Remove(battleID);
            oneBattle.Remove();
        }
    }

    /// <summary>
    /// 获取一场战斗
    /// </summary>
    public static IOneBattle GetOnebattle(uint battleID)
    {
        if (Instance._battleDic.TryGetValue(battleID, out IOneBattle oneBattle))
            return oneBattle;
        Debug.Error("战斗获取失败，战斗不存在");
        return default;
    }
}



/// <summary>
/// 个人战斗数据
/// </summary>
//[Serializable]
public interface BattleData
{
    /// <summary>
    /// 发起攻击的人
    /// </summary>
    public IRole StartAttackRoleData { get; set; }

    /// <summary>
    /// 目标的数据
    /// </summary>
    public IRole TargetRoleData { get; set; }

    /// <summary>
    /// 执行攻击的方法
    /// </summary>
    public void Attack();
}

/// <summary>
/// 战斗等待接口
/// </summary>
public interface IBattleWait
{
    public void Wait();
}