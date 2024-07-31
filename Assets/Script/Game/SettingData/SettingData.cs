using System.Collections.Generic;
using LitJson;
using UnityEngine;

/// <summary>
/// 设置的数据
/// </summary>
public class SettingData
{
    /// <summary>
    /// 保存
    /// </summary>
    public void Save()
    {
        PlayerPrefs.SetString("GameSettings", JsonMapper.ToJson(this));
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 配对模式
    /// </summary>
    public bool CheatMode;

    /// <summary>
    /// 副标题是否可见
    /// </summary>
    public bool SubtitleVisiable = true;

    /// <summary>
    /// 音量影响
    /// </summary>
    public float EffectsVolume = 100f;

    /// <summary>
    /// BGM影响
    /// </summary>
    public float BGMVolume = 100f;

    /// <summary>
    /// 是否静音
    /// </summary>
    public bool IsEffectsMute;

    /// <summary>
    /// BGM是否静音
    /// </summary>
    public bool IsBGMMute;

    /// <summary>
    /// 语言
    /// </summary>
    public string Language;

    /// <summary>
    /// 音频语言
    /// </summary>
    public string AudioLanguage;

    /// <summary>
    /// 是否振动
    /// </summary>
    public bool IsVibrate = true;

    /// <summary>
    /// 垂直同步
    /// </summary>
    public int VSync = 1;

    /// <summary>
    /// FPS
    /// </summary>
    public int FPS = 60;

    /// <summary>
    /// 动态操纵杆打开
    /// </summary>
    public bool DynamicJoystickOpen;

    /// <summary>
    /// 按键映射
    /// </summary>
    public Dictionary<string, KeyCode> KeyMap = new Dictionary<string, KeyCode>();

    /// <summary>
    /// 成绩信息
    /// </summary>
    public List<int> AchievementInfo = new List<int>();
}