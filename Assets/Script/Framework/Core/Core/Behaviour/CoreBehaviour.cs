using System.Collections;
using System.Collections.Generic;
using Framework.Core;
using UnityEngine;
using Time = UnityEngine.Time;


/*--------脚本描述-----------

描述:
    生命周期

-----------------------*/

namespace Framework.Core
{
    [CreateCore(typeof(CoreBehaviour), 4)]
    public class CoreBehaviour : ICore
    {
        public static CoreBehaviour Instance;

        /// <summary>
        /// int key请用枚举转
        /// </summary>
        private Dictionary<int, Coroutine> CoroutineDic;

        public BehaviourController behaviourController { get; private set; }


        public void Init()
        {
            Instance = this;
            CoroutineDic = new Dictionary<int, Coroutine>();
            behaviourController = BehaviourController.Instance;
        }

        public IEnumerator AsyncEnter()
        {
            yield break;
        }

        public IEnumerator Exit()
        {
            yield break;
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
            if (typeof(IUpdate).IsAssignableFrom(typeof(T)))
                BehaviourController.Instance.Add(t);
            if (typeof(IFixedUpdate).IsAssignableFrom(typeof(T)))
                BehaviourController.Instance.Add(t, EMonoType.FixedUpdate);
        }

        public static void Remove<T>(T t) where T : IBehaviour
        {
            //https://www.cnblogs.com/radray/p/4529482.html
            if (typeof(IUpdate).IsAssignableFrom(typeof(T)))
                BehaviourController.Instance.Remove(t);
            if (typeof(IFixedUpdate).IsAssignableFrom(typeof(T)))
                BehaviourController.Instance.Remove(t, EMonoType.FixedUpdate);
        }

        public static void AddCoroutine(int coroutineKey, IEnumerator coroutine)
        {
            if (Instance.CoroutineDic.ContainsKey(coroutineKey))
            {
                $"协程已经存在{coroutineKey}".LogError();
                return;
            }

            Instance.CoroutineDic.Add(coroutineKey, BehaviourController.Instance.StartCoroutine(coroutine));
        }

        public static void StopCoroutine(int coroutineKey)
        {
            if (Instance.CoroutineDic.TryGetValue(coroutineKey, out Coroutine coroutine))
            {
                BehaviourController.Instance.StopCoroutine(coroutine);
                Instance.CoroutineDic.Remove(coroutineKey);
                return;
            }

            "停止失败请，协程不存在".LogError();
        }

        public static Coroutine AddCoroutine(IEnumerator coroutine)
        {
            return BehaviourController.Instance.StartCoroutine(coroutine);
        }

        public static void StopCoroutine(Coroutine coroutine)
        {
            BehaviourController.Instance.StopCoroutine(coroutine);
        }

        public static void StopAllCoroutines()
        {
            BehaviourController.Instance.StopAllCoroutines();
        }
    }
}