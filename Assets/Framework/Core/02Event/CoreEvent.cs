using System;
using System.Collections.Generic;
using Debug= UnityEngine.Debug;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
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
        #endregion
    }
}
