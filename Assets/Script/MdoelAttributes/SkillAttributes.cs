using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 技能属性
/// </summary>
public class SkillAttributes : AttributesData
{
    /// <summary>
    /// 技能数据
    /// </summary>
    public SkillData SkillData { get; set; }

    public virtual int Damage { get; }
}
