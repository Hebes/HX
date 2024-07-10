using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Framework.Core
{
    public class CoreTimer : ICore
    {
        public void Init()
        {
        }

        public IEnumerator AsyncEnter()
        {
            yield return null;
        }

        public IEnumerator Exit()
        {
            yield break;
        }

        private List<TimerData> _taskList = new List<TimerData>();

        public void OnUpdate()
        {
            for (var i = _taskList.Count - 1; i >= 0; i--)
            {
                var data = _taskList[0];
                if ((data.CountUpTimer += Time.deltaTime) > data.TotalTime)
                    data.Action.Invoke(data);
            }
        }

        public void AddTask(TimerData timerData)
        {
            _taskList.Add(timerData);
        }

        public void UnAddTack(Action<TimerData> actionValue)
        {
            for (var i = _taskList.Count - 1; i >= 0; i--)
            {
                var timerData = _taskList[i];
                if (timerData.Action == actionValue)
                    _taskList.Remove(timerData);
            }
        }
    }

    /// <summary>
    /// 计时器数据
    /// </summary>
    public class TimerData
    {
        
        public float TotalTime;
        public float CountUpTimer;
        public Action<TimerData> Action;
    }
}