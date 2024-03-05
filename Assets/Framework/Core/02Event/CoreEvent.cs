using System;
using System.Collections.Generic;

/*--------脚本描述-----------

描述:
    事件中心模块

-----------------------*/

namespace Core
{
    public partial class CoreEvent : ICore, IDebug
    {
        public static CoreEvent Instance;
        private Dictionary<int, List<IEvent>> eventDic;

        public void ICoreInit()
        {
            Instance = this;
            eventDic = new Dictionary<int, List<IEvent>>();
            AddDebuggerAction();
        }

        #region IDebug
        public Action<string> Log { get; set; }
        public Action<string> Warn { get; set; }
        public Action<string> Error { get; set; }

        private void AddDebuggerAction()
        {
            this.AddDebuggerAction(UnityEngine.Debug.Log, UnityEngine.Debug.LogWarning, UnityEngine.Debug.LogError);
        }
        private void DebugLog(string content) => Log.Invoke(content);
        private void DebugWarn(string content) => Warn.Invoke(content);
        private void DebugError(string content) => Error.Invoke(content);
        #endregion
    }
}


/*--------脚本描述-----------

描述:
	事件接口

-----------------------*/

namespace Core
{
    public interface IEvent : IID
    {
        /// <summary>
        /// 方法名称
        /// </summary>
        public string MethodName { get; set; }
    }
}
