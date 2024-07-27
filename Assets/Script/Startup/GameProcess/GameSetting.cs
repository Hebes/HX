using UnityEngine;
using UnityEngine.SceneManagement;

[GameProcess(typeof(GameSetting),2)]
public class GameSetting : IProcessStateNode
{
    private ProcessFsmSystem _processFsmSystem;

    public void OnCreate(object obj)
    {
        _processFsmSystem = (ProcessFsmSystem)obj;
    }

    public void OnEnter(object obj)
    {
#if UNITY_EDITOR
        Application.runInBackground = true; //应用程序在后台时是否应该被运行？
#endif
        // Application.targetFrameRate = 120; //目标帧速率
        // //平台是安卓 或者ios 时 帧率为30
        // if (Application.platform == RuntimePlatform.Android ||
        //     Application.platform == RuntimePlatform.IPhonePlayer)
        //     Application.targetFrameRate = 30;
        Time.timeScale = 1.0f; //游戏 时间刻度 1:1时间
        Time.fixedDeltaTime = 0.033f; //每秒顶多30次，与帧同步服务器一致

        string levelName = SceneManager.GetActiveScene().name;
        Debug.LogWarning(this + ".初始化所在场景 | 当前场景 : " + levelName);

        //游戏初始化之后就禁止屏幕的休眠
        Screen.sleepTimeout = SleepTimeout.NeverSleep; //睡眠超时.从不睡眠

        //SystemInfo访问系统和硬件信息    图形设备 != null   图形设备开始与"Mali-4"  检查是否是低端机?
        if (SystemInfo.graphicsDeviceName != null && SystemInfo.graphicsDeviceName.StartsWith("Mali-4"))
            Debug.LogError("低端GPU|" + SystemInfo.graphicsDeviceName + "|不走GPUSkinning|" + SystemInfo.operatingSystem +
                           "|" + SystemInfo.deviceModel);
        //标示是否低端机，在加载完配置表之后 TODO
        //设置一个 变量 如果为低端机 替换低模 低分辨率 降低特效之类的操作 
        //end

        //打开进度条控制
        //SceneLoadingWindow loadingWindow = SceneLoadingCanvasMgr.Instance.GetLoadingCanvas();
        //loadingWindow.ShowInfo();
        //loadingWindow.tipsValue = WorldConfig.welcomtTip;//加载标题
        _processFsmSystem.ChangeState(nameof(GameDoHotUpdate));
    }

    public void OnUpdate()
    {
    }

    public void OnExit()
    {
    }
}