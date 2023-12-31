public interface IBattle : IID
{
}

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
/// 个人战斗数据
/// </summary>
//[Serializable]
public interface BattleData
{
    /// <summary>
    /// 自己的数据
    /// </summary>
    public IRole ownData { get; set; }

    /// <summary>
    /// 目标的数据
    /// </summary>
    public IRole TargetData { get; set; }


    public void Attack();
}


/// <summary>
/// 战斗等待接口
/// </summary>
public interface IBattleWait
{
    public void Wait();
}

