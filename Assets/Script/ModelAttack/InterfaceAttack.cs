/// <summary>
/// 攻击方式
/// </summary>
public interface IAttack
{
    /// <summary>
    /// 攻击方式->技能
    /// </summary>
    ISkill Skill { get; set; }
}
