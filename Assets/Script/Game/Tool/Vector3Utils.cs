using UnityEngine;

/// <summary>
/// Vector3工具
/// </summary>
public static class Vector3Utils
{
    public static Vector3 AddX(this Vector3 vector3, float delta)
    {
        vector3.x += delta;
        return vector3;
    }

    public static Vector3 AddY(this Vector3 vector3, float delta)
    {
        vector3.y += delta;
        return vector3;
    }

    public static Vector3 SetX(this Vector3 vector3, float x)
    {
        vector3.x = x;
        return vector3;
    }

    public static Vector3 SetY(this Vector3 vector3, float y)
    {
        vector3.y = y;
        return vector3;
    }

    public static Vector3 SetZ(this Vector3 vector3, float z)
    {
        vector3.z = z;
        return vector3;
    }
}