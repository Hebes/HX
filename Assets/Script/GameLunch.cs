using System;
using System.Collections;
using Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 游戏启动器
/// Framework ---->  框架 进入热更?或者进入(暂时进入)
/// Game      ---->  必定进入热更
/// </summary>
public class GameLunch : MonoBehaviour
{
    private int percent; //加载界面的进度

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        Debug.Log(" Start: 初始化开始!");
        StartCoroutine(GameStart());
    }

    private void Initialize()
    {
        gameObject.AddComponent<ShowFPS>();
        //TODO 加载进度界面
    }

    private IEnumerator GameStart()
    {
        //TODO 闪屏


        yield return Setting(); // 初始化设置 如帧率 程序是否后台运行
        yield return DoHotUpdate(); //热更
        yield return ResourceUpdating(); //加载配置文件
        yield return EnterHuatuoHotUpdate(); //加载dll后 用热更 启动代码
    }

    private IEnumerator Setting()
    {
#if UNITY_EDITOR
        Application.runInBackground = true; //应用程序在后台时是否应该被运行？
#endif
        Application.targetFrameRate = 120; //目标帧速率
        //平台是安卓 或者ios 时 帧率为30
        if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer)
            Application.targetFrameRate = 30;
        Time.timeScale = 1.0f; //游戏 时间刻度 1:1时间
        Time.fixedDeltaTime = 0.033f; //每秒顶多30次，与帧同步服务器一致

        string levelName = SceneManager.GetActiveScene().name;
        Debug.LogWarning(this + ".初始化所在场景 | 当前场景 : " + levelName);

        //游戏初始化之后就禁止屏幕的休眠
        Screen.sleepTimeout = SleepTimeout.NeverSleep; //睡眠超时.从不睡眠

        //SystemInfo访问系统和硬件信息    图形设备 != null   图形设备开始与"Mali-4"  检查是否是低端机?
        if (SystemInfo.graphicsDeviceName != null && SystemInfo.graphicsDeviceName.StartsWith("Mali-4"))
            Debug.LogError("低端GPU|" + SystemInfo.graphicsDeviceName + "|不走GPUSkinning|" + SystemInfo.operatingSystem + "|" + SystemInfo.deviceModel);
        //标示是否低端机，在加载完配置表之后 TODO
        //设置一个 变量 如果为低端机 替换低模 低分辨率 降低特效之类的操作 
        //end

        //打开进度条控制
        //SceneLoadingWindow loadingWindow = SceneLoadingCanvasMgr.Instance.GetLoadingCanvas();
        //loadingWindow.ShowInfo();
        //loadingWindow.tipsValue = WorldConfig.welcomtTip;//加载标题

        yield break;
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
        yield break;
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
        yield break;
    }

    private IEnumerator EnterHuatuoHotUpdate()
    {
//         var asset = ResMgr.Instance.GetAssetCache<TextAsset>("HotUpdateDlls/Game.dll.bytes");
//         if (asset != null)
//         {
// #if !UNITY_EDITOR
//            this.entryAssembly = System.Reflection.Assembly.Load(asset.bytes);
// #else
//             this.entryAssembly = AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == "Game");
// #endif
//         }
//         else
//         {
//             Debug2.LogColor($"加载huatuo热更 失败! asset != null : {asset != null}" ,LogColor.Cyan);
//             yield break;
//         }
//
        this.EnterGameApp(true);
        yield break;
    }

    private void SetLoadingPercentage(int value)
    {
        //SceneLoadingCanvasMgr.Instance.SetLoadingPercentage(value);
    }

    private void SetPercentLerp(float max)
    {
        percent = (int)Mathf.Lerp(percent, max, 0.5f);
        SetLoadingPercentage(percent);
    }

    private void EnterGameApp(bool isHotupdate)
    {
        
        return;
        // if (isHotupdate)
        // {
        //     //必须用反射调用hotdll 不然编译死了 就没有解释执行的空间了
        //     var appType = this.entryAssembly.GetType("Main");
        //     var mainMethod = appType.GetMethod("RunMain");
        //     mainMethod.Invoke(null, null);
        // }
        // else
        // {
        //     //Main.RunMainNative(); //热更代码 实例调用 不然无法热更 打包会出错
        //     //Debug2.LogColor($"加载huatuo热更 失败! isHotupdate : {isHotupdate}" ,LogColor.Cyan);
        // }
    }

    [ContextMenu("输出电脑配置")]
    public void GetSystemInfo()
    {
        //设备的模型
        Debug.LogError("设备模型" + SystemInfo.deviceModel);
        //设备的名称
        Debug.LogError("设备名称" + SystemInfo.deviceName);
        //设备的类型
        Debug.LogError("设备类型（PC电脑，掌上型）" + SystemInfo.deviceType.ToString());
        //系统内存大小
        Debug.LogError("系统内存大小MB" + SystemInfo.systemMemorySize.ToString());
        //操作系统
        Debug.LogError("操作系统" + SystemInfo.operatingSystem);
        //设备的唯一标识符
        Debug.LogError("设备唯一标识符" + SystemInfo.deviceUniqueIdentifier);
        //显卡设备标识ID
        Debug.LogError("显卡ID" + SystemInfo.graphicsDeviceID.ToString());
        //显卡名称
        Debug.LogError("显卡名称" + SystemInfo.graphicsDeviceName);
        //显卡类型
        Debug.LogError("显卡类型" + SystemInfo.graphicsDeviceType.ToString());
        //显卡供应商
        Debug.LogError("显卡供应商" + SystemInfo.graphicsDeviceVendor);
        //显卡供应唯一ID
        Debug.LogError("显卡供应唯一ID" + SystemInfo.graphicsDeviceVendorID.ToString());
        //显卡版本号
        Debug.LogError("显卡版本号" + SystemInfo.graphicsDeviceVersion);
        //显卡内存大小
        Debug.LogError("显存大小MB" + SystemInfo.graphicsMemorySize.ToString());
        //显卡是否支持多线程渲染
        Debug.LogError("显卡是否支持多线程渲染" + SystemInfo.graphicsMultiThreaded.ToString());
        //支持的渲染目标数量
        Debug.LogError("支持的渲染目标数量" + SystemInfo.supportedRenderTargetCount.ToString());
    }
}