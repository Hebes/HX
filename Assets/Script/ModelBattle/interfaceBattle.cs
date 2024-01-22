using Core;
using System.Collections.Generic;
using UnityEngine;
using Debug = Core.Debug;

/// <summary>
/// 基础战斗接口
/// </summary>
public interface IBattle : IID
{
    /// <summary>
    /// 一场战斗的类型
    /// </summary>
    public BattleType battleType { get; set; }

    /// <summary>
    ///一场战斗的状态
    /// </summary>
    public EBattlePerformAction BattleSate { get; set; }

    /// <summary>
    /// 每场战斗的初始化
    /// </summary>
    public void BattleInit();

    /// <summary>
    /// 每场战斗的更新
    /// </summary>
    public void BattleUpdata();

    /// <summary>
    /// 移除一场战斗后需要做哪些事
    /// </summary>
    public void BattleRemove();

    /// <summary>
    /// 战斗的队伍
    /// 1.可能是敌人在左边，进入二打一模式
    /// 2.可能是自己人右边，敌人3队进行二打一模式
    /// </summary>
    public Dictionary<ETeamPoint, ITeamInstance> BattleTeamDic { get; set; }

    /// <summary>
    /// 行动列表
    /// </summary>

    public List<IBattleAction> BattleActionList { get; set; }


    //public List<ITeamActual> LeftTeamList { get; set; }
    //public List<ITeamActual> RightTeamList { get; set; }


    /// <summary>
    /// 添加战斗队伍
    /// </summary>
    public static void AddBattleTeam(IBattle battleCarrier, ITeamInstance team)
    {
        if (battleCarrier.BattleTeamDic == null)
            battleCarrier.BattleTeamDic = new Dictionary<ETeamPoint, ITeamInstance>();

        if (battleCarrier.BattleTeamDic.ContainsKey(team.TeamPoint))
        {
            Debug.Error($"当前队伍占位已存在{team.TeamPoint}");
            return;
        }
        battleCarrier.BattleTeamDic.Add(team.TeamPoint, team);
    }

    /// <summary>
    /// 删除战斗队伍
    /// </summary>
    /// <param name="battleCarrier"></param>
    /// <param name="team"></param>
    public static void RemoveBattleTeam(IBattle battleCarrier, ITeamInstance team)
    {
        if (battleCarrier.BattleTeamDic.ContainsKey(team.TeamPoint))
            battleCarrier.BattleTeamDic.Remove(team.TeamPoint);
    }


    /// <summary>
    /// 随机自己方一个人
    /// </summary>
    /// <returns></returns>
    public static IRoleInstance RandomOwnRole(IBattle battleCarrier, ERoleOrTeamType teamType)
    {
        ExpansionProfiler.ProfilerBeginSample("随机自己方一个人");
        List<IRoleInstance> role = new List<IRoleInstance>();
        foreach (ITeamInstance item in battleCarrier.BattleTeamDic.Values)
        {
            foreach (IRoleInstance item1 in item.RoleList)
                role.Add(item1);
        }

        int number = 0;
        //随机一个己方人物
        switch (teamType)
        {
            case ERoleOrTeamType.Player:
            case ERoleOrTeamType.NPC:
                role = role.FindAll((IRoleInstance data) =>
                {
                    return data.RoleType == ERoleOrTeamType.Player ||
                    data.RoleType == ERoleOrTeamType.NPC;
                });
                number = Random.Range(0, role.Count);
                ExpansionProfiler.ProfilerEndSample();
                return role[number];
            case ERoleOrTeamType.Enemy:
                role = role.FindAll((IRoleInstance data) =>
                {
                    return data.RoleType == ERoleOrTeamType.Enemy;
                });
                number = Random.Range(0, role.Count);
                ExpansionProfiler.ProfilerEndSample();
                return role[number];
            default:
                Debug.Error("ETeamType未知类型错误");
                ExpansionProfiler.ProfilerEndSample();
                return null;
        }
    }
}

/// <summary>
/// 个人战斗数据接口
/// </summary>
public interface IBattleAction
{
    /// <summary>
    /// 攻击者的数据
    /// </summary>
    public IRoleInstance AttackerData { get; set; }

    /// <summary>
    /// 目标的数据
    /// </summary>
    public IRoleInstance TargetData { get; set; }

    /// <summary>
    /// 攻击方式
    /// </summary>
    public IAttackPattern AttackPattern { get; set; }
}

/// <summary>
/// 战斗帮助类
/// </summary>
public static class HelperBattle
{
    #region 战斗行动操作
    /// <summary>
    /// 添加战斗行动
    /// </summary>
    /// <param name="battleActionCarrier"></param>
    /// <param name="battleAction"></param>
    /// <returns></returns>
    public static bool AddBattleAction(this IBattle battleActionCarrier, IBattleAction battleAction)
    {
        if (battleActionCarrier.BattleActionList == null)
            battleActionCarrier.BattleActionList = new List<IBattleAction>();
        return battleActionCarrier.BattleActionList.AddNotContainElement(battleAction);
    }
    /// <summary>
    /// 移除战斗行动
    /// </summary>
    /// <param name="battleActionCarrier"></param>
    /// <param name="battleAction"></param>
    /// <returns></returns>
    public static bool RemoveBattleAction(this IBattle battleActionCarrier, IBattleAction battleAction)
    {
        return battleActionCarrier.BattleActionList.RemoveContainElement(battleAction);
    }
    /// <summary>
    /// 检查战斗是否存在
    /// </summary>
    /// <param name="battleActionCarrier"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    public static bool ChackBattleActionExist(this IBattle battleActionCarrier, IRole role, out IBattleAction battleAction)
    {
        battleAction = default;
        foreach (IBattleAction item in battleActionCarrier.BattleActionList)
        {
            if (item.AttackerData != role) continue;
            battleAction = item;
            return true;
        }
        return false;
    }
    /// <summary>
    /// 是否包含战斗数据
    /// </summary>
    /// <param name="battleActionCarrier"></param>
    /// <param name="battleAction"></param>
    /// <returns></returns>
    public static bool ChackBattleActionExist(this IBattle battleActionCarrier, IBattleAction battleAction)
    {
        return battleActionCarrier.BattleActionList.Contains(battleAction);
    }
    #endregion


    #region 角色操作
    /// <summary>
    /// 确认角色是否存活
    /// </summary>
    /// <param name="battleCarrier"></param>
    /// <param name="roleActual"></param>
    /// <returns></returns>
    public static bool ChackRoleSurvival(this IBattle battleCarrier, IRoleInstance roleActual)
    {
        foreach (ITeamInstance item in battleCarrier.BattleTeamDic.Values)
        {
            if (item.ChackRoleSurvival(roleActual))
                return true;
        }
        return false;
    }
    /// <summary>
    /// 随机敌方一个角色
    /// </summary>
    /// <param name="battleCarrier"></param>
    /// <param name="teamType"></param>
    /// <returns></returns>
    public static IRoleInstance RandomEnemyRole(this IBattle battleCarrier, ERoleOrTeamType teamType)
    {
        ExpansionProfiler.ProfilerBeginSample("随机一个敌人消耗时间");
        //获取所有角色,收集敌人
        List<IRoleInstance> role = new List<IRoleInstance>();
        foreach (ITeamInstance item in battleCarrier.BattleTeamDic.Values)
        {
            foreach (IRoleInstance item1 in item.RoleList)
            {
                switch (teamType)
                {
                    case ERoleOrTeamType.Player:
                    case ERoleOrTeamType.NPC:
                        if (item1.RoleType == ERoleOrTeamType.Enemy)
                            role.Add(item1);
                        break;
                    case ERoleOrTeamType.Enemy:
                        if (item1.RoleType == ERoleOrTeamType.Player || item1.RoleType == ERoleOrTeamType.NPC)
                            role.Add(item1);
                        break;
                    default:
                        Debug.Error("ETeamType未知类型错误");
                        break;
                }
            }
        }
        int number = Random.Range(0, role.Count);
        ExpansionProfiler.ProfilerEndSample();
        return role[number];
    }

    /// <summary>
    /// 随机自己方一个角色
    /// </summary>
    /// <param name="battleCarrier"></param>
    /// <param name="teamType"></param>
    /// <returns></returns>
    public static IRoleInstance RandomOwnRole(this IBattle battleCarrier, ERoleOrTeamType teamType) => IBattle.RandomOwnRole(battleCarrier, teamType);
    #endregion


    #region 队伍操作
    /// <summary>
    /// 添加一只队伍
    /// </summary>
    /// <param name="battleCarrier"></param>
    /// <param name="team"></param>
    public static void AddBattleTeam(this IBattle battleCarrier, ITeamInstance team) => IBattle.AddBattleTeam(battleCarrier, team);

    /// <summary>
    /// 检查队伍是否存活
    /// </summary>
    /// <param name="battleCarrier"></param>
    /// <param name="teamArray"></param>
    /// <returns></returns>
    public static bool ChackTeamSurvival(this IBattle battleCarrier, params ETeamPoint[] teamPointArray)
    {
        foreach (ITeamInstance item in battleCarrier.BattleTeamDic.Values)
        {
            foreach (ETeamPoint item1 in teamPointArray)
            {
                if (item.TeamPoint != item1) continue;
                if (item.ChackTeamRoleSurvival())
                    return true;
            }
        }
        return false;
    }
    #endregion
}
