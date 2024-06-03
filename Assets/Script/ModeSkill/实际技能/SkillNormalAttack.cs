using Core;

/// <summary>
/// 普通攻击
/// </summary>
public class SkillNormalAttack : SkillData
{
    public SkillNormalAttack(RoleData roleData)
    {
        SkillType = ESkillType.NormalAttack;
        RoleData = roleData;
        AttributeData = new NormalAttackAttributes(this);
        Damage = AttributeData.Damage;
    }

    public override void SkillTrigger()
    {
        base.SkillTrigger();
        ExtensionDebug.Log("普攻技能触发");
    }
    public override void SkillOver()
    {
        base.SkillOver();
        ExtensionDebug.Log("普攻技能结束");
    }
}

/// <summary>
/// 普通攻击属性
/// </summary>
public class NormalAttackAttributes : SkillAttributes
{

    public NormalAttackAttributes(SkillNormalAttack skillData)
    {
        SkillData = skillData;
    }

    public override int Damage { get => SkillData.RoleData.RoleAttributes.CurrentATK;}
}
