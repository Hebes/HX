// using System;
// using UnityEngine;
//
// /// <summary>
// /// UI按钮导航
// /// </summary>
// [RequireComponent(typeof(UIButton))]
// public class UIButtonNavigation : MonoBehaviour
// {
// 	public UIButton button
// 	{
// 		get
// 		{
// 			UIButton result;
// 			if ((result = _button) == null)
// 			{
// 				result = (_button = GetComponent<UIButton>());
// 			}
// 			return result;
// 		}
// 	}
//
// 	public static UIButtonNavigation current
// 	{
// 		get
// 		{
// 			GameObject hoveredObject = UICamera.hoveredObject;
// 			if (hoveredObject == null)
// 			{
// 				return null;
// 			}
// 			return hoveredObject.GetComponent<UIButtonNavigation>();
// 		}
// 	}
//
// 	public bool isColliderEnabled
// 	{
// 		get
// 		{
// 			if (!enabled || !gameObject.activeInHierarchy)
// 			{
// 				return false;
// 			}
// 			Collider component = GetComponent<Collider>();
// 			if (component != null)
// 			{
// 				return component.enabled;
// 			}
// 			Collider2D component2 = GetComponent<Collider2D>();
// 			return component2 != null && component2.enabled;
// 		}
// 	}
//
// 	protected virtual void OnEnable()
// 	{
// 		list.Add(this);
// 		if (mStarted)
// 		{
// 			Start();
// 		}
// 	}
//
// 	private void Start()
// 	{
// 		mStarted = true;
// 		if (startsSelected && isColliderEnabled)
// 		{
// 			UICamera.hoveredObject = gameObject;
// 		}
// 	}
//
// 	protected virtual void OnDisable()
// 	{
// 		list.Remove(this);
// 	}
//
// 	private static bool IsActive(GameObject go)
// 	{
// 		if (!go || !go.activeInHierarchy)
// 		{
// 			return false;
// 		}
// 		Collider component = go.GetComponent<Collider>();
// 		if (component != null)
// 		{
// 			return component.enabled;
// 		}
// 		Collider2D component2 = go.GetComponent<Collider2D>();
// 		return component2 != null && component2.enabled;
// 	}
//
// 	public GameObject GetLeft()
// 	{
// 		if (IsActive(onLeft))
// 		{
// 			return onLeft;
// 		}
// 		if (constraint == Constraint.Vertical || constraint == Constraint.Explicit)
// 		{
// 			return null;
// 		}
// 		return Get(Vector3.left, 1f, 2f);
// 	}
//
// 	public GameObject GetRight()
// 	{
// 		if (IsActive(onRight))
// 		{
// 			return onRight;
// 		}
// 		if (constraint == Constraint.Vertical || constraint == Constraint.Explicit)
// 		{
// 			return null;
// 		}
// 		return Get(Vector3.right, 1f, 2f);
// 	}
//
// 	public GameObject GetUp()
// 	{
// 		if (IsActive(onUp))
// 		{
// 			return onUp;
// 		}
// 		if (constraint == Constraint.Horizontal || constraint == Constraint.Explicit)
// 		{
// 			return null;
// 		}
// 		return Get(Vector3.up, 2f);
// 	}
//
// 	public GameObject GetDown()
// 	{
// 		if (IsActive(onDown))
// 		{
// 			return onDown;
// 		}
// 		if (constraint == Constraint.Horizontal || constraint == Constraint.Explicit)
// 		{
// 			return null;
// 		}
// 		return Get(Vector3.down, 2f);
// 	}
//
// 	public GameObject Get(Vector3 myDir, float x = 1f, float y = 1f)
// 	{
// 		Transform transform = this.transform;
// 		myDir = transform.TransformDirection(myDir);
// 		Vector3 center = GetCenter(gameObject);
// 		float num = float.MaxValue;
// 		GameObject result = null;
// 		for (int i = 0; i < list.size; i++)
// 		{
// 			UIButtonNavigation uibuttonNavigation = list[i];
// 			if (!(uibuttonNavigation == this) && uibuttonNavigation.constraint != Constraint.Explicit && uibuttonNavigation.isColliderEnabled)
// 			{
// 				UIWidget component = uibuttonNavigation.GetComponent<UIWidget>();
// 				if (!(component != null) || component.alpha != 0f)
// 				{
// 					Vector3 direction = GetCenter(uibuttonNavigation.gameObject) - center;
// 					float num2 = Vector3.Dot(myDir, direction.normalized);
// 					if (num2 >= 0.707f)
// 					{
// 						direction = transform.InverseTransformDirection(direction);
// 						direction.x *= x;
// 						direction.y *= y;
// 						float sqrMagnitude = direction.sqrMagnitude;
// 						if (sqrMagnitude <= num)
// 						{
// 							result = uibuttonNavigation.gameObject;
// 							num = sqrMagnitude;
// 						}
// 					}
// 				}
// 			}
// 		}
// 		return result;
// 	}
//
// 	protected static Vector3 GetCenter(GameObject go)
// 	{
// 		UIWidget component = go.GetComponent<UIWidget>();
// 		UICamera uicamera = UICamera.FindCameraForLayer(go.layer);
// 		if (uicamera != null)
// 		{
// 			Vector3 vector = go.transform.position;
// 			if (component != null)
// 			{
// 				Vector3[] worldCorners = component.worldCorners;
// 				vector = (worldCorners[0] + worldCorners[2]) * 0.5f;
// 			}
// 			vector = uicamera.cachedCamera.WorldToScreenPoint(vector);
// 			vector.z = 0f;
// 			return vector;
// 		}
// 		if (component != null)
// 		{
// 			Vector3[] worldCorners2 = component.worldCorners;
// 			return (worldCorners2[0] + worldCorners2[2]) * 0.5f;
// 		}
// 		return go.transform.position;
// 	}
//
// 	public virtual void OnNavigate()
// 	{
// 		if (UIPopupList.isOpen)
// 		{
// 			return;
// 		}
// 		GameObject gameObject = null;
// 		if (Core.Input.UI.Left.OnPressedForSeveralSeconds(0.333333343f, true))
// 		{
// 			gameObject = GetLeft();
// 		}
// 		else if (Core.Input.UI.Right.OnPressedForSeveralSeconds(0.333333343f, true))
// 		{
// 			gameObject = GetRight();
// 		}
// 		else if (Core.Input.UI.Up.OnPressedForSeveralSeconds(0.333333343f, true))
// 		{
// 			gameObject = GetUp();
// 		}
// 		else if (Core.Input.UI.Down.OnPressedForSeveralSeconds(0.333333343f, true))
// 		{
// 			gameObject = GetDown();
// 		}
// 		if (gameObject != null)
// 		{
// 			R.Audio.PlayEffect(3);
// 		}
// 		if (Core.Input.UI.Confirm.OnClick)
// 		{
// 			gameObject = onClick;
// 			if (gameObject != null)
// 			{
// 				UIKeyInput.SaveHoveredObject();
// 			}
// 			button.SendMessage("OnClick");
// 			UIPlayAnimation[] components = GetComponents<UIPlayAnimation>();
// 			if (components != null)
// 			{
// 				for (int i = 0; i < components.Length; i++)
// 				{
// 					if (components[i].enabled && components[i].trigger == Trigger.OnClick)
// 					{
// 						Direction playDirection = components[i].playDirection;
// 						if (playDirection != Direction.Forward)
// 						{
// 							if (playDirection != Direction.Reverse)
// 							{
// 								if (playDirection == Direction.Toggle)
// 								{
// 									components[i].Play(true);
// 								}
// 							}
// 							else
// 							{
// 								components[i].Play(true, false);
// 							}
// 						}
// 						else
// 						{
// 							components[i].Play(true, false);
// 						}
// 					}
// 				}
// 				R.Audio.PlayEffect(139);
// 			}
// 		}
// 		if (gameObject != null)
// 		{
// 			button.state = UIButtonColor.State.Normal;
// 			UIKeyInput.HoveredObject = gameObject;
// 		}
// 	}
//
// 	public virtual void OnKey(KeyCode key)
// 	{
// 	}
//
// 	public static BetterList<UIButtonNavigation> list = new BetterList<UIButtonNavigation>();
//
// 	private UIButton _button;
//
// 	public Constraint constraint;
//
// 	public GameObject onUp;
//
// 	public GameObject onDown;
//
// 	public GameObject onLeft;
//
// 	public GameObject onRight;
//
// 	public GameObject onClick;
//
// 	public GameObject onTab;
//
// 	public bool startsSelected;
//
// 	[NonSerialized]
// 	private bool mStarted;
//
// 	public enum Constraint
// 	{
// 		None,
// 		Vertical,
// 		Horizontal,
// 		Explicit
// 	}
// }
