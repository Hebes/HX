using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;//Object并非C#基础中的Object，而是 UnityEngine.Object


/*--------脚本描述-----------

描述:
    UI脚本主要类

-----------------------*/

namespace Core
{
    [Serializable]
    public class UIData
    {
        public string key;
        public Object gameObject;
    }


    public class UIComponent : MonoBehaviour
    {
        public List<UIData> dataList = new List<UIData>();

        public T Get<T>(string key) where T : class
        {
            foreach (UIData data in dataList)
            {
                if (data.key == key)
                    return data.gameObject as T;
            }
            return null;
        }

        public T GetComponent<T>(string key) where T : Component
        {
            foreach (UIData data in dataList)
            {
                if (data.key == key)
                    return (data.gameObject as GameObject).GetComponent<T>();
            }
            return null;
        }
    }
}