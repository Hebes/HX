﻿/// <summary>
/// 战斗动作
/// </summary>
//[Serializable]
public class BattleAction : IBattleAction
{
    //TODO 需要用到对象池,防止再次NEW
    private IRoleActual _ownRoleData;
    private IRoleActual _targetRoleData;
    private IAttack _attack;

    public IRoleActual AttackerData { get => _ownRoleData; set => _ownRoleData = value; }
    public IRoleActual TargetData { get => _targetRoleData; set => _targetRoleData = value; }
    public IAttack Attack { get => _attack; set => _attack = value; }
}
