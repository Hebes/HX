using UnityEngine;


/*--------脚本描述-----------

描述:
    单例类

-----------------------*/

namespace Core
{
    public interface ISingleton
    {
        public void SingletonInit();
    }

    /// <summary>
    /// 内存中的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : new()
    {
        private static T instance;
        public static T Instance => instance ?? new T();
    }

    /// <summary>
    /// 内存中的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton_Init<T> where T : ISingleton, new()
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                    instance.SingletonInit();
                }
                return instance;
            }
        }
    }

    /// <summary>
    /// 通过new物体创建的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton_NewMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).ToString();
                    instance = obj.AddComponent<T>();
                    DontDestroyOnLoad(obj);
                }
                return instance;
            }
        }
    }

    /// <summary>
    /// 已有物体创建的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singlent_Mono<T> : MonoBehaviour where T : Singlent_Mono<T>
    {
        private static T instance;
        public static T Instance => instance;

        protected virtual void Awake()
        {
            if (instance != null)
                Destroy(gameObject);
            else
                instance = (T)this;
        }
        protected virtual void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
    }

    /// <summary>
    /// 通过new物体创建的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton_NewMonoInit<T> : MonoBehaviour where T : MonoBehaviour, ISingleton
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).ToString();
                    instance = obj.AddComponent<T>();
                    instance.SingletonInit();
                    DontDestroyOnLoad(obj);
                }

                return instance;
            }
        }
    }
}