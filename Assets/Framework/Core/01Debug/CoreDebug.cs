using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    日志模块

-----------------------*/


namespace Core
{
    public class CoreDebug : ICore
    {
        public static CoreDebug Instance;

        public void ICoreInit()
        {
            Instance = this;

            //日志设置
            LogConfig logConfig = new LogConfig();
            logConfig.enableLog=true;
            logConfig.LogPrefix = "#";
            logConfig.enableTime = false;
            logConfig.enableMillisecond = true;
            logConfig.LogSeparate = ">>";
            logConfig.enableThreadID = true;
            logConfig.enableTrace = true;
            logConfig.enableSave = true;
            logConfig.enableCover = false;
            logConfig.saveName = "HXLog.txt";
            logConfig.loggerType = LoggerType.Unity;
#if UNITY_EDITOR
            //logConfig.savePath = $"{Application.persistentDataPath}/LogOut/ActiveLog/",
            logConfig.savePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}/LogOut/";
                //savePath = $"{Application.dataPath}/Log",
#endif
            Debug.InitDebugSettings(logConfig);

            //被动日志
            //SystemExceptionDebug.InitSystemExceptionDebug();

            //屏幕显示日志
            GameObject debugGo = new GameObject("日志");
            debugGo.AddComponent<UIDebugger1>();
            GameObject.DontDestroyOnLoad(debugGo);
            //GameObject debugGo = new GameObject("UIDebug");
            //debugGo.AddComponent<UIDebugger>();
            //GameObject.DontDestroyOnLoad(debugGo);
            Debug.Log("日志模块初始化完毕!");
        }
    }
}
