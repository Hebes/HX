using System;
using Framework.Core;

/// <summary>
/// 游戏流程注解
/// </summary>
public class GameProcess : Attribute
{
    public IStateNode StateNode;

    public GameProcess(Type type)
    {
        StateNode = (IStateNode)Activator.CreateInstance(type);
    }
}