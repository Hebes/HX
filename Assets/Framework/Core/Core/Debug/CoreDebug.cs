using System;
using System.Collections;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

/*--------脚本描述-----------

描述:
    日志模块

-----------------------*/

namespace Framework.Core
{
    /// <summary>
    /// 日志
    /// </summary>
    [CreateCore(typeof(CoreDebug),1)]
    public class CoreDebug : ICore
    {
        public IEnumerator AsyncInit()
        {
            yield break;
        }

        public void Init()
        {
            //日志设置
            LogConfig logConfig = new()
            {
                EnableTime = true,
                LoggerType = LoggerType.Unity
            };
#if UNITY_EDITOR
            //logConfig.savePath = $"{Application.persistentDataPath}/LogOut/ActiveLog/",
            logConfig.SavePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}/LogOut/";
            //savePath = $"{Application.dataPath}/Log",
#endif
            new DBug().Init(logConfig, Debug.Log, Debug.LogWarning, Debug.LogError);

            //屏幕显示日志
            var debugGo = new GameObject("日志");
            debugGo.AddComponent<UIDebugger1>();
            Object.DontDestroyOnLoad(debugGo);
            //GameObject debugGo = new GameObject("UIDebug");
            //debugGo.AddComponent<UIDebugger>();
            //GameObject.DontDestroyOnLoad(debugGo);
            UnityEngine.Debug.Log("日志模块初始化完毕!");

            Application.logMessageReceived += Handler;
        }

        /// <summary>
        /// 被动日志
        /// </summary>
        /// <param name="logString"></param>
        /// <param name="stackTrace"></param>
        /// <param name="type"></param>
        private static void Handler(string logString, string stackTrace, LogType type)
        {
            if (type != LogType.Error || type != LogType.Exception || type != LogType.Assert) return;
            string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}/LogOut/"; 
            UnityEngine.Debug.Log("显示堆栈调用：" + new System.Diagnostics.StackTrace().ToString());
            UnityEngine.Debug.Log("接收到异常信息" + logString);
            string Time = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            //string logPath = Path.Combine(systemDebugConfig.SavePath, $"Passive_{Time}.txt");
            string logPath = $"{path}Passive_{Time}.txt";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.AppendAllText(logPath, $"==============================================\r\n");
            File.AppendAllText(logPath, $"[时间]:{Time}\r\n");
            File.AppendAllText(logPath, $"[类型]:{type}\r\n");
            File.AppendAllText(logPath, $"[报错信息]:{logString}\r\n");
            File.AppendAllText(logPath, $"[堆栈跟踪]:{stackTrace}\r\n");
            UnityEngine.Debug.Log($"被动日志生成路径:{logPath}");
        }
    }
}
