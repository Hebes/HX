using Core;
using System;
using System.Collections.Generic;

/// <summary>
/// 两只队伍的战斗
/// </summary>
public class TwoTeamBattle : IBattle, IBattleBehaviour
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
    /// 所有人的战斗执行动作列表
    /// </summary>
    private List<BattleData> m_battleDataList;

    /// <summary>
    /// 战斗的位置
    /// 1.可能是敌人在左边，进入二打一模式
    /// 2.可能是自己人右边，敌人3队进行二打一模式
    /// </summary>
    private Dictionary<ETeamPoint, ITeam> _rolePointDic;

    /// <summary>
    /// 获取战斗状态
    /// </summary>
    public EBattlePerformAction GetBattleState => m_battleState;

    public uint ID { get => battleId; set => battleId = value; }


    public void BattleInit()
    {
        m_battleDataList = new List<BattleData>();
        _rolePointDic = new Dictionary<ETeamPoint, ITeam>();
    }
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
    public void BattleRemove()
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
    public void AddBattleTeam(ITeam team)
    {
        if (_rolePointDic.ContainsKey(team.TeamPoint))
        {
            Debug.Error($"当前队伍占位已存在{team.TeamPoint}");
            return;
        }
        _rolePointDic.Add(team.TeamPoint, team);
    }

    /// <summary>
    /// 添加战斗角色
    /// </summary>
    /// <param name="roleBattlePoint"></param>
    /// <param name="roleList"></param>
    public void AddBattleRole(ERoleBattlePoint roleBattlePoint, List<IRole> roleList)
    {
        Debug.Error("请添加逻辑");
        //if (m_rolePointDic.ContainsKey(roleBattlePoint))
        //{
        //    m_rolePointDic[roleBattlePoint].Clear();
        //    m_rolePointDic[roleBattlePoint] = roleList;
        //}
        //else
        //{
        //    m_rolePointDic.Add(roleBattlePoint, new List<IRole>(roleList));
        //}
    }

    /// <summary>
    /// 移除战斗角色
    /// </summary>
    public void RemoveBattleRole(IRole role)
    {
        //switch (role.roleBattlePoint)
        //{
        //    case ERoleBattlePoint.Left:
        //        m_leftRoleList.RemoveContainElement(role);
        //        break;
        //    case ERoleBattlePoint.Right:
        //        m_rightRoleList.RemoveContainElement(role);
        //        break;
        //    default:
        //        Debug.Error("当前位置错误,请添加");
        //        break;
        //}
    }

    ///// <summary>
    ///// 添加一场战斗
    ///// </summary>
    //public void AddBattleData(BattleData battleData)
    //{
    //    m_battleDataList.AddNotContainElement(battleData);
    //}

    ///// <summary>
    ///// 移除一场
    ///// </summary>
    //public void RemoveBattleData(BattleData battleData)
    //{
    //    m_battleDataList.RemoveContainElement(battleData);
    //}
}
