using Core;
using System.Collections.Generic;

/// <summary>
/// 普通攻击
/// </summary>
public class SkillNormalAttack : ISkill, ISkillBehaviour, IBuffCarrier
{
    private ESkillType _skillType = ESkillType.NormalAttack;
    private uint _id;
    private string _name;
    private string _description;
    private List<IBuffData> _buffList;

    public ESkillType SkillType { get => _skillType; set => _skillType = value; }
    public uint ID { get => _id; set => _id = value; }
    public string Name { get => _name; set => _name = value; }
    public string Des { get => _description; set => _description = value; }
    public List<IBuffData> BuffList { get => _buffList; set => _buffList = value; }

    public void Over()
    {
        Debug.Log("普攻技能结束");
    }

    public void Trigger()
    {
        BuffList = new List<IBuffData>();
        Debug.Log("普攻技能触发");
    }
}
