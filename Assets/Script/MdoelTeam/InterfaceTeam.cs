using System.Collections.Generic;
using Core;

public interface ITeam : IID
{
    /// <summary>
    /// 生成队伍ID
    /// </summary>
    public static void GenerateTeamID(List<IRole> roleList)
    {
        if (roleList.Count==0)
        {
            Debug.Error("当前队伍人数为空，请检查"); 
        }
        foreach (var item in roleList)
        {
            
        }
    }
}
