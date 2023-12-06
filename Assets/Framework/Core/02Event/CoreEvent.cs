using Cysharp.Threading.Tasks;
using System;
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
    public partial class CoreEvent : ICore
    {
        public static CoreEvent Instance;
        private Dictionary<int, IEvent> eventDic;

        public void ICoreInit()
        {
            Instance = this;
            eventDic = new Dictionary<int, IEvent>();
        }

        //清理
        public static void Clear()
        {
            Instance.eventDic.Clear();
        }
    }
}
