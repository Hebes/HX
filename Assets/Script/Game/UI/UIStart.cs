using System.Collections;
using DG.Tweening;
using ExpansionUnity;
using Framework.Core;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 开始菜单
/// </summary>
public class UIStart : WindowBase
{
    public GameObject T_Container_mainGameObject;
    public GameObject T_StartGameGameObject;
    public GameObject T_OptionGameObject;

    public CanvasGroupComponent T_Container_mainCanvasGroupComponent;


    public override void OnAwake()
    {
        base.OnAwake();

        UIComponent uiComponent = GetComponent<UIComponent>();
        T_Container_mainGameObject = uiComponent.Get("T_Container_main");
        T_StartGameGameObject = uiComponent.Get("T_StartGame");
        T_OptionGameObject = uiComponent.Get("T_Option");

        T_Container_mainCanvasGroupComponent = uiComponent.GetComponent<CanvasGroupComponent>("T_Container_main");


        T_StartGameGameObject.AddEventTrigger(EventTriggerType.PointerClick, StartGame);
        T_OptionGameObject.AddEventTrigger(EventTriggerType.PointerClick, Option);
    }

    private void Option(PointerEventData obj)
    {
        HideWindow();
        PopUpWindow<UIOption>();
    }

    private void StartGame(PointerEventData obj)
    {
        // if (!Core.Input.JoystickIsOpen)
        // {
        //     return;
        // }
        // if (UIStartController.IsEnterWithVoice)
        // {
        //     base.StartCoroutine(this.OnStartWithVoiceOverClick());
        // }
        // else
        // {
        //     base.StartCoroutine(this.OnStartClickCoroutine());
        // }

        OnStartClickCoroutine().StartCoroutine();
    }

    /// <summary>
    /// 开始按钮点击
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnStartClickCoroutine()
    {
        T_Container_mainCanvasGroupComponent.Disappear();
        //TODO InputSetting.Stop(false);
        //UIKeyInput.LoadHoveredObject();
        //this._loading.alpha = 1f;
        SceneGateManager.Instance.AllowSceneActivation = false; //不允许场景激活
        GetWindow<UIPause>().Enabled = true;
        if (SaveManager.IsAutoSaveDataExists) //是否存在自动保存数据
        {
            yield return R.GameData.Load(); //加载数据
            MobileInputPlayer.Instance.VisiableBladeStorm(); //显示技能可见刀锋风暴
            LevelManager.LoadLevelByPosition(R.GameData.SceneName, R.GameData.PlayerPosition, true);
            yield return ProgressAnimCoroutine().StartCoroutine();
            R.Player.Transform.position = R.GameData.PlayerPosition;
            R.Player.Action.TurnRound(R.GameData.PlayerAttributeGameData.faceDir);
            R.Camera.Controller.CameraResetPostionAfterSwitchScene();
        }
        else
        {
            _tutorialGate.Enter(true);
            SingletonMono<MobileInputPlayer>.Instance.VisiableBladeStorm();
            StartCoroutine(ProgressAnimCoroutine());
        }
    }

    private void KillTweenerIfPlaying()
    {
        if (_tweener != null && _tweener.IsPlaying())
            _tweener.Kill();
    }

    private Tweener _tweener;


    [Header("教程门")] [SerializeField] private SceneGate _tutorialGate;

    private IEnumerator ProgressAnimCoroutine()
    {
        float nowprocess = 0f;
        float currentVelocity = 0f;
        float smoothTime = 0.1f;
        while (nowprocess < 99.5f)
        {
            float toProcess;
            if (R.SceneGate.Progress < 0.9f)
            {
                toProcess = R.SceneGate.Progress * 100f;
            }
            else
            {
                toProcess = 100f;
            }

            if (nowprocess < toProcess)
            {
                nowprocess = Mathf.SmoothDamp(nowprocess, toProcess, ref currentVelocity, smoothTime);
            }

            //this._loading.value = nowprocess / 100f;
            yield return null;
        }

        // DOTween.To(() => this._black.color, delegate(Color c) { this._black.color = c; }, Color.black, 2f).WaitForCompletion();
        // InputSetting.Resume();
        // R.Mode.ExitMode(Mode.AllMode.UI);
        // UIKeyInput.LoadHoveredObject();
        // R.SceneGate.AllowSceneActivation = true;
        // R.Ui.ShowUI(false);
        yield return new WaitForSeconds(3f);
    }
}