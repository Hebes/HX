using System;

/*--------脚本描述-----------

描述:
	事件数据

-----------------------*/

namespace Core
{
    public struct EventCommonData : IComparable<EventCommonData>, IEvent
    {

        public long ID { get; set; }//用于排序
        public string MethodName { get; set; }// 方法名称
        public Action EventAction { get; set; }


        public int CompareTo(EventCommonData other) => ID.CompareTo(other.ID);
    }

    public struct EventCommonData<T> : IComparable<EventCommonData<T>>, IEvent
    {
        public delegate void Event(T t);

        public long ID { get; set; }
        public Event EventAction;
        public string MethodName { get; set; }

        public int CompareTo(EventCommonData<T> other) => ID.CompareTo(other.ID);
    }
}
