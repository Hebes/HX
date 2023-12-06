using Farm2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

/*--------脚本描述-----------

描述:
    普通事件

-----------------------*/

namespace Core
{
    public partial class CoreEvent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="eventCommonData"></param>
        /// <param name="listid">事件排序ID</param>
        public static void EventAdd(int id, EventCommonData.Event eventCommonData, int listid = int.MaxValue)
        {
            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
                (eventInfo as EventCommon).Add(eventCommonData, listid);
            else
                Instance.eventDic.Add(id, new EventCommon(eventCommonData, listid));
        }
        public static void EventRemove(int id, EventCommonData.Event @event)
        {
            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
                (eventInfo as EventCommon).Remove(@event);
        }
        public static void EventSort(int id)
        {
            //如果显示空指针异常,请检查监听的参数和触发的参数是否一致
            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
                (eventInfo as EventCommon).Sort();
        }
        public static void EventTrigger(int id)
        {
            //如果显示空指针异常,请检查监听的参数和触发的参数是否一致
            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
                (eventInfo as EventCommon).Trigger();
        }



        public static void EventAdd<T>(int id, EventCommonData<T> eventCommonData)
        {
            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
                (eventInfo as EventCommon<T>).Add(eventCommonData);
            else
                Instance.eventDic.Add(id, new EventCommon<T>(eventCommonData));
        }
        public static void EventRemove<T>(int id, EventCommonData<T> eventCommonData)
        {
            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
                (eventInfo as EventCommon<T>).Remove(eventCommonData);
        }
        public static void EventTrigger<T>(int id, T t)
        {
            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
                (eventInfo as EventCommon<T>).Trigger(t);//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
        }



        public static void EventAdd<T, K>(int id, EventInfoCommon<T, K>.CommonEvent commonAction)
        {
            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
                (eventInfo as EventInfoCommon<T, K>).commonAction += commonAction;
            else
                Instance.eventDic.Add(id, new EventInfoCommon<T, K>(commonAction));
        }
        public static void EventRemove<T, K>(int id, EventInfoCommon<T, K>.CommonEvent commonAction)
        {
            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
                (eventInfo as EventInfoCommon<T, K>).commonAction -= commonAction;
        }
        public static void EventTrigger<T, K>(int id, T t, K k)
        {
            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
                (eventInfo as EventInfoCommon<T, K>).Trigger(t, k);//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
        }



        public static void EventAdd<T, K, V>(int id, EventInfoCommon<T, K, V>.CommonEvent commonAction)
        {
            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
                (eventInfo as EventInfoCommon<T, K, V>).commonAction += commonAction;
            else
                Instance.eventDic.Add(id, new EventInfoCommon<T, K, V>(commonAction));
        }
        public static void EventRemove<T, K, V>(int id, EventInfoCommon<T, K, V>.CommonEvent commonAction)
        {
            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
                (eventInfo as EventInfoCommon<T, K, V>).commonAction -= commonAction;
        }
        public static void EventTrigger<T, K, V>(int id, T t, K k, V v)
        {
            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
                (eventInfo as EventInfoCommon<T, K, V>).Trigger(t, k, v);//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
        }



        public static void EventAdd<T, K, V, N>(int id, EventInfoCommon<T, K, V, N>.CommonEvent commonAction)
        {
            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
                (eventInfo as EventInfoCommon<T, K, V, N>).commonAction += commonAction;
            else
                Instance.eventDic.Add(id, new EventInfoCommon<T, K, V, N>(commonAction));
        }
        public static void EventRemove<T, K, V, N>(int id, EventInfoCommon<T, K, V, N>.CommonEvent commonAction)
        {
            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
                (eventInfo as EventInfoCommon<T, K, V, N>).commonAction -= commonAction;
        }
        public static void EventTrigger<T, K, V, N>(int id, T t, K k, V v, N n)
        {
            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
                (eventInfo as EventInfoCommon<T, K, V, N>).Trigger(t, k, v, n);//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
        }



        public static void EventAdd<T, K, V, N, M>(int id, EventInfoCommon<T, K, V, N, M>.CommonEvent commonAction)
        {
            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
                (eventInfo as EventInfoCommon<T, K, V, N, M>).commonAction += commonAction;
            else
                Instance.eventDic.Add(id, new EventInfoCommon<T, K, V, N, M>(commonAction));
        }
        public static void EventRemove<T, K, V, N, M>(int id, EventInfoCommon<T, K, V, N, M>.CommonEvent commonAction)
        {
            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
                (eventInfo as EventInfoCommon<T, K, V, N, M>).commonAction -= commonAction;
        }
        public static void EventTrigger<T, K, V, N, M>(int id, T t, K k, V v, N n, M m)
        {
            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
                (eventInfo as EventInfoCommon<T, K, V, N, M>).Trigger(t, k, v, n, m);//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
        }
    }
}
