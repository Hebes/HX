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
    private EBattlePerformAction m_battleState;

    /// <summary>
    /// 战斗执行动作列表
    /// </summary>
    private List<BattleData> m_battleDataList;

    /// <summary>
    /// 战斗的位置
    /// 1.可能是敌人在左边，进入二打一模式
    /// 2.可能是自己人右边，敌人3队进行二打一模式
    /// </summary>
    private Dictionary<ERoleBattlePoint, List<IRole>> m_rolePointDic;

    /// <summary>
    /// 获取战斗状态
    /// </summary>
    public EBattlePerformAction GetBattleState => m_battleState;



    /// <summary>
    /// 每场战斗的初始化
    /// </summary>
    public void Init(uint battleId)
    {
        m_battleDataList = new List<BattleData>();
        m_rolePointDic = new Dictionary<ERoleBattlePoint, List<IRole>>();
        this.battleId = battleId;
    }

    /// <summary>
    /// 每场战斗的更新
    /// </summary>
    public void BattleUpdata()
    {
        switch (m_battleState)
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
    /// 移除一场战斗
    /// </summary>
    public void Remove()
    {
        m_battleDataList = null;
    }



    /// <summary>
    /// 检查是否存活
    /// </summary>
    private void Checkalive()
    {
        //if (m_leftRoleList.Count<1)//左边死完了
        //{
        //    m_battleState = EBattlePerformAction.WIN;
        //}
        //else if (m_rightRoleList.Count<1)//右边死完了
        //{
        //    m_battleState = EBattlePerformAction.LOSE;
        //}
        //else//都还存活的话
        //{
        //    //call function 
        //    //clearAttackPanel();
        //    //HeroInput = HeroGUI.ACTIOVATE;
        //}
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
        //m_battleDataList
        //if (m_battleDataList.Count <= 0) return;
        //m_battleState = EBattlePerformAction.TAKEACTION;
    }



    /// <summary>
    /// 添加战斗角色
    /// </summary>
    public void AddBattleRole(IRole role)
    {
        switch (role.roleBattlePoint)
        {
            case ERoleBattlePoint.Left:
                if (m_rolePointDic.ContainsKey(ERoleBattlePoint.Left))
                {
                    bool isContains = m_rolePointDic[ERoleBattlePoint.Left].Contains(role);
                    if (isContains)
                        Debug.Error("角色数据已经存在");
                    else
                        m_rolePointDic[ERoleBattlePoint.Left].Add(role);
                }
                else
                {
                    m_rolePointDic.Add(ERoleBattlePoint.Left, new List<IRole>() { role });
                }
                break;
            case ERoleBattlePoint.Right:
                //TODO 这里需要完善
                //m_rightRoleList.AddNotContainElement(role);
                break;
            default:
                Debug.Error("当前位置错误,请添加");
                break;
        }
    }

    /// <summary>
    /// 添加战斗角色
    /// </summary>
    /// <param name="roleBattlePoint"></param>
    /// <param name="roleList"></param>
    public void AddBattleRole(ERoleBattlePoint roleBattlePoint, List<IRole> roleList)
    {
        if (m_rolePointDic.ContainsKey(roleBattlePoint))
        {
            m_rolePointDic[roleBattlePoint].Clear();
            m_rolePointDic[roleBattlePoint] = roleList;
        }
        else
        {
            m_rolePointDic.Add(roleBattlePoint, new List<IRole>(roleList));
        }
    }

    /// <summary>
    /// 移除战斗角色
    /// </summary>
    public void RemoveBattleRole(IRole role)
    {
        switch (role.roleBattlePoint)
        {
            case ERoleBattlePoint.Left:
                m_leftRoleList.RemoveContainElement(role);
                break;
            case ERoleBattlePoint.Right:
                m_rightRoleList.RemoveContainElement(role);
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
        m_battleDataList.AddNotContainElement(battleData);
    }

    /// <summary>
    /// 移除一场
    /// </summary>
    public void RemoveBattleData(BattleData battleData)
    {
        m_battleDataList.RemoveContainElement(battleData);
    }

}

/// <summary>
/// 个人战斗数据
/// </summary>
//[Serializable]
public interface BattleData
{
    /// <summary>
    /// 自己的数据
    /// </summary>
    public IRole ownData { get; set; }

    /// <summary>
    /// 目标的数据
    /// </summary>
    public IRole TargetData { get; set; }


    public void Attack();
}

/// <summary>
/// 战斗等待接口
/// </summary>
public interface IBattleWait
{
    public void Wait();
}