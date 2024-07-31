using System;

/// <summary>
/// 提高参数
/// </summary>
public class EnhanceArgs : EventArgs
{
    public EnhanceArgs(string name, int upToLevel)
    {
        this.Name = name;
        this.UpToLevel = upToLevel;
    }

    public string Name;

    public int UpToLevel;
}