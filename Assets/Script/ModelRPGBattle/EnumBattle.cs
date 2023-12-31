
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
public interface IRole
{
    /// <summary>
    /// 编号
    /// </summary>
    int ID { get; set; }

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

    void Init();
    void Update();
}


