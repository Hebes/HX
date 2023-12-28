using System.Collections.Generic;
using Core;

/// <summary>
/// 一支队伍
/// </summary>
public class TeamTypeOne : ITeam
{
    /// <summary>
    /// 队伍ID
    /// </summary>
    private uint _id;
    private List<IRole> _teamList;

    public TeamTypeOne(uint id)
    {
        _id = id;
        _teamList = new List<IRole>(4);//只能有4个人
    }

    /// <summary>
    /// 队伍ID
    /// </summary>
    public uint ID { get => _id; set => _id = value; }



    /// <summary>
    /// 添加队员
    /// </summary>
    /// <param name="role">角色</param>
    public void AddRole(IRole role)
    {
        if (_teamList.Count >= 4 || !_teamList.AddNotContainElement(role))
            Debug.Error("添加错误，当前只能有4个人");
    }

    /// <summary>
    /// 移除队员
    /// </summary>
    /// <param name="role">角色</param>
    public void RemoveRole(IRole role)
    {
        _teamList.RemoveContainElement(role);
    }
}
