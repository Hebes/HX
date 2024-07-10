using System.Collections;
using Framework.Core;

/// <summary>
/// 游戏进入
/// </summary>
[GameProcess(typeof(GameEnter),1)]
public class GameEnter : IProcessStateNode
{
    private ProcessFsmSystem _processFsmSystem;
    
    public void OnCreate(ProcessFsmSystem obj)
    {
        _processFsmSystem = obj;
    }

    public void OnEnter(object obj)
    {
        GameLunch.Instance.gameObject.AddComponent<ShowFPS>();
        //TODO 加载进度界面
        _processFsmSystem.ChangeState(nameof(GameSetting));
        // GameLunch.Instance.StartCoroutine(Init());
        // return;
        // IEnumerator Init()
        // {
        //     //初始化核心
        //     CoreRun coreRun = new();
        //     yield return coreRun.CoreIEnumeratorInit();
        //     //显示主界面
        //     //CoreUI.ShwoUIPanel<MainMenuView>(ConfigPrefab.prefabUIMianMenu);
        //     //加载子模块
        //     ModelRun modelRun = new();
        //     yield return modelRun.ModelInit();
        // }
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
    }
}