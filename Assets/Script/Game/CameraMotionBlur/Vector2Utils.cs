using UnityEngine;

/// <summary>
/// Vector2工具
/// </summary>
public static class Vector2Utils
{
    /// <summary>
    /// 坡度
    /// </summary>
    /// <param name="vector2"></param>
    /// <returns></returns>
    public static float Slope(this Vector2 vector2)
    {
        return vector2.y / vector2.x;
    }
}