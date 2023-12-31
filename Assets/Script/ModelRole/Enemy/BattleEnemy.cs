using System;

/// <summary>
/// 具体战斗敌人实例
/// </summary>
public class BattleEnemy : IRole
{
    private int m_id;
    private ERoleType m_roleType;
    private ERoleBattlePoint m_roleBattlePoint;
    private ETurnState m_turnState;
    private float max_colldown;         //最大的冷却时间
    private int cur_colldown;           //当前的冷却时间

    public BattleEnemy(int id, ERoleType roleType, ERoleBattlePoint roleBattlePoint, ETurnState turnState, float max_colldown)
    {
        m_id = id;
        m_roleType = roleType;
        m_roleBattlePoint = roleBattlePoint;
        m_turnState = turnState;
        this.max_colldown = max_colldown;
    }

    public int ID { get => m_id; set => m_id = value; }
    public ERoleType RoleType { get => m_roleType; set => m_roleType = value; }
    public ERoleBattlePoint RoleBattlePoint { get => m_roleBattlePoint; set => m_roleBattlePoint = value; }
    public ETurnState TurnState { get => m_turnState; set => m_turnState = value; }
    public float Max_colldown { get => max_colldown; set => max_colldown = value; }

    public void Init()
    {
        m_turnState = ETurnState.PROCESSING;
    }

    public void Update()
    {
        switch (m_turnState)
        {
            case ETurnState.PROCESSING:
                UpgradeProgressBar();
                break;
            case ETurnState.CHOOSEACTION:
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

    /// <summary>
    /// 进度条上升
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void UpgradeProgressBar()
    {
        cur_colldown = cur_colldown + Time.deltaTime;
        if (cur_colldown >= max_colldown)//如果冷却时间到了
            m_turnState = ETurnState.CHOOSEACTION;
    }
}
