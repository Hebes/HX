//using Cysharp.Threading.Tasks;
//using System;


///*--------脚本描述-----------

//描述:
//	等待监听

//-----------------------*/

//namespace Core
//{
//    public class EventInfoAsync : IEvent
//    {
//        public delegate IEnumerator EventAsync();
//        public event EventAsync eventAsync;

//        public EventInfoAsync(EventAsync eventAsync)
//        {
//            this.eventAsync += eventAsync;
//        }
//        public IEnumerator TriggerAsync()
//        {
//            Delegate[] actionIEnumeratorEventDelegate = eventAsync.GetInvocationList();//委托的调用列表。
//            yield return IEnumerator.WhenAll(Array.ConvertAll(actionIEnumeratorEventDelegate, del => ((EventAsync)del)()));//按顺序执行
//        }
//    }

//    public class EventInfoAsync<T> : IEvent
//    {
//        public delegate IEnumerator EventAsync(T t);
//        public event EventAsync eventAsync;

//        public EventInfoAsync(EventAsync eventAsync)
//        {
//            this.eventAsync += eventAsync;
//        }
//        public IEnumerator TriggerAsync(T obj)
//        {
//            Delegate[] actionIEnumeratorEventDelegate = eventAsync.GetInvocationList();
//            yield return IEnumerator.WhenAll(Array.ConvertAll(actionIEnumeratorEventDelegate, del => ((EventAsync)del)(obj)));
//        }
//    }

//    public class EventInfoAsync<T, K> : IEvent
//    {
//        public delegate IEnumerator EventAsync(T t, K k);
//        public event EventAsync eventAsync;
//        public EventInfoAsync(EventAsync eventAsync)
//        {
//            this.eventAsync += eventAsync;
//        }
//        public IEnumerator TriggerAsync(T obj1, K obj2)
//        {
//            Delegate[] actionIEnumeratorEventDelegate = eventAsync.GetInvocationList();
//            yield return IEnumerator.WhenAll(Array.ConvertAll(actionIEnumeratorEventDelegate, del => ((EventAsync)del)(obj1, obj2)));
//        }
//    }
//}
