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
        public Dictionary<string, List<PoolData>> PoolDic; //对象回收池字典
        private Dictionary<IPool, PoolData> UseDic; //对象使用池字典
        private Dictionary<string, string> PathDic; //加载物体的路径字典
        private GameObject _poolObj; //父物体

        public void Init()
        {
            Instance = this;
            PoolDic = new Dictionary<string, List<PoolData>>();
            PathDic = new Dictionary<string, string>()
            {
                { "Test1", "AssetsPackage/Prefab/Test1" }
            };
            _poolObj = new GameObject("PoolManager");
            Object.DontDestroyOnLoad(_poolObj);
        }

        public IEnumerator AsyncEnter()
        {
            yield return null;
        }

        public IEnumerator Exit()
        {
            yield break;
        }

        public T Get<T>(float desMilliseconds = -1) where T : IPool, new()
        {
            var tNameValue = nameof(T);
            if (!PoolDic.ContainsKey(tNameValue))
                PoolDic.Add(tNameValue, new List<PoolData>());

            PoolData poolData;
            //Mono组件
            if (typeof(T).IsSubclassOf(typeof(Component)))
            {
                if (PoolDic[tNameValue].Count > 0)
                {
                    poolData = PoolDic[tNameValue][0];
                    PoolDic[tNameValue].Remove(poolData);
                    poolData.SetData(tNameValue, DateTime.Now, poolData.Pool, desMilliseconds);
                    UseDic.Add(poolData.Pool, poolData);
                    return (T)poolData.Pool;
                }

                //加载物体
                var loadPath = PathDic[tNameValue];
                var gameObject = CoreResource.Load<GameObject>(loadPath);
                var gameObjectTemp = Object.Instantiate(gameObject);
                var tComponentTemp = gameObjectTemp.GetComponent<T>();
                if (tComponentTemp == null && typeof(Component).IsAssignableFrom(typeof(T)))
                    gameObjectTemp.AddComponent(typeof(T));
                var component = gameObjectTemp.GetComponent<T>();
                poolData = new PoolData(this);
                poolData.SetData(tNameValue, DateTime.Now, component, desMilliseconds);
                UseDic.Add(component, poolData);
                return component;
            }

            //class
            if (PoolDic[tNameValue].Count > 0)
            {
                poolData = PoolDic[tNameValue][0];
                PoolDic[tNameValue].Remove(poolData);
                poolData = new PoolData(this);
                poolData.SetData(tNameValue, DateTime.Now, poolData.Pool, desMilliseconds);
                UseDic.Add(poolData.Pool, poolData);
                return (T)poolData.Pool;
            }
            var t = new T();
            poolData = new PoolData(this);
            poolData.SetData(tNameValue, DateTime.Now,t , desMilliseconds);
            UseDic.Add(poolData.Pool, poolData);
            return t;
        }

        public T GetMono<T>() where T : Component, IPool
        {
            //如果缓存池中有的话
            if (PoolDic.TryGetValue(nameof(T), out var data))
            {
                if (data.Count > 0)
                {
                    var poolData = data[0];
                    poolData.Pool.Get(null);
                    return poolData.Pool as T;
                }
            }


            //加载物体
            var loadPath = PathDic[nameof(T)];
            GameObject gameObject = CoreResource.Load<GameObject>(loadPath);
            GameObject gameObjectTemp = Object.Instantiate(gameObject);
            T t = gameObjectTemp.GetComponent<T>();
            if (t == null) t = gameObjectTemp.AddComponent<T>();
            t.Get(null);
            return t;
        }

        public T GetMono<T>(GameObject gameObject) where T : Component, IPool
        {
            //如果缓存池中有的话
            if (PoolDic.TryGetValue(nameof(T), out var data))
            {
                if (data.Count > 0)
                {
                    var poolData = data[0];
                    poolData.Pool.Get(null);
                    return poolData.Pool as T;
                }
            }

            var gameObjectTemp = Object.Instantiate(gameObject);
            var t = gameObjectTemp.GetComponent<T>();
            if (t == null) t = gameObjectTemp.AddComponent<T>();
            t.Get(null);
            return t;
        }

        public T GetClass<T>() where T : class, IPool, new()
        {
            T t;
            if (PoolDic.TryGetValue(nameof(T), out var data))
            {
                if (data.Count > 0) //说明有
                    return data[0].Pool as T;
            }

            t = new T();
            t.Get(null);
            return t;
        }

        public void Push<T>(T t) where T : IPool
        {
            var tName = nameof(T);
            PoolDic[tName].Add(UseDic[t]);
            UseDic.Remove(t);
            t.Push();
            //检查到期的对象移除
            var poolDataList = PoolDic[tName];
            for (var i = poolDataList.Count - 1; i >= 0; i--)
                poolDataList[i].RemoveOverTime();
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

    public class PoolData : IID
    {
        public PoolData(CorePool value)
        {
            CorePool = value;
            ID = IdWorker.Singleton.nextId();
        }

        public CorePool CorePool;

        /// <summary>
        /// 归属字典的Key
        /// </summary>
        public string DicKey;

        /// <summary>
        /// 推入时间
        /// </summary>
        public DateTime PushTime;

        /// <summary>
        /// 对象销毁时间
        /// -1:表示不销毁 0:立即销毁 1000:1秒后销毁 以此类推
        /// 毫秒计算 1000毫秒等于1秒
        /// </summary>
        public float DesMilliseconds;

        /// <summary>
        /// 对象
        /// </summary>
        public IPool Pool;

        /// <summary>
        /// 清理超时的
        /// </summary>
        public void RemoveOverTime()
        {
            var elapsedTime = PushTime - DateTime.Now;
            if (elapsedTime.Milliseconds > DesMilliseconds)
                CorePool.PoolDic[DicKey].Remove(this);
        }

        public long ID { get; set; }

        /// <summary>
        /// 取出后重新设置数据
        /// </summary>
        /// <param name="dicKey"></param>
        /// <param name="pushTime"></param>
        /// <param name="pool"></param>
        /// <param name="desMilliseconds"></param>
        public void SetData(string dicKey, DateTime pushTime, IPool pool, float desMilliseconds = -1)
        {
            DicKey = dicKey;
            PushTime = pushTime;
            Pool = pool;
            DesMilliseconds = desMilliseconds;
            Pool.Get(this);
        }
    }
}