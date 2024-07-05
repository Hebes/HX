using System.Collections.Generic;
using System;

/*--------脚本描述-----------

描述:
    普通事件

-----------------------*/


namespace Framework.Core
{
    public partial class CoreEvent
    {
        /// <summary>
        /// 检查
        /// </summary>
        /// <param name="id"></param>
        private bool CheckEventList(List<IEvent> eventInfoList, string methodName)
        {
            foreach (IEvent item in eventInfoList)
            {
                if (item.MethodName != methodName) continue;
                EDebug.Error($"{methodName}方法已经添加");
                return true;
            }
            return false;
        }


        public static void EventAdd(int id, Action action, int listid = int.MaxValue)
        {
            if (Instance.eventDic.TryGetValue(id, out List<IEvent> eventInfo))
            {
                if (Instance.CheckEventList(eventInfo, action.Method.Name))
                    return;
            }
            else
            {
                Instance.eventDic.Add(id, new List<IEvent>());
            }

            eventInfo.Add(new EventCommonData()
            {
                ID = listid,
                EventAction = action,
                MethodName = action.Method.Name,
            });
        }
        public static void EventRemove(int id, Action action)
        {
            if (Instance.eventDic.TryGetValue(id, out List<IEvent> eventInfo))
            {
                foreach (IEvent temp in eventInfo)
                {
                    if (temp.MethodName != action.Method.Name) continue;
                    eventInfo.Remove(temp);
                    break;
                }
            }
        }
        public static void EventSort(int id)
        {
            if (Instance.eventDic.TryGetValue(id, out List<IEvent> eventInfo))
                eventInfo.Sort();
        }
        public static void EventTrigger(int id)
        {
            //如果显示空指针异常,请检查监听的参数和触发的参数是否一致
            if (Instance.eventDic.TryGetValue(id, out List<IEvent> eventInfo))
            {
                foreach (IEvent item in eventInfo)
                {
                    if (item is EventCommonData @event)
                        @event.EventAction.Invoke();
                }
            }
        }



        public static void EventAdd<T>(int id, Action<T> action, int listid = int.MaxValue)
        {
            if (Instance.eventDic.TryGetValue(id, out List<IEvent> eventInfo))
            {
                if (Instance.CheckEventList(eventInfo, action.Method.Name))
                    return;
            }
            else
            {
                Instance.eventDic.Add(id, new List<IEvent>());
            }

            eventInfo.Add(new EventCommonData<T>()
            {
                ID = listid,
                EventAction = action,
                MethodName = action.Method.Name,
            });
        }
        public static void EventRemove<T>(int id, Action<T> action)
        {
            if (Instance.eventDic.TryGetValue(id, out List<IEvent> eventInfo))
            {
                foreach (IEvent temp in eventInfo)
                {
                    if (temp.MethodName != action.Method.Name) continue;
                    eventInfo.Remove(temp);
                    break;
                }
            }
        }
        public static void EventTrigger<T>(int id, T t)
        {
            //如果显示空指针异常,请检查监听的参数和触发的参数是否一致
            if (Instance.eventDic.TryGetValue(id, out List<IEvent> eventInfo))
            {
                foreach (IEvent item in eventInfo)
                {
                    if (item is EventCommonData<T> @event)
                        @event.EventAction.Invoke(t);
                }
            }
        }



        //public static void EventAdd<T, K>(int id, EventInfoCommon<T, K>.CommonEvent commonAction)
        //{
        //    if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
        //        (eventInfo as EventInfoCommon<T, K>).commonAction += commonAction;
        //    else
        //        Instance.eventDic.Add(id, new EventInfoCommon<T, K>(commonAction));
        //}
        //public static void EventRemove<T, K>(int id, EventInfoCommon<T, K>.CommonEvent commonAction)
        //{
        //    if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
        //        (eventInfo as EventInfoCommon<T, K>).commonAction -= commonAction;
        //}
        //public static void EventTrigger<T, K>(int id, T t, K k)
        //{
        //    if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
        //        (eventInfo as EventInfoCommon<T, K>).Trigger(t, k);//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
        //}



        //public static void EventAdd<T, K, V>(int id, EventInfoCommon<T, K, V>.CommonEvent commonAction)
        //{
        //    if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
        //        (eventInfo as EventInfoCommon<T, K, V>).commonAction += commonAction;
        //    else
        //        Instance.eventDic.Add(id, new EventInfoCommon<T, K, V>(commonAction));
        //}
        //public static void EventRemove<T, K, V>(int id, EventInfoCommon<T, K, V>.CommonEvent commonAction)
        //{
        //    if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
        //        (eventInfo as EventInfoCommon<T, K, V>).commonAction -= commonAction;
        //}
        //public static void EventTrigger<T, K, V>(int id, T t, K k, V v)
        //{
        //    if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
        //        (eventInfo as EventInfoCommon<T, K, V>).Trigger(t, k, v);//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
        //}



        //public static void EventAdd<T, K, V, N>(int id, EventInfoCommon<T, K, V, N>.CommonEvent commonAction)
        //{
        //    if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
        //        (eventInfo as EventInfoCommon<T, K, V, N>).commonAction += commonAction;
        //    else
        //        Instance.eventDic.Add(id, new EventInfoCommon<T, K, V, N>(commonAction));
        //}
        //public static void EventRemove<T, K, V, N>(int id, EventInfoCommon<T, K, V, N>.CommonEvent commonAction)
        //{
        //    if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
        //        (eventInfo as EventInfoCommon<T, K, V, N>).commonAction -= commonAction;
        //}
        //public static void EventTrigger<T, K, V, N>(int id, T t, K k, V v, N n)
        //{
        //    if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
        //        (eventInfo as EventInfoCommon<T, K, V, N>).Trigger(t, k, v, n);//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
        //}



        //public static void EventAdd<T, K, V, N, M>(int id, EventInfoCommon<T, K, V, N, M>.CommonEvent commonAction)
        //{
        //    if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
        //        (eventInfo as EventInfoCommon<T, K, V, N, M>).commonAction += commonAction;
        //    else
        //        Instance.eventDic.Add(id, new EventInfoCommon<T, K, V, N, M>(commonAction));
        //}
        //public static void EventRemove<T, K, V, N, M>(int id, EventInfoCommon<T, K, V, N, M>.CommonEvent commonAction)
        //{
        //    if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
        //        (eventInfo as EventInfoCommon<T, K, V, N, M>).commonAction -= commonAction;
        //}
        //public static void EventTrigger<T, K, V, N, M>(int id, T t, K k, V v, N n, M m)
        //{
        //    if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
        //        (eventInfo as EventInfoCommon<T, K, V, N, M>).Trigger(t, k, v, n, m);//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
        //}
    }
}
