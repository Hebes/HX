using System;

/// <summary>
/// 得分
/// </summary>
public class BloodPalaceTotalScore : IComparable<BloodPalaceTotalScore>
{
    /// <summary>
    /// 添加关卡得分
    /// </summary>
    /// <param name="levelScore"></param>
    public void AddLevelScore(BloodPalaceLevelScore levelScore)
    {
        this.Score += levelScore.Score;
        this.NotHurt &= levelScore.NotHurt;
        this.Time += levelScore.Time;
    }

    /// <summary>
    /// 比较函数
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(BloodPalaceTotalScore other)
    {
        return -1 * this.Score.CompareTo(other.Score);
    }

    /// <summary>
    /// 得分
    /// </summary>
    public int Score;

    /// <summary>
    /// 是否没有受到伤害
    /// </summary>
    public bool NotHurt = true;

    /// <summary>
    /// 时间
    /// </summary>
    public float Time;
}