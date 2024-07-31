using UnityEngine;

/// <summary>
/// 枢轴
/// </summary>
public interface IPivot
{
    /// <summary>
    /// 获取游戏助手偏移量
    /// </summary>
    /// <returns></returns>
    Vector3 GetGameAssistantOffset();

    /// <summary>
    /// 获得攻击伤害效果抵消
    /// </summary>
    /// <returns></returns>
    Vector3 GetAttackHurtEffectOffset();

    /// <summary>
    /// 获得攻击伤害数抵消
    /// </summary>
    /// <returns></returns>
    Vector2 GetAttackHurtNumberOffset();

    /// <summary>
    /// 获取血量条偏移
    /// </summary>
    /// <returns></returns>
    Vector2 GetHPBarOffset();
}