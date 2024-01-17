using System;

/// <summary>
/// 战斗动作
/// </summary>
[Serializable]
public class BattleAction : IBattleAction
{
    //TODO 需要用到对象池,防止再次NEW
    private IRoleInstance ownRoleData;
    private IRoleInstance targetRoleData;
    private IAttackPattern _attack;

    public IRoleInstance AttackerData { get => ownRoleData; set => ownRoleData = value; }
    public IRoleInstance TargetData { get => targetRoleData; set => targetRoleData = value; }
    public IAttackPattern AttackPattern { get => _attack; set => _attack = value; }
}
