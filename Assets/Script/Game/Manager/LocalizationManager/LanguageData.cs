using System;

/// <summary>
/// 语言数据
/// </summary>
[Serializable]
public class LanguageData
{
    /// <summary>
    /// 是否开启
    /// </summary>
    /// <returns></returns>
    public bool IsEnabled()
    {
        return (this.Flags & 1) == 0;
    }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name;

    /// <summary>
    /// 代码
    /// </summary>
    public string Code;

    /// <summary>
    /// 标记
    /// </summary>
    public byte Flags;

    /// <summary>
    /// 是否被压缩
    /// </summary>
    [NonSerialized] public bool Compressed;
}