using System.Collections.Generic;
using Core;

/// <summary>
/// 一支队伍
/// </summary>
public class TeamTypeOne : ITeamActual
{
    private uint _teamID;
    private List<IRoleActual> _teamList;
    private ETeamPoint _teamPoint;
    private ETeamType _teamType;
    private bool _enterBattle = false;


    public uint ID { get => _teamID; set => _teamID = value; }
    public ETeamPoint TeamPoint { get => _teamPoint; set => _teamPoint = value; }
    public List<IRoleActual> RoleList { get => _teamList; set => _teamList = value; }
    public ETeamType TeamType { get => _teamType; set => _teamType = value; }
    public bool EnterBattle { get => _enterBattle; set => _enterBattle = value; }

    public TeamTypeOne()
    {
        _teamList = new List<IRoleActual>();
    }

    public void TeamUpdata()
    {
        foreach (IRoleActual item in _teamList)
        {
            if (_enterBattle)
                item.RoleBattleUpdata();
            else
                item.RoleUpdata();
        }
    }


    public void AddRole(IRoleActual role) => ITeamCarrier.AddRole(this, role);
    public bool ChackTeamSurvival() => ITeamCarrier.ChackTeamSurvival(this);
    public void RemoveRole(IRoleActual role) => ITeamCarrier.RemoveRole(this, role);
}
