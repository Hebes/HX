using Core;
using System.Collections.Generic;

/// <summary>
/// 基础战斗接口
/// </summary>
public interface IBattle : IID
{
    /// <summary>
    /// 一场战斗的类型
    /// </summary>
    public BattleType battleType { get; set; }
}

/// <summary>
/// 战斗生命周期接口
/// </summary>
public interface IBattleBehaviour
{
    /// <summary>
    /// 每场战斗的初始化
    /// </summary>
    public void BattleInit();

    /// <summary>
    /// 每场战斗的更新
    /// </summary>
    public void BattleUpdata();

    /// <summary>
    /// 移除一场战斗后需要做哪些事
    /// </summary>
    public void BattleRemove();
}

/// <summary>
/// 战斗队伍的持有者接口
/// </summary>
public interface IBattleCarrier : IID
{
    /// <summary>
    /// 战斗的队伍
    /// 1.可能是敌人在左边，进入二打一模式
    /// 2.可能是自己人右边，敌人3队进行二打一模式
    /// </summary>
    public Dictionary<ETeamPoint, ITeam> BattleTeamDic { get; set; }

    /// <summary>
    /// 添加战斗队伍
    /// </summary>
    public static void AddBattleTeam(IBattleCarrier battleCarrier, ITeam team)
    {
        if (battleCarrier.BattleTeamDic == null)
            battleCarrier.BattleTeamDic = new Dictionary<ETeamPoint, ITeam>();

        if (battleCarrier.BattleTeamDic.ContainsKey(team.TeamPoint))
        {
            Debug.Error($"当前队伍占位已存在{team.TeamPoint}");
            return;
        }
        battleCarrier.BattleTeamDic.Add(team.TeamPoint, team);
    }

    /// <summary>
    /// 删除战斗队伍
    /// </summary>
    /// <param name="battleCarrier"></param>
    /// <param name="team"></param>
    public static void RemoveBattleTeam(IBattleCarrier battleCarrier, ITeam team)
    {
        if (battleCarrier.BattleTeamDic.ContainsKey(team.TeamPoint))
            battleCarrier.BattleTeamDic.Remove(team.TeamPoint);
    }
}


/// <summary>
/// 个人战斗数据接口
/// </summary>
public interface IBattleAction
{
    /// <summary>
    /// 自己的数据
    /// </summary>
    public IRole ownData { get; set; }

    /// <summary>
    /// 目标的数据
    /// </summary>
    public IRole TargetData { get; set; }


    //public void Attack();
}

/// <summary>
/// 行动持有者接口
/// </summary>
public interface IBattleActionCarrier : IID
{
    /// <summary>
    /// 行动列表
    /// </summary>

    public List<IBattleAction> BattleActionList { get; set; }

    /// <summary>
    /// 添加战斗行动
    /// </summary>
    public static void AddBattleAction(IBattleActionCarrier battleActionCarrier, IBattleAction battleAction)
    {
        battleActionCarrier.BattleActionList.Add(battleAction);
    }

    /// <summary>
    /// 移除战斗行动
    /// </summary>
    public static void RemoveBattleAction(IBattleActionCarrier battleActionCarrier, IBattleAction battleAction)
    {
        battleActionCarrier.BattleActionList.Remove(battleAction);
    }
}


/// <summary>
/// 战斗等待接口
/// </summary>
public interface IBattleWait
{
    public void Wait();
}

