using System.Collections.Generic;
using Framework.Core;

/// <summary>
/// 音频数据
/// </summary>
public class AudioClipData
{
    public int id { get; set; }

    public AudioClipData.AudioClipDataType type { get; set; }

    public string name { get; set; }

    public string path { get; set; }

    public string desc { get; set; }

    public float pitchMin { get; set; }

    public float pitchMax { get; set; }

    public float volumeMin { get; set; }

    public float volumeMax { get; set; }

    public static AudioClipData FindById(int id)
    {
        AudioClipData result;
        try
        {
            result = DB.AudioClipData[id];
        }
        catch (KeyNotFoundException)
        {
            ("AudioClipDataID: " + id + " 不存在").Error();
            throw;
        }

        return result;
    }

    public static AudioClipData SetValue(string[] strings)
    {
        return new AudioClipData
        {
            id = int.Parse(strings[0]),
            type = (AudioClipData.AudioClipDataType)int.Parse(strings[1]),
            name = strings[2],
            path = strings[3],
            desc = strings[4],
            pitchMin = float.Parse(strings[5]),
            pitchMax = float.Parse(strings[6]),
            volumeMin = float.Parse(strings[7]),
            volumeMax = float.Parse(strings[8])
        };
    }

    public enum AudioClipDataType
    {
        BGM = 1,
        EnemyBoss,
        EnemyElite,
        EnemyNormal,
        PlayerMove,
        PlayerAtk,
        PlayerMaterial,
        UI,
        Group,
        Scene,
        Special,
        Video
    }
}