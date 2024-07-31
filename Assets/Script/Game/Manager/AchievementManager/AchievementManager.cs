using System.Collections;
using System.Collections.Generic;
using Framework.Core;

/// <summary>
/// 成就系统
/// </summary>
[CreateCore(typeof(AchievementManager), 1)]
public class AchievementManager : ICore
{
    public static AchievementManager Instance;
    
    public void Init()
    {
        Instance = this;
        "启动成就管理器".Log();
    }

    public IEnumerator AsyncEnter()
    {
        yield break;
    }

    public IEnumerator Exit()
    {
        yield break;
    }

    /// <summary>
    /// 成就信息
    /// </summary>
    public readonly List<int> AchievementInfoList = new List<int>();
    private readonly Dictionary<int, AchievementInfo> _achievementInfoDic = new Dictionary<int, AchievementInfo>();

    /// <summary>
    /// 获得成就解锁状态
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool GetAchievementUnlockState(int index)
    {
        return AchievementInfoList.Contains(index);
    }

    /// <summary>
    /// 解锁成就
    /// </summary>
    /// <param name="index"></param>
    private void UnlockAchievement(int index)
    {
        if (!this.GetAchievementUnlockState(index))
            AchievementInfoList.Add(index);
        //R.Settings.Save();
    }

    /// <summary>
    /// 获取成就信息
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public AchievementInfo GetAchievementInfo(int index)
    {
        return _achievementInfoDic[index];
    }
    
    
    /// <summary>
    /// 奖的成就
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool AwardAchievement(int index)
    {
        if (this.GetAchievementUnlockState(index))return false;
        this.UnlockAchievement(index);
        //R.Ui.TrophyNotification.AwardTrophy(this.GetAchievementInfo(index).Name, index.ToString());
        return true;
    }
    
    /// <summary>
    /// 所有成就
    /// </summary>
    public void AwardAll()
    {
        for (var i = 1; i < _achievementInfoDic.Count; i++)
            AwardAchievement(i);
    }
}