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
    public Dictionary<ESkillType, ISkillCarrierList> SkillDataDic { get; set; }

    /// <summary>
    /// 添加技能
    /// </summary>
    public static void AddSkill(ISkillCarrier skillCarrier, ISkill skill)
    {
        if (skillCarrier.SkillDataDic == null)
            skillCarrier.SkillDataDic = new Dictionary<ESkillType, ISkillCarrierList>();

        if (!skillCarrier.SkillDataDic.ContainsKey(skill.SkillType))
            skillCarrier.SkillDataDic.Add(skill.SkillType, null);
        ISkillCarrierList.AddSkill(skillCarrier.SkillDataDic[skill.SkillType], skill);
    }

    /// <summary>
    /// 移除技能
    /// </summary>
    public static void RemoveSkill(ISkillCarrier skillCarrier, ISkill skill)
    {
        if (skillCarrier.SkillDataDic.ContainsKey(skill.SkillType))
            ISkillCarrierList.RemoveSkill(skillCarrier.SkillDataDic[skill.SkillType], skill);
        Debug.Log("技能不存在");
    }

    /// <summary>
    /// 检查技能是否存在
    /// </summary>
    public bool ChackHoldSkill(ISkillCarrier skillCarrier, ISkill skill)
    {
        if (skillCarrier.SkillDataDic.ContainsKey(skill.SkillType))
            return ISkillCarrierList.ChackHoldSkill(skillCarrier.SkillDataDic[skill.SkillType], skill);
        Debug.Error("当前技能不存在");
        return false;
    }
}

/// <summary>
/// 技能数据
/// </summary>
public interface ISkill : IID, IName, IDescribe
{
    public ESkillType SkillType { get; set; }
}

/// <summary>
/// 技能中持有技能
/// </summary>
public interface ISkillCarrierList
{
    public List<ISkill> SkillList { get; set; }

    /// <summary>
    /// 添加技能
    /// </summary>
    public static void AddSkill(ISkillCarrierList skillCarrierList, ISkill skill)
    {
        if (skillCarrierList.SkillList == null)
            skillCarrierList.SkillList = new List<ISkill>();

        if (skillCarrierList.SkillList.Contains(skill))
        {
            //Debug.Error("技能已存在，不添加（不包括以后会有重复获取技能增加熟练度操作）");
            Debug.Log($"{skill.Name}技能已存在,跳过添加，暂时没写熟练度机制");
            return;
        }
        skillCarrierList.SkillList.Add(skill);
    }

    /// <summary>
    /// 删除已经存在的技能
    /// </summary>
    /// <param name="skillCarrierList"></param>
    /// <param name="skill"></param>
    public static void RemoveSkill(ISkillCarrierList skillCarrierList, ISkill skill)
    {
        skillCarrierList.SkillList.Remove(skill);
    }

    /// <summary>
    /// 检查技能是否存在
    /// </summary>
    public static bool ChackHoldSkill(ISkillCarrierList skillCarrierList, ISkill skill)
    {
        return skillCarrierList.SkillList.Contains(skill);
    }
}