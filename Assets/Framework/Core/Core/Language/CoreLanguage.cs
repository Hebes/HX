using System.Collections;
using System.Collections.Generic;
using Framework.Core;
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

public class LanguageData
{
    public string Chinese;

    public string English;
}

[CreateCore(typeof(CoreLanguage), 2)]
public class CoreLanguage : ICore
{
    public static CoreLanguage Instance { get; private set; }
    private Dictionary<string, LanguageData> LanguageDataDic { get; set; }
    private List<LanguageComponent> LanguageComponentList { get; set; }
    public ELanguageType LanguageMode { get; set; } = ELanguageType.Chinese;
    public Font Font { get; set; }

    public void Init()
    {
        Instance = this;
        LanguageDataDic = new Dictionary<string, LanguageData>();
        LanguageComponentList = new List<LanguageComponent>();
    }

    public IEnumerator AsyncInit()
    {
        this.Log("协程多语言初始化");
        yield return null;
    }


    /// <summary>
    /// 切换语言
    /// </summary>
    public void OnChangeLanguage(ELanguageType languageMode)
    {
        LanguageMode = languageMode;
        foreach (var languageComponent in LanguageComponentList)
            languageComponent.Change();
    }

    /// <summary>
    /// 设置字体
    /// </summary>
    /// <param name="fontValue"></param>
    public void SetFont(Font fontValue)
    {
        Font = fontValue;
    }

    /// <summary>
    /// 获取文字
    /// </summary>
    public static string Get(string key)
    {
        if (Instance.LanguageDataDic.TryGetValue(key, out var value))
        {
            switch (Instance.LanguageMode)
            {
                default:
                case ELanguageType.Chinese:
                    return value.Chinese;
                case ELanguageType.English:
                    return value.English;
            }
        }

        Debug.LogError($"多语言未配置：{key}");
        return key;
    }

    /// <summary>
    /// 添加多语言数据
    /// </summary>
    public void AddLanguageDataDicData(LanguageData languageData)
    {
        if (LanguageDataDic.TryAdd(languageData.Chinese, languageData)) return;
        Debug.LogError($"当前已包含字符串{languageData.Chinese}");
    }

    /// <summary>
    /// 添加组件数据
    /// </summary>
    public static void AddLanguageComponent(LanguageComponent languageComponent)
    {
        if (Instance.LanguageComponentList.Contains(languageComponent))
        {
            Debug.LogError($"当前内存已有{languageComponent.key}组件");
            return;
        }

        Instance.LanguageComponentList.Add(languageComponent);
    }
}