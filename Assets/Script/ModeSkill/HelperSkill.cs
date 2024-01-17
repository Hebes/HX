using Core;
using System.Collections.Generic;

public interface ISkillBehaviour : IID
{
    /// <summary>
    /// 技能初始化
    /// </summary>
    void SkillInit();

    /// <summary>
    /// 表现效果
    /// </summary>
    void SkillTrigger();

    /// <summary>
    /// 技能结束
    /// </summary>
    void SkillOver();
}

/// <summary>
/// 技能持有者接口
/// </summary>
public interface ISkillCarrier : IID
{
    /// <summary>
    /// 持有技能
    /// </summary>
    public Dictionary<ESkillType, List<ISkill>> SkillDataDic { get; set; }
}

/// <summary>
/// 技能数据
/// </summary>
public interface ISkill : IID, IName, IDescribe, IATK
{
    public ESkillType SkillType { get; set; }
}

/// <summary>
/// 技能帮助类
/// </summary>
public static class HelperSkill
{
    #region 技能相关
    /// <summary>
    /// 添加技能
    /// </summary>
    /// <param name="skillCarrier"></param>
    /// <param name="skill"></param>
    public static bool AddSkill(this ISkillCarrier skillCarrier, ISkill skill)
    {
        if (skillCarrier.SkillDataDic == null)
            skillCarrier.SkillDataDic = new Dictionary<ESkillType, List<ISkill>>();

        if (skillCarrier.SkillDataDic.TryGetValue(skill.SkillType, out List<ISkill> skillList))
            return skillList.AddSkillOne(skill);
        skillCarrier.SkillDataDic.Add(skill.SkillType, new List<ISkill>() { skill });
        return true;
    }

    /// <summary>
    /// 移除技能
    /// </summary>
    public static bool RemoveSkill(this ISkillCarrier skillCarrier, ISkill skill)
    {
        if (skillCarrier.SkillDataDic.TryGetValue(skill.SkillType, out List<ISkill> skillList))
            return skillList.RemoveSkillOne(skill);
        return false;
    }

    /// <summary>
    /// 检查技能类型是否存在
    /// </summary>
    public static bool ChackSkillExist(ISkillCarrier skillCarrier, ISkill skill)
    {
        if (skillCarrier.SkillDataDic.TryGetValue(skill.SkillType, out List<ISkill> skillList))
            return skillList.ChackSkillOne(skill);
        return false;
    }

    /// <summary>
    /// 添加一个技能
    /// </summary>
    public static bool AddSkillOne(this List<ISkill> skillList, ISkill skill)
    {
        if (skillList == null)
            skillList = new List<ISkill>();

        if (skillList.Contains(skill))
        {
            Debug.Log($"{skill.Name}技能已存在,跳过添加，暂时没写熟练度机制");
            return false;
        }
        skillList.Add(skill);
        ISkillBehaviour skillBehaviour = skill.ChackInherit<ISkill, ISkillBehaviour>();
        skillBehaviour.SkillInit();
        return true;
    }

    /// <summary>
    /// 删除一个技能
    /// </summary>
    /// <param name="skillList"></param>
    /// <param name="skill"></param>
    /// <returns></returns>
    public static bool RemoveSkillOne(this List<ISkill> skillList, ISkill skill)
    {
        if (skillList == null)
            skillList = new List<ISkill>();

        if (skillList.Contains(skill))
        {
            skillList.Remove(skill);
            ISkillBehaviour skillBehaviour = skill.ChackInherit<ISkill, ISkillBehaviour>();
            skillBehaviour.SkillOver();
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检查技能是否存在
    /// </summary>
    /// <param name="skillList"></param>
    /// <param name="skill"></param>
    /// <returns></returns>
    public static bool ChackSkillOne(this List<ISkill> skillList, ISkill skill)
    {
        if (skillList == null)
            skillList = new List<ISkill>();
        return skillList.Contains(skill);
    }
    #endregion

}

