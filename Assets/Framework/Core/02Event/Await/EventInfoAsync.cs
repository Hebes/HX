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
//        public delegate UniTask EventAsync();
//        public event EventAsync eventAsync;

//        public EventInfoAsync(EventAsync eventAsync)
//        {
//            this.eventAsync += eventAsync;
//        }
//        public async UniTask TriggerAsync()
//        {
//            Delegate[] actionUniTaskEventDelegate = eventAsync.GetInvocationList();//委托的调用列表。
//            await UniTask.WhenAll(Array.ConvertAll(actionUniTaskEventDelegate, del => ((EventAsync)del)()));//按顺序执行
//        }
//    }

//    public class EventInfoAsync<T> : IEvent
//    {
//        public delegate UniTask EventAsync(T t);
//        public event EventAsync eventAsync;

//        public EventInfoAsync(EventAsync eventAsync)
//        {
//            this.eventAsync += eventAsync;
//        }
//        public async UniTask TriggerAsync(T obj)
//        {
//            Delegate[] actionUniTaskEventDelegate = eventAsync.GetInvocationList();
//            await UniTask.WhenAll(Array.ConvertAll(actionUniTaskEventDelegate, del => ((EventAsync)del)(obj)));
//        }
//    }

//    public class EventInfoAsync<T, K> : IEvent
//    {
//        public delegate UniTask EventAsync(T t, K k);
//        public event EventAsync eventAsync;
//        public EventInfoAsync(EventAsync eventAsync)
//        {
//            this.eventAsync += eventAsync;
//        }
//        public async UniTask TriggerAsync(T obj1, K obj2)
//        {
//            Delegate[] actionUniTaskEventDelegate = eventAsync.GetInvocationList();
//            await UniTask.WhenAll(Array.ConvertAll(actionUniTaskEventDelegate, del => ((EventAsync)del)(obj1, obj2)));
//        }
//    }
//}
