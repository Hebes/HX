using System.Collections.Generic;

/// <summary>
/// 对象池控制器
/// </summary>
public static class PoolController
{
    public static readonly Dictionary<string, ObjectPool> EffectDict = new Dictionary<string, ObjectPool>();
}