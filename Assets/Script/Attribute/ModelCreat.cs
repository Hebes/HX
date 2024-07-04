using System;

/// <summary>
/// 创建模块注解
/// </summary>
public class ModelCreat : Attribute, IComparable<ModelCreat>
{
    public readonly Type Type;
    public readonly int Num;

    public ModelCreat(Type type, int num)
    {
        Type = type;
        Num = num;
    }

    public int CompareTo(ModelCreat other)
    {
        return Num.CompareTo(other.Num);
    }
}