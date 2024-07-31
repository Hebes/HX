using System;
using UnityEngine;

public class Pivot : MonoBehaviour, IPivot
{
    public Vector3 GetGameAssistantOffset()
    {
        EnemyAttribute component = base.GetComponent<EnemyAttribute>();
        return (!(component != null)) ? this.gameAssistantOffset : new Vector3(0f, component.bounds.size.y / 2f, 0f);
    }

    public Vector3 GetAttackHurtEffectOffset()
    {
        return this.attackHurtEffectOffset;
    }

    public Vector2 GetAttackHurtNumberOffset()
    {
        return this.attackHurtNumberOffset;
    }

    public Vector2 GetHPBarOffset()
    {
        return this.HPBarOffset;
    }

    public Vector3 gameAssistantOffset = new Vector3(0f, 1.3f, 0f);

    public Vector3 attackHurtEffectOffset;

    public Vector2 attackHurtNumberOffset;

    public Vector2 HPBarOffset;
}