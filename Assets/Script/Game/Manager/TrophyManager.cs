using System;

public class TrophyManager
{
    /// <summary>
    /// 奖杯
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool AwardTrophy(int index) => AchievementManager.Instance.AwardAchievement(index);

    /// <summary>
    /// 触发所有成就
    /// </summary>
    public void AwardAllTrophies() => AchievementManager.Instance.AwardAll();
}