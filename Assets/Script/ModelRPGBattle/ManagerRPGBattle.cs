using Core;
using System;
using System.Collections.Generic;

public class ManagerRPGBattle : IModelInit, IUpdata
{
    public static ManagerRPGBattle Instance;

    /// <summary>
    /// 所有的战斗列表->int 是NPC的ID合并起来
    /// </summary>
    private Dictionary<uint, OneBattle> _battleDic;


    public void Init()
    {
        Instance = this;
        _battleDic = new Dictionary<uint, OneBattle>();
        CoreBehaviour.Add(this);
    }


    public void OnUpdata()
    {
        foreach (OneBattle item in _battleDic.Values)
            item.BattleUpdata();
    }



    /// <summary>
    /// 添加一场战斗
    /// </summary>
    public static void AddOneBattle(uint battleID, OneBattle oneBattle)
    {
        oneBattle.Init(battleID);
        if (!Instance._battleDic.TryAdd(battleID, oneBattle))
            Debug.Error("战斗添加失败,已存在");
    }

    /// <summary>
    /// 移除一场
    /// </summary>
    public static void RemoveOneBattle(uint battleID)
    {
        if (Instance._battleDic.TryGetValue(battleID, out OneBattle oneBattle))
        {
            oneBattle.Remove();
            Instance._battleDic.Remove(battleID);
            return;
        }
    }

    /// <summary>
    /// 获取一场战斗
    /// </summary>
    public static OneBattle GetOnebattle(uint battleID)
    {
        if (Instance._battleDic.TryGetValue(battleID, out OneBattle oneBattle))
            return oneBattle;
        Debug.Error("战斗获取失败，战斗不存在");
        return default;
    }
}

/// <summary>
/// 一场战斗
/// </summary>
public class OneBattle
{
    /// <summary>
    /// 战斗编号
    /// </summary>
    public uint battleId;

    /// <summary>
    /// 战斗的状态
    /// </summary>
    private EBattlePerformAction _battleState;

    /// <summary>
    /// 战斗执行动作列表
    /// </summary>
    private List<BattleData> _battleDataList;

    /// <summary>
    /// 战斗的左边
    /// </summary>
    private List<IRole> _leftRoleList;

    /// <summary>
    /// 战斗右边
    /// </summary>
    private List<IRole> _rightRoleList;

    /// <summary>
    /// 获取战斗状态
    /// </summary>
    public EBattlePerformAction GetBattleState => _battleState;


    /// <summary>
    /// 每场战斗的初始化
    /// </summary>
    public void Init(uint battleId)
    {
        _battleDataList = new List<BattleData>();
        _leftRoleList = new List<IRole>();
        _rightRoleList = new List<IRole>();
        this.battleId = battleId;
    }

    /// <summary>
    /// 每场战斗的更新
    /// </summary>
    public void BattleUpdata()
    {
        switch (_battleState)
        {
            case EBattlePerformAction.WAIT:
                Wait();
                break;
            case EBattlePerformAction.TAKEACTION:
                Takeaction();
                break;
            case EBattlePerformAction.PERFROMACTION:
                Perfromaction();
                break;
            case EBattlePerformAction.CHECKALIVE:
                Checkalive();
                break;
            case EBattlePerformAction.WIN:
                break;
            case EBattlePerformAction.LOSE:
                break;
            default:
                Debug.Error("战斗出现未知错误");
                break;
        }
    }

    /// <summary>
    /// 检查是否存活
    /// </summary>
    private void Checkalive()
    {
        if (_leftRoleList.Count<1)//左边死完了
        {
            _battleState = EBattlePerformAction.WIN;
        }
        else if (_rightRoleList.Count<1)//右边死完了
        {
            _battleState = EBattlePerformAction.LOSE;
        }
        else//都还存活的花
        {
            //call function 
            //clearAttackPanel();
            //HeroInput = HeroGUI.ACTIOVATE;
        }
    }

    /// <summary>
    /// 执行动作
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void Perfromaction()
    {
    }

    /// <summary>
    /// 采取行动
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void Takeaction()
    {
    }

    /// <summary>
    /// 等待
    /// </summary>
    private void Wait()
    {
        if (_battleDataList.Count <= 0) return;
        _battleState = EBattlePerformAction.TAKEACTION;
    }



    /// <summary>
    /// 移除一场战斗
    /// </summary>
    public void Remove()
    {
        _battleDataList = null;
    }

    /// <summary>
    /// 添加战斗角色
    /// </summary>
    public void AddBattleRole(IRole role)
    {
        switch (role.roleBattlePoint)
        {
            case ERoleBattlePoint.Left:
                _leftRoleList.AddNotContainElement(role);
                break;
            case ERoleBattlePoint.Right:
                _rightRoleList.AddNotContainElement(role);
                break;
            default:
                Debug.Error("当前位置错误,请添加");
                break;
        }
    }

    /// <summary>
    /// 移除战斗角色
    /// </summary>
    public void  RemoveBattleRole(IRole role)
    {
        switch (role.roleBattlePoint)
        {
            case ERoleBattlePoint.Left:
                _leftRoleList.RemoveContainElement(role);
                break;
            case ERoleBattlePoint.Right:
                _rightRoleList.RemoveContainElement(role);
                break;
            default:
                Debug.Error("当前位置错误,请添加");
                break;
        }
    }

    /// <summary>
    /// 添加一场战斗
    /// </summary>
    public void AddBattleData(BattleData battleData)
    {
        _battleDataList.AddNotContainElement(battleData);
    }

    /// <summary>
    /// 移除一场
    /// </summary>
    public void RemoveBattleData(BattleData battleData)
    {
        _battleDataList.RemoveContainElement(battleData);
    }

}

/// <summary>
/// 战斗数据
/// </summary>
[Serializable]
public class BattleData
{
    /// <summary>
    /// 自己的数据
    /// </summary>
    public IRole ownData;

    /// <summary>
    /// 目标的数据
    /// </summary>
    public IRole TargetData;

}