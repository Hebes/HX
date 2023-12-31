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
public interface ISkillCarrier : IID, IName
{
    /// <summary>
    /// 持有技能
    /// </summary>
    public Dictionary<ESkillType, List<ISkill>> SkillDataDic { get; set; }

    /// <summary>
    /// 添加技能
    /// </summary>
    public static void AddSkill(ISkillCarrier skillCarrier, ISkill skill)
    {
        if (skillCarrier.SkillDataDic == null)
            skillCarrier.SkillDataDic = new Dictionary<ESkillType, List<ISkill>>();

        if (!ChackSkillType(skillCarrier, skill))
            skillCarrier.SkillDataDic.Add(skill.SkillType, new List<ISkill>());
        ISkillCarrier.AddSkill(skillCarrier.SkillDataDic[skill.SkillType], skill);
    }

    /// <summary>
    /// 移除技能
    /// </summary>
    public static void RemoveSkill(ISkillCarrier skillCarrier, ISkill skill)
    {
        if (ChackSkillType(skillCarrier, skill))
            ISkillCarrier.RemoveSkill(skillCarrier.SkillDataDic[skill.SkillType], skill);
        Debug.Log("技能不存在");
    }

    /// <summary>
    /// 检查技能是否存在
    /// </summary>
    public bool ChackHoldSkill(ISkillCarrier skillCarrier, ISkill skill)
    {
        if (ChackSkillType(skillCarrier, skill))
            return ISkillCarrier.ChackHoldSkill(skillCarrier.SkillDataDic[skill.SkillType], skill);
        Debug.Error("当前技能不存在");
        return false;
    }

    /// <summary>
    /// 检查技能类型是否存在
    /// </summary>
    public static bool ChackSkillType(ISkillCarrier skillCarrier, ISkill skill)
    {
        return skillCarrier.SkillDataDic.ContainsKey(skill.SkillType);
    }


    /// <summary>
    /// 添加技能
    /// </summary>
    public static void AddSkill(List<ISkill> skillList, ISkill skill)
    {
        if (skillList == null)
            skillList = new List<ISkill>();

        if (ChackHoldSkill(skillList, skill))
        {
            //Debug.Error("技能已存在，不添加（不包括以后会有重复获取技能增加熟练度操作）");
            Debug.Log($"{skill.Name}技能已存在,跳过添加，暂时没写熟练度机制");
            return;
        }
        skillList.Add(skill);
        if (skill is ISkillBehaviour skillBehaviour)
            skillBehaviour.SkillInit();
    }

    /// <summary>
    /// 删除已经存在的技能
    /// </summary>
    /// <param name="skillCarrierList"></param>
    /// <param name="skill"></param>
    public static void RemoveSkill(List<ISkill> skillList, ISkill skill)
    {
        skillList.Remove(skill);
    }

    /// <summary>
    /// 检查技能是否存在
    /// </summary>
    public static bool ChackHoldSkill(List<ISkill> skillList, ISkill skill)
    {
        return skillList.Contains(skill);
    }
}

/// <summary>
/// 技能数据
/// </summary>
public interface ISkill : IID, IName, IDescribe
{
    public ESkillType SkillType { get; set; }
}

public static  class InterfaceSkill
{
    public static void AddSkill(this ISkillCarrier ISkillCarrier, ISkill skill) => ISkillCarrier.AddSkill(ISkillCarrier, skill);
}

