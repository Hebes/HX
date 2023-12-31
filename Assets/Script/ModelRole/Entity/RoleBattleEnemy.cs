using Core;
using System.Collections.Generic;

/// <summary>
/// 具体战斗敌人实例
/// </summary>
public class RoleBattleEnemy : IRole, IRoleBehaviour, ISkillCarrier, IAttributes
{
    private uint _id;
    private string _name;
    private ERoleType _roleType;
    private ERoleBattlePoint _roleBattlePoint;
    private ETurnState _turnState;
    private float _max_colldown;         //最大的冷却时间
    private Dictionary<ESkillType, List<ISkill>> _skillDataDic;
    private int _maxHP;
    private int _currentHP;


    public void RoleBattleEnemyInit(uint id, string name, ERoleType roleType, ERoleBattlePoint roleBattlePoint, float max_colldown)
    {
        _id = id;
        _name = name;
        _roleType = roleType;
        _roleBattlePoint = roleBattlePoint;
        this._max_colldown = max_colldown;
    }

    public ERoleType RoleType { get => _roleType; set => _roleType = value; }
    public ERoleBattlePoint RoleBattlePoint { get => _roleBattlePoint; set => _roleBattlePoint = value; }
    public ETurnState TurnState { get => _turnState; set => _turnState = value; }
    public float Max_colldown { get => _max_colldown; set => _max_colldown = value; }
    public string Name { get => _name; set => _name = value; }
    public uint ID { get => _id; set => _id = value; }
    public Dictionary<ESkillType, List<ISkill>> SkillDataDic { get => _skillDataDic; set => _skillDataDic = value; }
    public int MaxHP { get => _maxHP; set => _maxHP = value; }
    public int CurrentHP { get => _currentHP; set => _currentHP = value; }

    private int cur_colldown;           //当前的冷却时间


    public void RoleRemove()
    {

    }

    public void RoleInit()
    {
        _turnState = ETurnState.PROCESSING;
    }

    public void RoleUpdata()
    {
        switch (_turnState)
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
                Debug.Error("敌人行动错误");
                break;
        }
    }

    /// <summary>
    /// 进度条上升
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void UpgradeProgressBar()
    {
        //cur_colldown = cur_colldown + Time.deltaTime;
        //if (cur_colldown >= _max_colldown)//如果冷却时间到了
        //    _turnState = ETurnState.CHOOSEACTION;
    }
}
