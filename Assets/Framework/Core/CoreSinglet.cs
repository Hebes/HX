using UnityEngine;

/*--------脚本描述-----------

描述:
    单例类

-----------------------*/

namespace Framework.Core
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
        private static T _instance;
        public static T Instance => _instance ?? new T();
    }

    /// <summary>
    /// 内存中的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonInit<T> where T : ISingleton, new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = new T();
                _instance.SingletonInit();
                return _instance;
            }
        }
    }

    /// <summary>
    /// 通过new物体创建的
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonNewMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance) return _instance;
                var obj = new GameObject(typeof(T).ToString());
                _instance = obj.AddComponent<T>();
                DontDestroyOnLoad(obj);
                return _instance;
            }
        }
    }
}