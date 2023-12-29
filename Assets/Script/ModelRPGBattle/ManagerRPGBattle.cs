using Core;
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
    public void OnUpdata()
    {
        foreach (IBattleBehaviour item in _battleDic.Values)
            item.BattleUpdata();
    }


    /// <summary>
    /// 添加一场战斗
    /// </summary>
    public static void AddBattle(IBattle oneBattle)
    {
        if (oneBattle is IBattleBehaviour battleBehaviour)
            battleBehaviour.Init();
        if (!Instance._battleDic.TryAdd(oneBattle.ID, oneBattle))
            Debug.Error("战斗添加失败,已存在");
    }

    /// <summary>
    /// 移除一场
    /// </summary>
    public static void RemoveBattle(uint battleID)
    {
        if (Instance._battleDic.TryGetValue(battleID, out IBattle oneBattle))
        {
            Instance._battleDic.Remove(battleID);
            if (oneBattle is IBattleBehaviour battleBehaviour)
                battleBehaviour.Remove();
            return;
        }
        Debug.Error($"需要移除的战斗不存在{battleID}");
    }

    /// <summary>
    /// 获取一场战斗
    /// </summary>
    public static IBattle GetOnebattle(uint battleID)
    {
        if (Instance._battleDic.TryGetValue(battleID, out IBattle oneBattle))
            return oneBattle;
        Debug.Error("战斗获取失败，战斗不存在");
        return default;
    }
}
