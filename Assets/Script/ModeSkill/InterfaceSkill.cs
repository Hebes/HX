using Core;
using System.Collections.Generic;

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
public interface ISkillCarrier : IID
{
    /// <summary>
    /// 持有技能
    /// </summary>
    public Dictionary<ESkillType, List<ISkill>> SkillDataDic { get; set; }

    /// <summary>
    /// 添加技能
    /// </summary>
    public static void AddSkill(IRole role, ISkill skillData)
    {
        if (role is ISkillCarrier skillCarrier)
        {
            if (skillCarrier.SkillDataDic.ContainsKey(skillData.SkillType))
            {
                if (skillCarrier.SkillDataDic[skillData.SkillType].Contains(skillData))
                    return;
                else
                    skillCarrier.SkillDataDic[skillData.SkillType].Add(skillData);
            }
            else
            {
                skillCarrier.SkillDataDic.Add(skillData.SkillType, new List<ISkill>() { skillData });
            }
        }
        else
        {
            Debug.Error($"{role.Name}没有继承ISkillCarrier接口，无法添加技能");
        }
    }

    /// <summary>
    /// 检查技能是否存在
    /// </summary>
    public void ChackHoldSkill(ISkillCarrier skillCarrier, ISkill skill)
    {

    }

}

/// <summary>
/// 技能数据
/// </summary>
public interface ISkill : IID, IName, IDescribe
{
    public ESkillType SkillType { get; set; }
}