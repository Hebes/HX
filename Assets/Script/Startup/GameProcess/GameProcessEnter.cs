using System.Collections;
using DG.Tweening;
using Framework.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 流程进入
/// </summary>
[GameProcess(typeof(GameProcessEnter), 1)]
public class GameProcessEnter : IProcessStateNode
{
    private ProcessFsmSystem _processFsmSystem;

    public void OnCreate(ProcessFsmSystem obj)
    {
        _processFsmSystem = obj;
    }

    public void OnEnter(object obj)
    {
        GameLunch.Instance.gameObject.AddComponent<ShowFPS>();
        //闪屏效果
        //UISplashController.Instance.Run();
        //TODO 加载进度界面
        _processFsmSystem.ChangeState(nameof(GameSetting));
    }

    public void OnUpdate()
    {
    }

    public void OnExit()
    {
    }
    
    //加载场景
    
    private static AsyncOperation _asyncOperation;
    public static AsyncOperation LoadScene(string levelName)
    {
        if (_asyncOperation is { isDone: false })
        {
            return _asyncOperation;
        }
        _asyncOperation = SceneManager.LoadSceneAsync(levelName);
        if (_asyncOperation == null)
        {
            IProcessStateNode.Log(levelName + "场景不存在，是不是没放在build里？还是名字写错了？");
        }
        return _asyncOperation;
    }
}