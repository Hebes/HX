using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// <summary>
// @Author: zrh
// @Date: 2023,02,09,10:33
// @Description:
// </summary>

namespace Game.UI
{
    [RequireComponent(typeof(UIRecycleList))]
	[RequireComponent(typeof(ScrollRect))]
	public class UIListCircularForRecycleList : MonoBehaviour, IBeginDragHandler, IEndDragHandler
	{
		public RectTransform viewport;

		public RectTransform content;

		public UIRecycleList elements;

		public RectTransform center;

		public float radius;

		public float coefficient = -1f;

		public float snapSpeed = 1f;

		public int showCount = 5;

		public float showThreshold = 20f;

		public Button upButton;

		public Button downButton;

		public Zrh_Scrollbar scrollbar;

		private bool isDraging = false;

		private bool isSliding = false;

		private float slideTarget;

		private float originalPosition;

		private float slidingPosition = 0f;

		private int elementCount = 0;

		private void Awake()
		{
			upButton.onClick.AddListener(OnUpButtonClicked);
			var upLongPressButton = upButton.GetComponent<LongPressButton>();
			if (upLongPressButton != null)
			{
				upLongPressButton.onClick.AddListener(OnUpButtonClicked);
			}

			downButton.onClick.AddListener(OnDownButtonClicked);
			var downLongPressButton = downButton.GetComponent<LongPressButton>();
			if (downLongPressButton != null)
			{
				downLongPressButton.onClick.AddListener(OnDownButtonClicked);
			}
		}

		private void Update()
		{
			DoUpdate(Time.deltaTime);
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			isDraging = true;

			isSliding = false;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			isDraging = false;
		}

		public void DoUpdate(float deltaTime)
		{
			if (elements.DataCount != elementCount)
			{
				elementCount = elements.DataCount;

				upButton.gameObject.SetActive(elementCount > showCount);
				downButton.gameObject.SetActive(elementCount > showCount);
				scrollbar.gameObject.SetActive(elementCount > showCount);
			}

			SnapToCenter(deltaTime);

			Slide(deltaTime);

			for (var i = 0; i < elements.InstantiateItems.Count; i++)
			{
				UpdateElementPosition(i);
			}
		}

		private void OnUpButtonClicked()
		{
			if (isDraging || isSliding || scrollbar.IsDraging || elements.DataCount < showCount)
			{
				return;
			}

			var minData = FindMinIndex();

			if (minData.minIndex > -1 && minData.absoluteMinDistance < 0.1f)
			{
				if (elements.CurrentIndex > 0 || minData.minIndex > showCount / 2)
				{
					isSliding = true;

					originalPosition = content.localPosition.y;
					slidingPosition = 0f;

					slideTarget = -(elements.CellRect.y);
				}
			}
		}

		private void OnDownButtonClicked()
		{
			if (isDraging || isSliding || scrollbar.IsDraging || elements.DataCount < showCount)
			{
				return;
			}

			var minData = FindMinIndex();

			if (minData.minIndex > -1 && minData.absoluteMinDistance < 0.1f)
			{
				if (elements.CurrentIndex + elements.InstantiateItems.Count < elements.DataCount || elements.InstantiateItems.Count - minData.minIndex - 1 > showCount / 2)
				{
					isSliding = true;

					originalPosition = content.localPosition.y;
					slidingPosition = 0f;

					slideTarget = elements.CellRect.y;
				}
			}
		}

		private void UpdateElementPosition(int index)
		{
			var element = elements.InstantiateItems[index];

			// 读Cell节点下第一个子节点，因为Cell节点锚点和Pivot都要靠上
			var elementTransform = element.transform.GetChild(0);

			var elementLocalPosition = viewport.InverseTransformPoint(elementTransform.position);

			var deltaY = elementLocalPosition.y - center.localPosition.y;

			var deltaY2 = deltaY * deltaY;
			var radius2 = radius * radius;
			var xLength = radius2 > deltaY2 ? Mathf.Sqrt(radius2 - deltaY2) : 0f;

			elementLocalPosition.x = center.localPosition.x + xLength * coefficient;

			elementTransform.position = viewport.TransformPoint(elementLocalPosition);
		}

		private void SnapToCenter(float deltaTime)
		{
			if (elements.DataCount <= showCount)
			{
				return;
			}

			var minData = FindMinIndex();
			
			if (!isDraging && !isSliding && !scrollbar.IsDraging)
			{
				var delta = Mathf.Lerp(0f, minData.minDistance, deltaTime * snapSpeed);
				if (Mathf.Abs(delta) > 0.001f)
				{
					content.localPosition = new Vector3(content.localPosition.x, content.localPosition.y - delta, content.localPosition.z);
				}
			}
		}

		private void Slide(float deltaTime)
		{
			if (isSliding)
			{
				slidingPosition = Mathf.Lerp(slidingPosition, slideTarget, deltaTime * snapSpeed);

				if (Mathf.Abs(slidingPosition - slideTarget) < 0.1f)
				{
					isSliding = false;

					return;
				}

				content.localPosition = new Vector3(content.localPosition.x, originalPosition + slidingPosition, content.localPosition.z);
			}
		}

		private MinData FindMinIndex()
		{
			var absoluteMinDistance = float.MaxValue;
			var minIndex = -1;
			var minDistance = 0f;

			for (var i = 0; i < elements.InstantiateItems.Count; i++)
			{
				var element = elements.InstantiateItems[i];

				// 读Cell节点下第一个子节点，因为Cell节点锚点和Pivot都要靠上
				var elementTransform = element.transform.GetChild(0);

				var elementLocalPosition = viewport.InverseTransformPoint(elementTransform.position);

				var deltaY = elementLocalPosition.y - center.localPosition.y;

				var absoluteDeltaY = Mathf.Abs(deltaY);
				if (absoluteDeltaY < absoluteMinDistance)
				{
					absoluteMinDistance = absoluteDeltaY;
					minDistance = deltaY;
					minIndex = i;
				}
			}

			return new MinData
			{
				minIndex = minIndex,
				absoluteMinDistance = absoluteMinDistance,
				minDistance = minDistance,
			};
		}

		private struct MinData
		{
			public int minIndex;

			public float absoluteMinDistance;

			public float minDistance;
		}
	}
}