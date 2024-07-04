using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object; //Object并非C#基础中的Object，而是 UnityEngine.Object


/*--------脚本描述-----------

描述:
    UI脚本主要类

-----------------------*/

namespace Framework.Core
{
    [Serializable]
    public class UIData
    {
        public string key;
        public Object gameObject;
    }

    [RequireComponent(typeof(CanvasGroup))]
    public class UIComponent : MonoBehaviour
    {
        public List<UIData> dataList = new List<UIData>();

        public GameObject Get(string key)
        {
            foreach (var data in dataList)
            {
                if (data.key != key) continue;
                return data.gameObject as GameObject;
            }

            return default;
        }

        public T GetComponent<T>(string key) where T : Component => Get(key)?.GetComponent<T>();
    }
}