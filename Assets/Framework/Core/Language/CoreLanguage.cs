﻿using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------脚本描述-----------

描述:
	多语言

-----------------------*/

/// <summary> 语言类型 </summary>
public enum ELanguageType
{
    Chinese = 0,
    English = 1,
}

public class CoreLanguage : ICore
{
    public static CoreLanguage Instance;
    public event Action languageChangeEvt;               //回调事件
    private Dictionary<string, string> _languageDic;      //语言字典
    public Font _font;

    public void Init()
    {
        Instance = this;
        _languageDic = new Dictionary<string, string>();
        //TODO 加载多语言
    }

    public IEnumerator AsyncInit()
    {
        yield return null;
    }


    /// <summary>
    /// 切换语言
    /// </summary>
    public void ChangeLanguage()
    {
        languageChangeEvt?.Invoke();
    }

    /// <summary>
    /// 设置字典
    /// </summary>
    /// <param name="keyValuePairs"></param>
    public void SetLanguageDic(Dictionary<string, string> keyValuePairs)
    {
        _languageDic = keyValuePairs;
    }

    /// <summary>
    /// 获取文字
    /// </summary>
    public static string GetText(string key)
    {
        if (Instance._languageDic.ContainsKey(key)) 
            return Instance._languageDic[key];
        ExtensionDebug.Warn($"多语言未配置：{key}");
        return key;
    }

    public static void ChangeTextLanguage(Transform transform,string key)
    {
        LanguageComponent languageText = transform.GetComponent<LanguageComponent>() == null ?
            transform.gameObject.AddComponent<LanguageComponent>() : transform.GetComponent<LanguageComponent>();
        languageText.SetKeyAndChange(key);
    }
}
