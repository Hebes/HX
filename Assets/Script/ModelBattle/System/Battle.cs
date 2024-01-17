using Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = Core.Debug;

/// <summary>
/// 战斗
/// </summary>
public class Battle : IBattle
{
    #region 字段
    /// <summary>
    /// 战斗编号
    /// </summary>
    public long battleId;

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
    private Dictionary<ETeamPoint, ITeamInstance> _rolePointDic;
    #endregion

    #region 接口属性
    public long ID { get => battleId; set => battleId = value; }
    public Dictionary<ETeamPoint, ITeamInstance> BattleTeamDic { get => _rolePointDic; set => _rolePointDic = value; }
    public List<IBattleAction> BattleActionList { get => _battleActionList; set => _battleActionList = value; }
    public BattleType battleType { get => _battleType; set => _battleType = value; }
    public EBattlePerformAction BattleSate { get => _battleState; set => _battleState = value; }
    #endregion

    #region 本类字段
    /// <summary>
    /// 战斗是否暂停
    /// </summary>
    public bool isStaaleStop;
    #endregion

    #region 本类属性


    #endregion

    #region 生命周期
    public void BattleInit()
    {
        //CoreUI.ShwoUIPanel<UISkill>(ConfigPrefab.prefabUISkill);  //技能界面
        CoreUI.ShwoUIPanel<UIBattle>(ConfigPrefab.prefabUIBattle);  //战斗界面
        foreach (ITeamInstance item in BattleTeamDic.Values)          //设置战斗开启
            item.IsEnterBattle = true;
    }
    public void BattleUpdata()
    {
        //战斗是否暂停
        if (isStaaleStop) return;

        //战斗队伍循环
        foreach (ITeamInstance item in BattleTeamDic.Values)
            item.TeamUpdata();

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
    #endregion

    #region 战斗状态
    /// <summary>
    /// 等待
    /// </summary>
    private void Wait()
    {
        if (_battleActionList.Count <= 0) return;
        _battleState = EBattlePerformAction.TAKEACTION;
    }
    /// <summary>
    /// 采取行动
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void Takeaction()
    {
        IBattleAction battleAction = _battleActionList[0];//战斗的动作
        switch (battleAction.AttackerData.RoleType)
        {
            case ERoleOrTeamType.Player:
            case ERoleOrTeamType.NPC:
            case ERoleOrTeamType.Enemy:
                RoleStateBattle roleStateBattle = battleAction.AttackerData.RoleState.GetRoleSate<RoleStateBattle>();
                //检查被攻击者是否还存活
                if (!this.ChackRoleSurvival(battleAction.TargetData))//敌人不存活
                    battleAction.TargetData = roleStateBattle.battle.RandomEnemyRole(battleAction.AttackerData.Team.TeamType);//随机一个敌人
                roleStateBattle.TurnState = ERoleTurnState.ACTION;
                break;
            default:
                Debug.Error($"角色类型错误{battleAction.AttackerData.RoleType}");
                break;
        }
        _battleState = EBattlePerformAction.PERFROMACTION;
    }
    /// <summary>
    /// 执行动作
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void Perfromaction()
    {
    }
    /// <summary>
    /// 检查是否存活
    /// </summary>
    private void Checkalive()
    {
        if (!this.ChackTeamSurvival(ETeamPoint.Right1, ETeamPoint.Right2, ETeamPoint.Right3, ETeamPoint.Right4))//右侧的人还有存活
        {
            _battleState = EBattlePerformAction.WIN;
            return;
        }
        if (!this.ChackTeamSurvival(ETeamPoint.Left1, ETeamPoint.Left2, ETeamPoint.Left3, ETeamPoint.Left4))//右侧的人还有存活
        {
            _battleState = EBattlePerformAction.LOSE;
            return;
        }
        //clearAttackPanel();
        //HeroInput = HeroGUI.ACTIOVATE;
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
    #endregion
}