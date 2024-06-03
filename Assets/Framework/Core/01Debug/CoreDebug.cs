using System;
using System.Collections;
using UnityEngine;

/*--------脚本描述-----------

描述:
    日志模块

-----------------------*/

namespace Core
{
    public class CoreDebug : ICore
    {
        public IEnumerator AsyncInit()
        {
            yield break;
        }

        public void Init()
        {
            //日志设置
            LogConfig logConfig = new LogConfig();
            logConfig.enableLog = true;
            logConfig.LogPrefix = "#";
            logConfig.enableTime = false;
            logConfig.enableMillisecond = true;
            logConfig.LogSeparate = ">>";
            logConfig.enableThreadID = true;
            logConfig.enableTrace = true;
            logConfig.enableSave = true;
            logConfig.enableCover = false;
            logConfig.saveName = "Log.txt";
            logConfig.loggerType = LoggerType.Unity;
#if UNITY_EDITOR
            //logConfig.savePath = $"{Application.persistentDataPath}/LogOut/ActiveLog/",
            logConfig.savePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}/LogOut/";
            //savePath = $"{Application.dataPath}/Log",
#endif
            new CDebug().Init(logConfig, Debug.Log, Debug.LogWarning, Debug.LogError);

            //被动日志
            //SystemExceptionDebug.InitSystemExceptionDebug();

            //屏幕显示日志
            GameObject debugGo = new GameObject("日志");
            debugGo.AddComponent<UIDebugger1>();
            GameObject.DontDestroyOnLoad(debugGo);
            //GameObject debugGo = new GameObject("UIDebug");
            //debugGo.AddComponent<UIDebugger>();
            //GameObject.DontDestroyOnLoad(debugGo);
            UnityEngine.Debug.Log("日志模块初始化完毕!");
        }
    }
}
