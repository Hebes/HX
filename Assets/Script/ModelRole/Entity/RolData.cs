using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 角色基类，战斗在这边
/// </summary>
public class RoleData : IRoleInstance
{
    #region 私有字段
    protected long _id;
    protected string _name;
    protected ERoleOrTeamType _roleType;
    protected ERoleTurnState _turnState;
    protected ERoleBattlePoint _roleBattlePoint;
    protected ERoleSateType _roleSateType;
    protected Dictionary<ESkillType, List<ISkill>> _skillDataDic;
    protected GameObject _go;
    protected RoleAttributes roleAttributes;
    protected IRoleState _roleState;
    protected ITeamInstance team;
    #endregion



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
        _id = id;
        _name = name;
        _roleType = roleType;
        _roleBattlePoint = roleBattlePoint;
    }

    /// <summary>
    /// 设置角色属性
    /// </summary>
    public virtual void SetRoleAttributes(RoleAttributes roleAttributes)
    {
        this.roleAttributes = roleAttributes;
    }

    //public void SwitchState<T>()
    //{
    //    RoleStateBattle roleState = this.SwitchRoleState<RoleStateBattle>();
    //    roleState.SetBattleData(this,)
    //}
    #endregion


    #region 接口属性
    public RoleData Role => this;
    public ERoleOrTeamType RoleType { get => _roleType; set => _roleType = value; }
    public ERoleBattlePoint RoleBattlePoint { get => _roleBattlePoint; set => _roleBattlePoint = value; }
    public string Name { get => _name; set => _name = value; }
    public long ID { get => _id; set => _id = value; }
    public Dictionary<ESkillType, List<ISkill>> SkillDataDic { get => _skillDataDic; set => _skillDataDic = value; }
    public GameObject gameObject { get => _go; set => _go = value; }
    public IRoleState RoleState { get => _roleState; set => _roleState = value; }
    public ERoleSateType RoleSateType { get => _roleSateType; set => _roleSateType = value; }
    public ITeamInstance Team { get => team; set => team = value; }
    public RoleAttributes RoleAttributes { get => roleAttributes; set => roleAttributes = value; }
    #endregion
}
