﻿using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ManagerSkill : IModel
{
    public static ManagerSkill Instance { get; private set; }

    public IEnumerator Enter()
    {
        Instance = this;
        yield return null;
    }

    public IEnumerator Exit()
    {
        yield return null;
    }

    /// <summary>
    /// 发动技能
    /// </summary>
    public static void TriggerSkill(ISkillCarrier skillCarrier, ISkill skillData)
    {
        //if (skillCarrier.skillList.Contains(skillData) && skillData is ISkillBehaviour skillBehaviour)
        //{
        //    skillBehaviour.Trigger();
        //    CoreBehaviour.Add(skillBehaviour);
        //}
    }

    /// <summary>
    /// 技能结束
    /// </summary>
    public static void OverSkill(ISkill skillData)
    {
        //if (skillData is ISkillBehaviour skillBehaviour)
        //{
        //    skillBehaviour.Over();
        //    CoreBehaviour.Remove(skillBehaviour);
        //}
    }

    
}
