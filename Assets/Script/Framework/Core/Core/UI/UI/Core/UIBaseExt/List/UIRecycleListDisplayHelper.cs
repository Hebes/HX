using System;
using System.Collections;
using UnityEngine;

namespace Game.UI
{
    [DisallowMultipleComponent]
	[RequireComponent(typeof(UIRecycleList))]
	public class UIRecycleListDisplayHelper : MonoBehaviour
	{
		public float displayInterval = 0.25f;

		private UIRecycleList uiList;

		private readonly IList cachedDataList = new ArrayList();
		private readonly IList tempDataList = new ArrayList();
		private Action<int> cachedHandlerPreAdd;

		public bool IsDisplaying { get; private set; }
		private int currentDisplayIndex = 0;

		private Coroutine displayCoroutine;

		private void Awake()
		{
			uiList = GetComponent<UIRecycleList>();
		}

		public void StartDisplayOneByOne(IList dataList, Action<int> handlerPreAdd)
		{
			Stop();

			for (var i = 0; i < dataList.Count; i++)
			{
				cachedDataList.Add(dataList[i]);
			}
			cachedHandlerPreAdd = handlerPreAdd;

			IsDisplaying = true;
			
			displayCoroutine = StartCoroutine(DisplayOneByOne());
		}
		
		public void Stop()
		{
			if (!IsDisplaying) return;

			if (displayCoroutine != null)
			{
				StopCoroutine(displayCoroutine);
				displayCoroutine = null;
			}
			
			uiList.Clear();
			uiList.SetContent(cachedDataList);
			uiList.MoveToIndex(cachedDataList.Count - 1);

			while (currentDisplayIndex < cachedDataList.Count)
			{
				if (cachedHandlerPreAdd != null)
				{
					cachedHandlerPreAdd(currentDisplayIndex);
				}

				currentDisplayIndex++;
			}

			IsDisplaying = false;
		}
		
		public void Clear()
		{
			cachedDataList.Clear();
			tempDataList.Clear();
			currentDisplayIndex = 0;

			if (uiList != null)
			{
				uiList.Clear();
				uiList.ResetContentPosition();
			}
		}

		public int GetCellCount()
		{
			return cachedDataList.Count;
		}

		private IEnumerator DisplayOneByOne()
		{
			while (currentDisplayIndex < cachedDataList.Count)
			{
				yield return new WaitForSeconds(displayInterval);
				
				tempDataList.Add(cachedDataList[currentDisplayIndex]);

				uiList.Clear();
				uiList.SetContent(tempDataList);
				uiList.MoveToIndex(tempDataList.Count - 1);

				if (cachedHandlerPreAdd != null)
				{
					cachedHandlerPreAdd(currentDisplayIndex);
				}

				currentDisplayIndex++;
			}

			IsDisplaying = false;
		}
	}
}