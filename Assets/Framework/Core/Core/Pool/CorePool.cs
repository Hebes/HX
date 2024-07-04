using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Framework.Core;

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
        public Dictionary<string, List<PoolData>> poolDic;
        private GameObject poolObj;
        public Dictionary<string, string> PathDic;

        public void Init()
        {
            Instance = this;
            poolDic = new Dictionary<string, List<PoolData>>();
            PathDic = new Dictionary<string, string>() 
            {
                { "Test1","AssetsPackage/Prefab/Test1"}
            };
            poolObj = new GameObject("PoolManager");
            GameObject.DontDestroyOnLoad(poolObj);
        }

        public IEnumerator AsyncInit()
        {
            yield return null;
        }

        public T GetMono<T>() where T : Component, IPool
        {
            //如果缓存池中有的话
            if (poolDic.TryGetValue(typeof(T).FullName, out List<PoolData> data))
            {
                if (data.Count > 0)
                {
                    IPool poolData = data[0].pool;
                    poolData.Get();
                    poolDic[typeof(T).FullName].RemoveAt(0);
                    return poolData as T;
                }
            }


            //加载物体
            var loadPath = PathDic[typeof(T).FullName];
            GameObject gameObject = CoreResource.Load<GameObject>(loadPath);
            GameObject gameObjectTemp = GameObject.Instantiate(gameObject);
            T t = gameObjectTemp.GetComponent<T>() == null ? gameObjectTemp.AddComponent<T>() : gameObjectTemp.GetComponent<T>();
            t.Get();
            return t;
        }

        /// <summary>
        /// 获取(挂在脚本类型,从已有物体实例化创建)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public T GetMono<T>(GameObject gameObject) where T : Component, IPool
        {
            //如果缓存池中有的话
            if (poolDic.TryGetValue(typeof(T).FullName, out List<PoolData> data))
            {
                if (data.Count > 0)
                {
                    IPool poolData = data[0].pool;
                    poolData.Get();
                    poolDic[typeof(T).FullName].RemoveAt(0);
                    return poolData as T;
                }
            }
            GameObject gameObjectTemp = GameObject.Instantiate(gameObject);
            T t = gameObjectTemp.GetComponent<T>() == null ? gameObjectTemp.AddComponent<T>() : gameObjectTemp.GetComponent<T>();
            t.Get();
            return t;
        }

        public T GetClass<T>() where T : class, IPool, new()
        {
            T t = default;
            if (poolDic.TryGetValue(typeof(T).FullName, out List<PoolData> data))
            {
                if (data.Count > 0)//说明有
                {
                    t = data[0].pool as T;
                    data.RemoveAt(0);
                    return t;
                }
            }
            t = new T();
            t.Get();
            return t;
        }

        /// <summary>
        /// 推入
        /// </summary>
        public  void PushMono<T>(T t) where T : Component, IPool
        {
            string funllName = typeof(T).FullName;

            if (!poolDic.ContainsKey(funllName))
                poolDic.Add(funllName, new List<PoolData>());

            var poolDataList = poolDic[funllName];
            PoolData poolData = default;
            poolData.pool = t;
            poolData.Pushtime = DateTime.Now;
            poolDataList.Add(poolData);
            t.Push();

            //清理超时的
            for (int i = poolDataList.Count - 1; i >= 0; i--)
            {
                var dataTemp = poolDataList[i];
                if (Clear(dataTemp))
                    poolDataList.Remove(dataTemp);
                else
                    break;
            }
            
        }

        public void PushClass<T>(T t) where T : IPool
        {
            string funllName = typeof(T).FullName;

            if (!poolDic.ContainsKey(funllName))
                poolDic.Add(funllName, new List<PoolData>());

            //添加
            var poolDataList = poolDic[funllName];
            PoolData poolData = default;
            poolData.pool = t;
            poolData.Pushtime = DateTime.Now;
            poolDataList.Add(poolData);
            t.Push();

            //清理超时的
            for (int i = poolDataList.Count - 1; i >= 0; i--)
            {
                var dataTemp = poolDataList[i];
                if (Clear(dataTemp))
                    poolDataList.Remove(dataTemp);
                else
                    break;
            }
        }

        /// <summary>
        /// 清除超时的
        /// </summary>
        /// <param name="poolData"></param>
        private bool Clear(PoolData poolData)
        {
            TimeSpan elapsedTime = poolData.Pushtime - DateTime.Now;
            if (elapsedTime.Milliseconds > poolData.pool.DesMilliseconds)
                return true;
            return false;
        }

        /// <summary>
        /// 对象池设置父物体
        /// </summary>
        public static void SetParentMono<T>(T t) where T : Component, IPool
        {
            //设置父物体
            Transform transform = Instance.poolObj.transform.Find(typeof(T).FullName);
            if (transform == null)
            {
                transform = new GameObject(typeof(T).FullName).transform;
                transform.SetParent(Instance.poolObj.transform, false);
            }
            t.transform.SetParent(transform, false);
        }
    }

    public struct PoolData
    {
        public DateTime Pushtime;

        public IPool pool;
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
        public float DesMilliseconds { get;  }

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
