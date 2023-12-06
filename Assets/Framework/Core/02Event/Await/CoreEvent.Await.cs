//using Cysharp.Threading.Tasks;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

///*--------脚本描述-----------

//描述:
//    等待

//-----------------------*/

//namespace Core
//{
//    public partial class CoreEvent
//    {

//        public static void EventAddAsync(string eventName, EventInfoAsync.EventAsync eventAsync)
//        {
//            if (Instance.eventDic.TryGetValue(eventName, out IEvent eventInfo))
//                (eventInfo as EventInfoAsync).eventAsync += eventAsync;
//            else
//                Instance.eventDic.Add(eventName, new EventInfoAsync(eventAsync));
//        }
//        public static async UniTask EventTriggerAsync(string eventName)
//        {
//            if (Instance.eventDic.TryGetValue(eventName, out IEvent eventInfo))
//                await (eventInfo as EventInfoAsync).TriggerAsync();//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
//        }


//        public static void EventAddAsync<T>(string eventName, EventInfoAsync<T>.EventAsync eventAsync)
//        {
//            if (Instance.eventDic.TryGetValue(eventName, out IEvent eventInfo))
//                (eventInfo as EventInfoAsync<T>).eventAsync += eventAsync;
//            else
//                Instance.eventDic.Add(eventName, new EventInfoAsync<T>(eventAsync));
//        }
//        public static async UniTask EventTriggerAsync<T>(string eventName, T t)
//        {
//            if (Instance.eventDic.TryGetValue(eventName, out IEvent eventInfo))
//                await (eventInfo as EventInfoAsync<T>).TriggerAsync(t);//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
//        }


//        public static void EventAddAsync<T, K>(string eventName, EventInfoAsync<T, K>.EventAsync eventAsync)
//        {
//            if (Instance.eventDic.TryGetValue(eventName, out IEvent eventInfo))
//                (eventInfo as EventInfoAsync<T, K>).eventAsync += eventAsync;
//            else
//                Instance.eventDic.Add(eventName, new EventInfoAsync<T, K>(eventAsync));
//        }
//        public static async UniTask EventTriggerAsync<T, K>(string eventName, T t, K k)
//        {
//            if (Instance.eventDic.TryGetValue(eventName, out IEvent eventInfo))
//                await (eventInfo as EventInfoAsync<T, K>).TriggerAsync(t, k);//如果显示空指针异常,请检查监听的参数和触发的参数是否一致
//        }
//    }
//}
