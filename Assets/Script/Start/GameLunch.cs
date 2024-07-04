using Framework.Core;
using UnityEngine;

/// <summary>
/// 入口脚本
/// </summary>
public class GameLunch : MonoBehaviour
{
    public static GameLunch Instance;
    private FsmSystem _fsmSystem;

    private void Awake()
    {
        Instance = this;
        _fsmSystem = new FsmSystem(this);
        var gameProcessList = Utility.Reflection.GetAttribute<GameProcess>();
        foreach (var gameProcess in gameProcessList)
            _fsmSystem.AddNode(gameProcess.StateNode);
        _fsmSystem.Run(typeof(GameEnter));
    }
}