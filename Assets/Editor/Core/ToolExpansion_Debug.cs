using System.Reflection;
using UnityEditor;

namespace Tool
{
    public static  class ToolExpansion_Debug
    {
        /// <summary>
        /// 清空日志
        /// </summary>
        public static void ClearConsole()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
            System.Type logEntries = assembly.GetType("UnityEditor.LogEntries");
            MethodInfo clearConsoleMethod = logEntries.GetMethod("Clear");
            clearConsoleMethod.Invoke(new object(), null);
        }
    }
}
