using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework.Core
{
    /// <summary>
    /// 思路-》每个管理类单独实现
    /// </summary>
    public class Pool2
    {
    }

    public interface IPool2
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

    public static class Pool2Expansion
    {
        public static T Get<T>(this List<T> dataListValue, GameObject gameObjectValue = null) where T : IPool, new()
        {
            if (!typeof(T).IsSubclassOf(typeof(IPool)))
                throw new Exception($"{typeof(T).FullName}请继承IPool");
            T t;
            if (dataListValue.Count > 0)
            {
                t = dataListValue[0];
                t.Get();
                return t;
            }

            if (gameObjectValue && typeof(Component).IsAssignableFrom(typeof(T)))
            {
                var tComponentTemp = gameObjectValue.GetComponent<T>();
                if (tComponentTemp == null && typeof(Component).IsAssignableFrom(typeof(T)))
                    gameObjectValue.AddComponent(typeof(T));
                t = gameObjectValue.GetComponent<T>();
                t.Get();
                return t;
            }

            t = new T();
            t.Get();
            return t;
        }
    }
}