using FieldEdge;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Core;
using Debug = Core.Debug;
using UnityEngine;

/*--------脚本描述-----------

描述:
	多语言

-----------------------*/

namespace Farm2D
{
    /// <summary> 语言类型 </summary>
    public enum ELanguageType
    {
        Chinese = 0,
        English = 1,
    }

    public class ManagerLanguage : IModelInit
    {
        public static ManagerLanguage Instance;
        public event Action OnLanguageChangeEvt;               //回调事件
        private Dictionary<string, string> LanguageKeyDic;      //语言字典
        public Font font;

        public void Init()
        {
            Instance = this;
            LanguageKeyDic=new Dictionary<string, string>();
        }

        /// <summary>
        /// 切换语言
        /// </summary>
        public void OnLanguageChange()
        {
            OnLanguageChangeEvt?.Invoke();
        }

        /// <summary>
        /// 设置字典
        /// </summary>
        /// <param name="keyValuePairs"></param>
        public void OnSetLanguageTDic(Dictionary<string, string> keyValuePairs)
        {
            LanguageKeyDic = keyValuePairs;
        }

        /// <summary>
        /// 获取文字
        /// </summary>
        public string GetText(string key)
        {
            if (LanguageKeyDic.ContainsKey(key))
                return LanguageKeyDic[key];
            Debug.Error($"多语言未配置：{key}");
            return key;
        }
    }
}
