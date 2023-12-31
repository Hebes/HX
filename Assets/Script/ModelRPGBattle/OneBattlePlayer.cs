using System.Collections.Generic;
using System.Diagnostics;
using Core;
using Debug = Core.Debug;

public class OneBattlePlayer : IOneBattle
{
    /// <summary>
    /// 战斗编号
    /// </summary>
    private uint battleId;

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


    uint IOneBattle.BattleId { get => battleId; }
    EBattlePerformAction IOneBattle.BattleState { get => m_battleState; }
    List<BattleData> IOneBattle.BattleDataList { get => m_battleDataList; }
    Dictionary<ERoleBattlePoint, List<IRole>> IOneBattle.RolePointDic { get => m_rolePointDic; }


    public void Init()
    {
        m_battleDataList = new List<BattleData>();
        m_rolePointDic = new Dictionary<ERoleBattlePoint, List<IRole>>();
    }

    public void Remove()
    {
    }

    public void Updata()
    {
        switch (m_battleState)
        {
            case EBattlePerformAction.WAIT:
                if (m_battleDataList.Count > 0)
                    m_battleState = EBattlePerformAction.TAKEACTION;
                break;
            case EBattlePerformAction.TAKEACTION:
                BattleData battleData = m_battleDataList[0];//第一条攻击数据
                if (battleData.StartAttackRoleData.RoleType == ERoleType.Enemy)//如果是敌人的话
                    battleData.Attack();
                else if (battleData.StartAttackRoleData.RoleType == ERoleType.Friend)
                    battleData.Attack();
                else if (battleData.StartAttackRoleData.RoleType == ERoleType.Player)
                {

                }
                break;
            case EBattlePerformAction.PERFROMACTION:
                break;
            case EBattlePerformAction.CHECKALIVE:
                break;
            case EBattlePerformAction.WIN:
                break;
            case EBattlePerformAction.LOSE:
                break;
            default:
                Debug.Error("战斗状态错误");
                break;
        }
    }
}
