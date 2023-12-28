
/// <summary>
/// 战斗执行
/// </summary>
public enum EBattlePerformAction
{
    /// <summary>
    /// 等待
    /// </summary>
    WAIT,
    /// <summary>
    /// 采取行动
    /// </summary>
    TAKEACTION,
    /// <summary>
    /// 执行动作
    /// </summary>
    PERFROMACTION,
    /// <summary>
    /// 检查敌人或者玩家是否存活
    /// </summary>
    CHECKALIVE,
    /// <summary>
    /// 赢
    /// </summary>
    WIN,
    /// <summary>
    /// 输
    /// </summary>
    LOSE
}

/// <summary>
/// 角色
/// </summary>
public interface IRole : IID
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
/// 角色战斗的位置
/// </summary>
public enum ERoleBattlePoint
{
    Left,
    Centre,
    Right,
}

/// <summary>
/// 角色类型
/// </summary>
public enum ERoleType
{
    /// <summary>
    /// 玩家
    /// </summary>
    Player,
    /// <summary>
    /// 朋友
    /// </summary>
    NPC,
    /// <summary>
    /// 敌人
    /// </summary>
    Enemy,
    /// <summary>
    /// 商人
    /// </summary>
    Dealer,
}
