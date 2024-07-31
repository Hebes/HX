using System;
using UnityEngine;

/// <summary>
/// 美术材质
/// </summary>
[AddComponentMenu("Advanced Platformer 2D/Material")]
public class APMaterial : MonoBehaviour
{
    public bool m_overrideFriction;

    public float m_dynFriction = 1f;

    public float m_staticFriction = 1f;

    public float m_groundBounciness;

    public APMaterial.BoolValue m_wallJump;

    public APMaterial.BoolValue m_wallSlide;

    public APMaterial.FloatValue m_wallFriction;

    public enum BoolValue
    {
        Default,
        True,
        False
    }

    [Serializable]
    public class FloatValue
    {
        public bool m_override;

        public float m_value;
    }
}