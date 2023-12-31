/// <summary>
/// 角色
/// </summary>
public interface IRole : IID, IName
{
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
/// 角色生命周期
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
