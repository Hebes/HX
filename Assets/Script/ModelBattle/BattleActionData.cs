using Core;
using System;

/// <summary>
/// 战斗动作
/// </summary>
[Serializable]
public class BattleActionData : IID
{
    //TODO 需要用到对象池,防止再次NEW
    public long ID { get; set; }
    public RoleData AttackerData { get; set; }
    public RoleData TargetData { get; set; }
    public SkillData AttackData { get; set; }
}
