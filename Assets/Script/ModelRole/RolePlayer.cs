using System;
using System.Collections.Generic;

public class RolePlayer : IRole, ISkillCarrier, IRoleBehaviour, IBuffCarrier
{
    private ERoleType _roleType = ERoleType.Player;
    private ERoleBattlePoint _roleBattlePoint = ERoleBattlePoint.Right;
    private uint _id;
    private string _name;
    private Dictionary<ESkillType, List<ISkill>> _skillDataDic;
    private List<IBuffData> _buffList;

    public ERoleType roleType { get => _roleType; set => _roleType = value; }
    public ERoleBattlePoint roleBattlePoint { get => _roleBattlePoint; set => _roleBattlePoint = value; }
    public uint ID { get => _id; set => _id = value; }
    public string Name { get => _name; set => _name = value; }
    public Dictionary<ESkillType, List<ISkill>> SkillDataDic { get => _skillDataDic; set => _skillDataDic = value; }
    public List<IBuffData> BuffList { get => _buffList; set => _buffList = value; }



    public void Remove()
    {
    }

    public void RoleInit()
    {

    }
}
