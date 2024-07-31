using UnityEngine;

/// <summary>
/// 敌人的手臂
/// </summary>
public class EnemyArm : MonoBehaviour
{
    public void SetAngularSpeed(float speed)
    {
        GetComponent<Rigidbody2D>().angularVelocity = speed;
    }
}