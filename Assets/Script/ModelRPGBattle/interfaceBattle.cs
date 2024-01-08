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

    /// <summary>
    ///一场战斗的状态
    /// </summary>
    public EBattlePerformAction BattleSate { get; set; }
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

    /// <summary>
    /// 随机一个敌人(NPC和Player敌人就是Enemy，相反就是...)
    /// </summary>
    public static void RandomEnemyRole(IBattleCarrier battleCarrier, ETeamType teamType)
    {
        foreach (ITeam item in battleCarrier.BattleTeamDic.Values)
        {
            
        }
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
    public IRole OwnData { get; set; }

    /// <summary>
    /// 目标的数据
    /// </summary>
    public IRole TargetData { get; set; }

    /// <summary>
    /// 攻击方式
    /// </summary>
    public IAttack Attack { get; set; }
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
        if (battleActionCarrier.BattleActionList == null)
            battleActionCarrier.BattleActionList = new List<IBattleAction>();
        battleActionCarrier.BattleActionList.Add(battleAction);
    }

    /// <summary>
    /// 移除战斗行动
    /// </summary>
    public static void RemoveBattleAction(IBattleActionCarrier battleActionCarrier, IBattleAction battleAction)
    {
        battleActionCarrier.BattleActionList.Remove(battleAction);
    }

    /// <summary>
    /// 切换战斗是否存在
    /// </summary>
    public static bool ChackBattleExist(IBattleActionCarrier battleActionCarrier, IRole role)
    {
        foreach (IBattleAction item in battleActionCarrier.BattleActionList)
        {
            if (item.OwnData == role)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 切换战斗是否存在
    /// </summary>
    public static bool ChackBattleExist(IBattleActionCarrier battleActionCarrier, IBattleAction battleAction)
    {
        return battleActionCarrier.BattleActionList.Contains(battleAction);
    }

    
}


/// <summary>
/// 战斗等待接口
/// </summary>
public interface IBattleWait
{
    public void Wait();
}


public static class HelperBattle
{
    public static void AddBattle(this IBattleActionCarrier battleActionCarrier, IBattleAction battleAction)
    {
        IBattleActionCarrier.AddBattleAction(battleActionCarrier, battleAction);
    }
    public static void AddBattle(this IBattle battle, IBattleAction battleAction)
    {
        if (battle is IBattleActionCarrier battleActionCarrier)
            IBattleActionCarrier.AddBattleAction(battleActionCarrier, battleAction);
        else
            Debug.Log("战斗battle请继承IBattleActionCarrier");
    }
    public static void RemoveBattle(this IBattleActionCarrier battleActionCarrier, IBattleAction battleAction)
    {
        IBattleActionCarrier.RemoveBattleAction(battleActionCarrier, battleAction);
    }
    public static void RemoveBattle(this IBattle battle, IBattleAction battleAction)
    {
        if (battle is IBattleActionCarrier battleActionCarrier)
            IBattleActionCarrier.RemoveBattleAction(battleActionCarrier, battleAction);
        else
            Debug.Log("战斗battle请继承IBattleActionCarrier");
    }
    public static void ChackBattleExist(this IBattleActionCarrier battleActionCarrier, IRole role)
    {
        IBattleActionCarrier.ChackBattleExist(battleActionCarrier, role);
    }
    public static void ChackBattleExist(this IBattleActionCarrier battleActionCarrier, IBattleAction battleAction)
    {
        IBattleActionCarrier.ChackBattleExist(battleActionCarrier, battleAction);
    }
}
