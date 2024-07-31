using UnityEngine;

/// <summary>
/// 攻击效果
/// </summary>
public class AtkEffector : MonoBehaviour
{
    public void SetData(JsonData1 atkData, int atkId)
    {
        if (this.playerAtk != null)
        {
            this.playerAtk.SetData(atkData, atkId);
        }
    }

    public PlayerAtk playerAtk;

    [SerializeField]
    public Vector3 pos;

    [SerializeField]
    public bool UseAtkData = true;

    [SerializeField]
    public bool CanHitGround;
}