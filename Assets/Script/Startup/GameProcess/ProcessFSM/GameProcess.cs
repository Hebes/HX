using System;

/// <summary>
/// 游戏流程注解
/// </summary>
public class GameProcess : Attribute, IComparable<GameProcess>
{
    public readonly Type Type;
    private readonly int _numberValue;// 执行顺序
    public GameProcess(Type type, int numberValue)
    {
        if (!typeof(IProcessStateNode).IsAssignableFrom(type))throw new Exception($"{nameof(type)}没有继承接口");
        Type = type;
        _numberValue = numberValue;
    }

    public int CompareTo(GameProcess other)
    {
        return _numberValue.CompareTo(other._numberValue);
    }
}