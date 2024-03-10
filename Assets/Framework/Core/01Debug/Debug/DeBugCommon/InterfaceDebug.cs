namespace Core
{
    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILogger
    {
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
}
