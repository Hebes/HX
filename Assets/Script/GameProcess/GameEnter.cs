using System.Collections;
using Framework.Core;

/// <summary>
/// 游戏进入
/// </summary>
[GameProcess(typeof(GameEnter))]
public class GameEnter : IStateNode
{
    private FsmSystem _fsmSystem;

    public void OnCreate(FsmSystem machine)
    {
        _fsmSystem = machine;
    }

    public void OnEnter()
    {
        GameLunch.Instance.StartCoroutine(Init());
        return;
        IEnumerator Init()
        {
            //初始化核心
            CoreRun coreRun = new();
            yield return coreRun.CoreIEnumeratorInit();
            //显示主界面
            CoreUI.ShwoUIPanel<MainMenuView>(ConfigPrefab.prefabUIMianMenu);
            //加载子模块
            ModelRun modelRun = new();
            yield return modelRun.ModelInit();
        }
    }

    public void OnUpdate()
    {
    }

    public void OnExit()
    {
    }
}