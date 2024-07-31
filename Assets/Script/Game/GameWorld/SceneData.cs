using System;

/// <summary>
/// 世界数据
/// </summary>
public class SceneData
{
    /// <summary>
    /// 是否暂停
    /// </summary>
    public bool isPausing => WorldTime.IsPausing;

    /// <summary>
    /// 清空
    /// </summary>
    public void Clear()
    {
        assessmentData = new BattleAssessmentData();//战斗评估数据
    }

    /// <summary>
    /// 是否可以跑AI
    /// </summary>
    public bool CanAIRun;

    /// <summary>
    /// 是否进入屏障模式
    /// </summary>
    public bool BloodPalaceMode;

    public BloodPalaceTotalScore TotalScore;

    public BloodPalaceLevelScore LevelScore = new BloodPalaceLevelScore();

    public int currentBattleZoneId = -1;

    public BattleAssessmentData assessmentData = new BattleAssessmentData();
}