using Core;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色基类，战斗在这边
/// </summary>
public class RoleData : IID, IName, ISkillCarrier
{

    #region 初始化API,请按照顺序初始化 
    /// <summary>
    /// 设置角色信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="roleType"></param>
    /// <param name="roleBattlePoint"></param>
    /// <param name="max_colldown"></param>
    public virtual void SetRole(long id, string name, ERoleOrTeamType roleType, ERoleBattlePoint roleBattlePoint)
    {
        ID = id;
        Name = name;
        RoleType = roleType;
        RoleBattlePoint = roleBattlePoint;
    }

    /// <summary>
    /// 设置角色属性
    /// </summary>
    public virtual void SetRoleAttributes(TeamData team, RoleAttributes roleAttributes)
    {
        Team = team;
        RoleAttributes = roleAttributes;
    }
    #endregion

    #region 本类属性
    public TeamData Team { get; set; }
    public RoleAttributes RoleAttributes { get; set; }
    public ERoleSateType RoleSateType { get => RoleState.RoleSateType; }
    #endregion

    #region 接口属性
    /// <summary>
    /// 角色类型
    /// </summary>
    public ERoleOrTeamType RoleType { get; set; }
    /// <summary>
    /// 角色战斗的位置
    /// </summary>
    public ERoleBattlePoint RoleBattlePoint { get; set; }
    public string Name { get; set; }
    public long ID { get; set; }
    public List<SkillData> SkillList { get; set; }
    /// <summary>
    /// 物体
    /// </summary>
    public GameObject gameObject { get; set; }
    /// <summary>
    /// 角色状态
    /// </summary>
    public IRoleState RoleState { get; set; }
    /// <summary>
    /// 是否死亡
    /// </summary>
    public bool IsAlive { get; set; }
    #endregion
}
