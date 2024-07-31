using Framework.Core;
using UnityEngine;

/// <summary>
/// 摄像机效果代理预制数据
/// </summary>
public class CameraEffectProxyPrefabData
{
    public int id { get; set; }

    public string type { get; set; }

    public string name { get; set; }

    public string path { get; set; }

    public string usedEffect { get; set; }

    public string desc { get; set; }

    public static GameObject GetPrefab(int id)
    {
        CameraEffectProxyPrefabData cameraEffectProxyPrefabData = DB.CameraEffectProxyPrefabData[id];
        return (cameraEffectProxyPrefabData.path + cameraEffectProxyPrefabData.name).Load<GameObject>();
    }

    public static CameraEffectProxyPrefabData SetValue(string[] strings)
    {
        return new CameraEffectProxyPrefabData
        {
            id = int.Parse(strings[0]),
            type = strings[1],
            name = strings[2],
            path = strings[3],
            usedEffect = strings[4],
            desc = strings[5]
        };
    }
}