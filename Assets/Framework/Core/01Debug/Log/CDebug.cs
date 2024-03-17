#define COREDUBUGOPEN
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

/*--------脚本描述-----------

描述:
    日志总控
    Project->Player->Other Settings->Script Define Symbols

-----------------------*/

namespace Core
{
    public class CDebug
    {
        public static CDebug Instance { get; set; }
        private LogConfig cfg;                       //配置文件
        private StreamWriter LogFiLeWriter = null;   //输出流,整个日志文件
        private const string logLock = "LogLock";           //日志锁
        private ILogger logger = null;                      //日志接口

        #region 开启案例
        //        public void Init()
        //        {
        //            //日志设置
        //            LogConfig logConfig = new LogConfig();
        //            logConfig.enableLog = true;
        //            logConfig.LogPrefix = "#";
        //            logConfig.enableTime = false;
        //            logConfig.enableMillisecond = true;
        //            logConfig.LogSeparate = ">>";
        //            logConfig.enableThreadID = true;
        //            logConfig.enableTrace = true;
        //            logConfig.enableSave = true;
        //            logConfig.enableCover = false;
        //            logConfig.saveName = "Log.txt";
        //            logConfig.loggerType = LoggerType.Unity;
        //#if UNITY_EDITOR
        //            //logConfig.savePath = $"{Application.persistentDataPath}/LogOut/ActiveLog/",
        //            logConfig.savePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}/LogOut/";
        //            //savePath = $"{Application.dataPath}/Log",
        //#endif
        //            new CDebug().Init(logConfig, Debug.Log, Debug.LogWarning, Debug.LogError);
        //        }
        #endregion

        public void Init(LogConfig cfg, Action<string> Log, Action<string> Warn, Action<string> Error)
        {
            Instance = this;
            //日志类型
            switch (cfg.loggerType)
            {
                case LoggerType.Unity: logger = new UnityDebug(); break;
                case LoggerType.Console: logger = new ConsoleDebug(); break;
            }
            logger.LogAct = Log;
            logger.WarnAct = Warn;
            logger.ErrorAct = Error;


            this.cfg = cfg ?? new LogConfig();
            //是否启用日志保存
            if (!cfg.enableSave) return;
            //日志覆盖
            if (cfg.enableCover)//覆盖
            {
                string path = $"{cfg.savePath}{cfg.saveName}";
                try
                {
                    if (Directory.Exists(cfg.savePath))//存在这个路径
                    {
                        if (File.Exists(path))//存在这个文件
                            File.Delete(path);
                    }
                    else
                    {
                        Directory.CreateDirectory(cfg.savePath);
                    }
                    LogFiLeWriter = File.AppendText(path);
                    LogFiLeWriter.AutoFlush = true;
                }
                catch (Exception) { LogFiLeWriter = null; }
            }
            else
            {
                string prefix = DateTime.Now.ToString("yyyyMMdd@HH-mm-s");
                string path = $"{cfg.savePath}{prefix}{cfg.saveName}";
                try
                {
                    Log("日志输出路径：" + path);
                    if (Directory.Exists(cfg.savePath) == false)
                        Directory.CreateDirectory(cfg.savePath);
                    LogFiLeWriter = File.AppendText(path);
                    LogFiLeWriter.AutoFlush = true;
                }
                catch (Exception) { LogFiLeWriter = null; }
            }
        }

        //日志 Project->Player->Other Settings->Script Define Symbols
        [Conditional("COREDUBUGOPEN")]
        public void Log(string msg, LogCoLor logCoLor, params object[] args)
        {
            if (cfg.enableLog == false) return;
            msg = DecorateLog(string.Format(msg, args));
            lock (logLock)
            {
                logger.Log(msg, logCoLor);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[L]{msg}"));
            }
        }
        [Conditional("COREDUBUGOPEN")]
        public void Log(string msg, params object[] args)
        {
            Log(msg, LogCoLor.None, args);
        }
        [Conditional("COREDUBUGOPEN")]
        public void Log(object obj)
        {
            Log(obj.ToString());
        }


        //打印堆栈
        public void Trace(string msg, params object[] args)
        {
            if (cfg.enableLog == false) return;
            msg = DecorateLog(string.Format(msg, args), true);
            lock (logLock)
            {
                logger.Log(msg, LogCoLor.Magenta);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[T]{msg}"));
            }
        }
        public void Trace(object obj)
        {
            Trace(obj.ToString());
        }


        //打印警告日志
        public void Warn(string msg, params object[] args)
        {
            if (cfg.enableLog == false) return;
            msg = DecorateLog(string.Format(msg, args));
            lock (logLock)
            {
                logger.Warn(msg);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[W]{msg}"));
            }
        }
        public void Warn(object obj)
        {
            Warn(obj.ToString());
        }


        //打印错误日志
        public void Error(string msg, params object[] args)
        {
            if (cfg.enableLog == false)
                return;
            msg = DecorateLog(string.Format(msg, args), true);
            lock (logLock)
            {
                logger.Error(msg);
                if (cfg.enableSave)
                    WriteToFile(string.Format($"[E]{msg}"));
            }
        }
        public void Error(object obj)
        {
            Error(obj.ToString());
        }


        #region Tool
        //日志打印
        private string DecorateLog(string msg, bool isTrace = false)
        {
            StringBuilder sb = new StringBuilder(cfg.LogPrefix, 100);
            if (cfg.enableTime)//启用时间
                sb.AppendFormat($"时间:{DateTime.Now.ToString("hh:mm:ss--fff")}");
            if (cfg.enableMillisecond)
                sb.AppendFormat($"调用时间(精确到毫秒级){DateTime.Now.TimeOfDay}");
            if (cfg.enableThreadID)//启用线程
                sb.AppendFormat($"{GetThreadID()}");
            sb.AppendFormat($" {cfg.LogSeparate} {msg}");//日志分离
            if (isTrace)//是否追踪日志 堆栈
                sb.AppendFormat($" \nStackTrace:{GetLogTrace()}");
            return sb.ToString();
        }
        //获取日志追踪
        private string GetLogTrace()
        {
            StackTrace st = new StackTrace(3, true);//3 跳跃3帧 true-> 获取场下文信息
            string traceInfo = string.Empty;
            for (int i = 0; i < st.FrameCount; i++)
            {
                StackFrame sf = st.GetFrame(i);
                //traceInfo += string.Format($"\n\t{sf.GetFileName()}::{sf.GetMethod()}line:{sf.GetFileLineNumber()}");
                //traceInfo += string.Format($"\n\t{sf.GetFileName()}::\n\t{sf.GetMethod()}\tline:{sf.GetFileLineNumber()}");
                traceInfo += string.Format($"\n\t脚本:{sf.GetFileName()}::方法{sf.GetMethod()}行: {sf.GetFileLineNumber()}");
            }
            return traceInfo;
        }
        //获取线程Id
        private object GetThreadID()
        {
            return string.Format($" 线程ID:{Thread.CurrentThread.ManagedThreadId}");//ThreadID
        }
        //日志写入文件
        private void WriteToFile(string msg)
        {
            if (cfg.enableSave && LogFiLeWriter != null)
            {
                try
                {
                    LogFiLeWriter.WriteLine(msg);
                }
                catch
                {
                    LogFiLeWriter = null;
                    return;
                }
            }
        }
        //打印数组数据For Debug
        public void PrintBytesArray(byte[] bytes, string prefix, Action<string> printer = null)
        {
            string str = prefix + "->\n";
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i % 10 == 0)
                {
                    str += bytes[i] + "\n";
                }
                str += bytes[i] + " ";
            }
            if (printer != null)
            {
                printer(str);
            }
            else
            {
                Log(str);
            }
        }
        #endregion
    }


}

namespace Core
{
    /// <summary> 日志配置 </summary>
    public class LogConfig
    {
        /// <summary> 启用日志 </summary>
        public bool enableLog = true;
        /// <summary> 日志前缀 </summary>
        public string LogPrefix = "#";
        /// <summary> 是否启用时间 </summary>
        public bool enableTime = true;
        /// <summary> 是否启用毫秒级调用(可以定位代码的调用顺序) </summary>
        public bool enableMillisecond = true;
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

        /// <summary> 日志保存保存路径 </summary>
        public string savePath
        {
            get
            {
                if (_savePath == null)
                {
                    switch (loggerType)
                    {
                        case LoggerType.Unity:
                            //persistentDataPath移动端唯一可读可写的路径
                            Type type = Type.GetType("UnityEngine.Application, UnityEngine");
                            _savePath = type.GetProperty("persistentDataPath").GetValue(null).ToString() + "/Logs/";
                            break;
                        case LoggerType.Console:
                            //AppDomain.CurrentDomain.BaseDirectory，获取基目录，基目录：指应用程序所在的目录
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
        public string SaveName { get => saveName; set => saveName = value; }
    }


    /// <summary>
    /// 日志颜色
    /// </summary>
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


    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILogger
    {
        public Action<string> LogAct { get; set; }
        public Action<string> WarnAct { get; set; }
        public Action<string> ErrorAct { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="LogCoLor"></param>
        public void Log(string msg, LogCoLor LogCoLor = LogCoLor.None);

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg"></param>
        public void Warn(string msg);

        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="msg"></param>
        public void Error(string msg);
    }
}

namespace Core
{
    /// <summary>
    /// Unity
    /// </summary>
    public class UnityDebug : ILogger
    {
        public Action<string> LogAct { get; set; }
        public Action<string> WarnAct { get; set; }
        public Action<string> ErrorAct { get; set; }


        public void Log(string msg, LogCoLor LogCoLor = LogCoLor.None)
        {
            if (LogCoLor != LogCoLor.None)
                msg = ColorUnityLog(msg, LogCoLor);
            LogAct.Invoke(msg);
        }
        public void Warn(string msg)
        {
            WarnAct.Invoke(msg);
        }
        public void Error(string msg)
        {
            ErrorAct.Invoke(msg);
        }


        private string ColorUnityLog(string msg, LogCoLor logCoLor = LogCoLor.None)
        {
            switch (logCoLor)
            {
                default:
                case LogCoLor.None: msg = string.Format($"<coLor=#FF000O>{msg}</coLor>"); break;
                case LogCoLor.DarkRed: msg = string.Format($"<coLor=#FF000O>{msg}</coLor>"); break;
                case LogCoLor.Green: msg = string.Format($"<coLor=#00FF00>{msg}</coLor>"); break;
                case LogCoLor.Blue: msg = string.Format($"<coLor=#0000FF>{msg}</coLor>"); break;
                case LogCoLor.Cyan: msg = string.Format($"<coLor=#00FFFF>{msg}</coLor>"); break;
                case LogCoLor.Magenta: msg = string.Format($"<coLor=#FF00FF>{msg}</coLor>"); break;
                case LogCoLor.DarkYellow: msg = string.Format($"<coLor=#FFff0O>{msg}</coLor>"); break;
            }
            return msg;
        }
    }

    /// <summary>
    /// 控制台
    /// </summary>
    public class ConsoleDebug : ILogger
    {
        public Action<string> LogAct { get; set; }
        public Action<string> WarnAct { get; set; }
        public Action<string> ErrorAct { get; set; }


        public void Log(string msg, LogCoLor LogCoLor = LogCoLor.None)
        {
            Console.ForegroundColor = WriteConsoleLog(LogCoLor);
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public void Warn(string msg)
        {
            Log(msg, LogCoLor.DarkYellow);
        }
        public void Error(string msg)
        {
            Log(msg, LogCoLor.DarkRed);
        }


        public ConsoleColor WriteConsoleLog(LogCoLor logCoLor = LogCoLor.None)
        {
            switch (logCoLor)
            {
                default:
                case LogCoLor.None: return ConsoleColor.White;
                case LogCoLor.DarkRed: return ConsoleColor.DarkRed;
                case LogCoLor.Green: return ConsoleColor.Green; ;
                case LogCoLor.Blue: return ConsoleColor.Blue;
                case LogCoLor.Cyan: return ConsoleColor.Cyan;
                case LogCoLor.Magenta: return ConsoleColor.Magenta;
                case LogCoLor.DarkYellow: return ConsoleColor.DarkYellow;
            }
        }
    }
}

namespace Core
{
    public static class ExtensionDebug
    {
        public static void Log(this string msg, LogCoLor logCoLor = LogCoLor.None, params object[] args) => CDebug.Instance.Log(msg, logCoLor, args);
        public static void Log(this object obj) => CDebug.Instance.Log(obj);
        public static void Warn(this string msg, params object[] args) => CDebug.Instance.Warn(msg, args);
        public static void Error(this string msg, params object[] args) => CDebug.Instance.Error(msg, args);
    }
}