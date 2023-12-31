using Core;
using System.Collections.Generic;
using System.Data;

public interface ISkillBehaviour
{
    /// <summary>
    /// 表现效果
    /// </summary>
    void Trigger();

    /// <summary>
    /// 技能结束
    /// </summary>
    void Over();
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
        if (skillCarrier.SkillDataDic.ContainsKey(skill.SkillType))
        {
            if (!skillCarrier.SkillDataDic[skill.SkillType].Contains(skill))
                skillCarrier.SkillDataDic[skill.SkillType].Add(skill);
            else
                Debug.Log($"{skillCarrier.Name}技能已存在,跳过添加，暂时没写熟练度机制");
        }
        else
        {
            skillCarrier.SkillDataDic.Add(skill.SkillType, new List<ISkill>() { skill });
        }
    }

    /// <summary>
    /// 检查技能是否存在
    /// </summary>
    public bool ChackHoldSkill(ISkillCarrier skillCarrier, ISkill skill)
    {
        if (skillCarrier.SkillDataDic.ContainsKey(skill.SkillType))
        {
            if (skillCarrier.SkillDataDic[skill.SkillType].Contains(skill))
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }

}

/// <summary>
/// 技能数据
/// </summary>
public interface ISkill : IID, IName, IDescribe
{
    public ESkillType SkillType { get; set; }
}