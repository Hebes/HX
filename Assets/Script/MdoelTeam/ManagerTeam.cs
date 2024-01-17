using System.Collections.Generic;

/// <summary>
/// 队伍管理器
/// </summary>
public class ManagerTeam : IModelInit
{
    public static ManagerTeam Instance;
    private Dictionary<long, ITeam> _teamDic;
    private long _teamId;

    public void Init()
    {
        Instance = this;
        _teamDic = new Dictionary<long, ITeam>();
    }

    /// <summary>
    /// 生成队伍ID
    /// </summary>
    public static long GenerateTeamID()
    {
        return Instance._teamId++;
    }
}
