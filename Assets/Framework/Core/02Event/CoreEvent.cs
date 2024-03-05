using System;
using System.Collections;
using System.Collections.Generic;

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

        public Action<string> Log { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action<string> Warn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action<string> Error { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IEnumerator ICoreInit()
        {
            Instance = this;
            eventDic = new Dictionary<int, List<IEvent>>();
            yield return null;
        }

        //清理
        public static void Clear()
        {
            Instance.eventDic.Clear();
        }
    }
}
