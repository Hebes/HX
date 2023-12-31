using Core;
using UnityEngine;

/// <summary>
/// 玩家脚本
/// </summary>
//[RequireComponent(typeof(ItemPickUp))]                  //物品拾取
//[RequireComponent(typeof(TriggerObscuringItemFader))]   //触发物品模糊
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour, IFixedUpdate, IUpdata
{
    private int m_PlayerID = 1;                                         //前10的请不要用
    private ERoleType m_roleType = ERoleType.Player;
    

    private void Awake()
    {
        CoreBehaviour.Add(this);
    }
    public void OnFixedUpdate()
    {

    }
    public void OnUpdata()
    {
    }
}
