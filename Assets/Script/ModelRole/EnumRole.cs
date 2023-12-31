/// <summary>
/// 玩家和NPC和敌人公用，状态枚举
/// </summary>
public enum ETurnState
{
    /// <summary>
    /// 进度条上升
    /// </summary>
    PROCESSING,
    /// <summary>
    /// 选择敌人行动  
    /// </summary>
    CHOOSEACTION,
    /// <summary>
    /// 等待
    /// </summary>
    WAITING,
    /// <summary>
    /// 行动
    /// </summary>
    ACTION,
    /// <summary>
    /// 死去的
    /// </summary>
    DEAD,
}

/// <summary>
/// 角色战斗的位置
/// </summary>
public enum ERoleBattlePoint
{
    /// <summary>
    /// 左边
    /// </summary>
    Left,

    /// <summary>
    /// 中间
    /// </summary>
    Centre,

    /// <summary>
    /// 右边
    /// </summary>
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
    /// 朋友->NPC
    /// </summary>
    Friend,
    /// <summary>
    /// 敌人
    /// </summary>
    Enemy,
    /// <summary>
    /// 商人
    /// </summary>
    Dealer,
}