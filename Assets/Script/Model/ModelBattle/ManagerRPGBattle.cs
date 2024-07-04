using Framework.Core;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using ExpansionUnity;
using UnityEngine;

public class ManagerRPGBattle : IModel, IUpdata
{
    public static ManagerRPGBattle Instance;

    /// <summary>
    /// 所有的战斗列表->int 是NPC的ID合并起来
    /// </summary>
    private Dictionary<long, BattleData> _battleDic;

    public void Init()
    {
        Instance = this;
        _battleDic = new Dictionary<long, BattleData>();
        CoreBehaviour.Add(this);
    }

    public  IEnumerator AsyncEnter()
    {
       
        yield return null;
    }

    public  IEnumerator Exit()
    {
        yield return null;
    }

    public void CoreBehaviourUpdata()
    {
        foreach (var item in _battleDic.Values)
            item.BattleUpdata();
    }
}

/// <summary>
/// 战斗帮助类
/// </summary>
public static class HelperBattle
{
    #region 战斗行动操作
    /// <summary>
    /// 添加战斗行动
    /// </summary>
    /// <param name="battleActionCarrier"></param>
    /// <param name="battleAction"></param>
    /// <returns></returns>
    public static bool AddBattleAction(this BattleData battle, BattleActionData battleAction)
    {
        battle.BattleActionList ??= new List<BattleActionData>();
        return battle.BattleActionList.AddNotContainElement(battleAction);
    }
    /// <summary>
    /// 移除战斗行动
    /// </summary>
    /// <param name="battleActionCarrier"></param>
    /// <param name="battleAction"></param>
    /// <returns></returns>
    public static bool RemoveBattleAction(this BattleData battle, BattleActionData battleAction)
    {
        return battle.BattleActionList.RemoveContainElement(battleAction);
    }
    /// <summary>
    /// 检查战斗是否存在
    /// </summary>
    /// <param name="battleActionCarrier"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    public static bool ChackBattleActionExist(this BattleData battle, RoleData role, out BattleActionData battleAction)
    {
        battleAction = default;
        foreach (var item in battle.BattleActionList)
        {
            if (item.AttackerData != role) continue;
            battleAction = item;
            return true;
        }
        return false;
    }
    /// <summary>
    /// 是否包含战斗数据
    /// </summary>
    /// <param name="battleActionCarrier"></param>
    /// <param name="battleAction"></param>
    /// <returns></returns>
    public static bool ChackBattleActionExist(this BattleData v1, BattleActionData v2)
    {
        return v1.BattleActionList.Contains(v2);
    }
    #endregion


    #region 角色操作
    /// <summary>
    /// 确认角色是否存活
    /// </summary>
    /// <param name="battleCarrier"></param>
    /// <param name="roleActual"></param>
    /// <returns></returns>
    public static bool ChackRoleSurvival(this BattleData battleCarrier, RoleData roleActual)
    {
        foreach (KeyValuePair<ETeamPoint, TeamData> item in battleCarrier.BattleTeamDic)
        {
            if (item.Value.RoleList.Contains(roleActual))
                return true;
        }
        return false;
    }
    /// <summary>
    /// 随机敌方一个角色
    /// </summary>
    /// <param name="battleCarrier"></param>
    /// <param name="teamType"></param>
    /// <returns></returns>
    public static RoleData RandomEnemyRole(this BattleData battleCarrier, ERoleOrTeamType teamType)
    {
        ExpansionProfiler.ProfilerBeginSample("随机一个敌人消耗时间");
        //获取所有角色,收集敌人
        List<RoleData> role = new List<RoleData>();
        foreach (TeamData item in battleCarrier.BattleTeamDic.Values)
        {
            switch (teamType)//如果输入的是enemy
            {
                case ERoleOrTeamType.Player:
                case ERoleOrTeamType.NPC:
                    if (item.TeamType == ERoleOrTeamType.Enemy)
                        role.AddRange(item.RoleList);
                    break;
                case ERoleOrTeamType.Enemy:
                    if (item.TeamType == ERoleOrTeamType.NPC || 
                        item.TeamType == ERoleOrTeamType.Player)//当前队伍类型是NPC或者玩家都添加
                        role.AddRange(item.RoleList);
                    break;
            }
        }
        int number = Random.Range(0, role.Count);
        ExpansionProfiler.ProfilerEndSample();
        return role[number];
    }
    /// <summary>
    /// 随机自己方一个角色
    /// </summary>
    /// <param name="battleCarrier"></param>
    /// <param name="teamType"></param>
    /// <returns></returns>
    public static RoleData RandomOwnRole(this BattleData battleCarrier, ERoleOrTeamType teamType)
    {
        ExpansionProfiler.ProfilerBeginSample("随机自己方一个人");
        List<RoleData> role = new List<RoleData>();
        foreach (TeamData item in battleCarrier.BattleTeamDic.Values)
        {
            if (teamType == item.TeamType)//如果是NPC
                role.AddRange(item.RoleList);
            switch (teamType)
            {
                case ERoleOrTeamType.Player:
                    if (item.TeamType == ERoleOrTeamType.NPC)//判断当前队伍是否是NPC，NPC也添加
                        role.AddRange(item.RoleList);
                    break;
                case ERoleOrTeamType.NPC:
                    if (item.TeamType == ERoleOrTeamType.Player)//判断当前队伍是否是玩家，玩家也添加
                        role.AddRange(item.RoleList);
                    break;
                case ERoleOrTeamType.Enemy:
                    break;
            }
        }
        int number = Random.Range(0, role.Count);
        ExpansionProfiler.ProfilerEndSample();
        return role[number];
    }
    #endregion


    #region 队伍操作
    /// <summary>
    /// 添加战斗队伍
    /// </summary>
    public static void AddBattleTeam(BattleData battle, TeamData team)
    {
        if (battle.BattleTeamDic == null)
            battle.BattleTeamDic = new Dictionary<ETeamPoint, TeamData>();

        if (battle.BattleTeamDic.ContainsKey(team.TeamPoint))
            EDebug.Error($"当前队伍占位已存在{team.TeamPoint}");
        else
            battle.BattleTeamDic.Add(team.TeamPoint, team);
    }
    /// <summary>
    /// 检查队伍是否存活
    /// </summary>
    /// <param name="battleCarrier"></param>
    /// <param name="teamArray"></param>
    /// <returns></returns>
    public static bool ChackTeamSurvival(this BattleData battle, params ETeamPoint[] teamPointArray)
    {
        foreach (TeamData item in battle.BattleTeamDic.Values)
        {
            foreach (ETeamPoint item1 in teamPointArray)
            {
                if (item.TeamPoint != item1) continue;
                if (item.ChackTeamRoleSurvival())
                    return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 删除战斗队伍
    /// </summary>
    /// <param name="battleCarrier"></param>
    /// <param name="team"></param>
    //public static void RemoveBattleTeam(IBattle battleCarrier, ITeamInstance team)
    //{
    //    if (battleCarrier.BattleTeamDic.ContainsKey(team.TeamPoint))
    //        battleCarrier.BattleTeamDic.Remove(team.TeamPoint);
    //}
    #endregion
}