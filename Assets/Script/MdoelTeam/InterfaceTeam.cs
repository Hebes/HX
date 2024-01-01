using System.Collections.Generic;

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