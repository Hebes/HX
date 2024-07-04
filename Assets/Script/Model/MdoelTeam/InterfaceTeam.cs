using System.Collections.Generic;
using Framework.Core;
using UnityEngine;


/// <summary>
/// 队伍帮助类
/// </summary>
public static class HelperTeam
{
    #region 角色操作
    /// <summary>
    /// 创建一个角色
    /// </summary>
    //public static void CreatRole<T>() where T : class, IRoleInstance, new()
    //{
    //    //TODO 后面从对象池加入
    //    T t = new T(); 
    //    //t
    //}

    /// <summary>
    /// 添加队员
    /// </summary>
    /// <param name="team"></param>
    /// <param name="role"></param>
    public static bool AddRole(this TeamData team, RoleData role)
    {
        if (team.RoleList == null)
            team.RoleList = new List<RoleData>(4);//限定4人

        if (team.RoleList.Count >= 4)
        {
            EDebug.Error($"添加{role.Name}错误，当前只能有4个人");
            return false;
        }
        if (!team.RoleList.AddNotContainElement(role))
        {
            EDebug.Error($"添加{role.Name}错误，已经存在这个角色");
            return false;
        }
        return true;
    }

    ///// <summary>
    ///// 移除队员
    ///// </summary>
    ///// <param name="team"></param>
    ///// <param name="role"></param>
    //public static bool RemoveRole(this TeamData team, RoleData role)
    //{
    //    if (!team.RoleList.RemoveContainElement(role))
    //    {
    //        Debug.Error($"移除失败请检查,该队伍没有队员{role.Name}");
    //        return false;
    //    }
    //    role.
    //    role.ChackInherit<IRoleInstance, IRoleState>()?.StateExit();
    //    return true;
    //}
    #endregion

    #region 队伍操作
    /// <summary>
    /// 确认队伍中是否有人存活
    /// </summary>
    /// <param name="team"></param>
    public static bool ChackTeamRoleSurvival(this TeamData team)
    {
        foreach (RoleData item in team.RoleList)
        {
            if (item.RoleSateType != ERoleSateType.Dead)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 确认角色是否存活
    /// </summary>
    /// <param name="team"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    //public static bool ChackRoleSurvival(this ITeam team, IRoleInstance role)
    //{
    //    foreach (IRoleInstance item in team.RoleList)
    //    {
    //        if (role.ID == item.ID && item.RoleSateType != ERoleSateType.Dead)
    //            return true;
    //    }
    //    return false;
    //}

    /// <summary>
    /// 获取随机角色
    /// </summary>
    /// <param name="team"></param>
    /// <returns></returns>
    //public static IRoleInstance RandomRole(this ITeam team)
    //{
    //    if (ChackTeamRoleCount(team,out int count))
    //    {
    //        int number = Random.Range(0, team.RoleList.Count);
    //        return team.RoleList[number];
    //    }
    //    return default;
    //}

    /// <summary>
    /// 检查队伍角色数量
    /// </summary>
    /// <param name="team">队伍</param>
    /// <param name="count">队伍角色数量</param>
    /// <returns></returns>
    //public static bool ChackTeamRoleCount(this ITeam team, out int count)
    //{
    //    count = team.RoleList.Count;
    //    return team.RoleList.Count > 0;
    //}
    #endregion

}