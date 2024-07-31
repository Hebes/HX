using System;
using System.Collections.Generic;
using Framework.Core;

public static class DB
{
    public static IDictionary<int, AudioClipData> AudioClipData => DB._audioClipData;

    public static IDictionary<int, CameraEffectProxyPrefabData> CameraEffectProxyPrefabData => DB._cameraEffectProxyPrefabData;

    public static IList<EnemyAttrData> EnemyAttrData => DB._enemyAttrData;

    public static IDictionary<string, DatabaseDatabase> Enhancements => DB._enhancements;

    public static IDictionary<string, VoiceOver> VoiceOvers => DB._voiceOvers;

    public static void Preload()
    {
        if (DB._isPreloaded) return;
        string fileName = "AudioClipData";

        Func<string, int> setKey = new Func<string, int>(int.Parse);

        "请调用自己的加载文件".Error();
        // DB._audioClipData =
        //     CSVHelper.Csv2Dictionary<int, AudioClipData>(fileName, setKey, new Func<string[], AudioClipData>(DatabaseModel.AudioClipData.SetValue));
        // string fileName2 = "CameraEffectProxyPrefabData";
        //
        // Func<string, int> setKey2 = new Func<string, int>(int.Parse);
        //
        // DB._cameraEffectProxyPrefabData = CSVHelper.Csv2Dictionary<int, CameraEffectProxyPrefabData>(fileName2, setKey2,
        //     new Func<string[], CameraEffectProxyPrefabData>(DatabaseModel.CameraEffectProxyPrefabData.SetValue));
        // string fileName3 = "EnemyAttrData";
        //
        // DB._enemyAttrData = CSVHelper.Csv2List<EnemyAttrData>(fileName3, new Func<string[], EnemyAttrData>(DatabaseModel.EnemyAttrData.SetValue));
        // string fileName4 = "Enhancement";
        // Func<string, string> setKey3 = (string a) => a;
        //
        // DB._enhancements = CSVHelper.Csv2Dictionary<string, EnhancementSaveData>(fileName4, setKey3, new Func<string[], EnhancementSaveData>(EnhancementSaveData.SetValue));
        // string fileName5 = "VoiceOver";
        // Func<string, string> setKey4 = (string a) => a;
        //
        // DB._voiceOvers = CSVHelper.Csv2Dictionary<string, VoiceOver>(fileName5, setKey4, new Func<string[], VoiceOver>(VoiceOver.SetValue));
        // DB._isPreloaded = true;
    }

    private static IList<EnemyAttrData> _enemyAttrData;

    private static IDictionary<int, AudioClipData> _audioClipData;

    private static IDictionary<int, CameraEffectProxyPrefabData> _cameraEffectProxyPrefabData;

    private static IDictionary<string, DatabaseDatabase> _enhancements;

    private static IDictionary<string, VoiceOver> _voiceOvers;

    private static bool _isPreloaded;
}