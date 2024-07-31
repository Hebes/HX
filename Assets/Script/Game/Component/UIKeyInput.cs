// using System;
// using System.Collections.Generic;
// using Core;
// using UnityEngine;
//
// /// <summary>
// /// UI按键输入
// /// </summary>
// public class UIKeyInput : MonoBehaviour
// {
// 	/// <summary>
// 	/// 在对象
// 	/// </summary>
// 	public static GameObject HoveredObject
// 	{
// 		get
// 		{
// 			return  UICamera.hoveredObject;
// 		}
// 		set
// 		{
// 			UICamera.hoveredObject = value;
// 			UIKeyInput._hoveredButtonNavigation = ((!(value != null)) ? null : value.GetComponent<UIButtonNavigation>());
// 		}
// 	}
//
// 	public static void SaveHoveredObject()
// 	{
// 		UIKeyInput.LastHoveredObjects.Push(UIKeyInput.HoveredObject);
// 	}
//
// 	public static void SaveAndSetHoveredObject(GameObject gameObject)
// 	{
// 		UIKeyInput.SaveHoveredObject();
// 		UIKeyInput.HoveredObject = gameObject;
// 	}
//
// 	/// <summary>
// 	/// 加载悬停对象
// 	/// </summary>
// 	public static void LoadHoveredObject()
// 	{
// 		UIKeyInput.HoveredObject = ((UIKeyInput.LastHoveredObjects.Count == 0) ? null : UIKeyInput.LastHoveredObjects.Pop());
// 	}
//
// 	private void Update()
// 	{
// 		this.UpdateInput();
// 		this.UpdateButtonNavigation();
// 	}
//
// 	private void UpdateButtonNavigation()
// 	{
// 		UIButtonNavigation uibuttonNavigation = (!(UIKeyInput.HoveredObject != null)) ? null : UIKeyInput._hoveredButtonNavigation;
// 		if (uibuttonNavigation != null)
// 		{
// 			if (uibuttonNavigation.button.state != UIButtonColor.State.Pressed)
// 			{
// 				uibuttonNavigation.button.state = UIButtonColor.State.Pressed;
// 			}
// 			uibuttonNavigation.OnNavigate();
// 			if (R.Mode.IsInUIMode() && UnityEngine.Input.GetKeyDown(KeyCode.Tab))
// 			{
// 				uibuttonNavigation.OnKey(KeyCode.Tab);
// 			}
// 		}
// 	}
//
// 	private void UpdateInput()
// 	{
// 		if (Input.UI.Pause.OnClick || Input.Shi.Pause.OnClick)
// 		{
// 			UIKeyInput.OnPauseClick();
// 		}
// 		if (Input.UI.Debug.OnClick && UnityEngine.Debug.isDebugBuild)
// 		{
// 			this.OnDebugClick();
// 		}
// 	}
//
// 	public static YieldInstruction OnPauseClick()
// 	{
// 		UIKeyInput._pauseButtonClickCount++;
// 		if (UIKeyInput._pauseButtonClickCount % 2 != 0)
// 		{
// 			return R.Ui.Pause.PauseThenOpenWithAnim();
// 		}
// 		return R.Ui.Pause.CloseWithAnimThenResume();
// 	}
//
// 	/// <summary>
// 	/// 点击调试
// 	/// </summary>
// 	public void OnDebugClick()
// 	{
// 		this._debugButtonClickCount++;
// 		if (this._debugButtonClickCount % 2 != 0)
// 		{
// 			SingletonMono<UIDebug>.Instance.Open();
// 		}
// 		else
// 		{
// 			SingletonMono<UIDebug>.Instance.Close();
// 		}
// 	}
//
// 	private void OnApplicationPause(bool pause)
// 	{
// 		if (!pause && R.Ui.Pause.Enabled && !WorldTime.IsPausing)
// 		{
// 			UIKeyInput.OnPauseClick();
// 		}
// 	}
//
// 	private static readonly Stack<GameObject> LastHoveredObjects = new Stack<GameObject>();
//
// 	private static UIButtonNavigation _hoveredButtonNavigation;
//
// 	private static int _pauseButtonClickCount;
//
// 	private int _debugButtonClickCount;
// }
