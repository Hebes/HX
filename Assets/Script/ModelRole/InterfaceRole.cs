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

