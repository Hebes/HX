using System.Collections.Generic;

public interface ISkillCarrier
{
    /// <summary>
    /// 归属那个角色
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// 技能列表
    /// </summary>
    public List<ISkill> skillList { get; }
}
