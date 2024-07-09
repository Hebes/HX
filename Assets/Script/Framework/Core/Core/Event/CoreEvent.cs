using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Core;

/*--------脚本描述-----------

描述:
    事件中心模块

-----------------------*/

namespace Framework.Core
{
    [CreateCore(typeof(CoreEvent), 3)]
    public class CoreEvent : ICore
    {
        public void Init()
        {
            I = this;
            _eventDic = new Dictionary<Enum, IEventData>();
        }

        public IEnumerator AsyncInit()
        {
            yield return null;
        }

        public static CoreEvent I;
        private Dictionary<Enum, IEventData> _eventDic;

        #region 没有参数

        public void Add(Enum enumValue, Action action)
        {
            if (!_eventDic.ContainsKey(@enumValue))
                _eventDic.Add(@enumValue, new EventData());
            var temp = (EventData)_eventDic[enumValue];
            temp.Add(action);
        }

        public void Trigger(Enum enumValue)
        {
            if (!_eventDic.ContainsKey(@enumValue))
                throw new Exception($"没有{nameof(Enum)}");
            var actionList = (EventData)_eventDic[enumValue];
            actionList.Trigger();
        }

        public void UnAdd(Enum enumValue, Action action)
        {
            if (!_eventDic.ContainsKey(@enumValue))
                throw new Exception($"没有{nameof(Enum)}");
            var actionList = (EventData)_eventDic[enumValue];
            actionList.UnAdd(action);
        }

        #endregion

        #region 带参数

        public void Add<T>(Enum enumValue, Action<T> action)
        {
            if (!_eventDic.ContainsKey(@enumValue))
                _eventDic.Add(@enumValue, new EventData<T>());
            var temp = (EventData<T>)_eventDic[enumValue];
            temp.Add(action);
        }

        public void Trigger<T>(Enum enumValue, T data)
        {
            if (!_eventDic.ContainsKey(@enumValue))
                throw new Exception($"没有{nameof(Enum)}");
            var actionList = (EventData<T>)_eventDic[enumValue];
            actionList.Trigger(data);
        }

        public void UnAdd<T>(Enum enumValue, Action<T> action)
        {
            if (!_eventDic.ContainsKey(@enumValue))
                throw new Exception($"没有{nameof(Enum)}");
            var actionList = (EventData<T>)_eventDic[enumValue];
            actionList.UnAdd(action);
        }

        #endregion
    }

    public interface IEventData
    {
    }

    public class EventData : IEventData
    {
        public List<Action> ActionList;

        public void Add(Action action)
        {
            if (ActionList.Contains(action))
                throw new Exception($"已经有当前方法{nameof(action)}");
            ActionList.Add(action);
        }

        public void UnAdd(Action action)
        {
            ActionList.Remove(action);
        }

        public void Trigger()
        {
            foreach (var action in ActionList)
                action();
        }
    }

    public class EventData<T> : IEventData
    {
        public List<Action<T>> ActionList;

        public void Add(Action<T> action)
        {
            if (ActionList.Contains(action))
                throw new Exception($"已经有当前方法{nameof(action)}");
            ActionList.Add(action);
        }

        public void UnAdd(Action<T> action)
        {
            ActionList.Remove(action);
        }

        public void Trigger(T data)
        {
            foreach (var action in ActionList)
                action(data);
        }
    }
}