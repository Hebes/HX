using UnityEngine;

/// <summary>
/// 伤害检查
/// </summary>
public class HurtCheck : MonoBehaviour
{
    [Header("身体类型")] [SerializeField] public HurtCheck.BodyType bodyType;

    public enum BodyType
    {
        /// <summary>
        /// 角
        /// </summary>
        Horn,

        /// <summary>
        /// 身体
        /// </summary>
        Body,

        /// <summary>
        /// 尾巴
        /// </summary>
        Tail,
    }
}