using System;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    日志配置

-----------------------*/

namespace Core
{
    /// <summary> 日志颜色 </summary>
    public enum LogCoLor
    {
        /// <summary> 空 </summary>
        None,
        /// <summary> 深红 </summary>
        DarkRed,
        /// <summary> 绿色 </summary>
        Green,
        /// <summary> 蓝色 </summary>
        Blue,
        /// <summary> 青色 </summary>
        Cyan,
        /// <summary> 紫色 </summary>
        Magenta,
        /// <summary> 深黄 </summary>
        DarkYellow,
    }

    /// <summary> 日志平台 </summary>
    public enum LoggerType
    {
        /// <summary> Unity编辑器 </summary>
        Unity,
        /// <summary> 服务器 </summary>
        Console,
    }

    /// <summary> 日志配置 </summary>
    public class LogConfig
    {
        /// <summary> 启用日志 </summary>
        public bool enableLog = true;
        /// <summary> 日志前缀 </summary>
        public string LogPrefix = "#";
        /// <summary> 启用时间 </summary>
        public bool enableTime = true;
        /// <summary> 日志分离 </summary>
        public string LogSeparate = ">>";
        /// <summary> 启用线程ID </summary>
        public bool enableThreadID = true;
        /// <summary> 启用跟踪 </summary>
        public bool enableTrace = true;
        /// <summary> 启用保存 </summary>
        public bool enableSave = true;
        /// <summary> 日志覆盖 </summary>
        public bool enableCover = false;
        /// <summary> 保存路径 </summary>
        public string _savePath;
        /// <summary> 保存名称 </summary>
        public string saveName = "PELog.txt";
        /// <summary> 日志类型 </summary>
        public LoggerType loggerType = LoggerType.Unity;
        /// <summary> 保存路径 </summary>
        public string savePath
        {
            get
            {
                if (_savePath == null)
                {
                    switch (loggerType)
                    {
                        case LoggerType.Unity:
                            Type type = Type.GetType("UnityEngine.Application, UnityEngine");
                            _savePath = type.GetProperty("persistentDataPath").GetValue(null).ToString() + "/PELog/";
                            break;
                        case LoggerType.Console:
                            _savePath = string.Format($"{AppDomain.CurrentDomain.BaseDirectory}Logs\\");
                            break;
                    }
                }
                return _savePath;
            }
            set
            {
                _savePath = value;
            }
        }
    }
}
