using System.Collections.Generic;
using Framework.Core;

/// <summary>
/// 技能持有者接口
/// </summary>
public interface ISkillCarrier : IID
{
    /// <summary>
    /// 持有技能
    /// </summary>
    public List<SkillData> SkillList { get; set; }
}