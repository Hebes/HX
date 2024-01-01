﻿using System.Collections.Generic;
using Core;

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
///队伍的持有者
/// </summary>
public interface ITeamCarrier : IID
{
    /// <summary>
    /// 队伍列表
    /// </summary>
    public List<IRole> RoleList { get; set; }

    /// <summary>
    /// 添加队员
    /// </summary>
    /// <param name="role">角色</param>
    public static void AddRole(ITeamCarrier teamCarrier, IRole role)
    {
        if (teamCarrier.RoleList == null)
            teamCarrier.RoleList = new List<IRole>();

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
    public static void RemoveRole(ITeamCarrier teamCarrier, IRole role)
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
}