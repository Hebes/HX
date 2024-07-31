using System;

/// <summary>
/// 关卡得分
/// </summary>
public class BloodPalaceLevelScore
{
    /// <summary>
    /// 得分
    /// </summary>
    public int Score => OriginalScore * (!NotHurt ? 1 : 5);

    /// <summary>
    /// 清空
    /// </summary>
    public void Clear()
    {
        WaveScore = 0;
        BeautyScore = 0;
        Time = 0f;
        FlashPercent = 0f;
        OriginalScore = 0;
        NotHurt = true;
    }

    /// <summary>
    /// 每波得分
    /// </summary>
    public int WaveScore;

    /// <summary>
    /// 优点得分
    /// </summary>
    public int BeautyScore;

    /// <summary>
    /// 时间
    /// </summary>
    public float Time;

    /// <summary>
    /// 闪的百分比
    /// </summary>
    public float FlashPercent;

    /// <summary>
    /// 初始得分
    /// </summary>
    public int OriginalScore;

    /// <summary>
    /// 是否没有受到伤害
    /// </summary>
    public bool NotHurt = true;
}