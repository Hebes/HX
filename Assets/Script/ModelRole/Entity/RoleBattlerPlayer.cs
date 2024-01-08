//using UnityEngine;
//using Core;
//using System.Collections.Generic;

//public class RoleBattlerPlayer : IRoleActual, IRoleAttackCount, ISkillCarrier, IAttributes
//{
//    private uint _playerID;
//    private string _name;
//    private ERoleBattlePoint _roleBattlePoint = ERoleBattlePoint.Point1;//玩家默认右边，以后会改位置//被偷袭可能会在左边
//    private ETurnState _turnState;
//    private ERoleType _roleType = ERoleType.Player;
//    private float _max_colldown;    //最大的冷却时间
//    private Dictionary<ESkillType, List<ISkill>> _skillDataDic;
//    private int _maxHP;
//    private int _currentHP;
//    private int _attackCount;
//    private GameObject _go;
//    private IBattle _battle;    //战斗接口
//    private ITeam _team; //队伍
//    private int _maxATK;
//    private int _currentATK;

//    public uint ID { get => _playerID; set => _playerID = value; }
//    public string Name { get => _name; set => _name = value; }
//    public ERoleType RoleType { get => _roleType; set => _roleType = value; }
//    public ERoleBattlePoint RoleBattlePoint { get => _roleBattlePoint; set => _roleBattlePoint = value; }
//    public ETurnState TurnState { get => _turnState; set => _turnState = value; }
//    public Dictionary<ESkillType, List<ISkill>> SkillDataDic { get => _skillDataDic; set => _skillDataDic = value; }
//    public float Max_colldown { get => _max_colldown; set => _max_colldown = value; }
//    public int MaxHP { get => _maxHP; set => _maxHP = value; }
//    public int CurrentHP { get => _currentHP; set => _currentHP = value; }
//    public int AttackCount { get => _attackCount; set => _attackCount = value; }
//    public GameObject Go { get => _go; set => _go = value; }


//    /// <summary>
//    /// 当前的冷却时间
//    /// </summary>
//    private float _cur_colldown;

//    public RoleBattlerPlayer()
//    {
//    }

//    /// <summary>
//    /// 添加数据
//    /// </summary>
//    public void AddData(IBattle battle, ITeam team)
//    {
//        this._battle = battle;
//        this._team = team;
//    }


//    public void RoleInit()
//    {
//        _turnState = ETurnState.PROCESSING;
//    }

//    public void RoleUpdata()
//    {
//        switch (_turnState)
//        {
//            case ETurnState.PROCESSING:
//                UpgradeProgressBar();
//                break;
//            case ETurnState.CHOOSEACTION:
//                ChooseAction();
//                break;
//            case ETurnState.WAITING:
//                break;
//            case ETurnState.ACTION:
//                break;
//            case ETurnState.DEAD:
//                break;
//            default:
//                break;
//        }
//    }

//    public void RoleRemove()
//    {
//    }



//    private void ChooseAction()
//    {
//    }

//    // <summary>
//    /// 升级进度条  冷却版
//    /// </summary>
//    private void UpgradeProgressBar()
//    {
//        _cur_colldown = _cur_colldown + Time.deltaTime;
//        if (_cur_colldown >= _max_colldown)//如果冷却时间到了
//            _turnState = ETurnState.CHOOSEACTION;
//    }

//    public void TakeDamage(int getDamageAmount)
//    {
//    }
//}
