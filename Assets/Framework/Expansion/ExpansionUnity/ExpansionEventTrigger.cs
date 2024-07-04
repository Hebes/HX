using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/*--------脚本描述-----------

描述:
    EventTrigger拓展

-----------------------*/

namespace Framework.Core
{
    public static class ExpansionEventTrigger
    {
        /// <summary>
        /// 为EventTrigger的事件类型绑定Action方法
        /// </summary>
        /// <param name="trigger">EventTrigger组件对象</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="listenedAction">要执行的方法</param>
        private static void AddEventTrigger(this EventTrigger trigger, EventTriggerType eventType, Action<PointerEventData> listenedAction)
        {
            //EventTrigger.Entry entry = new EventTrigger.Entry();
            //entry.eventID = eventType;
            //entry.callback.AddListener(data => listenedAction.Invoke((PointerEventData)data));
            //trigger.triggers.Add(entry);
        }

        /// <summary>
        /// UI物体移除EventTrigger
        /// </summary>
        /// <param name="obj">要移除事件的ui物体</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="action">调用的方法</param>
        public static void RemoveEventTrigger(this EventTrigger trigger, EventTriggerType eventType, UnityAction<BaseEventData> action)
        {
            var entry = new EventTrigger.Entry();
            entry.eventID = eventType;
            entry.callback.AddListener(action);
            trigger.triggers.Remove(entry);
        }

        /// <summary>
        /// 添加EventTrigger组件
        /// </summary>
        /// <param name="tf"></param>
        /// <param name="eventType"></param>
        /// <param name="listenedAction"></param>
        public static void AddEventTriggerListener(this Transform tf, EventTriggerType eventType, Action<PointerEventData> listenedAction)
        {
            tf.gameObject.AddEventTriggerListener(eventType, listenedAction);
        }

        /// <summary>
        /// 添加EventTrigger组件
        /// </summary>
        /// <param name="tf"></param>
        /// <param name="eventType"></param>
        /// <param name="listenedAction"></param>
        public static void AddEventTriggerListener(this GameObject go, EventTriggerType eventType, Action<PointerEventData> listenedAction)
        {
            //添加或获取组件
            EventTrigger eventTrigger = go.GetComponent<EventTrigger>() == null ? go.AddComponent<EventTrigger>() : go.GetComponent<EventTrigger>();
            //添加事件监听
            eventTrigger.AddEventTrigger(eventType, listenedAction);
        }
    }
}
