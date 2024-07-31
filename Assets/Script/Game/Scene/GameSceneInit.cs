using Framework.Core;
using UnityEngine;

/// <summary>
/// 场景初始化
/// </summary>
public class GameSceneInit : MonoBehaviour
{
    private void Awake()
    {
        if (!GameSceneInit._first) return;
        QualitySettings.vSyncCount = R.Settings.VSync;  
        if (string.IsNullOrEmpty(R.Settings.Language))
        {
            R.Settings.Language = LocalizationManager.CurrentLanguage;
            R.Settings.Save();
        }
        else
        {
            LocalizationManager.CurrentLanguage = R.Settings.Language;
        }

        if (string.IsNullOrEmpty(R.Settings.AudioLanguage))
        {
            R.Settings.AudioLanguage = R.Settings.Language;
            R.Settings.Save();
        }

        Cursor.visible = false;
        Preload x = Object.FindObjectOfType<Preload>();
        GameObject gameObject = null;
        if (x == null)
        {
            // gameObject = Asset.LoadFromResources<GameObject>("Prefab/Core", "Preload");
            gameObject ="Preload".Combine("Prefab/Core").Load<GameObject>(); 
            gameObject = UnityEngine.Object.Instantiate<GameObject>(gameObject);
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
        }

        GameObject gameObject2 = GameObject.FindGameObjectWithTag("Core");
        if (gameObject2 == null)
        {
            //gameObject2 = Asset.LoadFromResources<GameObject>("Prefab/Core", "Core");
            gameObject2 = "Core".Combine("Prefab/Core").Load<GameObject>(); 
            gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject2);
        }

        UnityEngine.Object.DontDestroyOnLoad(gameObject2);
        GameObject gameObject3 = GameObject.FindGameObjectWithTag("World");
        if (gameObject3 == null)
        {
            //gameObject3 = Asset.LoadFromResources<GameObject>("Prefab/Core", "World");
            gameObject3 = "World".Combine("Prefab/Core").Load<GameObject>(); 
            gameObject3 = UnityEngine.Object.Instantiate<GameObject>(gameObject3);
        }

        UnityEngine.Object.DontDestroyOnLoad(gameObject3);
        GameObject gameObject4 = GameObject.FindGameObjectWithTag("BattleZoneTrigger");
        if (gameObject4 == null)
        {
            //gameObject4 = Asset.LoadFromResources<GameObject>("Prefab/Core", "BattleZoneTrigger");
            gameObject4 = "BattleZoneTrigger".Combine("Prefab/Core").Load<GameObject>(); 
            gameObject4 = UnityEngine.Object.Instantiate<GameObject>(gameObject4);
        }

        UnityEngine.Object.DontDestroyOnLoad(gameObject4);
        EffectController effectController = UnityEngine.Object.FindObjectOfType<EffectController>();
        GameObject gameObject5;
        if (effectController == null)
        {
            //gameObject5 = Asset.LoadFromResources<GameObject>("Prefab/Core", "EffectGenerator");
            gameObject5 = "EffectGenerator".Combine("Prefab/Core").Load<GameObject>(); 
            gameObject5 = UnityEngine.Object.Instantiate<GameObject>(gameObject5);
        }
        else
        {
            gameObject5 = effectController.gameObject;
        }

        UnityEngine.Object.DontDestroyOnLoad(gameObject5);
        if (Camera.main == null)
        {
            //GameObject gameObject6 = Asset.LoadFromResources<GameObject>("Prefab/Core", "camera");
            GameObject gameObject6 = "camera".Combine("Prefab/Core").Load<GameObject>(); 
            UnityEngine.Object.Instantiate<GameObject>(gameObject6);
        }

        if (Camera.main != null)
        {
            GameObject gameObject6 = Camera.main.transform.parent.gameObject;
            UnityEngine.Object.DontDestroyOnLoad(gameObject6);
        }
    }

    private void Start()
    {
        if (!GameSceneInit._first) return;
        //UI的Canvas
        GameObject mainCanvasTemp = GameObject.FindGameObjectWithTag(ConfigTag.UIRoot);
        if (mainCanvasTemp == null)
        {
            mainCanvasTemp = ConfigPrefab.MainCanvas.Load<GameObject>(ConfigPath.UIPath);
            mainCanvasTemp = UnityEngine.Object.Instantiate<GameObject>(mainCanvasTemp);
        }

        DontDestroyOnLoad(mainCanvasTemp);

        //玩家
        GameObject playerTemp = GameObject.FindGameObjectWithTag(ConfigTag.Player);
        if (!playerTemp)
        {
            playerTemp = ConfigPrefab.Player.Load<GameObject>(ConfigPath.RolePath);
            playerTemp = UnityEngine.Object.Instantiate<GameObject>(playerTemp);
        }

        DontDestroyOnLoad(playerTemp);
        Vector3 position = playerTemp.transform.position;
        position.z = LayerManager.ZNum.MMiddle_P; //设置Z深度
        playerTemp.transform.position = position;

        if (R.GameData.WindyVisiable) //玩家宠物是否可见
        {
            //玩家跟随的宠物
            GameObject windyTemp = GameObject.FindGameObjectWithTag(ConfigTag.Windy);
            if (windyTemp == null)
            {
                windyTemp = ConfigPrefab.Windy.Load<GameObject>(ConfigPath.RolePath);
                windyTemp = UnityEngine.Object.Instantiate<GameObject>(windyTemp);
            }

            UnityEngine.Object.DontDestroyOnLoad(windyTemp);
        }

        //离开门
        SceneGate sceneGate = SceneGateManager.Instance.FindGate(1);
        sceneGate?.Exit(0f, SceneGate.OpenType.None);
        GameSceneInit._first = false;
    }

    /// <summary>
    /// 是否是第一次
    /// </summary>
    private static bool _first = true;
}