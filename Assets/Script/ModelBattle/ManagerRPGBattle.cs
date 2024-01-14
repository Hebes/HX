using Core;
using System;
using System.Collections.Generic;

public class ManagerRPGBattle : IModelInit, IUpdata
{
    public static ManagerRPGBattle Instance;

    /// <summary>
    /// 所有的战斗列表->int 是NPC的ID合并起来
    /// </summary>
    private Dictionary<uint, IBattleActual> _battleDic;


    public void Init()
    {
        Instance = this;
        _battleDic = new Dictionary<uint, IBattleActual>();
        CoreBehaviour.Add(this);
    }
    public void CoreBehaviourUpdata()
    {
        foreach (IBattleActual item in _battleDic.Values)
            item.BattleUpdata();
    }


    public static void AddBattle(IBattleActual battle)
    {
        if (Instance._battleDic.TryAdd(battle.ID, battle))
        {
            battle.BattleInit();
            return;
        }
        Debug.Error("战斗添加失败,已存在");
    }
    public static void RemoveBattle(uint battleID)
    {
        if (Instance._battleDic.TryGetValue(battleID, out IBattleActual battle))
        {
            battle.BattleRemove();
            Instance._battleDic.Remove(battleID);
        }
    }
    public static IBattleActual GetOnebattle(uint battleID)
    {
        if (Instance._battleDic.TryGetValue(battleID, out IBattleActual oneBattle))
            return oneBattle;
        Debug.Error("战斗获取失败，战斗不存在");
        return default;
    }
}