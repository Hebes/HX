using System;
using System.Collections.Generic;

/// <summary>
/// 战斗的NPC类型
/// </summary>
public class RoleBattleNPC : IRole, IRoleBehaviour, ISkillCarrier, IAttributes, IRoleAttackCount
{
    private uint _id;
    private string _name;
    private ERoleBattlePoint _roleBattlePoint = ERoleBattlePoint.Point2;//玩家默认右边，以后会改位置//被偷袭可能会在左边
    private ETurnState _turnState;
    private ERoleType _roleType;
    private float _max_colldown;    //最大的冷却时间
    private Dictionary<ESkillType, List<ISkill>> _skillDataDic;
    private int _maxHP;
    private int _currentHP;
    private int _attackCount;


    public float Max_colldown { get => _max_colldown; set => _max_colldown = value; }
    public ERoleType RoleType { get => _roleType; set => _roleType = value; }
    public ERoleBattlePoint RoleBattlePoint { get => _roleBattlePoint; set => _roleBattlePoint = value; }
    public ETurnState TurnState { get => _turnState; set => _turnState = value; }
    public uint ID { get => _id; set => _id = value; }
    public string Name { get => _name; set => _name = value; }
    public Dictionary<ESkillType, List<ISkill>> SkillDataDic { get => _skillDataDic; set => _skillDataDic = value; }
    public int MaxHP { get => _maxHP; set => _maxHP = value; }
    public int CurrentHP { get => _currentHP; set => _currentHP = value; }
    public int AttackCount { get => _attackCount; set => _attackCount = value; }



    public void RoleInit()
    {
    }

    public void RoleRemove()
    {
    }

    public void RoleUpdata()
    {
    }
}
