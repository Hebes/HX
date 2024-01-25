using UnityEngine;
using Core;
using Debug = Core.Debug;
using Unity.VisualScripting;

/// <summary>
/// 角色实际接口
/// </summary>
public interface IRoleInstance : IRole, ISkillCarrier
{
    /// <summary>
    /// 玩家数据
    /// </summary>
    public RoleData RoleInfo { get; }

    /// <summary>
    /// 队伍
    /// </summary>
    public ITeamInstance Team { get; set; }

    /// <summary>
    /// 玩家属性
    /// </summary>
    public RoleAttributes RoleAttributes { get; set; }
}


/// <summary>
/// 角色接口
/// </summary>
public interface IRole : IID, IName
{
    /// <summary>
    /// 物体
    /// </summary>
    public GameObject gameObject { get; set; }

    /// <summary>
    /// 角色类型
    /// </summary>
    public ERoleOrTeamType RoleType { get; set; }

    /// <summary>
    /// 角色战斗的位置
    /// </summary>
    public ERoleBattlePoint RoleBattlePoint { get; set; }

    /// <summary>
    /// 角色状态
    /// </summary>
    public ERoleSateType RoleSateType { get; set; }

    /// <summary>
    /// 角色状态
    /// </summary>
    public IRoleState RoleState { get; set; }
}

/// <summary>
/// 攻击次数接口
/// </summary>
public interface IRoleAttackCount
{
    /// <summary>
    /// 攻击次数，指的是攻击开始的次数
    /// </summary>
    public int AttackCount { get; set; }
}

/// <summary>
/// 伤害接口
/// </summary>
public interface IDamage : IID
{
    /// <summary>
    /// 给与伤害
    /// </summary>
    public void DoDamage();

    /// <summary>
    /// 遭受伤害
    /// </summary>
    public void TakeDamage(int getDamageAmount);
}

/// <summary>
/// 角色状态
/// </summary>
public interface IRoleState : IID
{
    /// <summary>
    /// 角色信息
    /// </summary>
    public IRoleInstance RoleInstance { get; set; }

    /// <summary>
    /// 角色状态
    /// </summary>
    public ERoleSateType RoleSateType { get; }

    /// <summary>
    /// 角色初始化
    /// </summary>
    public void StateEnter();

    /// <summary>
    /// 角色的循环
    /// </summary>
    public void StateUpdata();

    /// <summary>
    /// 移除角色需要做的事情
    /// </summary>
    public void StateExit();
}

/// <summary>
/// 角色帮助类
/// </summary>
public static class HelperRole
{
    /// <summary>
    /// 切换角色状态
    /// </summary>
    public static T SwitchRoleState<T>(this IRoleInstance RoleState) where T : IRoleState, new()
    {
        RoleState.RoleState = new T();
        RoleState.RoleSateType = RoleState.RoleState.RoleSateType;
        RoleState.RoleState.RoleInstance = RoleState;
        RoleState.RoleState.StateEnter();
        return (T)RoleState.RoleState;
    }

    /// <summary>
    /// 获取角色状态
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="roleState"></param>
    /// <returns></returns>
    public static T GetRoleSate<T>(this IRoleState roleState) where T : IRoleState
    {
        if (roleState is T t)
            return t;
        Debug.Error($"脚本不继承IRoleState,或者角色未转换到这个状态,当前状态{roleState.RoleSateType}");
        return default;
    }
}