using Core;
using System.Collections.Generic;

/// <summary>
/// 一支队伍
/// </summary>
public class TeamData : IID
{
    public long ID { get; set; }
    /// <summary>
    /// 队伍位置
    /// </summary>
    public ETeamPoint TeamPoint { get; set; }
    /// <summary>
    /// 队伍列表
    /// </summary>
    public List<RoleData> RoleList { get; set; }
    /// <summary>
    /// 队伍类型
    /// </summary>
    public ERoleOrTeamType TeamType { get; set; }
    /// <summary>
    /// 是否进入战斗
    /// </summary>
    public bool IsEnterBattle { get; set; }
    /// <summary>
    /// 队伍生命周期
    /// </summary>
    public void TeamUpdata()
    {
        foreach (RoleData temp in RoleList)
            temp.RoleState.StateUpdata();
    }
}
