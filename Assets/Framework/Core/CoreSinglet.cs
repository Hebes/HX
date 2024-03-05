using UnityEngine;


/*--------脚本描述-----------

描述:
    单例类

-----------------------*/

namespace Core
{

    /// <summary>
    /// 内存中的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonBase<T> where T : new()
    {
        private static T instance;
        public static T Instance => instance ?? new T();
    }

    /// <summary>
    /// 通过new物体创建的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonNewMono<T> : MonoBehaviour where T : MonoBehaviour
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
    public class SinglentMono<T> : MonoBehaviour where T : SinglentMono<T>
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
    /// 内存中的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonBaseInit<T> where T : ISingletonInit, new()
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
    public class SingletonNewMonoInit<T> : MonoBehaviour where T : MonoBehaviour, ISingletonInit
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
public interface ISingletonInit
{
    public void SingletonInit();
}
