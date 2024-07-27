using System.Collections;

/// <summary>
/// 资源加载
/// </summary>
[GameProcess(typeof(GameResUpda), 3)]
public class GameResUpda : IProcessStateNode
{
    private ProcessFsmSystem _processFsmSystem;

    public void OnCreate(object obj)
    {
        _processFsmSystem = (ProcessFsmSystem)obj;
    }

    public void OnEnter(object obj)
    {
        
    }

    public void OnUpdate()
    {
    }

    public void OnExit()
    {
    }

    private IEnumerator ResourceUpdating()
    {
        // var start = DateTime.Now;
        //
        // percent = 0;
        // int count = 0;
        // Debug2.LogColor("[Loading] - 开始加载配置表： 毫秒 " + (DateTime.Now - start).Milliseconds+ "|实时启动后时间:" + Time.realtimeSinceStartup, LogColor.Cyan);
        // //加载csv 配置文件 
        // StartCoroutine(TableMgr.Instance.Initialize());
        // //end
        // while (!(TableMgr.Instance.localConfigLoadDone && percent > 60))
        // {
        //     count++;
        //     if (count % 2 == 0)//减慢一倍增加速度
        //     {
        //         percent = Math.Min(100, ++percent);
        //     }
        //     SetLoadingPercentage(percent);
        //     yield return null;
        // }
        // Debug2.LogColor("[Loading] - 载配置表完成： 毫秒 " + (DateTime.Now - start).Milliseconds+ "|实时启动后时间:" + Time.realtimeSinceStartup, LogColor.Cyan);
        // //loadingWindow.InitLoading("update");
        // percent = 100;
        // SetLoadingPercentage(percent);
        // yield return new WaitForSeconds(0.5f);
        yield return null;
        
    }
}