using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    protected SingletonMono()
    {
    }

    public static T Instance
    {
        get
        {
            if (ApplicationIsQuitting)
            {
                Debug.LogWarning("[单例] 实例化 '" + typeof(T) + "申请退出时已销毁。不会再次创建-返回null");
                return null;
            }

            object @lock = Lock;
            lock (@lock)
            {
                if (_instance == null)
                    _instance = FindObjectOfType<T>();
            }

            return _instance;
        }
    }

    protected virtual void OnDestroy()
    {
        ApplicationIsQuitting = true;
    }

    public static bool IsValid()
    {
        return _instance != null;
    }

    private static T _instance;

    private static readonly object Lock = new object();

    public static bool ApplicationIsQuitting;
}