///*--------脚本描述-----------

//描述:
//    有返回值事件监听

//-----------------------*/

//namespace Core
//{
//    public partial class CoreEvent
//    {
//        public static void EventAddReturn(int id, EventInfoReturn.EventReturn eventAsync)
//        {
//            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
//                (eventInfo as EventInfoReturn).eventReturn += eventAsync;
//            else
//                Instance.eventDic.Add(id, new EventInfoReturn(eventAsync));
//        }
//        public static R EventTriggerReturn<R>(int id) where R : UnityEngine.Object
//        {
//            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
//                return (eventInfo as EventInfoReturn).Trigger<R>();//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
//            return null;
//        }

//        public static void EventAddReturn<T>(int id, EventInfoReturn<T>.EventReturn eventAsync)
//        {
//            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
//                (eventInfo as EventInfoReturn<T>).eventReturn += eventAsync;
//            else
//                Instance.eventDic.Add(id, new EventInfoReturn<T>(eventAsync));
//        }
//        /// <summary>
//        /// 事件触发
//        /// </summary>
//        /// <typeparam name="R">返回的类型</typeparam>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="id"></param>
//        /// <param name="t"></param>
//        /// <returns></returns>
//        public static R EventTriggerReturn<R, T>(int id, T t) where R : UnityEngine.Object
//        {
//            if (Instance.eventDic.TryGetValue(id, out IEvent eventInfo))
//                return (eventInfo as EventInfoReturn<T>).Trigger<R>(t);//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
//            return null;
//        }
//    }
//}
