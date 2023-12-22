using System;

/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	事件数据

-----------------------*/

namespace Core
{
    public struct EventCommonData : IComparable<EventCommonData>
    {
        public int id;                  //计数和排序
        public delegate void Event();
        public Event EventAction;

        public int CompareTo(EventCommonData other) => id.CompareTo(other.id);
    }

    public struct EventCommonData<T> : IComparable<EventCommonData<T>>
    {
        public int id;
        public delegate void Event(T t);
        public Event EventAction;

        public int CompareTo(EventCommonData<T> other) => id.CompareTo(other.id);
    }
}
