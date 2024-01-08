using Core;
using System;
using System.Collections.Generic;

/// <summary>
/// 队伍的战斗
/// </summary>
public class TeamBattle : IBattle, IBattleBehaviour, IBattleCarrier, IBattleActionCarrier
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
    /// 所有人的战斗执行动作列表
    /// </summary>
    private List<IBattleAction> _battleActionList = new List<IBattleAction>();

    //TODO 后面或许要加一场战斗的类型player对战敌人或者NPC对战敌人
    private BattleType _battleType;

    /// <summary>
    /// 战斗的位置
    /// 1.可能是敌人在左边，进入二打一模式
    /// 2.可能是自己人右边，敌人3队进行二打一模式
    /// </summary>
    private Dictionary<ETeamPoint, ITeam> _rolePointDic;


    public uint ID { get => battleId; set => battleId = value; }
    public Dictionary<ETeamPoint, ITeam> BattleTeamDic { get => _rolePointDic; set => _rolePointDic = value; }
    public List<IBattleAction> BattleActionList { get => _battleActionList; set => _battleActionList = value; }
    public BattleType battleType { get => _battleType; set => _battleType = value; }
    public EBattlePerformAction BattleSate { get => _battleState; set => _battleState = value; }

    /// <summary>
    /// 战斗是否暂停
    /// </summary>
    public bool isStaaleStop;


    public void BattleInit()
    {
        //显示战斗界面
        //CoreUI.ShwoUIPanel<UISkill>(ConfigPrefab.prefabUISkill);
        CoreUI.ShwoUIPanel<UIBattle>(ConfigPrefab.prefabUIBattle);
    }
    public void BattleUpdata()
    {
        if (isStaaleStop) return;
        switch (_battleState)
        {
            case EBattlePerformAction.WAIT: Wait(); break;
            case EBattlePerformAction.TAKEACTION: Takeaction(); break;
            case EBattlePerformAction.PERFROMACTION: Perfromaction(); break;
            case EBattlePerformAction.CHECKALIVE: Checkalive(); break;
            case EBattlePerformAction.WIN: Win(); break;
            case EBattlePerformAction.LOSE: Lose(); break;
            default: Debug.Error("战斗出现未知错误"); break;
        }
    }
    public void BattleRemove()
    {
        _battleActionList = null;
        GC.Collect();
        //TODO 一些其他操作。或者触发事件
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
        if (_battleActionList.Count <= 0) return;
        _battleState = EBattlePerformAction.TAKEACTION;
    }

    /// <summary>
    /// 胜利
    /// </summary>
    private void Win()
    {

    }

    /// <summary>
    /// 输了
    /// </summary>
    private void Lose()
    {

    }



    public void AddBattleTeam(ITeam team) => IBattleCarrier.AddBattleTeam(this, team);
    public void RemoveBattleTeam(ITeam team) => IBattleCarrier.RemoveBattleTeam(this, team);
    public void AddBattleAction(IBattleAction battleAction) => IBattleActionCarrier.AddBattleAction(this, battleAction);
    public void RemoveBattleAction(IBattleAction battleAction) => IBattleActionCarrier.RemoveBattleAction(this, battleAction);
    //public 
}
