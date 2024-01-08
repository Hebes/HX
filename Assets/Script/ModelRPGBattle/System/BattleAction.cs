/// <summary>
/// 战斗动作
/// </summary>
//[Serializable]
public class BattleAction : IBattleAction
{
    //TODO 需要用到对象池,防止再次NEW
    private IRole _ownRoleData;
    private IRole _targetRoleData;
    private IAttack _attack;

    public IRole OwnData { get => _ownRoleData; set => _ownRoleData = value; }
    public IRole TargetData { get => _targetRoleData; set => _targetRoleData = value; }
    public IAttack Attack { get => _attack; set => _attack = value; }
}
