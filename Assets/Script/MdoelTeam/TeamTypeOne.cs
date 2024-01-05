﻿using System.Collections.Generic;
using Core;

/// <summary>
/// 一支队伍
/// </summary>
public class TeamTypeOne : ITeam, ITeamCarrier
{
    private uint _teamID;
    private List<IRole> _teamList;
    private ETeamPoint _teamPoint;
    private ETeamType _teamType;



    public uint ID { get => _teamID; set => _teamID = value; }
    public ETeamPoint TeamPoint { get => _teamPoint; set => _teamPoint = value; }
    public List<IRole> RoleList { get => _teamList; set => _teamList = value; }
    public ETeamType TeamType { get => _teamType; set => _teamType = value; }

    public TeamTypeOne()
    {
        _teamList = new List<IRole>();
    }

    public void TeamUpdata()
    {
        foreach (IRole item in _teamList)
        {
            if (item is IRoleBehaviour roleBehaviour)
                roleBehaviour.RoleUpdata();
        }
    }


    public void AddRole(IRole role) => ITeamCarrier.AddRole(this, role);
    public bool ChackTeamSurvival() => ITeamCarrier.ChackTeamSurvival(this);
    public void RemoveRole(IRole role) => ITeamCarrier.RemoveRole(this, role);
}
