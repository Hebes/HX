using Core;
using System;
using System.Collections.Generic;

public class ManagerRPGBattle : IModelInit, IUpdata
{
    public static ManagerRPGBattle Instance;

    /// <summary>
    /// 所有的战斗列表->int 是NPC的ID合并起来
    /// </summary>
    private Dictionary<uint, IBattle> _battleDic;


    public void Init()
    {
        Instance = this;
        _battleDic = new Dictionary<uint, IBattle>();
        CoreBehaviour.Add(this);
    }
    public void CoreBehaviourUpdata()
    {
        foreach (IBattle item in _battleDic.Values)
        {
            if (item is IBattleBehaviour battleBehaviour)
                battleBehaviour.BattleUpdata();
        }
    }


    public static void AddBattle(IBattle battle)
    {
        if (Instance._battleDic.TryAdd(battle.ID, battle))
        {
            if (battle is IBattleBehaviour battleBehaviour)
            {
                battleBehaviour.BattleInit();
                return;
            }
        }
        Debug.Error("战斗添加失败,已存在");
    }
    public static void RemoveBattle(uint battleID)
    {
        if (Instance._battleDic.TryGetValue(battleID, out IBattle battle))
        {
            Instance._battleDic.Remove(battleID);
            if (battle is IBattleBehaviour battleBehaviour)
                battleBehaviour.BattleRemove();
        }
    }
    public static IBattle GetOnebattle(uint battleID)
    {
        if (Instance._battleDic.TryGetValue(battleID, out IBattle oneBattle))
            return oneBattle;
        Debug.Error("战斗获取失败，战斗不存在");
        return default;
    }
}