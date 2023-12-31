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
    private List<IBuff> _buffList;

    public ESkillType SkillType { get => _skillType; set => _skillType = value; }
    public uint ID { get => _id; set => _id = value; }
    public string Name { get => _name; set => _name = value; }
    public string Des { get => _description; set => _description = value; }
    public List<IBuff> BuffList { get => _buffList; set => _buffList = value; }

    public void SkillInit()
    {
        _buffList = new List<IBuff>();
    }
    public void SkillTrigger()
    {
        Debug.Log("普攻技能触发");
    }
    public void SkillOver()
    {
        Debug.Log("普攻技能结束");
    }
}
