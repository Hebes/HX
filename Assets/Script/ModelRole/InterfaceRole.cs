/// <summary>
/// 角色
/// </summary>
public interface IRole : IID, IName
{
    /// <summary>
    /// 角色类型
    /// </summary>
    ERoleType roleType { get; set; }

    /// <summary>
    /// 角色战斗的位置
    /// </summary>
    ERoleBattlePoint roleBattlePoint { get; set; }
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
    /// 移除角色需要做的事情
    /// </summary>
    public void Remove();

}
