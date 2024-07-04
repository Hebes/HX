using System;
using System.Collections.Generic;
using Framework.Core;
using UnityEngine;

/// <summary>
/// 战斗
/// </summary>
public class BattleData : IID
{
    #region 接口属性
    /// <summary>
    /// 战斗场次编号
    /// </summary>
    public long ID { get; set; }
    /// <summary>
    /// 战斗的队伍
    /// </summary>
    public Dictionary<ETeamPoint, TeamData> BattleTeamDic { get; set; }
    /// <summary>
    /// 战斗行动列表
    /// </summary>
    public List<BattleActionData> BattleActionList { get; set; }
    /// <summary>
    /// 战斗的状态
    /// </summary>
    public EBattlePerformAction BattleSate { get; set; }
    public List<TeamData> teamEnemyDataList { get; set; }
    public List<TeamData> teamOwnDataList { get; set; }
    #endregion

    #region 本类字段
    /// <summary>
    /// 战斗是否暂停
    /// </summary>
    public bool isStaaleStop;
    #endregion

    #region 本来方法

    #endregion

    #region 生命周期
    public void BattleInit()
    {
        //CoreUI.ShwoUIPanel<UISkill>(ConfigPrefab.prefabUISkill);  //技能界面
        CoreUI.ShwoUIPanel<UIBattle>(ConfigPrefab.prefabUIBattle);  //战斗界面
        foreach (TeamData item in BattleTeamDic.Values)          //设置战斗开启
            item.IsEnterBattle = true;

    }
    public void BattleUpdata()
    {
        //战斗是否暂停
        if (isStaaleStop) return;

        //战斗队伍循环
        foreach (TeamData item in BattleTeamDic.Values)
            item.TeamUpdata();

        switch (BattleSate)
        {
            case EBattlePerformAction.WAIT: Wait(); break;
            case EBattlePerformAction.TAKEACTION: Takeaction(); break;
            case EBattlePerformAction.PERFROMACTION: Perfromaction(); break;
            case EBattlePerformAction.CHECKALIVE: Checkalive(); break;
            case EBattlePerformAction.WIN: Win(); break;
            case EBattlePerformAction.LOSE: Lose(); break;
            default: EDebug.Error("战斗出现未知错误"); break;
        }
    }
    public void BattleRemove()
    {
        BattleActionList = null;
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
        if (BattleActionList.Count <= 0) return;
        BattleSate = EBattlePerformAction.TAKEACTION;
    }
    /// <summary>
    /// 采取行动
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void Takeaction()
    {
        BattleActionData battleAction = BattleActionList[0];//战斗的动作
        switch (battleAction.AttackerData.RoleType)
        {
            case ERoleOrTeamType.Player:
                break;
            case ERoleOrTeamType.NPC:
            case ERoleOrTeamType.Enemy:
                RoleStateBattle roleStateBattle = battleAction.AttackerData.RoleState.GetRoleSate<RoleStateBattle>();
                //检查被攻击者是否还存活
                if (!this.ChackRoleSurvival(battleAction.TargetData))//敌人不存活
                    battleAction.TargetData = roleStateBattle.battle.RandomEnemyRole(battleAction.AttackerData.Team.TeamType);//随机一个敌人
                roleStateBattle.turnState = ERoleTurnState.ACTION;
                break;
            default:
                EDebug.Error($"角色类型错误{battleAction.AttackerData.RoleType}");
                break;
        }
        BattleSate = EBattlePerformAction.PERFROMACTION;
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
            BattleSate = EBattlePerformAction.WIN;
            return;
        }
        if (!this.ChackTeamSurvival(ETeamPoint.Left1, ETeamPoint.Left2, ETeamPoint.Left3, ETeamPoint.Left4))//右侧的人还有存活
        {
            BattleSate = EBattlePerformAction.LOSE;
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