using System;

/// <summary>
/// 游戏流程注解
/// </summary>
public class GameProcessAttribute : Attribute, IComparable<GameProcessAttribute>
{
    public readonly Type Type;
    private readonly int _numberValue;// 执行顺序
    public GameProcessAttribute(Type type, int numberValue)
    {
        if (!typeof(IProcessStateNode).IsAssignableFrom(type))throw new Exception($"{nameof(type)}没有继承接口");
        Type = type;
        _numberValue = numberValue;
    }

    public int CompareTo(GameProcessAttribute other)
    {
        return _numberValue.CompareTo(other._numberValue);
    }
}