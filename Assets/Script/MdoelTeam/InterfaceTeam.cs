using System.Collections.Generic;
using Core;
using UnityEngine;
using Debug = Core.Debug;

/// <summary>
/// 队伍
/// </summary>
public interface ITeam : IID
{
    /// <summary>
    /// 队伍位置
    /// </summary>
    public ETeamPoint TeamPoint { get; set; }

    /// <summary>
    /// 队伍类型
    /// </summary>
    public ETeamType TeamType { get; set; }
}

/// <summary>
/// 队伍的生命周期接口
/// </summary>
public interface ITeamBehaviour : IID
{
    public void TeamUpdata();
}

/// <summary>
///队伍的持有者
/// </summary>
public interface ITeamCarrier : IID
{
    /// <summary>
    /// 队伍列表
    /// </summary>
    public List<IRoleActual> RoleList { get; set; }

    /// <summary>
    /// 添加队员
    /// </summary>
    /// <param name="role">角色</param>
    public static void AddRole(ITeamCarrier teamCarrier, IRoleActual role)
    {
        if (teamCarrier.RoleList == null)
            teamCarrier.RoleList = new List<IRoleActual>(4);

        if (teamCarrier.RoleList.Count >= 4)
        {
            Debug.Error($"添加{role.Name}错误，当前只能有4个人");
            return;
        }
        if (!teamCarrier.RoleList.AddNotContainElement(role))
        {
            Debug.Error($"添加{role.Name}错误，已经存在这个角色");
            return;
        }
        if (role is IRoleBehaviour roleBehaviour)
            roleBehaviour.RoleInit();
    }

    /// <summary>
    /// 移除队员
    /// </summary>
    /// <param name="role">角色</param>
    public static void RemoveRole(ITeamCarrier teamCarrier, IRoleActual role)
    {
        if (!teamCarrier.RoleList.RemoveContainElement(role))
        {
            Debug.Error($"移除失败请检查,该队伍没有队员{role.Name}");
            return;
        }
        if (role is IRoleBehaviour roleBehaviour)
            roleBehaviour.RoleRemove();
    }

    /// <summary>
    /// 确认队伍是否存活
    /// </summary>
    public static bool ChackTeamSurvival(ITeamCarrier itemCarrier)
    {

        foreach (IRole item in itemCarrier.RoleList)
        {
            if (item.TurnState != ETurnState.DEAD)
                return true;
        }
        return false;
    }

    public static bool ChackRoleSurvival(ITeamCarrier itemCarrier, IRoleActual roleActual)
    {
        foreach (IRoleActual item in itemCarrier.RoleList)
        {
            if (roleActual.ID == item.ID && item.TurnState != ETurnState.DEAD)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 检查队伍角色存活数量
    /// </summary>
    /// <param name="itemCarrier"></param>
    /// <returns></returns>
    public static int ChackTeamSurvivalCount(ITeamCarrier itemCarrier)
    {
        return itemCarrier.RoleList.Count;
    }

    /// <summary>
    /// 随机一个人
    /// </summary>
    /// <returns></returns>
    public static IRole RandomRole(ITeamCarrier teamCarrier)
    {
        if (teamCarrier.RoleList.Count > 0)
            return teamCarrier.RoleList[Random.Range(0, teamCarrier.RoleList.Count)];
        return default;
    }
}

/// <summary>
/// 队伍的实际接口
/// </summary>
public interface ITeamActual : ITeam, ITeamCarrier, ITeamBehaviour
{

}


public static class HelperTeam
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="teamCarrier"></param>
    /// <param name="role"></param>
    public static void AddRole(this ITeamCarrier teamCarrier, IRoleActual role) => ITeamCarrier.AddRole(teamCarrier, role);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="teamCarrier"></param>
    /// <param name="role"></param>
    public static void RemoveRole(this ITeamCarrier teamCarrier, IRoleActual role) => ITeamCarrier.RemoveRole(teamCarrier, role);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="teamCarrier"></param>
    public static bool ChackTeamSurvival(this ITeamCarrier teamCarrier) => ITeamCarrier.ChackTeamSurvival(teamCarrier);

    /// <summary>
    /// 确认角色是否存活
    /// </summary>
    /// <param name="itemCarrier"></param>
    /// <param name="roleActual"></param>
    /// <returns></returns>
    public static bool ChackRoleSurvival(this ITeamCarrier itemCarrier, IRoleActual roleActual) => ITeamCarrier.ChackRoleSurvival(itemCarrier, roleActual);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="teamCarrier"></param>
    /// <returns></returns>
    public static IRole RandomRole(this ITeamCarrier teamCarrier) => ITeamCarrier.RandomRole(teamCarrier);

    /// <summary>
    /// 检查队伍角色存活数量
    /// </summary>
    /// <param name="teamCarrier"></param>
    /// <returns></returns>
    public static int ChackTeamSurvivalCount(this ITeamCarrier teamCarrier) => ITeamCarrier.ChackTeamSurvivalCount(teamCarrier);
}