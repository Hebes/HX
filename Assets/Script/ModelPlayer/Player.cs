using Core;
using UnityEngine;

/// <summary>
/// 玩家脚本
/// </summary>
//[RequireComponent(typeof(ItemPickUp))]                  //物品拾取
//[RequireComponent(typeof(TriggerObscuringItemFader))]   //触发物品模糊
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour, IFixedUpdate, IUpdata, IRole
{
    private int m_PlayerID = 1;                                         //前10的请不要用
    private ERoleType m_roleType = ERoleType.Player;
    private ERoleBattlePoint m_roleBattlePoint = ERoleBattlePoint.Right;//玩家默认右边，以后会改位置//被偷袭可能会在左边


    public int ID { get => m_PlayerID; set => m_PlayerID = value; }
    public ERoleType roleType { get => m_roleType; set => m_roleType = value; }
    public ERoleBattlePoint roleBattlePoint { get => m_roleBattlePoint; set => m_roleBattlePoint = value; }

    
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
