using System.Collections.Generic;
using Core;


/// <summary>
/// 队伍的实际接口
/// </summary>
public interface ITeamInstance : ITeam, ITeamBehaviour
{

}

/// <summary>
/// 一支队伍
/// </summary>
public class Team : ITeamInstance
{
    private long _teamID;
    private List<IRoleInstance> _teamList;
    private ETeamPoint _teamPoint;
    private ERoleOrTeamType _teamType;
    private bool _isEnterBattle = false;


    public long ID { get => _teamID; set => _teamID = value; }
    public ETeamPoint TeamPoint { get => _teamPoint; set => _teamPoint = value; }
    public List<IRoleInstance> RoleList { get => _teamList; set => _teamList = value; }
    public ERoleOrTeamType TeamType { get => _teamType; set => _teamType = value; }
    public bool IsEnterBattle { get => _isEnterBattle; set => _isEnterBattle = value; }

    public void TeamUpdata()
    {
        foreach (IRoleInstance item in _teamList)
        {
            if (item.RoleState is IRoleState roleBehaviour)
                roleBehaviour.RoleUpdata();
        }
    }
}
