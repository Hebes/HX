using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
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

        public IEnumerator ICoreInit()
        {
            Instance = this;
            poolDic = new Dictionary<string, List<IPool>>();
            poolObj = new GameObject("PoolManager");
            GameObject.DontDestroyOnLoad(poolObj);
            yield return null;
        }

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
                    poolData.GetAfter();
                    Instance.poolDic[typeof(T).FullName].Remove(poolData);
                    return poolData as T;
                }
            }
            //加载物体
            GameObject gameObject = CoreResource.Load<GameObject>(loadPath);
            GameObject gameObjectTemp = GameObject.Instantiate(gameObject);
            T t = gameObjectTemp.GetComponent<T>() == null ? gameObjectTemp.AddComponent<T>() : gameObjectTemp.GetComponent<T>();
            t.GetAfter();
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
                    poolData.GetAfter();
                    data.Remove(poolData);
                    return poolData as T;
                }
            }
            GameObject gameObjectTemp = GameObject.Instantiate(gameObject);
            T t = gameObjectTemp.GetComponent<T>() == null ? gameObjectTemp.AddComponent<T>() : gameObjectTemp.GetComponent<T>();
            t.GetAfter();
            return t;
        }

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
                UnityEngine.Debug.Error("请调用其他GetMono方法");
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
            t.GetAfter();
            return t;
        }

        /// <summary>
        /// 推入
        /// </summary>
        public static void Push<T>(T t) where T : IPool
        {
            if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
            {
                UnityEngine.Debug.Error("请调用其他PushMono方法");
                return;
            }

            if (Instance.poolDic.TryGetValue(typeof(T).FullName,out List<IPool> data))
                data.Add(t);
            else
                Instance.poolDic.Add(typeof(T).FullName, new List<IPool>() { t });
            t.PushBefore();
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
            t.PushBefore();
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
}
