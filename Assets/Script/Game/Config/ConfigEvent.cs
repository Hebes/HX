/// <summary>
/// Game的事件
/// </summary>
public enum EGameEvent
{
    /// <summary>
    /// 通过门
    /// </summary>
    PassGate,
    
    /// <summary>
    /// 品质变化->分辨率
    /// </summary>
    QualityChange,
    
    /// <summary>
    /// 敌人伤害攻击
    /// </summary>
    EnemyHurtAtk,
    
    /// <summary>
    /// 敌人死亡
    /// </summary>
    EnemyKilled,
    
    /// <summary>
    /// 升级
    /// </summary>
    EnhanceLevelup,
    
    /// <summary>
    /// 玩家伤害攻击
    /// </summary>
    PlayerHurtAtk,
    
    /// <summary>
    /// 评估
    /// </summary>
    Assessment,
    
    /// <summary>
    /// 玩家受伤
    /// </summary>
    PlayerHurt,
}
