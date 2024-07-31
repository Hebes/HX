using DG.Tweening;
using UnityEngine;

public class UIFlashController : MonoBehaviour
{
	private void Start()
	{
		this._currentFlashLevel = R.Player.Attribute.flashLevel;
		// if (R.Player.Attribute.flashLevel > 1)
		// {
		// 	this._flashItems[3].gameObject.SetActive(true);
		// 	this._flashItems[4].gameObject.SetActive(true);
		// }
		// else
		// {
		// 	this._flashItems[3].gameObject.SetActive(false);
		// 	this._flashItems[4].gameObject.SetActive(false);
		// }
	}

	private void Update()
	{
		if (this._isShown)
		{
			float? lastTimeFilled = this._lastTimeFilled;
			if (lastTimeFilled != null)
			{
				float? lastTimeFilled2 = this._lastTimeFilled;
				if (((lastTimeFilled2 == null) ? null : new float?(Time.time - lastTimeFilled2.GetValueOrDefault())) > 5f)
				{
					this._lastTimeFilled = null;
					this.Disappear();
				}
			}
		}
		if (this._currentFlashLevel != R.Player.Attribute.flashLevel)
		{
			// if (R.Player.Attribute.flashLevel > 1)
			// {
			// 	this._flashItems[3].gameObject.SetActive(true);
			// 	this._flashItems[4].gameObject.SetActive(true);
			// }
			// else
			// {
			// 	this._flashItems[3].gameObject.SetActive(false);
			// 	this._flashItems[4].gameObject.SetActive(false);
			// }
			// this._grid.Reposition();
			// this._currentFlashLevel = R.Player.Attribute.flashLevel;
		}
	}

	public void OnFlash(int id)
	{
		// this.Appear();
		// this._flashItems[id].Disapper();
	}

	public void OnRecover(int id, bool isFilled)
	{
		// this._flashItems[id].Appear();
		// if (isFilled)
		// {
		// 	this._lastTimeFilled = new float?(Time.time);
		// }
		// else
		// {
		// 	this._lastTimeFilled = null;
		// }
	}

	public void RecoverAll(int count)
	{
		// for (int i = 0; i < count; i++)
		// {
		// 	this._flashItems[i].Appear();
		// }
	}

	private void Appear()
	{
		// if (this._appearTweener != null && this._appearTweener.IsPlaying())
		// {
		// 	return;
		// }
		// if (this._disappearTweener != null && this._disappearTweener.IsPlaying())
		// {
		// 	this._disappearTweener.Kill(false);
		// 	this._isShown = false;
		// }
		// if (this._isShown)
		// {
		// 	return;
		// }
		// this._appearTweener = this._widget.DOFade(1f, 0.5f).OnComplete(delegate
		// {
		// 	this._isShown = true;
		// });
	}

	private void Disappear()
	{
		// this._disappearTweener = this._widget.DOFade(0f, 0.5f).OnComplete(delegate
		// {
		// 	this._isShown = false;
		// });
	}

	// [SerializeField]
	// private UIWidget _widget;
	//
	// [SerializeField]
	// private UIGrid _grid;
	//
	// [SerializeField]
	// private UIFlashItem[] _flashItems;

	private const int AutoDisappearDelay = 5;

	private float? _lastTimeFilled;

	private bool _isShown;

	private int _currentFlashLevel;

	private Tweener _appearTweener;

	private Tweener _disappearTweener;
}
