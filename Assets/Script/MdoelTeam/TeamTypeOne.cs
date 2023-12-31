using System.Collections.Generic;
using Core;

/// <summary>
/// 一支队伍
/// </summary>
public class TeamTypeOne : ITeam, IID
{
    private uint _teamID;
    private List<IRole> _teamList;
    private ETeamPoint _teamPoint;

    public uint ID { get => _teamID; set => _teamID = value; }
    public ETeamPoint TeamPoint { get => _teamPoint; set => _teamPoint = value; }


    public TeamTypeOne()
    {
        _teamList = new List<IRole>();
    }
    public TeamTypeOne(uint id, List<IRole> roles)
    {
        _teamID = id;
        if (roles == null)
        {
            _teamList = new List<IRole>(4);//只能有4个人
        }
        else
        {
            if (roles.Count > 4)
                Debug.Error("添加的队伍超过4人错误");
            else
                _teamList = roles;
        }
    }


    /// <summary>
    /// 添加队员
    /// </summary>
    /// <param name="role">角色</param>
    public void AddRole(IRole role)
    {
        if (_teamList.Count >= 4 || !_teamList.AddNotContainElement(role))
        {
            Debug.Error($"添加{role.Name}错误，当前只能有4个人");
            return;
        }
        if (role is IRoleBehaviour roleBehaviour)
            roleBehaviour.RoleInit();
    }

    /// <summary>
    /// 移除队员
    /// </summary>
    /// <param name="role">角色</param>
    public void RemoveRole(IRole role)
    {
        if (!_teamList.RemoveContainElement(role))
        {
            Debug.Error($"移除失败请检查,该队伍没有队员{role.Name}");
            return;
        }
        if (role is IRoleBehaviour roleBehaviour)
            roleBehaviour.Remove();
    }
}
