using Framework.Core;
using LitJson;
using UnityEngine;

public static class R
{
    /// <summary>
    /// 游戏数据
    /// </summary>
    private static GameData _gameData;

    private static GameObject _windy;
    private static SettingData _settings;

    public static readonly EnemyManager Enemy = new EnemyManager();
    public static readonly PlayerManager Player = new PlayerManager();
    public static readonly CameraManager Camera = new CameraManager();
    public static readonly SceneData SceneData = new SceneData();
    public static readonly Mode Mode = new Mode();
    public static TrophyManager Trophy = new TrophyManager();

    public static UIController Ui => UIController.Instance;


    /// <summary>
    /// 场景门管理器
    /// </summary>
    public static SceneGateManager SceneGate => SceneGateManager.Instance;


    /// <summary>
    /// 影响
    /// </summary>
    public static EffectController Effect => EffectController.Instance;

    /// <summary>
    /// 音效
    /// </summary>
    public static AudioManager Audio => AudioManager.Instance;

    /// <summary>
    /// 装备
    /// </summary>
    public static Equipment Equipment => R.GameData.Equipment;

    /// <summary>
    /// 跟随玩家的会飞的
    /// </summary>
    public static GameObject Windy
    {
        get
        {
            GameObject result;
            if ((result = R._windy) == null)
                result = R._windy = GameObject.FindGameObjectWithTag(ConfigTag.Windy);
            return result;
        }
    }

    /// <summary>
    /// 游戏数据
    /// </summary>
    public static GameData GameData
    {
        get
        {
            GameData result;
            if ((result = _gameData) == null)
                result = R._gameData = new GameData();
            return result;
        }
        set => _gameData = value;
    }


    public static SettingData Settings
    {
        get
        {
            if (R._settings != null)
            {
                return R._settings;
            }

            if (PlayerPrefs.HasKey("GameSettings"))
            {
                R._settings = JsonMapper.ToObject<SettingData>(PlayerPrefs.GetString("GameSettings"));
                "已加载设置".Log();
            }
            else
            {
                R._settings = new SettingData();
                "创建设置".Log();
            }

            return R._settings;
        }
    }
    
    public static void DeadReset()
    {
        R.Mode.Reset();
        Cliff.Reset();
        WorldTime.Reset();
        R.Ui.Reset();
        "死亡重置数据".Log();
    }
}