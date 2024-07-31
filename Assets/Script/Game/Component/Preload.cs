using Framework.Core;
using UnityEngine;

/// <summary>
/// 预加载
/// </summary>
public class Preload : MonoBehaviour
{
    private void Awake()
    {
        PlayerPrefs.DeleteAll();
        Singleton<ResolutionOption>.Instance.SetResolutionByQualitylevel();
        EnemyGenerator.PreloadEnemyPrefabs();
        "敌人的预制件已装好".Log();
        DB.Preload();
        "数据已加载".Log();
        EffectController.AllowPreload = true;
        GameObject Temp = ConfigPrefab.PrefabAnimationPreload.Combine(ConfigPath.CorePath).Load<GameObject>();
        GameObject gameObjectValue = Instantiate(Temp);
        gameObjectValue.transform.parent = transform;
        DontDestroyOnLoad(gameObject);
    }
}