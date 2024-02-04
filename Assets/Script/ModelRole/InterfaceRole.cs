using Core;
using UnityEngine;
using Debug = Core.Debug;

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
    public RoleData RoleData { get; set; }
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
    public static T SwitchRoleState<T>(this RoleData roleData) where T : IRoleState, new()
    {
        roleData.RoleState = new T();
        roleData.RoleState.RoleData = roleData;
        roleData.RoleState.StateEnter();
        return (T)roleData.RoleState;
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