using UnityEngine;

/// <summary>
/// 敌人的护甲
/// </summary>
[RequireComponent(typeof(EnemyAttribute))]
public abstract class EnemyArmor : MonoBehaviour
{
    private void Awake()
    {
        eAttr = GetComponent<EnemyAttribute>();
    }

    /// <summary>
    /// 击中装甲
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="data"></param>
    public abstract void HitArmor(int damage, string data);

    /// <summary>
    /// 破坏
    /// </summary>
    public abstract void Break();

    /// <summary>
    /// 玩家属性
    /// </summary>
    protected EnemyAttribute eAttr;
}