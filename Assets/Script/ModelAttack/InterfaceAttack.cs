/// <summary>
/// 攻击方式
/// </summary>
public interface IAttackPattern
{
    /// <summary>
    /// 攻击方式->技能
    /// </summary>
    ISkill Skill { get; set; }
}
