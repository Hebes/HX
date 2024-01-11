﻿using UnityEngine;

/// <summary>
/// 角色接口
/// </summary>
public interface IRole : IID, IName
{
    /// <summary>
    /// 物体
    /// </summary>
    GameObject Go { get; set; }
    /// <summary>
    /// 行动冷却时间
    /// </summary>
    float Max_colldown { get; set; }

    /// <summary>
    /// 角色类型
    /// </summary>
    ERoleType RoleType { get; set; }

    /// <summary>
    /// 角色战斗的位置
    /// </summary>
    ERoleBattlePoint RoleBattlePoint { get; set; }

    /// <summary>
    /// 当前状态枚举
    /// </summary>
    ETurnState TurnState { get; set; }
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
/// 角色生命周期接口
/// </summary>
public interface IRoleBehaviour : IID
{
    /// <summary>
    /// 角色初始化
    /// </summary>
    public void RoleInit();

    /// <summary>
    /// 角色的循环
    /// </summary>
    public void RoleUpdata();

    /// <summary>
    /// 移除角色需要做的事情
    /// </summary>
    public void RoleRemove();

}

public interface IRoleBattleBehaviour : IID
{
    /// <summary>
    /// 角色战斗初始化接口
    /// </summary>
    public void RoleBattleInit();

    /// <summary>
    /// 角色战斗的循环
    /// </summary>
    public void RoleBattleUpdata();

    /// <summary>
    /// 移除角色战斗需要做的事情
    /// </summary>
    public void RoleBattleRemove();
}


/// <summary>
/// 伤害接口
/// </summary>
public interface IDamage
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
/// 角色的实际继承接口
/// </summary>
public interface IRoleActual : IRole, IRoleBehaviour, IRoleBattleBehaviour, IDamage
{
    /// <summary>
    /// 自己归属的队伍
    /// </summary>
    public ITeamActual TeamActual { get; set; }

    /// <summary>
    /// 战斗的实际接口
    /// </summary>
    public IBattleActual BattleActual { get; set; }
}
