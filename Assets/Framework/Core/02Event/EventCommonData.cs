using System;
using System.Collections.Generic;

/*--------脚本描述-----------

描述:
	事件数据

-----------------------*/

namespace Core
{
    public struct EventCommonData : IComparable<EventCommonData>, IEvent
    {

        public long ID { get; set; }                //用于排序
        public string MethodName { get; set; }      // 方法名称
        public Action EventAction { get; set; }     //事件
        public int CompareTo(EventCommonData other) => ID.CompareTo(other.ID); //排序
    }

    public struct EventCommonData<T> : IComparable<EventCommonData<T>>, IEvent
    {
        public long ID { get; set; }
        public Action<T> EventAction { get; set; }
        public string MethodName { get; set; }
        public int CompareTo(EventCommonData<T> other) => ID.CompareTo(other.ID);
    }
}
