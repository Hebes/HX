using Core;
using System;
using System.Collections.Generic;

/// <summary>
/// 技能数据
/// </summary>
public class SkillData : IID, IName, IDescribe, IBuffList
{

    /// <summary>
    /// 技能的持有者
    /// </summary>
    public RoleData RoleData { get; set; }
    public SkillAttributes AttributeData { get; set; }
    /// <summary>
    /// 技能类型
    /// </summary>
    public ESkillType SkillType { get; set; }
    /// <summary>
    /// 技能范围->等级->大中小
    /// </summary>
    public int SkillLV { get; set; }
    public long ID { get; set; }
    public string Name { get; set; }
    public string Des { get; set; }
    public List<BuffData> BuffList { get; set; }
    public float Damage { get; set; }

    /// <summary>
    /// 技能初始化
    /// </summary>
    public virtual void SkillInit()
    {
        BuffList = new List<BuffData>();
    }
    /// <summary>
    /// 表现效果
    /// </summary>
    public virtual void SkillTrigger()
    {
    }
    /// <summary>
    /// 技能结束
    /// </summary>
    public virtual void SkillOver()
    {
    }
}
