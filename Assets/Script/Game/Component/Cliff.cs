using UnityEngine;

/// <summary>
/// 悬崖
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Cliff : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            RebornPoint = transform;
    }

    public static void Reset()
    {
        RebornPoint = null;
    }

    public static Transform RebornPoint;
}