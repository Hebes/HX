using Core;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI技能面板
/// </summary>
public class UISkill : UIBase, IUIAwake
{
    private GameObject skillBtn;
    private Transform content;

    public void UIAwake()
    {
        InitUIBase(EUIType.Fixed, EUIMode.HideOther, EUILucenyType.ImPenetrable);
        UIComponent UIComponent = GetComponent<UIComponent>();
        GameObject T_Content = UIComponent.Get<GameObject>("T_Content");
        GameObject T_SkillBtn = UIComponent.Get<GameObject>("T_SkillBtn");
        skillBtn = T_SkillBtn;
        content = T_Content.transform;
    }

    /// <summary>
    /// 设置技能
    /// </summary>
    public void SetSkills(List<ISkill> skillList)
    {
        foreach (ISkill item in skillList)
        {
            SkillBtn skillBtnTemp = CorePool.GetMono<SkillBtn>(skillBtn);
            skillBtnTemp.transform.SetParent(content, false);
            skillBtnTemp.SetSkillBtnData(item.Name);
        }
    }
}
