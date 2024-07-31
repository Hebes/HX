using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Framework.Core;
using UnityEngine;

/// <summary>
/// 本地管理器
/// </summary>
public static class LocalizationManager
{
    /// <summary>
    /// 当前语言
    /// </summary>
    public static string CurrentLanguage
    {
        get
        {
            LocalizationManager.InitializeIfNeeded();
            return LocalizationManager.mCurrentLanguage;
        }
        set
        {
            string supportedLanguage = LocalizationManager.GetSupportedLanguage(value);
            if (!string.IsNullOrEmpty(supportedLanguage) &&
                LocalizationManager.mCurrentLanguage != supportedLanguage)
            {
                LocalizationManager.SetLanguageAndCode(supportedLanguage, LocalizationManager.GetLanguageCode(supportedLanguage), true, false);
            }
        }
    }

    /// <summary>
    /// 现行语言代码
    /// </summary>
    public static string CurrentLanguageCode
    {
        get
        {
            LocalizationManager.InitializeIfNeeded();
            return LocalizationManager.mLanguageCode;
        }
        set
        {
            if (LocalizationManager.mLanguageCode != value)
            {
                string languageFromCode = LocalizationManager.GetLanguageFromCode(value);
                if (!string.IsNullOrEmpty(languageFromCode))
                {
                    LocalizationManager.SetLanguageAndCode(languageFromCode, value, true, false);
                }
            }
        }
    }

    /// <summary>
    /// 当前地区
    /// </summary>
    public static string CurrentRegion
    {
        get
        {
            string currentLanguage = LocalizationManager.CurrentLanguage;
            int num = currentLanguage.IndexOfAny("/\\".ToCharArray());
            if (num > 0)
            {
                return currentLanguage.Substring(num + 1);
            }

            num = currentLanguage.IndexOfAny("[(".ToCharArray());
            int num2 = currentLanguage.LastIndexOfAny("])".ToCharArray());
            if (num > 0 && num != num2)
            {
                return currentLanguage.Substring(num + 1, num2 - num - 1);
            }

            return string.Empty;
        }
        set
        {
            string text = LocalizationManager.CurrentLanguage;
            int num = text.IndexOfAny("/\\".ToCharArray());
            if (num > 0)
            {
                LocalizationManager.CurrentLanguage = text.Substring(num + 1) + value;
                return;
            }

            num = text.IndexOfAny("[(".ToCharArray());
            int num2 = text.LastIndexOfAny("])".ToCharArray());
            if (num > 0 && num != num2)
            {
                text = text.Substring(num);
            }

            LocalizationManager.CurrentLanguage = text + "(" + value + ")";
        }
    }

    /// <summary>
    /// 现行地区代码
    /// </summary>
    public static string CurrentRegionCode
    {
        get
        {
            string currentLanguageCode = LocalizationManager.CurrentLanguageCode;
            int num = currentLanguageCode.IndexOfAny(" -_/\\".ToCharArray());
            return (num >= 0) ? currentLanguageCode.Substring(num + 1) : string.Empty;
        }
        set
        {
            string text = LocalizationManager.CurrentLanguageCode;
            int num = text.IndexOfAny(" -_/\\".ToCharArray());
            if (num > 0)
            {
                text = text.Substring(0, num);
            }

            LocalizationManager.CurrentLanguageCode = text + "-" + value;
        }
    }

    /// <summary>
    /// 初始化需要的
    /// </summary>
    private static void InitializeIfNeeded()
    {
        if (!string.IsNullOrEmpty(LocalizationManager.mCurrentLanguage)) return;
        LocalizationManager.UpdateSources();
        LocalizationManager.SelectStartupLanguage();
    }

    /// <summary>
    /// 设置语言和代码
    /// </summary>
    /// <param name="LanguageName"></param>
    /// <param name="LanguageCode"></param>
    /// <param name="RememberLanguage"></param>
    /// <param name="Force"></param>
    public static void SetLanguageAndCode(string LanguageName, string LanguageCode, bool RememberLanguage = true, bool Force = false)
    {
        if (LocalizationManager.mCurrentLanguage != LanguageName || LocalizationManager.mLanguageCode != LanguageCode || Force)
        {
            if (RememberLanguage)
            {
                PlayerPrefs.SetString("I2 Language", LanguageName);
            }

            LocalizationManager.mCurrentLanguage = LanguageName;
            LocalizationManager.mLanguageCode = LanguageCode;
            if (LocalizationManager.mChangeCultureInfo)
            {
                LocalizationManager.SetCurrentCultureInfo();
            }
            else
            {
                LocalizationManager.IsRight2Left = LocalizationManager.IsRTL(LocalizationManager.mLanguageCode);
            }

            LocalizationManager.LocalizeAll(Force);
        }
    }

    /// <summary>
    /// 获取文化
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    private static CultureInfo GetCulture(string code)
    {
        CultureInfo result;
        try
        {
            result = CultureInfo.CreateSpecificCulture(code);
        }
        catch (Exception)
        {
            result = CultureInfo.InvariantCulture;
        }

        return result;
    }

    /// <summary>
    /// 启用更改文化信息
    /// </summary>
    /// <param name="bEnable"></param>
    public static void EnableChangingCultureInfo(bool bEnable)
    {
        if (!LocalizationManager.mChangeCultureInfo && bEnable)
        {
            LocalizationManager.SetCurrentCultureInfo();
        }

        LocalizationManager.mChangeCultureInfo = bEnable;
    }

    /// <summary>
    /// 设置当前区域性信息
    /// </summary>
    private static void SetCurrentCultureInfo()
    {
        Thread.CurrentThread.CurrentCulture = LocalizationManager.GetCulture(LocalizationManager.mLanguageCode);
        LocalizationManager.IsRight2Left = CultureInfo.CurrentCulture.TextInfo.IsRightToLeft;
    }

    /// <summary>
    /// 选择启动语言
    /// </summary>
    private static void SelectStartupLanguage()
    {
        string @string = PlayerPrefs.GetString("I2 Language", string.Empty);
        string text = Application.systemLanguage.ToString();
        if (text == "ChineseSimplified")
        {
            text = "Chinese (Simplified)";
        }

        if (text == "ChineseTraditional")
        {
            text = "Chinese (Traditional)";
        }

        if (LocalizationManager.HasLanguage(@string, true, false, true))
        {
            LocalizationManager.CurrentLanguage = @string;
            return;
        }

        string supportedLanguage = LocalizationManager.GetSupportedLanguage(text);
        if (!string.IsNullOrEmpty(supportedLanguage))
        {
            LocalizationManager.SetLanguageAndCode(supportedLanguage, LocalizationManager.GetLanguageCode(supportedLanguage), false, false);
            return;
        }

        int i = 0;
        int count = LocalizationManager.Sources.Count;
        while (i < count)
        {
            if (LocalizationManager.Sources[i].mLanguages.Count > 0)
            {
                for (int j = 0; j < LocalizationManager.Sources[i].mLanguages.Count; j++)
                {
                    if (LocalizationManager.Sources[i].mLanguages[j].IsEnabled())
                    {
                        LocalizationManager.SetLanguageAndCode(LocalizationManager.Sources[i].mLanguages[j].Name,
                            LocalizationManager.Sources[i].mLanguages[j].Code, false, false);
                        return;
                    }
                }
            }

            i++;
        }
    }

    //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public static event LocalizationManager.OnLocalizeCallback OnLocalizeEvent;

    public static string GetTermTranslation(string Term)
    {
        return LocalizationManager.GetTermTranslation(Term, LocalizationManager.IsRight2Left, 0, true, false, null);
    }

    public static string GetTermTranslation(string Term, bool FixForRTL)
    {
        return LocalizationManager.GetTermTranslation(Term, FixForRTL, 0, true, false, null);
    }

    public static string GetTermTranslation(string Term, bool FixForRTL, int maxLineLengthForRTL)
    {
        return LocalizationManager.GetTermTranslation(Term, FixForRTL, maxLineLengthForRTL, true, false, null);
    }

    public static string GetTermTranslation(string Term, bool FixForRTL, int maxLineLengthForRTL, bool ignoreRTLnumbers)
    {
        return LocalizationManager.GetTermTranslation(Term, FixForRTL, maxLineLengthForRTL, ignoreRTLnumbers, false, null);
    }

    public static string GetTermTranslation(string Term, bool FixForRTL, int maxLineLengthForRTL, bool ignoreRTLnumbers, bool applyParameters,
        GameObject localParametersRoot)
    {
        string result;
        if (LocalizationManager.TryGetTermTranslation(Term, out result, FixForRTL, maxLineLengthForRTL, ignoreRTLnumbers, applyParameters,
                localParametersRoot))
        {
            return result;
        }

        return string.Empty;
    }

    public static bool TryGetTermTranslation(string Term, out string Translation)
    {
        return LocalizationManager.TryGetTermTranslation(Term, out Translation, false, 0, true, false, null);
    }

    public static bool TryGetTermTranslation(string Term, out string Translation, bool FixForRTL)
    {
        return LocalizationManager.TryGetTermTranslation(Term, out Translation, FixForRTL, 0, true, false, null);
    }

    public static bool TryGetTermTranslation(string Term, out string Translation, bool FixForRTL, int maxLineLengthForRTL, bool ignoreRTLnumbers,
        bool applyParameters, GameObject localParametersRoot)
    {
        Translation = string.Empty;
        if (string.IsNullOrEmpty(Term))return false;
        LocalizationManager.InitializeIfNeeded();
        int i = 0;
        int count = LocalizationManager.Sources.Count;
        while (i < count)
        {
            if (LocalizationManager.Sources[i].TryGetTermTranslation(Term, out Translation))
            {
                if (applyParameters)
                {
                    LocalizationManager.ApplyLocalizationParams(ref Translation, localParametersRoot);
                }

                if (LocalizationManager.IsRight2Left && FixForRTL)
                {
                    Translation = LocalizationManager.ApplyRTLfix(Translation, maxLineLengthForRTL, ignoreRTLnumbers);
                }

                return true;
            }

            i++;
        }

        return false;
    }

    public static string ApplyRTLfix(string line)
    {
        return LocalizationManager.ApplyRTLfix(line, 0, true);
    }

    public static string ApplyRTLfix(string line, int maxCharacters, bool ignoreNumbers)
    {
        return string.Empty;
    }

    public static string FixRTL_IfNeeded(string text, int maxCharacters = 0, bool ignoreNumber = false)
    {
        if (LocalizationManager.IsRight2Left)
        {
            return LocalizationManager.ApplyRTLfix(text, maxCharacters, ignoreNumber);
        }

        return text;
    }

    public static void LocalizeAll(bool Force = false)
    {
        if (!Application.isPlaying)
        {
            LocalizationManager.DoLocalizeAll(Force);
            return;
        }

        LocalizationManager.mLocalizeIsScheduledWithForcedValue = (LocalizationManager.mLocalizeIsScheduledWithForcedValue || Force);
        if (LocalizationManager.mLocalizeIsScheduled)
        {
            return;
        }

        LocalizationManager.Coroutine_LocalizeAll().StartCoroutine();
    }

    private static IEnumerator Coroutine_LocalizeAll()
    {
        LocalizationManager.mLocalizeIsScheduled = true;
        yield return null;
        LocalizationManager.mLocalizeIsScheduled = false;
        LocalizationManager.mLocalizeIsScheduledWithForcedValue = false;
        LocalizationManager.DoLocalizeAll(LocalizationManager.mLocalizeIsScheduledWithForcedValue);
        yield break;
    }

    /// <summary>
    /// 一定要本地化
    /// </summary>
    /// <param name="Force"></param>
    private static void DoLocalizeAll(bool Force = false)
    {
       "进入阿拉伯多语言话".Log();
    }

    internal static void ApplyLocalizationParams(ref string translation, GameObject root)
    {
        Regex regex = new Regex("{\\[(.*?)\\]}");
        MatchCollection matchCollection = regex.Matches(translation);
        int i = 0;
        int count = matchCollection.Count;
        while (i < count)
        {
            Match match = matchCollection[i];
            string value = match.Groups[match.Groups.Count - 1].Value;
            string localizationParam = LocalizationManager.GetLocalizationParam(value, root);
            if (localizationParam != null)
            {
                translation = translation.Replace(match.Value, localizationParam);
            }

            i++;
        }
    }

    internal static string GetLocalizationParam(string ParamName, GameObject root)
    {
        if (root)
        {
            MonoBehaviour[] components = root.GetComponents<MonoBehaviour>();
            int i = 0;
            int num = components.Length;
            while (i < num)
            {
                ILocalizationParamsManager localizationParamsManager = components[i] as ILocalizationParamsManager;
                if (localizationParamsManager != null)
                {
                    string parameterValue = localizationParamsManager.GetParameterValue(ParamName);
                    if (parameterValue != null)
                    {
                        return parameterValue;
                    }
                }

                i++;
            }
        }

        int j = 0;
        int count = LocalizationManager.ParamManagers.Count;
        while (j < count)
        {
            string parameterValue = LocalizationManager.ParamManagers[j].GetParameterValue(ParamName);
            if (parameterValue != null)
            {
                return parameterValue;
            }

            j++;
        }

        return null;
    }

    /// <summary>
    /// 更新源
    /// </summary>
    /// <returns></returns>
    public static bool UpdateSources()
    {
        return false;
        // //注销已删除的源
        // for (var i = LocalizationManager.Sources.Count - 1; i >= 0; i--)
        // {
        //     if (LocalizationManager.Sources[i] != null) continue;
        //     LocalizationManager.Sources.Remove(LocalizationManager.Sources[i]);
        // }
        //
        // //在资源中注册源
        // foreach (var name in LocalizationManager.GlobalSources)
        // {
        //     GameObject asset = ResourceManager.pInstance.GetAsset<GameObject>(name);
        //     LanguageSource languageSource = (!asset) ? null : asset.GetComponent<LanguageSource>();
        //     if (languageSource && !LocalizationManager.Sources.Contains(languageSource))
        //     {
        //         LocalizationManager.AddSource(languageSource);
        //     }
        // }
        //
        // //注册场景源
        // LanguageSource[] array = (LanguageSource[])Resources.FindObjectsOfTypeAll(typeof(LanguageSource));
        // int i = 0;
        // int num = array.Length;
        // while (i < num)
        // {
        //     if (!LocalizationManager.Sources.Contains(array[i]))
        //     {
        //         LocalizationManager.AddSource(array[i]);
        //     }
        //
        //     i++;
        // }


        return LocalizationManager.Sources.Count > 0;
    }


    internal static void AddSource(LanguageSource Source)
    {
        if (LocalizationManager.Sources.Contains(Source))
        {
            return;
        }

        LocalizationManager.Sources.Add(Source);
        Source.Import_Google_FromCache();
        if (Source.GoogleUpdateDelay > 0f)
        {
            Source.Invoke("Delayed_Import_Google", Source.GoogleUpdateDelay);
        }
        else
        {
            Source.Import_Google(false);
        }

        if (Source.mDictionary.Count == 0)
        {
            Source.UpdateDictionary(true);
        }
    }

    public static bool IsGlobalSource(string SourceName)
    {
        return Array.IndexOf<string>(LocalizationManager.GlobalSources, SourceName) >= 0;
    }

    public static bool HasLanguage(string Language, bool AllowDiscartingRegion = true, bool Initialize = true, bool SkipDisabled = true)
    {
        if (Initialize)
        {
            LocalizationManager.InitializeIfNeeded();
        }

        int i = 0;
        int count = LocalizationManager.Sources.Count;
        while (i < count)
        {
            if (LocalizationManager.Sources[i].GetLanguageIndex(Language, false, SkipDisabled) >= 0)
            {
                return true;
            }

            i++;
        }

        if (AllowDiscartingRegion)
        {
            int j = 0;
            int count2 = LocalizationManager.Sources.Count;
            while (j < count2)
            {
                if (LocalizationManager.Sources[j].GetLanguageIndex(Language, true, SkipDisabled) >= 0)
                {
                    return true;
                }

                j++;
            }
        }

        return false;
    }

    public static string GetSupportedLanguage(string Language)
    {
        int i = 0;
        int count = LocalizationManager.Sources.Count;
        while (i < count)
        {
            int languageIndex = LocalizationManager.Sources[i].GetLanguageIndex(Language, false, true);
            if (languageIndex >= 0)
            {
                return LocalizationManager.Sources[i].mLanguages[languageIndex].Name;
            }

            i++;
        }

        int j = 0;
        int count2 = LocalizationManager.Sources.Count;
        while (j < count2)
        {
            int languageIndex2 = LocalizationManager.Sources[j].GetLanguageIndex(Language, true, true);
            if (languageIndex2 >= 0)
            {
                return LocalizationManager.Sources[j].mLanguages[languageIndex2].Name;
            }

            j++;
        }

        return string.Empty;
    }

    public static string GetLanguageCode(string Language)
    {
        int i = 0;
        int count = LocalizationManager.Sources.Count;
        while (i < count)
        {
            int languageIndex = LocalizationManager.Sources[i].GetLanguageIndex(Language, true, true);
            if (languageIndex >= 0)
            {
                return LocalizationManager.Sources[i].mLanguages[languageIndex].Code;
            }

            i++;
        }

        return string.Empty;
    }

    public static string GetLanguageFromCode(string Code)
    {
        int i = 0;
        int count = LocalizationManager.Sources.Count;
        while (i < count)
        {
            int languageIndexFromCode = LocalizationManager.Sources[i].GetLanguageIndexFromCode(Code);
            if (languageIndexFromCode >= 0)
            {
                return LocalizationManager.Sources[i].mLanguages[languageIndexFromCode].Name;
            }

            i++;
        }

        return string.Empty;
    }

    public static List<string> GetAllLanguages(bool SkipDisabled = true)
    {
        List<string> Languages = new List<string>();
        int i = 0;
        int count = LocalizationManager.Sources.Count;
        while (i < count)
        {
            Languages.AddRange(from x in LocalizationManager.Sources[i].GetLanguages(SkipDisabled)
                where !Languages.Contains(x)
                select x);
            i++;
        }

        return Languages;
    }

    public static List<string> GetCategories()
    {
        List<string> list = new List<string>();
        int i = 0;
        int count = LocalizationManager.Sources.Count;
        while (i < count)
        {
            LocalizationManager.Sources[i].GetCategories(false, list);
            i++;
        }

        return list;
    }

    public static List<string> GetTermsList(string Category = null)
    {
        if (LocalizationManager.Sources.Count == 0)
        {
            LocalizationManager.UpdateSources();
        }

        if (LocalizationManager.Sources.Count == 1)
        {
            return LocalizationManager.Sources[0].GetTermsList(Category);
        }

        HashSet<string> hashSet = new HashSet<string>();
        int i = 0;
        int count = LocalizationManager.Sources.Count;
        while (i < count)
        {
            hashSet.UnionWith(LocalizationManager.Sources[i].GetTermsList(Category));
            i++;
        }

        return new List<string>(hashSet);
    }

    public static TermData GetTermData(string term)
    {
        int i = 0;
        int count = LocalizationManager.Sources.Count;
        while (i < count)
        {
            TermData termData = LocalizationManager.Sources[i].GetTermData(term, false);
            if (termData != null)
            {
                return termData;
            }

            i++;
        }

        return null;
    }

    public static LanguageSource GetSourceContaining(string term, bool fallbackToFirst = true)
    {
        if (!string.IsNullOrEmpty(term))
        {
            int i = 0;
            int count = LocalizationManager.Sources.Count;
            while (i < count)
            {
                if (LocalizationManager.Sources[i].GetTermData(term, false) != null)
                {
                    return LocalizationManager.Sources[i];
                }

                i++;
            }
        }

        return (!fallbackToFirst || LocalizationManager.Sources.Count <= 0) ? null : LocalizationManager.Sources[0];
    }

    public static UnityEngine.Object FindAsset(string value)
    {
        int i = 0;
        int count = LocalizationManager.Sources.Count;
        while (i < count)
        {
            UnityEngine.Object @object = LocalizationManager.Sources[i].FindAsset(value);
            if (@object)
            {
                return @object;
            }

            i++;
        }

        return null;
    }

    public static string GetVersion()
    {
        return "2.6.10 b3";
    }

    public static int GetRequiredWebServiceVersion()
    {
        return 4;
    }

    public static string GetWebServiceURL(LanguageSource source = null)
    {
        if (source != null && !string.IsNullOrEmpty(source.Google_WebServiceURL))
        {
            return source.Google_WebServiceURL;
        }

        for (int i = 0; i < LocalizationManager.Sources.Count; i++)
        {
            if (LocalizationManager.Sources[i] != null && !string.IsNullOrEmpty(LocalizationManager.Sources[i].Google_WebServiceURL))
            {
                return LocalizationManager.Sources[i].Google_WebServiceURL;
            }
        }

        return string.Empty;
    }

    private static bool IsRTL(string Code)
    {
        return Array.IndexOf<string>(LocalizationManager.LanguagesRTL, Code) >= 0;
    }

    private static string mCurrentLanguage;

    private static string mLanguageCode;

    private static bool mChangeCultureInfo = false;

    public static bool IsRight2Left = false;

    public static List<LanguageSource> Sources = new List<LanguageSource>();

    public static string[] GlobalSources = new string[]
    {
        "I2Languages"
    };

    public static List<ILocalizationParamsManager> ParamManagers = new List<ILocalizationParamsManager>();

    private static bool mLocalizeIsScheduled = false;

    private static bool mLocalizeIsScheduledWithForcedValue = false;

    private static string[] LanguagesRTL = new string[]
    {
        "ar-DZ",
        "ar",
        "ar-BH",
        "ar-EG",
        "ar-IQ",
        "ar-JO",
        "ar-KW",
        "ar-LB",
        "ar-LY",
        "ar-MA",
        "ar-OM",
        "ar-QA",
        "ar-SA",
        "ar-SY",
        "ar-TN",
        "ar-AE",
        "ar-YE",
        "he",
        "ur",
        "ji"
    };

    public delegate void OnLocalizeCallback();
}