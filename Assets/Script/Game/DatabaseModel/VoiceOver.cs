using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

/// <summary>
/// 旁白
/// </summary>
public class VoiceOver
{
    public string Key { get; private set; }

    public string FilePath { get; private set; }

    public string FileName { get; private set; }

    /// <summary>
    /// 本地化的副标题
    /// </summary>
    public string LocalizedSubtitle => ScriptLocalization.Get("subtitle/" + this.Key);

    public static VoiceOver[] FindByKey(string key)
    {
        if (DB.VoiceOvers.ContainsKey(key))
        {
            return (from pair in DB.VoiceOvers
                where Regex.IsMatch(pair.Key, string.Format("^{0}(\\.\\d+)?$", key))
                select pair.Value).ToArray<VoiceOver>();
        }

        throw new KeyNotFoundException(key + "不在数据库VoiceOver中");
    }

    public static VoiceOver SetValue(string[] strings)
    {
        return new VoiceOver
        {
            Key = strings[0],
            FilePath = strings[1],
            FileName = strings[2]
        };
    }
}