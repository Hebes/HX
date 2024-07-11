using System;
using System.Collections;
using System.Collections.Generic;

/*--------脚本描述-----------

描述:
    事件中心模块

-----------------------*/

namespace Framework.Core
{
    public delegate void OnEventAction(object udata);
    
    [CreateCore(typeof(CoreEvent), 3)]
    public class CoreEvent : ICore
    {
        public void Init()
        {
            I = this;
            _eventDic = new Dictionary<Enum, EventData>();
        }

        public IEnumerator AsyncEnter()
        {
            yield return null;
        }
        public IEnumerator Exit()
        {
            yield break;
        }

        public static CoreEvent I;
        private Dictionary<Enum, EventData> _eventDic;


        public void Add(Enum enumValue, OnEventAction action)
        {
            if (!_eventDic.ContainsKey(@enumValue))
                _eventDic.Add(@enumValue, new EventData());
            var temp = _eventDic[enumValue];
            temp.Add(action);
        }

        public void Trigger(Enum enumValue, object data)
        {
            if (!_eventDic.ContainsKey(enumValue))
                throw new Exception($"没有{nameof(Enum)}");
            var actionList = _eventDic[enumValue];
            actionList.Trigger(data);
        }

        public void UnAdd(Enum enumValue, OnEventAction action)
        {
            if (!_eventDic.ContainsKey(@enumValue))
                throw new Exception($"没有{nameof(Enum)}");
            var actionList = _eventDic[enumValue];
            actionList.UnAdd(action);
        }
    }
    
    /// <summary>
    /// 事件数据
    /// </summary>
    public class EventData 
    {
        private List<OnEventAction> _actionList;

        public void Add(OnEventAction action)
        {
            if (_actionList.Contains(action))
                throw new Exception($"已经有当前方法{nameof(action)}");
            _actionList.Add(action);
        }

        public void UnAdd(OnEventAction action)
        {
            _actionList.Remove(action);
        }

        public void Trigger(object data)
        {
            foreach (var action in _actionList)
                action(data);
        }
    }
}