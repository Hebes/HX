using System;
using System.Collections.Generic;
using System.Reflection;
using Framework.Core;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using UnityEngine;


/*  代码规范
    驼峰命名法
    抽象类 :命名使用 Abstract或Base结尾
    异常类:使用Exception结尾
    设计模式:体现在名字中,如public class SysuserController，public class OrderFactory
    待办事宜 :TODO

    类(class)    :        单词首字母大写
    枚举(enum)   :        首字符E开头
    属性(property):       单词首字母大写
    变量(field):          公有(单词首字母大写), 单词结尾(如Dictionary变量以Dic结尾,List变量以List结尾)
    标签(Attribute):      以Attribute结尾
    结构(struct):         同class
    接口(interface):  首字符I开头
    常量(const):全体单词大写,单词与单词之间用_隔开,比如: NAME_GAME_OBJECT
    布尔类型成员:一般用Is, Has, Can开头
    回调函数名:首单词On开头,后面单词首字母大写,比如: OnUpdate, OnTimerCallback
    函数名:单词首字母大写,比如: Update, OnGUI
    委托(delegate)和事件(event):
                            静态私有,首字符s_开头,后面单词首字母大写,比如: s_OnDownloadComplete
                            私有, 保护,首字符m_开头,后面单词首字母大写,比如: m_OnDownloadComplete
                            其他,首单词on开头,后面单词首字母大写,比如: onDownloadComplete

    各层命名规约:
            1.获取单个对象的方法用 Get 做前缀。
            2.获取多个对象的方法用List做后缀，如：GetOrdersList。
            3.获取统计值的方法用 Count 做后缀。
            4.添加或更新的方法用 Save或Add。
            5.删除的方法用 Remove/Delete。
            6.修改的方法用 Update。
*/

/// <summary>
/// 游戏启动器
/// Framework ---->  框架 进入热更?或者进入(暂时进入)
/// Game      ---->  必定进入热更
///
/// 游戏流程 ----> GameProcessEnter ----> GameSetting ----> GameResUpda ----> GameDoHotUpdate ---->  GameHotUpdate---->
/// </summary>
public class GameLunch : MonoBehaviour
{
    private int _percent; //加载界面的进度
    public static GameLunch Instance;
    private ProcessFsmSystem _fsmSystem;
    private string _runName;

    private void Awake()
    {
        Instance = this;
        _fsmSystem = new ProcessFsmSystem(this);
        
        List<GameProcessAttribute> gameProcessList = new();
        var assembly = Assembly.GetExecutingAssembly();
        var typeArray = assembly.GetTypes();
        foreach (var type in typeArray)
        {
            if (!Attribute.IsDefined(type, typeof(GameProcessAttribute))) continue;
            var attribute = Attribute.GetCustomAttribute(type, typeof(GameProcessAttribute)); 
            gameProcessList.Add((GameProcessAttribute)attribute);
        }
        
        gameProcessList.Sort();
        foreach (var gameProcess in gameProcessList)
            _fsmSystem.AddNode(gameProcess.Type);
        _runName = gameProcessList[0].Type.FullName;
    }

    private void Start()
    {
        _fsmSystem.Run(_runName);
    }

    private void SetPercentLerp(float max)
    {
        _percent = (int)Mathf.Lerp(_percent, max, 0.5f);
        SetLoadingPercentage(_percent);
    }

    private void SetLoadingPercentage(int value)
    {
        //SceneLoadingCanvasMgr.Instance.SetLoadingPercentage(value);
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

    public static void Log(string str)
    {
        UnityEngine.Debug.Log(str);
    }
}