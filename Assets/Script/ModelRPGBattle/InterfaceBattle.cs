using System.Collections.Generic;

/// <summary>
/// 一场战斗的接口
/// </summary>
public interface IOneBattle
{
    /// <summary>
    /// 战斗编号
    /// </summary>
    public uint BattleId { get; }

    /// <summary>
    /// 战斗的状态
    /// </summary>
    public EBattlePerformAction BattleState { get; }

    /// <summary>
    /// 战斗执行动作列表
    /// </summary>
    public List<BattleData> BattleDataList { get; }

    /// <summary>
    /// 战斗的位置
    /// 1.可能是敌人在左边，进入二打一模式
    /// 2.可能是自己人右边，敌人3队进行二打一模式
    /// </summary>
    public Dictionary<ERoleBattlePoint, List<IRole>> RolePointDic { get; }


    public void Init();
    public void Updata();
    public void Remove();
}