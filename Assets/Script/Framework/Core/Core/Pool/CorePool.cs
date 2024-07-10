using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;

/*--------脚本描述-----------

描述:
    对象池模块

-----------------------*/


namespace Framework.Core
{
    [CreateCore(typeof(CorePool), 2)]
    public class CorePool : ICore
    {
        public static CorePool Instance;
        private Dictionary<string, List<PoolData>> poolDic;
        private GameObject _poolObj;
        private Dictionary<string, string> PathDic;

        public void Init()
        {
            Instance = this;
            poolDic = new Dictionary<string, List<PoolData>>();
            PathDic = new Dictionary<string, string>()
            {
                { "Test1", "AssetsPackage/Prefab/Test1" }
            };
            _poolObj = new GameObject("PoolManager");
            GameObject.DontDestroyOnLoad(_poolObj);
        }

        public IEnumerator AsyncEnter()
        {
            yield return null;
        }

        public IEnumerator Exit()
        {
            yield break;
        }

        public T Get<T>() where T : new()
        {
            var tNameValue = nameof(T);
            if (!typeof(T).IsSubclassOf(typeof(IPool)))
                throw new Exception($"{typeof(T).FullName}请继承IPool");
            if (!poolDic.ContainsKey(tNameValue))
                poolDic.Add(tNameValue, new List<PoolData>());


            //Mono组件
            if (typeof(T).IsSubclassOf(typeof(Component)))
            {
                if (poolDic[tNameValue].Count > 0)
                {
                    var poolData = poolDic[tNameValue][0].Pool;
                    poolDic[tNameValue].RemoveAt(0);
                    RunGet(poolData);
                    return (T)poolData;
                }

                //加载物体
                var loadPath = PathDic[tNameValue];
                var gameObject = CoreResource.Load<GameObject>(loadPath);
                var gameObjectTemp = Object.Instantiate(gameObject);
                var tComponentTemp = gameObjectTemp.GetComponent<T>();
                if (tComponentTemp == null && typeof(Component).IsAssignableFrom(typeof(T)))
                    gameObjectTemp.AddComponent(typeof(T));
                RunGet(tComponentTemp);
                return gameObjectTemp.GetComponent<T>();
            }

            //class
            if (poolDic[tNameValue].Count > 0)
            {
                var poolData = poolDic[tNameValue][0].Pool;
                poolDic[tNameValue].RemoveAt(0);
                RunGet(poolData);
                return (T)poolData;
            }

            var t = new T();
            RunGet(t);
            return t;

            void RunGet<TK>(TK k)
            {
                if (k is IPool pool1)
                    pool1.Get();
            }
        }

        public T GetMono<T>() where T : Component, IPool
        {
            //如果缓存池中有的话
            if (poolDic.TryGetValue(nameof(T), out var data))
            {
                if (data.Count > 0)
                {
                    IPool poolData = data[0].Pool;
                    poolData.Get();
                    poolDic[nameof(T)].RemoveAt(0);
                    return poolData as T;
                }
            }


            //加载物体
            var loadPath = PathDic[nameof(T)];
            GameObject gameObject = CoreResource.Load<GameObject>(loadPath);
            GameObject gameObjectTemp = Object.Instantiate(gameObject);
            T t = gameObjectTemp.GetComponent<T>();
            if (t == null) t = gameObjectTemp.AddComponent<T>();
            t.Get();
            return t;
        }

        public T GetMono<T>(GameObject gameObject) where T : Component, IPool
        {
            //如果缓存池中有的话
            if (poolDic.TryGetValue(nameof(T), out var data))
            {
                if (data.Count > 0)
                {
                    var poolData = data[0].Pool;
                    poolData.Get();
                    poolDic[nameof(T)].RemoveAt(0);
                    return poolData as T;
                }
            }

            var gameObjectTemp = Object.Instantiate(gameObject);
            var t = gameObjectTemp.GetComponent<T>();
            if (t == null) t = gameObjectTemp.AddComponent<T>();
            t.Get();
            return t;
        }

        public T GetClass<T>() where T : class, IPool, new()
        {
            T t = default;
            if (poolDic.TryGetValue(nameof(T), out var data))
            {
                if (data.Count > 0) //说明有
                {
                    t = data[0].Pool as T;
                    data.RemoveAt(0);
                    return t;
                }
            }

            t = new T();
            t.Get();
            return t;
        }

        public void Push<T>(T t) where T : IPool
        {
            if (!poolDic.ContainsKey(nameof(T)))
                poolDic.Add(nameof(T), new List<PoolData>());

            var poolData = new PoolData { Pool = t, PushTime = DateTime.Now };
            poolDic[nameof(T)].Add(poolData);
            t.Push();
            //清理超时的
            var poolDataList = poolDic[nameof(T)];
            for (var i = poolDataList.Count - 1; i >= 0; i--)
            {
                var poolDataTemp = poolDataList[i];
                var elapsedTime = poolDataTemp.PushTime - DateTime.Now;
                if (elapsedTime.Milliseconds > poolDataTemp.Pool.DesMilliseconds)
                    poolDataList.RemoveAt(i);
                   
            }
        }

        /// <summary>
        /// 对象池设置父物体
        /// </summary>
        public static void SetParentMono<T>(T t) where T : Component, IPool
        {
            //设置父物体
            var transform = Instance._poolObj.transform.Find(nameof(T));
            if (transform == null)
            {
                transform = new GameObject(nameof(T)).transform;
                transform.SetParent(Instance._poolObj.transform, false);
            }

            t.transform.SetParent(transform, false);
        }
    }

    public struct PoolData
    {
        public DateTime PushTime;

        public IPool Pool;
    }

    /// <summary>
    /// 对象池接口
    /// </summary>
    public interface IPool
    {
        /// <summary>
        /// 对象销毁时间
        /// -1:表示不销毁 0:立即销毁 1000:1秒后销毁 以此类推
        /// 毫秒计算 1000毫秒等于1秒
        /// </summary>
        public float DesMilliseconds { get; }

        /// <summary>
        /// 从对象池出来之后需要做的事
        /// </summary>
        public void Get();

        /// <summary>
        /// 进对象池之前需要做的事
        /// </summary>
        public void Push();
    }
}