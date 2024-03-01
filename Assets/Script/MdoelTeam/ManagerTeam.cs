//using System.Collections;
//using System.Collections.Generic;

///// <summary>
///// 队伍管理器
///// </summary>
//public class ManagerTeam : IModel
//{
//    public static ManagerTeam Instance;
//    private Dictionary<long, ITeam> _teamDic;
//    private long _teamId;

//    public IEnumerator Enter()
//    {
//        Instance = this;
//        _teamDic = new Dictionary<long, ITeam>();
//        yield return null;
//    }

//    public IEnumerator Exit()
//    {
//        yield return null;
//    }

//    /// <summary>
//    /// 生成队伍ID
//    /// </summary>
//    public static long GenerateTeamID()
//    {
//        return Instance._teamId++;
//    }
//}
