using System;
using System.Collections.Generic;

/// <summary>
/// 普通攻击
/// </summary>
public class SkillNormalAttack : ISkill, IBuffCarrier
{
    private ESkillType _skillType;

    public ESkillType SkillType { get => _skillType; set => _skillType = value; }
    public uint ID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Des { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public List<IBuffData> BuffList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}
