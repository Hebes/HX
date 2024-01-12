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
}

/// <summary>
/// 战斗生命周期接口
/// </summary>
public interface IBattleBehaviour : IID
{
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
}

/// <summary>
/// 战斗队伍的持有者接口
/// </summary>
public interface IBattleCarrier : IID
{
    /// <summary>
    /// 战斗的队伍
    /// 1.可能是敌人在左边，进入二打一模式
    /// 2.可能是自己人右边，敌人3队进行二打一模式
    /// </summary>
    public Dictionary<ETeamPoint, ITeamActual> BattleTeamDic { get; set; }

    //public List<ITeamActual> LeftTeamList { get; set; }
    //public List<ITeamActual> RightTeamList { get; set; }


    /// <summary>
    /// 添加战斗队伍
    /// </summary>
    public static void AddBattleTeam(IBattleCarrier battleCarrier, ITeamActual team)
    {
        if (battleCarrier.BattleTeamDic == null)
            battleCarrier.BattleTeamDic = new Dictionary<ETeamPoint, ITeamActual>();

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
    public static void RemoveBattleTeam(IBattleCarrier battleCarrier, ITeamActual team)
    {
        if (battleCarrier.BattleTeamDic.ContainsKey(team.TeamPoint))
            battleCarrier.BattleTeamDic.Remove(team.TeamPoint);
    }

    /// <summary>
    /// 随机一个敌人(NPC和Player敌人就是Enemy，相反就是...)
    /// </summary>
    public static IRoleActual RandomEnemyRole(IBattleCarrier battleCarrier, ETeamType teamType)
    {
        ExpansionProfiler.ProfilerBeginSample("随机一个敌人");
        //获取所有角色
        List<IRoleActual> role = new List<IRoleActual>();
        foreach (ITeamActual item in battleCarrier.BattleTeamDic.Values)
        {
            foreach (IRoleActual item1 in item.RoleList)
                role.Add(item1);
        }

        //收集敌人
        int number = 0;
        //随机一个敌人
        switch (teamType)
        {
            case ETeamType.Player:
            case ETeamType.NPC:
                role = role.FindAll((IRoleActual data) =>
                {
                    return data.RoleType == ERoleType.Enemy;
                });
                number = Random.Range(0, role.Count);
                ExpansionProfiler.ProfilerEndSample();
                return role[number];
            case ETeamType.Enemy:
                role = role.FindAll((IRoleActual data) =>
                {
                    return data.RoleType == ERoleType.Player ||
                    data.RoleType == ERoleType.NPC;
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

    /// <summary>
    /// 随机自己方一个人
    /// </summary>
    /// <returns></returns>
    public static IRoleActual RandomOwnRole(IBattleCarrier battleCarrier, ETeamType teamType)
    {
        ExpansionProfiler.ProfilerBeginSample("随机自己方一个人");
        List<IRoleActual> role = new List<IRoleActual>();
        foreach (ITeamActual item in battleCarrier.BattleTeamDic.Values)
        {
            foreach (IRoleActual item1 in item.RoleList)
                role.Add(item1);
        }

        int number = 0;
        //随机一个己方人物
        switch (teamType)
        {
            case ETeamType.Player:
            case ETeamType.NPC:
                role = role.FindAll((IRoleActual data) =>
                {
                    return data.RoleType == ERoleType.Player ||
                    data.RoleType == ERoleType.NPC;
                });
                number = Random.Range(0, role.Count);
                ExpansionProfiler.ProfilerEndSample();
                return role[number];
            case ETeamType.Enemy:
                role = role.FindAll((IRoleActual data) =>
                {
                    return data.RoleType == ERoleType.Enemy;
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

    /// <summary>
    /// 确认角色是否存活
    /// </summary>
    /// <returns></returns>
    public static bool ChackRoleSurvival(IBattleCarrier battleCarrier, IRoleActual roleActual)
    {
        foreach (ITeamActual item in battleCarrier.BattleTeamDic.Values)
        {
            if (item.ChackRoleSurvival(roleActual))
                return true;
        }
        return false;
    }
}


/// <summary>
/// 战斗的实际接口
/// </summary>
public interface IBattleActual : IBattle, IBattleBehaviour, IBattleCarrier, IBattleActionCarrier
{

}

/// <summary>
/// 个人战斗数据接口
/// </summary>
public interface IBattleAction
{
    /// <summary>
    /// 攻击者的数据
    /// </summary>
    public IRoleActual AttackerData { get; set; }

    /// <summary>
    /// 目标的数据
    /// </summary>
    public IRoleActual TargetData { get; set; }

    /// <summary>
    /// 攻击方式
    /// </summary>
    public IAttackPattern AttackPattern { get; set; }
}

/// <summary>
/// 行动持有者接口
/// </summary>
public interface IBattleActionCarrier : IID
{
    /// <summary>
    /// 行动列表
    /// </summary>

    public List<IBattleAction> BattleActionList { get; set; }

    /// <summary>
    /// 添加战斗行动
    /// </summary>
    public static void AddBattleAction(IBattleActionCarrier battleActionCarrier, IBattleAction battleAction)
    {
        if (battleActionCarrier.BattleActionList == null)
            battleActionCarrier.BattleActionList = new List<IBattleAction>();
        battleActionCarrier.BattleActionList.Add(battleAction);
    }

    /// <summary>
    /// 移除战斗行动
    /// </summary>
    public static void RemoveBattleAction(IBattleActionCarrier battleActionCarrier, IBattleAction battleAction)
    {
        battleActionCarrier.BattleActionList.Remove(battleAction);
    }

    /// <summary>
    /// 切换战斗是否存在
    /// </summary>
    public static bool ChackBattleExist(IBattleActionCarrier battleActionCarrier, IRole role)
    {
        foreach (IBattleAction item in battleActionCarrier.BattleActionList)
        {
            if (item.AttackerData == role)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 切换战斗是否存在
    /// </summary>
    public static bool ChackBattleExist(IBattleActionCarrier battleActionCarrier, IBattleAction battleAction)
    {
        return battleActionCarrier.BattleActionList.Contains(battleAction);
    }
}


public static class HelperBattle
{
    public static void AddBattle(this IBattleActionCarrier battleActionCarrier, IBattleAction battleAction)
    {
        IBattleActionCarrier.AddBattleAction(battleActionCarrier, battleAction);
    }
    public static void RemoveBattleAction(this IBattleActionCarrier battleActionCarrier, IBattleAction battleAction)
    {
        IBattleActionCarrier.RemoveBattleAction(battleActionCarrier, battleAction);
    }
    public static void ChackBattleExist(this IBattleActionCarrier battleActionCarrier, IRole role)
    {
        IBattleActionCarrier.ChackBattleExist(battleActionCarrier, role);
    }
    public static void ChackBattleExist(this IBattleActionCarrier battleActionCarrier, IBattleAction battleAction)
    {
        IBattleActionCarrier.ChackBattleExist(battleActionCarrier, battleAction);
    }

    /// <summary>
    /// 确认角色是否存活
    /// </summary>
    /// <param name="battleCarrier"></param>
    /// <param name="roleActual"></param>
    /// <returns></returns>
    public static bool ChackRoleSurvival(this IBattleCarrier battleCarrier, IRoleActual roleActual)
    {
        return IBattleCarrier.ChackRoleSurvival(battleCarrier, roleActual);
    }

    /// <summary>
    /// 随机敌方一个角色
    /// </summary>
    /// <param name="battleCarrier"></param>
    /// <param name="teamType"></param>
    /// <returns></returns>
    public static IRoleActual RandomEnemyRole(this IBattleCarrier battleCarrier, ETeamType teamType)
    {
        return IBattleCarrier.RandomEnemyRole(battleCarrier, teamType);
    }

    /// <summary>
    /// 随机自己方一个角色
    /// </summary>
    /// <param name="battleCarrier"></param>
    /// <param name="teamType"></param>
    /// <returns></returns>
    public static IRoleActual RandomOwnRole(this IBattleCarrier battleCarrier, ETeamType teamType)
    {
        return IBattleCarrier.RandomOwnRole(battleCarrier, teamType);
    }
}
