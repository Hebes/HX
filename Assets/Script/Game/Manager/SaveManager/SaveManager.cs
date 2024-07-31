using System;
using System.Collections;
using Framework.Core;
using UnityEngine;

/// <summary>
/// 保存管理类
/// </summary>
[CreateCore(typeof(AchievementManager), 1)]
public class SaveManager : ICore
{
    public void Init()
    {
        SaveData.OnGameLoaded += this.OnSavedGameLoaded;
    }

    public IEnumerator AsyncEnter()
    {
        yield break;
    }

    public IEnumerator Exit()
    {
        SaveData.OnGameLoaded -= this.OnSavedGameLoaded;
        yield break;
    }

    /// <summary>
    /// 存档数据
    /// </summary>
    private static GameData _gameDataLoaded;

    public static GameData GameData => SaveManager._gameDataLoaded;


    /// <summary>
    /// 存档数据路径
    /// </summary>
    private static string SaveDataPath
    {
        get
        {
            var savePath = Application.persistentDataPath + "/SaveData/";
            $"当前保存路径{savePath}".Log();
            return savePath;
        }
    }

    /// <summary>
    /// 是否正在存档
    /// </summary>
    public static bool IsBusy => SaveData.IsBusy;

    /// <summary>
    /// 自动保存数据是否存在
    /// </summary>
    public static bool IsAutoSaveDataExists => SaveData.IsAutoSaveDataExists();

    //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public static event EventHandler<SaveLoadedEventArgs> OnGameLoaded;


    /// <summary>
    /// 自动加载
    /// </summary>
    /// <returns></returns>
    public static Coroutine AutoLoad()
    {
        _gameDataLoaded = null;
        SaveData.Load();
        return AutoLoadAndWaitForLoadedCoroutine().StartCoroutine();

        static IEnumerator AutoLoadAndWaitForLoadedCoroutine()
        {
            while (SaveManager._gameDataLoaded == null)
                yield return null;
        }
    }

    /// <summary>
    /// 自动保存
    /// </summary>
    /// <param name="gameData"></param>
    /// <returns></returns>
    public static Coroutine AutoSave(GameData gameData)
    {
        return AutoSaveWhenIsNotBusy(gameData).StartCoroutine();

        static IEnumerator AutoSaveWhenIsNotBusy(GameData gameData)
        {
            while (SaveManager.IsBusy)
                yield return null;
            SaveData.Save(gameData);
        }
    }

    /// <summary>
    /// 自动删除
    /// </summary>
    public static void AutoDelete()
    {
        SaveData.Delete();
    }

    /// <summary>
    /// 修改保存数据
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Coroutine ModifySaveData(Action<GameData> action)
    {
        return ModifySaveDataCoroutine(action).StartCoroutine();

        static IEnumerator ModifySaveDataCoroutine(Action<GameData> action)
        {
            yield return SaveManager.AutoLoad();
            GameData gameData = _gameDataLoaded;
            action(gameData);
            SaveManager.AutoSave(gameData);
        }
    }

    /// <summary>
    /// 保存游戏加载
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnSavedGameLoaded(object sender, SaveLoadedEventArgs e)
    {
        GameData gameData = e.GameData;
        SaveManager._gameDataLoaded = gameData;
        if (SaveManager.OnGameLoaded != null)
            SaveManager.OnGameLoaded(sender, e);
    }
}