using System.Collections.Generic;

/// <summary>
/// 队伍管理器
/// </summary>
public class ManagerTeam : IModelInit
{
    public static ManagerTeam Instance;
    private Dictionary<uint, ITeam> _teamDic;
    private uint _teamId;

    public void Init()
    {
        Instance = this;
        _teamDic = new Dictionary<uint, ITeam>();
    }

    /// <summary>
    /// 生成队伍ID
    /// </summary>
    public static uint GenerateTeamID()
    {
        return Instance._teamId++;
    }
}
