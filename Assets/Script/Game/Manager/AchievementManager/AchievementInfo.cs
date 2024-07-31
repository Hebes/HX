using Framework.Core;

/// <summary>
/// 成就信息
/// </summary>
public struct AchievementInfo : IID
{
    /// <summary>
    /// ID
    /// </summary>
    public long ID { get; set; }

    /// <summary>
    /// 名称->都是简体中文
    /// </summary>
    public string Name;

    /// <summary>
    /// 详细
    /// </summary>
    public string Detail;
}