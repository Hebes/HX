/// <summary>
/// 战斗动作
/// </summary>
public class BattleAction : IBattleAction
{
    private IRole _ownRoleData;
    private IRole _targetRoleData;

    public IRole ownData { get => _ownRoleData; set => _ownRoleData = value; }
    public IRole TargetData { get => _targetRoleData; set => _targetRoleData = value; }
}
