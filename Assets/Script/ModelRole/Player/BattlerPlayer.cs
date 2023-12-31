using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Core;

public class BattlerPlayer : MonoBehaviour, IRole, IUpdata
{
    private int m_PlayerID;
    private ERoleBattlePoint m_roleBattlePoint = ERoleBattlePoint.Right;//玩家默认右边，以后会改位置//被偷袭可能会在左边
    private ETurnState m_turnState;
    private ERoleType m_roleType;


    public int ID { get => m_PlayerID; set => m_PlayerID = value; }
    public ERoleType RoleType { get => m_roleType; set => m_roleType = value; }
    public ERoleBattlePoint RoleBattlePoint { get => m_roleBattlePoint; set => m_roleBattlePoint = value; }
    public ETurnState TurnState { get => m_turnState; set => m_turnState = value; }



    /// <summary>
    /// 当前的冷却时间
    /// </summary>
    private float cur_colldown;

    /// <summary>
    /// 最大的冷却时间
    /// </summary>
    private float max_colldown;



    public void Init()
    {
        CoreBehaviour.Add(this);
        m_turnState = ETurnState.PROCESSING;
    }

    public void OnUpdata()
    {
        switch (m_turnState)
        {
            case ETurnState.PROCESSING:
                UpgradeProgressBar();
                break;
            case ETurnState.CHOOSEACTION:
                ChooseAction();
                break;
            case ETurnState.WAITING:
                break;
            case ETurnState.ACTION:
                break;
            case ETurnState.DEAD:
                break;
            default:
                break;
        }
    }

    private void ChooseAction()
    {
    }

    // <summary>
    /// 升级进度条  冷却版
    /// </summary>
    void UpgradeProgressBar()
    {
        cur_colldown = cur_colldown + Time.deltaTime;
        if (cur_colldown >= max_colldown)//如果冷却时间到了
            m_turnState = ETurnState.CHOOSEACTION;
    }
}
