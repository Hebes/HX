using System.Collections;

/// <summary>
/// 检测热更
/// </summary>
[GameProcess(typeof(GameDoHotUpdate), 4)]
public class GameDoHotUpdate : IProcessStateNode
{
    private ProcessFsmSystem _processFsmSystem;
    private GameLunch _gameLunch;

    public void OnCreate(object obj)
    {
        _processFsmSystem = (ProcessFsmSystem)obj;
        _gameLunch = (GameLunch)_processFsmSystem.Owner;
    }

    public void OnEnter(object obj)
    {
        _gameLunch.StartCoroutine(DoHotUpdate());
    }

    public void OnUpdate()
    {
    }

    public void OnExit()
    {
    }

    private IEnumerator DoHotUpdate()
    {
        // percent = 0;
        // SetLoadingPercentage(percent);//加载窗口进度条重置为0
        //
        // var start = DateTime.Now;
        // //获取包名 (渠道名 目前用不到 暂时固定)
        // yield return InitPackageName();
        // //Debug2.LogColor(string.Format("InitPackageName use {0}ms", (DateTime.Now - start).Milliseconds),LogColor.Cyan);
        //
        // // 启动检测更新
        // yield return CheckAndDownload();
        // //Debug2.LogColor(string.Format("CheckAndDownload use {0}ms", (DateTime.Now - start).Milliseconds),LogColor.Cyan);
        //
        // // 启动资源管理模块
        // start = DateTime.Now;
        // yield return AssetBundleManager.Instance.Initialize();
        // //Debug2.LogColor(string.Format("AssetBundleManager Initialize use {0}ms", (DateTime.Now - start).Milliseconds),LogColor.Cyan);
        //
        // //加载ab包
        // start = DateTime.Now;
        // yield return LoadAllAssetBundle();
        // //Debug2.LogColor(string.Format("LoadAllAssetBundle end use {0}ms", (DateTime.Now - start).Milliseconds),LogColor.Cyan);
        //
        // percent = 100;
        // SetLoadingPercentage(percent);//完成加载
        // //end
        yield return null;
        _processFsmSystem.ChangeState(nameof(GameHotUpdate));
    }
}