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
}
