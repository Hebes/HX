﻿using Cysharp.Threading.Tasks;
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

            //主动日志
            Debug.InitSettings(new LogConfig()
            {
                enableSave = false,
                loggerType = LoggerType.Unity,
#if !UNITY_EDITOR
                //savePath = $"{Application.persistentDataPath}/LogOut/ActiveLog/",
                savePath = $"{Application.dataPath}/LogOut/ActiveLog/",
#endif
                savePath = $"{Application.dataPath}/Log",
                saveName = "Debug主动输出日志.txt",
            });

            //被动日志
            //SystemExceptionDebug.InitSystemExceptionDebug();

            //屏幕显示日志
            GameObject debugGo = new GameObject("UIDebug");
            debugGo.AddComponent<UIDebugger>();
            GameObject.DontDestroyOnLoad(debugGo);
            Debug.Log("日志模块初始化完毕!");
        }
    }
}
