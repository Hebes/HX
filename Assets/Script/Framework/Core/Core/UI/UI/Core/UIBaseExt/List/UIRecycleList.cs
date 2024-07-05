using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// <summary>
// @Author: zrh
// @Date: 2023,02,07,9:54
// @Description:
// </summary>

namespace Game.UI
{
    /// <summary>
    /// 无限循环List
    /// 作者：EdisonLee
    /// Ref: http://www.cnblogs.com/fly-100/p/4549354.html?utm_source=tuicool&utm_medium=referral
    /// </summary>
    public class UIRecycleList : MonoBehaviour
    {
	    public enum Direction
        {
            Horizontal,
            Vertical
        }

        private RectTransform m_Cell;
        [SerializeField]
        private Vector2 m_Page;
        [SerializeField]
        private Direction direction = Direction.Horizontal;
        [SerializeField, Range(4, 10), Tooltip("缓冲区大小")]
        private int m_BufferNo;
        private List<RectTransform> m_InstantiateItems = new List<RectTransform>();
        private IList m_Datas;

	    public Vector2 border = Vector2.zero;
	    public Vector2 space = Vector2.zero;

	    public int directionSign = 1;
		
	    
        public Vector2 CellRect
        {
            get { return m_Cell != null ? m_Cell.sizeDelta : new Vector2(100, 100); }
        }

        public float CellScale
        {
            get { return direction == Direction.Horizontal ? CellRect.x + space.x : CellRect.y + space.y; }
        }

        private float m_PrevPos = 0;

        public float DirectionPos
        {
            get { return direction == Direction.Horizontal ? m_Rect.anchoredPosition.x : m_Rect.anchoredPosition.y; }
        }

		/// <summary>
		/// InstantiateItems的第一行（列）在整个datas中的位置
		/// </summary>
		private int m_CurrentIndex;

		/// <summary>
		/// InstantiateItems的第一行（列）在整个datas中的位置
		/// </summary>
		public int CurrentIndex
	    {
		    get { return m_CurrentIndex; }
	    }

        private Vector2 m_InstantiateSize = Vector2.zero;

        public Vector2 InstantiateSize
        {
            get
            {
                if (m_InstantiateSize == Vector2.zero)
                {
                    float rows, cols;
                    if (direction == Direction.Horizontal)
                    {
                        rows = m_Page.x;
                        cols = m_Page.y + (float)m_BufferNo;
                    }
                    else
                    {
                        rows = m_Page.x + (float)m_BufferNo;
                        cols = m_Page.y;
                    }
                    m_InstantiateSize = new Vector2(rows, cols);
                }
                return m_InstantiateSize;
            }
        }

        public int PageCount
        {
            get { return (int)m_Page.x * (int)m_Page.y; }
        }

        public int PageScale
        {
            get { return direction == Direction.Horizontal ? (int)m_Page.x : (int)m_Page.y; }
        }

	    public int PageNumber
	    {
		    get { return direction == Direction.Horizontal ? (int) m_Page.y + m_BufferNo : (int) m_Page.x + m_BufferNo; }
	    }

        private ScrollRect m_ScrollRect;
        private RectTransform m_Rect;
        private RectTransform root;

	    public Vector3 GetRootPosition()
	    {
		    return root.localPosition;
	    }

        public int EstimatedInstantiateCount
        {
            get { return (int)InstantiateSize.x * (int)InstantiateSize.y; }
        }

	    public List<RectTransform> InstantiateItems
	    {
		    get { return m_InstantiateItems; }
	    }

		public int DataCount
	    {
		    get { return m_Datas != null ? m_Datas.Count : 0; }
	    }

	    private Vector2 m_Rect_originalAnchorPosition;

		private List<RectTransform> cachedItems = new List<RectTransform>();

	    private bool isSetContenting = false;

	    /// <summary>
	    /// 设置list
	    /// </summary>
	    /// <param name="page">页面</param>
	    /// <param name="direction">排序规则</param>
	    /// <param name="bufferNo">缓冲区大小</param>
	    /// <param name="border">边界</param>
	    /// <param name="space">空间</param>
	    /// <param name="directionSign">方向标志</param>
	    public void Setting(Vector2 page, Direction direction, int bufferNo, Vector2 border, Vector2 space, int directionSign)
	    {
		    this.m_Page = page;
		    this.direction = direction;
		    this.m_BufferNo = bufferNo;
		    this.border = border;
		    this.space = space;
		    this.directionSign = directionSign;
	    }
        protected void Awake()
        {
            m_ScrollRect = GetComponentInParent<ScrollRect>();
            m_ScrollRect.horizontal = direction == Direction.Horizontal;
            m_ScrollRect.vertical = direction == Direction.Vertical;

            m_Cell = transform.GetChildComponent<RectTransform>("Cell");
            root = transform.GetChildComponent<RectTransform>("Mask/Root");
            m_Rect = root.GetComponent<RectTransform>();
            m_Cell.gameObject.SetActive(false);

	        m_Rect_originalAnchorPosition = m_Rect.anchoredPosition;
        }

        /// <summary>
        /// SetContent
        /// </summary>
        /// <param name="data"></param>
        public void SetContent(IList data)
        {
			// 先Clear
	        Clear();

	        isSetContenting = false;

            m_Datas = data;
            SetBound(GetRectByNum(m_Datas.Count));
            if (m_Datas.Count > EstimatedInstantiateCount)
            {
                while (m_InstantiateItems.Count < EstimatedInstantiateCount)
                {
                    AddItem(m_InstantiateItems.Count);
                }
            }
            else
            {
                while (m_InstantiateItems.Count > m_Datas.Count)
                {
                    RemoveItem(m_InstantiateItems.Count - 1);
                }
                while (m_InstantiateItems.Count < m_Datas.Count)
                {
                    AddItem(m_InstantiateItems.Count);
                }
            }
        }

        /// <summary>
        /// 每帧延迟0.1s执行新的创建
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IEnumerator YieldSetContent(IList data)
        {
			// 先Clear
	        Clear();

	        isSetContenting = true;

            m_Datas = data;
            SetBound(GetRectByNum(m_Datas.Count));
            if (m_Datas.Count > EstimatedInstantiateCount)
            {
                while (m_InstantiateItems.Count < EstimatedInstantiateCount)
                {
                    yield return new WaitForSeconds(0.1f);

                    AddItem(m_InstantiateItems.Count);
                }
            }
            else
            {
                while (m_InstantiateItems.Count > m_Datas.Count)
                {
                    RemoveItem(m_InstantiateItems.Count - 1);
                }
                while (m_InstantiateItems.Count < m_Datas.Count)
                {
                    yield return new WaitForSeconds(0.1f);

                    AddItem(m_InstantiateItems.Count);
                }
            }

	        isSetContenting = false;
        }

        [ContextMenu("Clear")]
        public void Clear()
        {
            for (var i = 0; i < m_InstantiateItems.Count; i++)
            {
	            m_InstantiateItems[i].gameObject.SetActiveEfficiently(false);
				cachedItems.Add(m_InstantiateItems[i]);
            }

            m_InstantiateItems.Clear();
            m_CurrentIndex = 0;
            m_PrevPos = 0;

			SetBound(Vector2.zero);
        }
		
	    public void RefreshContent()
	    {
		    for (var i = 0; i < m_InstantiateItems.Count; i++)
		    {
			    UpdateItem(m_CurrentIndex * PageScale + i, m_InstantiateItems[i].gameObject);
		    }
	    }

	    public void ResetContentPosition()
	    {
			if (m_Rect != null)
			{
				m_Rect.anchoredPosition = m_Rect_originalAnchorPosition;
			}
		}

	    public void MoveToIndex(int index)
	    {
		    if (direction == Direction.Horizontal)
		    {
			    var x = Mathf.Max(Mathf.Max(m_ScrollRect.viewport.sizeDelta.x - m_Rect.sizeDelta.x, 0f), -Mathf.Floor(index/InstantiateSize.x)*(CellRect.x + space.x) - border.x);
				m_Rect.anchoredPosition = new Vector2(x * directionSign, m_Rect.anchoredPosition.y);
		    }
		    else
		    {
			    var y = Mathf.Min(Mathf.Max(m_Rect.sizeDelta.y - m_ScrollRect.viewport.sizeDelta.y, 0f), Mathf.Floor(index/InstantiateSize.y)*(CellRect.y + space.y) + border.y);
				m_Rect.anchoredPosition = new Vector2(m_Rect.anchoredPosition.x, y * directionSign);
		    }

			Update();
	    }

        private void AddItem(int index)
        {
	        RectTransform item;
	        if (cachedItems.Count > 0)
	        {
		        item = cachedItems[0];
		        cachedItems.RemoveAt(0);
	        }
	        else
	        {
		        item = Instantiate(m_Cell);
	        }
            item.SetParent(root, false);
            item.anchorMax = directionSign > 0 ? Vector2.up : Vector2.zero;
            item.anchorMin = directionSign > 0 ? Vector2.up : Vector2.zero;
            item.pivot = directionSign > 0 ? Vector2.up : Vector2.zero;
            item.name = "item" + index;
	        item.anchoredPosition = direction == Direction.Horizontal
		        ? new Vector2((Mathf.Floor(index/InstantiateSize.x)*(CellRect.x + space.x) + border.x)*directionSign, -(index%InstantiateSize.x)*(CellRect.y + space.y) - border.y)
		        : new Vector2((index%InstantiateSize.y)*(CellRect.x + space.x) + border.x, (-Mathf.Floor(index/InstantiateSize.y)*(CellRect.y + space.y) - border.y)*directionSign);
            m_InstantiateItems.Add(item);
            item.gameObject.SetActiveEfficiently(true);
			UpdateItem(index, item.gameObject);
        }

        private void RemoveItem(int index)
        {
            var item = m_InstantiateItems[index];
            m_InstantiateItems.Remove(item);
            
			item.gameObject.SetActiveEfficiently(false);
			cachedItems.Add(item);
        }

        /// <summary>
        /// 由格子数量获取多少行多少列
        /// </summary>
        /// <param name="num"></param>格子个数
        /// <returns></returns>
        private Vector2 GetRectByNum(int num)
        {
            return direction == Direction.Horizontal
                ? new Vector2(m_Page.x, Mathf.CeilToInt(num / m_Page.x))
                : new Vector2(Mathf.CeilToInt(num / m_Page.y), m_Page.y);
        }

        /// <summary>
        /// 设置content的大小
        /// </summary>
        /// <param name="bound"></param>
        private void SetBound(Vector2 bound)
        {
	        if (m_Rect != null)
	        {
				m_Rect.sizeDelta = new Vector2(bound.y*(CellRect.x + space.x) - (bound.y > 0 ? space.x : 0) + border.x*2f, bound.x*(CellRect.y + space.y) - (bound.y > 0 ? space.y : 0) + border.y*2f);
	        }
        }

        public float MaxPrevPos
        {
            get
            {
                float result;
                Vector2 max = GetRectByNum(m_Datas.Count);
                if (direction == Direction.Horizontal)
                {
                    result = max.y - m_Page.y;
                }
                else
                {
                    result = max.x - m_Page.x;
                }
	            if (direction == Direction.Horizontal)
	            {
		            return result*CellScale + border.x;
	            }
	            else
	            {
		            return result*CellScale + border.y;
	            }
            }
        }

        public float scale
        {
            get { return direction == Direction.Horizontal ? 1f * directionSign : -1f * directionSign; }
        }

        private void Update()
        {
	        if (isSetContenting)
	        {
		        return;
	        }

	        var upCellCount = 0;
            while (scale * DirectionPos - m_PrevPos < -CellScale * 2)
            {
	            if (m_PrevPos <= -MaxPrevPos)
	            {
					// Debug.LogError(string.Format("UIRecycleList|Up|m_PrevPos <= -MaxPrevPos|m_PrevPos|{0}", m_PrevPos));
		            break;
	            }
				m_PrevPos -= CellScale;
	            upCellCount++;
				m_CurrentIndex++;
            }
	        if (upCellCount > 0)
	        {
		        var count = Mathf.Min(PageScale*upCellCount, m_InstantiateItems.Count);
				var range = m_InstantiateItems.GetRange(0, count);
				m_InstantiateItems.RemoveRange(0, count);
				m_InstantiateItems.AddRange(range);
		        for (var i = 0; i < range.Count; i++)
		        {
			        MoveItemToIndex((m_CurrentIndex - Mathf.Min(upCellCount, PageNumber))*PageScale + m_InstantiateItems.Count + i, range[i]);
		        }
	        }

	        var downCellCount = 0;
            while (scale * DirectionPos - m_PrevPos > -CellScale)
            {
	            if (Mathf.RoundToInt(m_PrevPos) >= 0)
	            {
					// Debug.LogError(string.Format("UIRecycleList|Down|Mathf.RoundToInt(m_PrevPos) >= 0|m_PrevPos|{0}", m_PrevPos));
		            break;
	            }
                m_PrevPos += CellScale;
                if (m_CurrentIndex < 1)
	            {
					// Debug.LogError(string.Format("UIRecycleList|Down|m_CurrentIndex < 0|m_CurrentIndex|{0}", m_CurrentIndex));
		            break;
	            }
				m_CurrentIndex--;
	            downCellCount++;
            }
	        if (downCellCount > 0)
	        {
		        var count = Mathf.Min(PageScale*downCellCount, m_InstantiateItems.Count);
				var range = m_InstantiateItems.GetRange(m_InstantiateItems.Count - count, count);
				m_InstantiateItems.RemoveRange(m_InstantiateItems.Count - count, count);
				m_InstantiateItems.InsertRange(0, range);
				for (var i = 0; i < range.Count; i++)
				{
					MoveItemToIndex(m_CurrentIndex * PageScale + i, range[i]);
				}
			}
			// Debug.LogError(string.Format("UIRecycleList|upCellCount|{0}|downCellCount|{1}|m_CurrentIndex|{2}|m_PrevPos|{3}", upCellCount, downCellCount, m_CurrentIndex, m_PrevPos));
		}

        private void MoveItemToIndex(int index, RectTransform item)
        {
            item.anchoredPosition = GetPosByIndex(index);
            UpdateItem(index, item.gameObject);
        }

        private Vector2 GetPosByIndex(int index)
        {
            float x, y;
            if (direction == Direction.Horizontal)
            {
                x = index % m_Page.x;
                y = Mathf.FloorToInt(index / m_Page.x);
            }
            else
            {
                x = Mathf.FloorToInt(index / m_Page.y);
                y = index % m_Page.y;
            }
	        return direction == Direction.Horizontal
		        ? new Vector2((y*(CellRect.x + space.x) + border.x)*directionSign, -x*(CellRect.y + space.y) - border.y)
		        : new Vector2(y*(CellRect.x + space.x) + border.x, (-x*(CellRect.y + space.y) - border.y)*directionSign);
        }

        private void UpdateItem(int index, GameObject item)
        {
            if(index < m_Datas.Count)
            {
                for (int i = 0; i < item.transform.childCount; i++)
                {
                    item.transform.GetChild(i).gameObject.SetActiveEfficiently(true);
                }
                var lit = item.GetComponent<UICell>();
                lit.SetContent(m_Datas[index]);
            }
            else
            {
                for (int i = 0; i < item.transform.childCount; i++)
                {
                    item.transform.GetChild(i).gameObject.SetActiveEfficiently(false);
                }
            }
        }
    }
}