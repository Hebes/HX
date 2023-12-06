/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    普通事件监听

-----------------------*/

using Farm2D;
using System.Collections.Generic;

namespace Core
{
    public class EventCommon : IEvent
    {
        private List<EventCommonData> commonEventsList;
        public EventCommon(EventCommonData.Event @event, int id = int.MaxValue)
        {
            commonEventsList = new List<EventCommonData>();
            Add(@event);
        }
        public void Add(EventCommonData.Event @event, int id = int.MaxValue)
        {
            EventCommonData eventCommonData = new EventCommonData();
            eventCommonData.id = id;//最大的就是最后一个数
            eventCommonData.EventAction = @event;//最大的就是最后一个数
            commonEventsList.Add(eventCommonData);
        }
        public void Remove(EventCommonData.Event @event)
        {
            for (int i = 0; i < commonEventsList.Count; i++)
            {
                if (commonEventsList[i].EventAction == @event)
                    commonEventsList.Remove(commonEventsList[i]);
            }
        }
        public void Sort() => commonEventsList.Sort();
        public void Trigger()
        {
            for (int i = 0; i < commonEventsList.Count; i++)
                commonEventsList[i].EventAction.Invoke();
        }
    }

    public class EventCommon<T> : IEvent
    {
        private List<EventCommonData<T>> commonEventsList;
        public EventCommon(EventCommonData<T> eventCommonData)
        {
            commonEventsList = new List<EventCommonData<T>>();
            Add(eventCommonData);
        }
        public void Add(EventCommonData<T> eventData)
        {
            commonEventsList.Add(eventData);
        }
        public void Remove(EventCommonData<T> eventData)
        {
            commonEventsList.Remove(eventData);
        }
        public void Sort() => commonEventsList.Sort();
        public void Trigger(T t)
        {
            for (int i = 0; i < commonEventsList.Count; i++)
                commonEventsList[i].EventAction.Invoke(t);
        }
    }

    public class EventInfoCommon<T, K> : IEvent
    {
        public delegate void CommonEvent(T t, K k);
        public event CommonEvent commonAction;

        public EventInfoCommon(CommonEvent commonAction)
        {
            this.commonAction += commonAction;
        }
        public void Trigger(T obj, K obj2)
        {
            commonAction?.Invoke(obj, obj2);
        }
    }

    public class EventInfoCommon<T, K, V> : IEvent
    {
        public delegate void CommonEvent(T t, K k, V v);
        public event CommonEvent commonAction;

        public EventInfoCommon(CommonEvent commonAction)
        {
            this.commonAction += commonAction;
        }
        public void Trigger(T t, K k, V v)
        {
            commonAction?.Invoke(t, k, v);
        }
    }

    public class EventInfoCommon<T, K, V, N> : IEvent
    {
        public delegate void CommonEvent(T t, K k, V v, N n);
        public event CommonEvent commonAction;

        public EventInfoCommon(CommonEvent commonAction)
        {
            this.commonAction += commonAction;
        }
        public void Trigger(T t, K k, V v, N n)
        {
            commonAction?.Invoke(t, k, v, n);
        }
    }

    public class EventInfoCommon<T, K, V, N, M> : IEvent
    {
        public delegate void CommonEvent(T t, K k, V v, N n, M m);
        public event CommonEvent commonAction;

        public EventInfoCommon(CommonEvent commonAction)
        {
            this.commonAction += commonAction;
        }
        public void Trigger(T t, K k, V v, N n, M m)
        {
            commonAction?.Invoke(t, k, v, n, m);
        }
    }
}
