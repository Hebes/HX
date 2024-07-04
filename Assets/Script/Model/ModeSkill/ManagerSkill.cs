using Framework.Core;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 技能
/// </summary>
public class ManagerSkill : IModel
{
    public static ManagerSkill Instance;

    public void Init()
    {
        Instance = this;
    }

    public IEnumerator AsyncEnter()
    {
        yield return null;
    }

    public IEnumerator Exit()
    {
        yield return null;
    }
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
    public static bool AddSkill(this ISkillCarrier skillCarrier, SkillData skill)
    {
        if (skillCarrier.SkillList == null)
            skillCarrier.SkillList = new List<SkillData>();
        bool isAdd = skillCarrier.SkillList.AddNotContainElement(skill);
        if (isAdd)
            skill.SkillInit();
        return isAdd;
    }

    /// <summary>
    /// 添加一个技能
    /// </summary>
    public static bool AddSkillOne(this List<SkillData> skillList, SkillData skill)
    {
        skillList ??= new List<SkillData>();

        if (skillList.Contains(skill))
        {
            EDebug.Log($"{skill.Name}技能已存在,跳过添加，暂时没写熟练度机制");
            return false;
        }

        skillList.Add(skill);

        return true;
    }

    public static SkillData GetSkill(this ISkillCarrier skillCarrier, ESkillType normalAttack, int skillLV)
    {
        skillCarrier.SkillList ??= new List<SkillData>();

        foreach (var item in skillCarrier.SkillList)
        {
            if (item.SkillType == normalAttack && item.SkillLV == skillLV)
                return item;
        }

        return default;
    }

    /// <summary>
    /// 移除技能
    /// </summary>
    //public static bool RemoveSkill(this ISkillCarrier skillCarrier, ISkill skill)
    //{
    //    if (skillCarrier.SkillDic.TryGetValue(skill.SkillType, out List<ISkill> skillList))
    //        return skillList.RemoveSkillOne(skill);
    //    return false;
    //}

    /// <summary>
    /// 检查技能类型是否存在
    /// </summary>
    //public static bool ChackSkillExist(ISkillCarrier skillCarrier, ISkill skill)
    //{
    //    if (skillCarrier.SkillDic.TryGetValue(skill.SkillType, out List<ISkill> skillList))
    //        return skillList.ChackSkillOne(skill);
    //    return false;
    //}


    /// <summary>
    /// 删除一个技能
    /// </summary>
    /// <param name="skillList"></param>
    /// <param name="skill"></param>
    /// <returns></returns>
    //public static bool RemoveSkillOne(this List<ISkill> skillList, ISkill skill)
    //{
    //    if (skillList == null)
    //        skillList = new List<ISkill>();

    //    if (skillList.Contains(skill))
    //    {
    //        skillList.Remove(skill);
    //        ISkillBehaviour skillBehaviour = skill.ChackInherit<ISkill, ISkillBehaviour>();
    //        skillBehaviour.SkillOver();
    //        return true;
    //    }
    //    return false;
    //}

    /// <summary>
    /// 检查技能是否存在
    /// </summary>
    /// <param name="skillList"></param>
    /// <param name="skill"></param>
    /// <returns></returns>
    //public static bool ChackSkillOne(this List<ISkill> skillList, ISkill skill)
    //{
    //    if (skillList == null)
    //        skillList = new List<ISkill>();
    //    return skillList.Contains(skill);
    //}

    #endregion
}