using System.Collections;
using Framework.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 管卡管理器
/// </summary>
public class LevelManager
{
    /// <summary>
    /// 当前场景名称
    /// </summary>
    public static string SceneName => SceneManager.GetActiveScene().name;

    /// <summary>
    /// 死亡人数
    /// </summary>
    private static int DeathCount
    {
        get => RoundStorage.Get("DeathCount", 0);
        set => RoundStorage.Set("DeathCount", value);
    }

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="levelName"></param>
    /// <returns></returns>
    public static AsyncOperation LoadScene(string levelName)
    {
        if (LevelManager._asyncOperation != null && !LevelManager._asyncOperation.isDone)
            return LevelManager._asyncOperation;
        LevelManager._asyncOperation = SceneManager.LoadSceneAsync(levelName);
        if (LevelManager._asyncOperation == null)
            (levelName + "场景不存在，是不是没放在build里？还是名字写错了？").Warning();
        return LevelManager._asyncOperation;
    }

    public static Coroutine LoadLevelByGateId(string levelName, SceneGate.OpenType openType = SceneGate.OpenType.None)
    {
        return LevelManager.LoadLevelByGateId(levelName, 1, openType);
    }

    public static Coroutine LoadLevelByGateId(string levelName, int gateId,
        SceneGate.OpenType openType = SceneGate.OpenType.None)
    {
        return R.SceneGate.Enter(new SwitchLevelGateData
        {
            ToLevelId = levelName,
            ToId = gateId,
            OpenType = openType
        }, false);
    }

    /// <summary>
    /// 按位置划分的负载等级
    /// </summary>
    /// <param name="levelName"></param>
    /// <param name="position"></param>
    /// <param name="needProgressBar"></param>
    /// <returns></returns>
    public static Coroutine LoadLevelByPosition(string levelName, Vector3 position, bool needProgressBar = false)
    {
        return R.SceneGate.Enter(new SwitchLevelGateData
        {
            ToLevelId = levelName,
            ToId = -1,
            TargetPosition = position
        }, needProgressBar);
    }

    public static Coroutine OnRoundOver()
    {
        return LevelManager.RoundOverCoroutine().StartCoroutine();
    }

    public static Coroutine OnPlayerDie(bool recordDeath = true)
    {
        return LevelManager.LoadGame(recordDeath).StartCoroutine();
    }

    private static IEnumerator RoundOverCoroutine()
    {
        yield break;
        //yield return R.Ui.LevelSelect.OpenWithAnim(true, true);
    }

    private static IEnumerator LoadGame(bool recordDeath)
    {
        R.DeadReset();
        yield return R.GameData.Load();
        yield return LevelManager.LoadLevelByPosition(R.GameData.SceneName, R.GameData.PlayerPosition, false);
        if (recordDeath)
        {
            LevelManager.DeathCount++;
            R.GameData.Save(true);
        }

        R.Player.Transform.position = R.GameData.PlayerPosition;
        R.Player.Action.TurnRound(R.GameData.PlayerAttributeGameData.faceDir);
        R.Camera.Controller.CameraResetPostionAfterSwitchScene();
        PlayerAction.Reborn();
        yield break;
    }

    private static AsyncOperation _asyncOperation;

    public struct LevelName
    {
        public const string Start = "ui_start";
    }
}