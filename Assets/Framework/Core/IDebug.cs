using System;
using System.Diagnostics;

namespace Core
{
    public interface IDebug
    {
        public Action<string> Log { get; set; }
        public Action<string> Warn { get; set; }
        public Action<string> Error { get; set; }
    }

    public static class Debugger
    {
        public static void AddDebuggerAction(this IDebug debug, Action<string> Log, Action<string> Warn, Action<string> Error)
        {
            debug.Log = Log;
            debug.Warn = Warn;
            debug.Error = Error;
        }

        [Conditional("CORE_DUBUG_OPEN")]
        public static void Log(this IDebug debug, string content) => debug.Log.Invoke(content);

        public static void Warn(this IDebug debug, string content) => debug.Warn.Invoke(content);

        public static void Error(this IDebug debug, string content) => debug.Error.Invoke(content);
    }
}

