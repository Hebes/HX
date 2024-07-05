#define COREDUBUGOPEN
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

/*--------脚本描述-----------

描述:
    日志总控
    Project->Player->Other Settings->Script Define Symbols

-----------------------*/

namespace Framework.Core
{
    public class DBug
    {
        public static DBug Instance { get; private set; }
        private LogConfig _cfg; //配置文件
        private StreamWriter _logFiLeWriter = null; //输出流,整个日志文件
        private const string LOGLock = "LogLock"; //日志锁
        private ILogger _logger = null; //日志接口
        private StringBuilder _sb;

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

        public void Init(LogConfig cfg, Action<string> log, Action<string> warn, Action<string> error)
        {
            Instance = this;
            _sb = new();
            //日志类型
            _logger = cfg.LoggerType switch
            {
                LoggerType.Unity => new UnityDebug(),
                LoggerType.Console => new ConsoleDebug(),
                _ => _logger
            };
            _logger.LogAct = log;
            _logger.WarnAct = warn;
            _logger.ErrorAct = error;


            _cfg = cfg ?? new LogConfig();
            //是否启用日志保存
            if (!cfg.EnableSave) return;
            //日志覆盖
            if (cfg.EnableCover) //覆盖
            {
                var path = $"{cfg.SavePath}{cfg.SaveName}";
                try
                {
                    if (!Directory.Exists(cfg.SavePath)) //存在这个路径
                        Directory.CreateDirectory(cfg.SavePath);
                    if (File.Exists(path)) //存在这个文件
                        File.Delete(path);
                    _logFiLeWriter = File.AppendText(path);
                    _logFiLeWriter.AutoFlush = true;
                }
                catch (Exception)
                {
                    _logFiLeWriter = null;
                }
            }
            else
            {
                var path = $"{cfg.SavePath}{DateTime.Now:yyyyMMdd@HH-mm-s}{cfg.SaveName}";
                try
                {
                    log("日志输出路径：" + path);
                    if (Directory.Exists(cfg.SavePath) == false)
                        Directory.CreateDirectory(cfg.SavePath);
                    _logFiLeWriter = File.AppendText(path);
                    _logFiLeWriter.AutoFlush = true;
                }
                catch (Exception)
                {
                    _logFiLeWriter = null;
                }
            }
        }

        //日志 Project->Player->Other Settings->Script Define Symbols
        [Conditional("COREDUBUGOPEN")]
        public void Log(string msg, LogCoLor logCoLor, params object[] args)
        {
            lock (LOGLock)
            {
                if (_cfg.EnableLog == false) return;
                msg = DecorateLog(string.Format(msg, args));
                _logger.Log(msg, logCoLor);
                if (_cfg.EnableSave)
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
            lock (LOGLock)
            {
                if (_cfg.EnableLog == false) return;
                msg = DecorateLog(string.Format(msg, args), true);
                _logger.Log(msg, LogCoLor.Magenta);
                if (_cfg.EnableSave)
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
            lock (LOGLock)
            {
                if (_cfg.EnableLog == false) return;
                msg = DecorateLog(string.Format(msg, args));
                _logger.Warn(msg);
                if (_cfg.EnableSave)
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
            lock (LOGLock)
            {
                if (_cfg.EnableLog == false)return;
                msg = DecorateLog(string.Format(msg, args), true);
                _logger.Error(msg);
                if (_cfg.EnableSave)
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
            _sb.Clear();
            _sb.Capacity = 100;
            lock (LOGLock)
            {
                _sb.Append(_cfg.LogPrefix);
                //StringBuilder sb = new StringBuilder(cfg.LogPrefix, 100);
                if (_cfg.EnableTime) //启用时间
                    _sb.AppendFormat($"时间:{DateTime.Now:hh:mm:ss--fff}");
                if (_cfg.EnableMillisecond)
                    _sb.AppendFormat($"调用时间(精确到毫秒级){DateTime.Now.TimeOfDay}");
                if (_cfg.EnableThreadID) //启用线程
                    _sb.AppendFormat($"{GetThreadID()}");
                _sb.AppendFormat($" {_cfg.LogSeparate} {msg}"); //日志分离
                if (isTrace) //是否追踪日志 堆栈
                    _sb.AppendFormat($" \nStackTrace:{GetLogTrace()}");
                return _sb.ToString();
            }
        }

        //获取日志追踪
        private string GetLogTrace()
        {
            var st = new StackTrace(3, true); //3 跳跃3帧 true-> 获取场下文信息
            var traceInfo = string.Empty;
            for (var i = 0; i < st.FrameCount; i++)
            {
                var sf = st.GetFrame(i);
                //traceInfo += string.Format($"\n\t{sf.GetFileName()}::{sf.GetMethod()}line:{sf.GetFileLineNumber()}");
                //traceInfo += string.Format($"\n\t{sf.GetFileName()}::\n\t{sf.GetMethod()}\tline:{sf.GetFileLineNumber()}");
                traceInfo += string.Format($"\n\t脚本:{sf.GetFileName()}::方法{sf.GetMethod()}行: {sf.GetFileLineNumber()}");
            }

            return traceInfo;
        }

        //获取线程Id
        private object GetThreadID()
        {
            return string.Format($" 线程ID:{Thread.CurrentThread.ManagedThreadId}"); //ThreadID
        }

        //日志写入文件
        private void WriteToFile(string msg)
        {
            if (!_cfg.EnableSave) return;
            if (_logFiLeWriter == null) return;
            try
            {
                _logFiLeWriter.WriteLine(msg);
            }
            catch
            {
                _logFiLeWriter = null;
            }
        }

        //打印数组数据For Debug
        public void PrintBytesArray(byte[] bytes, string prefix, Action<string> printer = null)
        {
            var str = prefix + "->\n";
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i % 10 == 0)
                    str += bytes[i] + "\n";

                str += bytes[i] + " ";
            }

            if (printer != null)
                printer(str);
            else
                Log(str);
        }

        #endregion
    }
}

namespace Framework.Core
{
    /// <summary> 日志配置 </summary>
    public class LogConfig
    {
        /// <summary> 启用日志 </summary>
        public bool EnableLog = true;

        /// <summary> 日志前缀 </summary>
        public string LogPrefix = "#";

        /// <summary> 是否启用时间 </summary>
        public bool EnableTime = true;

        /// <summary> 是否启用毫秒级调用(可以定位代码的调用顺序) </summary>
        public bool EnableMillisecond = true;

        /// <summary> 日志分离 </summary>
        public string LogSeparate = ">>";

        /// <summary> 启用线程ID </summary>
        public bool EnableThreadID = true;

        /// <summary> 启用跟踪 </summary>
        public bool EnableTrace = true;

        /// <summary> 启用保存 </summary>
        public bool EnableSave = true;

        /// <summary> 日志覆盖 </summary>
        public bool EnableCover = false;

        /// <summary> 保存路径 </summary>
        private string _savePath;

        /// <summary> 日志类型 </summary>
        public LoggerType LoggerType = LoggerType.Unity;

        /// <summary> 日志保存保存路径 </summary>
        public string SavePath
        {
            get
            {
                if (_savePath != null) return _savePath;
                switch (LoggerType)
                {
                    case LoggerType.Unity:
                        //persistentDataPath移动端唯一可读可写的路径
                        var type = Type.GetType("UnityEngine.Application, UnityEngine");
                        _savePath = type.GetProperty("persistentDataPath").GetValue(null).ToString() + "/Logs/";
                        break;
                    case LoggerType.Console:
                        //AppDomain.CurrentDomain.BaseDirectory，获取基目录，基目录：指应用程序所在的目录
                        _savePath = string.Format($"{AppDomain.CurrentDomain.BaseDirectory}Logs\\");
                        break;
                }

                return _savePath;
            }
            set => _savePath = value;
        }

        /// <summary> 保存名称 </summary>
        public string SaveName { get; set; } = "Log.txt";
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

        public void Log(string msg, LogCoLor logCoLor = LogCoLor.None);
        public void Warn(string msg);
        public void Error(string msg);
    }
}

namespace Framework.Core
{
    public class UnityDebug : ILogger
    {
        public Action<string> LogAct { get; set; }
        public Action<string> WarnAct { get; set; }
        public Action<string> ErrorAct { get; set; }


        public void Log(string msg, LogCoLor logCoLor = LogCoLor.None)
        {
            if (logCoLor != LogCoLor.None)
                msg = ColorUnityLog(msg, logCoLor);
            LogAct.Invoke(msg);
        }

        public void Warn(string msg) => WarnAct.Invoke(msg);
        public void Error(string msg) => ErrorAct.Invoke(msg);

        private string ColorUnityLog(string msg, LogCoLor logCoLor = LogCoLor.None)
        {
            switch (logCoLor)
            {
                default:
                case LogCoLor.None:
                    msg = string.Format($"<coLor=#FF000O>{msg}</coLor>");
                    break;
                case LogCoLor.DarkRed:
                    msg = string.Format($"<coLor=#FF000O>{msg}</coLor>");
                    break;
                case LogCoLor.Green:
                    msg = string.Format($"<coLor=#00FF00>{msg}</coLor>");
                    break;
                case LogCoLor.Blue:
                    msg = string.Format($"<coLor=#0000FF>{msg}</coLor>");
                    break;
                case LogCoLor.Cyan:
                    msg = string.Format($"<coLor=#00FFFF>{msg}</coLor>");
                    break;
                case LogCoLor.Magenta:
                    msg = string.Format($"<coLor=#FF00FF>{msg}</coLor>");
                    break;
                case LogCoLor.DarkYellow:
                    msg = string.Format($"<coLor=#FFff0O>{msg}</coLor>");
                    break;
            }

            return msg;
        }
    }

    public class ConsoleDebug : ILogger
    {
        public Action<string> LogAct { get; set; }
        public Action<string> WarnAct { get; set; }
        public Action<string> ErrorAct { get; set; }

        public void Log(string msg, LogCoLor logCoLor = LogCoLor.None)
        {
            Console.ForegroundColor = WriteConsoleLog(logCoLor);
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void Warn(string msg) => Log(msg, LogCoLor.DarkYellow);
        public void Error(string msg) => Log(msg, LogCoLor.DarkRed);

        private ConsoleColor WriteConsoleLog(LogCoLor logCoLor = LogCoLor.None)
        {
            switch (logCoLor)
            {
                default:
                case LogCoLor.None: return ConsoleColor.White;
                case LogCoLor.DarkRed: return ConsoleColor.DarkRed;
                case LogCoLor.Green:
                    return ConsoleColor.Green;
                    ;
                case LogCoLor.Blue: return ConsoleColor.Blue;
                case LogCoLor.Cyan: return ConsoleColor.Cyan;
                case LogCoLor.Magenta: return ConsoleColor.Magenta;
                case LogCoLor.DarkYellow: return ConsoleColor.DarkYellow;
            }
        }
    }
}

namespace Framework.Core
{
    public static class EDebug
    {
        public static void Log(this string msg, LogCoLor logCoLor = LogCoLor.None, params object[] args) => DBug.Instance.Log(msg, logCoLor, args);
        public static void Log(this object obj) => DBug.Instance.Log(obj);
        public static void Log(this object obj, string str) => DBug.Instance.Log(str);
        public static void Warn(this string msg, params object[] args) => DBug.Instance.Warn(msg, args);
        public static void Error(this string msg, params object[] args) => DBug.Instance.Error(msg, args);
        public static void Error(this object obj, string msg, params object[] args) => Error(msg, args);
    }
}