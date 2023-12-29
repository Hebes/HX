public interface IBattle : IID
{
}

public interface IBattleBehaviour
{
    public void BattleUpdata();
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

