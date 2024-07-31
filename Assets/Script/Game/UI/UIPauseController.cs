using System.Collections;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// 暂停界面
/// </summary>
public class UIPauseController : MonoBehaviour
{
	private bool HasEnteredLevelSelectSystem
	{
		get
		{
			return SaveStorage.Get("HasEnteredLevelSelectSystem", false);
		}
		set
		{
			SaveStorage.Set("HasEnteredLevelSelectSystem", value);
		}
	}

	// private void Start()
	// {
	// 	EventDelegate.Add(this._resume.onClick, new EventDelegate.Callback(this.OnResumeClick));
	// 	EventDelegate.Add(this._exit.onClick, new EventDelegate.Callback(this.OnExitClick));
	// 	EventDelegate.Add(this._levelSelect.onClick, new EventDelegate.Callback(this.OnLevelSelect));
	// }

	private void Update()
	{
		// if (this._panel.gameObject.activeInHierarchy && Core.Input.UI.Cancel.OnClick)
		// {
		// 	this.OnResumeClick();
		// }
	}

	public bool Enabled
	{
		get => Input.UI.Pause.IsOpen;
		set
		{
			Input.UI.Pause.IsOpen = value;
			SingletonMono<MobileInputPlayer>.Instance.OptionsVisible = value;
		}
	}

	private void Open()
	{
		// this._levelSelect.transform.parent.gameObject.SetActive(this.HasEnteredLevelSelectSystem);
		// R.Audio.PauseBGM();
		// R.Mode.EnterMode(Mode.AllMode.UI);
		// R.Ui.HideUI(true);
		// UIKeyInput.SaveAndSetHoveredObject(this._resume.gameObject);
		// this._panel.gameObject.SetActive(true);
		// AnalogTV analogTV = R.Ui.CameraGO.AddComponent<AnalogTV>();
		// analogTV.Shader = Shader.Find("Hidden/Colorful/Analog TV");
		// analogTV.NoiseIntensity = 1f;
		// analogTV.ScanlinesIntensity = 0f;
		// analogTV.ScanlinesCount = 696;
		// analogTV.Distortion = 0.18f;
		// analogTV.CubicDistortion = 0f;
		// analogTV.Scale = 1.02f;
	}

	// public YieldInstruction PauseThenOpenWithAnim()
	// {
	// 	this.Open();
	// 	this.Enabled = false;
	// 	//R.PauseGame();
	// 	AnalogTV analogTV = R.Ui.CameraGO.GetComponent<AnalogTV>();
	// 	analogTV.Scale = 0f;
	// 	return DOTween.To(() => analogTV.Scale, delegate(float scale)
	// 	{
	// 		analogTV.Scale = scale;
	// 	}, 1.02f, 0.5f).OnComplete(delegate
	// 	{
	// 		this.Enabled = true;
	// 	}).SetUpdate(true).WaitForCompletion();
	// }

	private void Close()
	{
		// UnityEngine.Object.Destroy(R.Ui.CameraGO.GetComponent<AnalogTV>());
		// this._panel.gameObject.SetActive(false);
		// UIKeyInput.LoadHoveredObject();
		// R.Mode.ExitMode(Mode.AllMode.UI);
		// R.Ui.ShowUI(true);
		// R.Audio.UnPauseBGM();
	}

	// public YieldInstruction CloseWithAnimThenResume()
	// {
	// 	// this.Enabled = false;
	// 	// AnalogTV analogTV = R.Ui.CameraGO.GetComponent<AnalogTV>();
	// 	// return DOTween.To(() => analogTV.Scale, delegate(float scale)
	// 	// {
	// 	// 	analogTV.Scale = scale;
	// 	// }, 0f, 0.5f).SetUpdate(true).OnComplete(delegate
	// 	// {
	// 	// 	this.Close();
	// 	// 	//R.ResumeGame();
	// 	// 	this.Enabled = true;
	// 	// }).WaitForCompletion();
	// }

	private void OnResumeClick()
	{
		if (!this.Enabled)
		{
			return;
		}
		//UIKeyInput.OnPauseClick();
	}

	private void OnExitClick()
	{
		if (!this.Enabled)
		{
			return;
		}
		//UIKeyInput.OnPauseClick();
		R.Ui.Reset();
		R.Player.Action.ChangeState(PlayerAction.StateEnum.Idle, 1f);
		R.Audio.StopVoiceOver();
		LevelManager.LoadLevelByGateId("ui_start", SceneGate.OpenType.None);
	}

	private void OnLevelSelect()
	{
		if (!this.Enabled)
		{
			return;
		}
		base.StartCoroutine(this.LevelSelectCoroutine());
	}

	private IEnumerator LevelSelectCoroutine()
	{
		// R.Audio.StopVoiceOver();
		// yield return UIKeyInput.OnPauseClick();
		// R.Ui.LevelSelect.OpenWithAnim(true, true);
		yield break;
	}

	// [SerializeField]
	// private UIButton _resume;
	//
	// [SerializeField]
	// private UIButton _exit;
	//
	// [SerializeField]
	// private UIButton _levelSelect;
	//
	// [SerializeField]
	// private UIPanel _panel;
}
