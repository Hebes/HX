using System;
using System.Collections;
using System.Collections.Generic;

/*--------脚本描述-----------

描述:
    事件中心模块

-----------------------*/

namespace Core
{
    public partial class CoreEvent : ICore
    {
        public static CoreEvent Instance;
        private Dictionary<int, List<IEvent>> eventDic;

        public void Init()
        {
            Instance = this;
            eventDic = new Dictionary<int, List<IEvent>>();
        }
        public IEnumerator AsyncInit()
        {
           yield return null;
        }
    }
}

namespace Core
{
    public static class ExtensionEvent 
    {
        
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
