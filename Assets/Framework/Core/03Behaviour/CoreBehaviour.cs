using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Time = UnityEngine.Time;


/*--------脚本描述-----------

描述:
    生命周期

-----------------------*/

namespace Core
{


    public class CoreBehaviour : ICore
    {
        public static CoreBehaviour Instance;

        /// <summary>
        /// int key请用枚举转
        /// </summary>
        private Dictionary<int, Coroutine> CoroutineDic;

        public BehaviourController behaviourController { get; private set; }

        public void ICoreInit()
        {
            Instance = this;
            CoroutineDic = new Dictionary<int, Coroutine>();
            behaviourController = BehaviourController.Instance;
            Debug.Log("初始化Mono完毕!");
        }

        /// <summary>
        /// 暂停
        /// </summary>
        /// <param name="m_Time">0暂停1不暂停</param>
        public void Pause(float m_Time)
        {
            Time.timeScale = m_Time;
        }

        public static void Add<T>(T t) where T : IBehaviour
        {
            //https://www.cnblogs.com/radray/p/4529482.html
            if (typeof(IUpdata).IsAssignableFrom(typeof(T)))
                BehaviourController.Instance.Add(t);
            if (typeof(IFixedUpdate).IsAssignableFrom(typeof(T)))
                BehaviourController.Instance.Add(t, EMonoType.FixedUpdate);
        }

        public static void Remove<T>(T t) where T : IBehaviour
        {
            //https://www.cnblogs.com/radray/p/4529482.html
            if (typeof(IUpdata).IsAssignableFrom(typeof(T)))
                BehaviourController.Instance.Remove(t);
            if (typeof(IFixedUpdate).IsAssignableFrom(typeof(T)))
                BehaviourController.Instance.Remove(t, EMonoType.FixedUpdate);
        }

        public static void AddCoroutine(int coroutineKey, IEnumerator coroutine)
        {
            if (!Instance.CoroutineDic.ContainsKey(coroutineKey))
                Instance.CoroutineDic.Add(coroutineKey, BehaviourController.Instance.StartCoroutine(coroutine));
            Debug.Error($"协程已经存在{coroutineKey}");
        }
        public static void RemoveCoroutine(int coroutineKey)
        {
            if (Instance.CoroutineDic.TryGetValue(coroutineKey, out Coroutine coroutine))
            {
                BehaviourController.Instance.StopCoroutine(coroutine);
                Instance.CoroutineDic.Remove(coroutineKey);
            }
            Debug.Error("停止失败请，协程不存在");
        }
        public static void StopAllCoroutines()
        {
            BehaviourController.Instance.StopAllCoroutines();
        }
    }
}
