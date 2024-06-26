﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------脚本描述-----------

描述:
    对象池模块

-----------------------*/


namespace Core
{
    public class CorePool : ICore
    {
        public static CorePool Instance;
        public Dictionary<string, List<IPool>> poolDic;
        private GameObject poolObj;


        public void Init()
        {
            Instance = this;
            poolDic = new Dictionary<string, List<IPool>>();
            poolObj = new GameObject("PoolManager");
            GameObject.DontDestroyOnLoad(poolObj);
        }

        public IEnumerator AsyncInit()
        {
            yield return null;
        }

        #region mono专用
        /// <summary>
        /// 获取(挂在脚本类型,从资源加载)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetMono<T>(string loadPath) where T : Component, IPool
        {
            //如果缓存池中有的话
            if (Instance.poolDic.TryGetValue(typeof(T).FullName, out List<IPool> data))
            {
                if (data.Count > 0)
                {
                    IPool poolData = data[0];
                    poolData.Get();
                    Instance.poolDic[typeof(T).FullName].Remove(poolData);
                    return poolData as T;
                }
            }
            //加载物体
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
        public static T GetMono<T>(GameObject gameObject) where T : Component, IPool
        {
            //如果缓存池中有的话
            if (Instance.poolDic.TryGetValue(typeof(T).FullName, out List<IPool> data))
            {
                if (data.Count > 0)
                {
                    IPool poolData = data[0];
                    poolData.Get();
                    data.Remove(poolData);
                    return poolData as T;
                }
            }
            GameObject gameObjectTemp = GameObject.Instantiate(gameObject);
            T t = gameObjectTemp.GetComponent<T>() == null ? gameObjectTemp.AddComponent<T>() : gameObjectTemp.GetComponent<T>();
            t.Get();
            return t;
        }

        /// <summary>
        /// 对象池获取
        /// </summary>
        /// <typeparam name="T">返回的类型</typeparam>
        /// <typeparam name="V">加载资源的类型</typeparam>
        /// <param name="v">已经加载的资源,只需要实例化</param>
        /// <returns></returns>
        public static T GetMono<T, V>(V v) where T : Object, IPool where V : Object, IPool
        {
            //如果缓存池中有的话
            if (Instance.poolDic.TryGetValue(typeof(T).FullName, out List<IPool> data))
            {
                if (data.Count > 0)
                {
                    IPool poolData = data[0];
                    poolData.Get();
                    data.Remove(poolData);
                    return poolData as T;
                }
            }

            V Temp = Object.Instantiate<V>(v);
            if (Temp is T dataTemp)
            {
                dataTemp.Get();
                return dataTemp;
            }
            Debug.LogError($"{typeof(T).FullName}不是{typeof(V).FullName},两个必须相同脚本");
            return null;
        }

        /// <summary>
        /// 推入
        /// </summary>
        public static void PushMono<T>(T t) where T : Component, IPool
        {
            //隐藏
            if (Instance.poolDic.ContainsKey(typeof(T).FullName))
                Instance.poolDic[typeof(T).FullName].Add(t);
            else
                Instance.poolDic.Add(typeof(T).FullName, new List<IPool>() { t });
            t.Push();
        }

        #endregion


        #region Class专用
        /// <summary>
        /// 获取类型为Calss的(就是class，不包含Mono任何数据)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>() where T : class, IPool, new()
        {
            T t = default;
            if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
            {
                ExtensionDebug.Error("请调用其他GetMono方法");
                return t;
            }

            if (Instance.poolDic.TryGetValue(typeof(T).FullName, out List<IPool> data))
            {
                if (data.Count > 0)//说明有
                {
                    t = data[0] as T;
                    data.Remove(t);
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
        public static void Push<T>(T t) where T : IPool
        {
            if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
            {
                ExtensionDebug.Error("请调用其他PushMono方法");
                return;
            }

            if (Instance.poolDic.TryGetValue(typeof(T).FullName, out List<IPool> data))
                data.Add(t);
            else
                Instance.poolDic.Add(typeof(T).FullName, new List<IPool>() { t });
            t.Push();
        }
        #endregion


        #region 其他
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
        #endregion
    }

    /// <summary>
    /// 对象池接口
    /// </summary>
    public interface IPool
    {
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
