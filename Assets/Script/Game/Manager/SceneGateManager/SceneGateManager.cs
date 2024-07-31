using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Core;
using UnityEngine;

/// <summary>
/// 场景门管理器
/// </summary>
public class SceneGateManager : SingletonNewMono<SceneGateManager>
{
    /// <summary>
    /// 允许场景激活
    /// </summary>
    public bool AllowSceneActivation = true;

    /// <summary>
    /// 异步加载
    /// </summary>
    private AsyncOperation Async;

    /// <summary>
    /// 被锁定
    /// </summary>
    public bool IsLocked;

    /// <summary>
    /// 当前场景中的门
    /// </summary>
    public List<SceneGate> GatesInCurrentScene = new List<SceneGate>();

    /// <summary>
    /// 进度
    /// </summary>
    public float Progress => Async == null ? 0f : Async.progress;

    /// <summary>
    /// 加载等级场景携程
    /// </summary>
    /// <param name="data"></param>
    /// <param name="needProgressBar"></param>
    /// <returns></returns>
    private IEnumerator LoadLevelCoroutine(SwitchLevelGateData data, bool needProgressBar)
    {
        this.IsLocked = true;
        R.Player.ActionController.ChangeState(PlayerAction.StateEnum.Idle);
        var player2Ground = Physics2D.Raycast(R.Player.Transform.position, Vector2.down, 100f, LayerManager.GroundMask).distance;
        this.Async = LevelManager.LoadScene(data.ToLevelId);
        this.Async.allowSceneActivation = false;
        while (!this.AllowSceneActivation)
            yield return null;

        if (needProgressBar)
            yield return ConfigPrefab.UICutTo.PopUpWindow<UICutTo>().FadeBlack(0.3f, false);
        Async.allowSceneActivation = true;
        yield return Async;
        Preload().StartCoroutine();
        IsLocked = false;
        if (data.ToId != -1)
        {
            Exit(FindGateData(data.ToId), player2Ground, data.OpenType);
        }
        else
        {
            SwitchLevelGateData data2 = new SwitchLevelGateData();
            data2.SelfPosition = data.TargetPosition;
            Exit(data2, 0f, SceneGate.OpenType.None);
        }
    }

    /// <summary>
    /// 进入
    /// </summary>
    /// <param name="data"></param>
    /// <param name="needProgressBar">需要进度条</param>
    /// <returns></returns>
    public Coroutine Enter(SwitchLevelGateData data, bool needProgressBar = false)
    {
        EGameEvent.PassGate.Trigger((gameObject, new PassGateEventArgs(PassGateEventArgs.PassGateStatus.Enter, data, LevelManager.SceneName)));
        string.Format("从 Gate {1} 离开 {0}", LevelManager.SceneName, data.MyId).Log();
        return EnterCoroutine(data, needProgressBar).StartCoroutine();

        IEnumerator EnterCoroutine(SwitchLevelGateData dataValue, bool needProgressBarValue)
        {
            R.SceneData.CanAIRun = false; //是否跑AI
            InputSetting.Stop(false);
            IsLocked = true;
            if (!needProgressBarValue)
                yield return ConfigPrefab.UICutTo.PopUpWindow<UICutTo>().FadeBlack(0.3f, false);
            IsLocked = false;
            yield return LoadLevelCoroutine(dataValue, needProgressBarValue).StartCoroutine();
        }
    }

    /// <summary>
    /// 离开
    /// </summary>
    /// <param name="data"></param>
    /// <param name="groundDis">玩家与地面的距离</param>
    /// <param name="enterGateOpenType"></param>
    /// <returns></returns>
    public Coroutine Exit(SwitchLevelGateData data, float groundDis, SceneGate.OpenType enterGateOpenType)
    {
        EGameEvent.PassGate.Trigger((gameObject, new PassGateEventArgs(PassGateEventArgs.PassGateStatus.Exit, data, LevelManager.SceneName)));
        (string.Format("从 Gate {1} 进入 {0}", LevelManager.SceneName, data.MyId)).Log();
        return ExitCoroutine(data, groundDis, enterGateOpenType).StartCoroutine();
    }

    /// <summary>
    /// 退出
    /// </summary>
    /// <param name="data">切换关卡大门数据</param>
    /// <param name="player2Ground">玩家与地面的距离</param>
    /// <param name="enterGateOpenType">打开方式</param>
    /// <returns></returns>
    private IEnumerator ExitCoroutine(SwitchLevelGateData data, float player2Ground, SceneGate.OpenType enterGateOpenType)
    {
        IsLocked = true;
        var pos = R.Player.Transform.position;
        pos.x = data.SelfPosition.x;
        if (data.InAir)
        {
            pos.y = data.SelfPosition.y;
        }
        else //不在地上
        {
            //射线检测->朝下检测
            float distance = Physics2D.Raycast(data.SelfPosition, Vector2.down, 100f, LayerManager.GroundMask).distance;
            float num = data.SelfPosition.y - distance;
            pos.y = player2Ground + num;
            if (data.OpenType != SceneGate.OpenType.PressKey)
            {
                if (enterGateOpenType != SceneGate.OpenType.Left)
                {
                    if (enterGateOpenType == SceneGate.OpenType.Right)
                        pos.x -= data.TriggerSize.x + 1f;
                }
                else
                {
                    pos.x += data.TriggerSize.x + 1f;
                }
            }
        }

        R.Player.Transform.position = pos; //设置玩家位置
        if (R.GameData.WindyVisiable && R.Windy != null)
        {
            switch (data.OpenType)
            {
                case SceneGate.OpenType.Right:
                    R.Windy.transform.position = pos + new Vector3(-2f, 1f, 0f);
                    break;
                case SceneGate.OpenType.Left:
                    R.Windy.transform.position = pos + new Vector3(2f, 1f, 0f);
                    break;
                default:
                    R.Windy.transform.position = pos;
                    break;
            }
        }

        if (!InputSetting.IsWorking())
        {
            InputSetting.Resume(false);
        }

        R.Camera.Controller.CameraResetPostionAfterSwitchScene();
        yield return new WaitForFixedUpdate();
        if (enterGateOpenType != SceneGate.OpenType.Left)
        {
            if (enterGateOpenType == SceneGate.OpenType.Right)
            {
                R.Player.ActionController.TurnRound(-1);
            }
        }
        else
        {
            R.Player.ActionController.TurnRound(1);
        }

        yield return R.Ui.BlackScene.FadeTransparent(0.3f, false);
        this.IsLocked = false;
        yield break;
    }

    /// <summary>
    /// 找到门数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public SwitchLevelGateData FindGateData(int id)
    {
        return this.FindGate(id).data;
    }

    /// <summary>
    /// 寻找门
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public SceneGate FindGate(int id)
    {
        for (var i = 0; i < this.GatesInCurrentScene.Count; i++)
        {
            if (GatesInCurrentScene[i].data.MyId != id) continue;
            return GatesInCurrentScene[i];
        }

        throw new NullReferenceException(string.Concat("场景中没有门 ", LevelManager.SceneName, " id ", id));
    }

    /// <summary>
    /// 预加载
    /// </summary>
    /// <returns></returns>
    private IEnumerator Preload()
    {
        yield break;
    }
}