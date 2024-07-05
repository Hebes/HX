using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;
using zhaorh.UI;

// <summary>
// @Author: zrh
// @Date: 2022,12,22,17:31
// @Description:
// </summary>

namespace Game.UI
{
    public abstract class UIListBaseByAutoLauout : MonoBehaviour
    {
        #region Field
        // 是否启用格子缓存
        [HideInInspector]
        public bool cache = true;
        // 格子之间的间距
        //public float space;

        // 格子
        protected readonly List<UICell> cells = new List<UICell>();
        // 缓存的格子
        protected readonly List<UICell> cellCaches = new List<UICell>();
        // cell template
        protected UICell cellTemplate;
        // root rect
        protected RectTransform root;
        // mask
        protected RectTransform mask;

        public ScrollRect scrollRect { get; private set; }

        public string cellPrefabName;
        #endregion

        // 单元格
        public List<UICell> Cells
        {
            get { return cells; }
        }

        /// <summary>
        /// 长度
        /// </summary>
        public int Length
        {
            get { return cells.Count; }
        }

        /// <summary>
        /// 获取格子
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public UICell this[int index]
        {
            get { return cells[index]; }
        }

        // init
        protected virtual void Awake()
        {
            mask = transform.GetChildComponent<RectTransform>("Mask");
            root = mask.GetChildComponent<RectTransform>("Root");
            cellTemplate = transform.GetChildComponent<UICell>("Cell");

            if (cellTemplate == null)
            {
                //TODO 需要修改
                //cellTemplate = ResMgr.Instance.ResLoad<UICell>(cellPrefabName);
            }

            cellTemplate.GetComponent<RectTransform>().pivot = new Vector2(0.5F, 0.5F);
            cellTemplate.gameObject.SetActive(false);
            //root.GetComponent<HorizontalOrVerticalLayoutGroup>().spacing = space;
            scrollRect = transform.GetComponent<ScrollRect>();
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        public T Cost<T>(int index) where T : UICell
        {
#if UNITY_EDITOR
            if (index >= cells.Count)
            {
                throw new IndexOutOfRangeException();
            }
#endif
            return (T)cells[index];
        }

        /// <summary>
        /// 添加一组数据(覆盖)
        /// </summary>
        /// <param name="array"></param>
        public IEnumerator YieldLaunch<T>(IList<T> array, int onceShowCount = 1)
        {
            //for (var i = 0; i < array.Length; ++i)
            //    Add(array.GetValue(i));
            var min = cells.Count < array.Count ? cells.Count : array.Count;
            var temp = new List<UICell>(array.Count);
            for (var i = 0; i < min; ++i) // update
            {
                var cell = cells[i];
                cell.SetContent(array[i]);
                temp.Add(cell);
            }
			for (var i = array.Count; i < cells.Count; ++i) // remove
            {
                var cell = cells[i];
                if (cache)
                {
                    cell.gameObject.SetActive(false);
                    cellCaches.Add(cell);
                }
                else
                {
                    Destroy(cell.gameObject);
                }
            }
			cells.Clear();
            cells.AddRange(temp);

	        var showCount = onceShowCount;

            for (var i = cells.Count; i < array.Count; ++i) // append
            {
                var cell = GetCellTemplate();
                cell.SetContent(array[i]);
                cell.rectTransform.SetParent(root);
                cell.rectTransform.Normalize();
                cell.name = cells.Count.ToString();
                cells.Add(cell);
                cell.rectTransform.SetAsLastSibling(); // 防止顺序混乱

	            showCount--;
	            if (showCount < 1)
	            {
		            showCount = onceShowCount;
		            yield return null;
	            }
            }
            
            SetPosition(0);
        }

        /// <summary>
        /// 添加一组数据(覆盖)
        /// </summary>
        /// <param name="array"></param>
        public void Launch<T>(IList<T> array)
        {
            //for (var i = 0; i < array.Length; ++i)
            //    Add(array.GetValue(i));
            var min = cells.Count < array.Count ? cells.Count : array.Count;
            var temp = new List<UICell>(array.Count);
            for (var i = 0; i < min; ++i) // update
            {
                var cell = cells[i];
                cell.SetContent(array[i]);
                temp.Add(cell);
            }
            for (var i = cells.Count; i < array.Count; ++i) // append
            {
                var cell = GetCellTemplate();
                cell.SetContent(array[i]);
                cell.rectTransform.SetParent(root);
                cell.rectTransform.Normalize();
                cell.name = temp.Count.ToString();
                temp.Add(cell);
                cell.rectTransform.SetAsLastSibling(); // 防止顺序混乱
            }
            for (var i = array.Count; i < cells.Count; ++i) // remove
            {
                var cell = cells[i];
                if (cache)
                {
                    cell.gameObject.SetActive(false);
                    cellCaches.Add(cell);
                }
                else Destroy(cell.gameObject);
            }
            cells.Clear();
            cells.AddRange(temp);
            SetPosition(0);
        }

        public void SetContent<T>(T[] dataList)
		{
			var dataIndex = 0;
			for (dataIndex = 0; dataIndex < dataList.Length; dataIndex++)
			{
				if (dataIndex < Cells.Count)
				{
					Cells[dataIndex].SetContent(dataList[dataIndex]);
				}
				else
				{
					Add(dataList[dataIndex]);
				}
			}
			for (var i = Cells.Count - 1; i >= dataIndex; i--)
			{
				RemoveAt(i);
			}
		}

		/// <summary>
		/// 追加一组数据
		/// </summary>
		public void AppendRange(Array array)
        {
            for (var i = 0; i < array.Length; ++i)
            {
                Add(array.GetValue(i));
            }
        }

        /// <summary>
        ///     添加格子
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public UICell Add(params object[] args)
        {
            var cell = GetCellTemplate();
            cell.SetContent(args);
			cell.rectTransform.SetParent(root);
            cell.rectTransform.Normalize();
            cell.rectTransform.localPosition = Vector3.zero;
            cell.name = cells.Count.ToString();
            cells.Add(cell);
            cell.rectTransform.SetAsLastSibling(); // 防止顺序混乱
			return cell;
        }

		/// <summary>
        ///     添加格子
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public UICell AddFirst(params object[] args)
        {
            var cell = GetCellTemplate();
            cell.SetContent(args);
			cell.rectTransform.SetParent(root);
            cell.rectTransform.Normalize();
            cell.rectTransform.localPosition = Vector3.zero;
            cell.name = cells.Count.ToString();
            cells.Insert(0, cell);
            cell.rectTransform.SetAsFirstSibling(); // 防止顺序混乱
			return cell;
        }
		
        /// <summary>
        /// </summary>
        /// <param name="cell"></param>
        public void Remove(UICell cell)
        {
            var index = Cells.FindIndex(c => c == cell);
            if (index == -1)
            {
                Debug.LogError($"列表中不存在{cell}");
                return;
            }
            RemoveAt(index);
        }

        /// <summary>
        ///     移除指定位置的格子
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            var cell = cells[index];
            cells.RemoveAt(index);
	        if (cache)
	        {
		        cell.gameObject.SetActive(false);
		        cellCaches.Add(cell);
	        }
	        else
	        {
		        Destroy(cell.gameObject);
	        }
        }

        /// <summary>
        ///     排序
        /// </summary>
        /// <param name="comparer"></param>
        public void Sort(IComparer<UICell> comparer)
        {
            cells.Sort(comparer);
            for (var i = 0; i < cells.Count; ++i)
            {
                var cell = cells[i];
                cell.rectTransform.SetSiblingIndex(i);
            }
        }

        /// <summary>
        ///  设置位置
        /// </summary>
        /// <param name="postion"></param>
        public abstract void SetPosition(float postion);

        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="postion"></param>
        public void SetPosition(Vector2 postion)
        {
            root.anchoredPosition = postion;
        }

        /// <summary>
        /// 移动到指定位置
        /// </summary>
        /// <param name="index">从0开始</param>
        /// <param name="direction">需要靠近的Mask方向</param>
        public abstract void MoveTo(int index, Direction direction);


        /// <summary>
        /// 获取一个模版
        /// </summary>
        /// <returns></returns>
        protected UICell GetCellTemplate()
        {
            UICell cell;
            if (cellCaches.Count > 0)
            {
                cell = cellCaches[0];
                cellCaches.RemoveAt(0);
                cell.gameObject.SetActive(true);
            }
            else
            {
                cell = Instantiate(cellTemplate);
                cell.gameObject.SetActive(true);
                cell.transform.SetParent(root);
                cell.transform.localScale = Vector3.one;
            }
            return cell;
        }

        /// <summary>
        /// 清楚所有格子 与数据
        /// </summary>
        public void Clear()
        {
            if (cache)
            {
                for (var i = cells.Count - 1; i >= 0; --i)
                {
                    var cell = cells[i];
                    cell.gameObject.SetActive(false);
                    cellCaches.Add(cell);
                }
                cells.Clear();
            }
            else
            {
                for (var i = cells.Count - 1; i >= 0; --i)
                    DestroyImmediate(cells[i].gameObject);
                cells.Clear();
            }
            SetPosition(0);
            OnClear();
        }

        /// <summary>
        /// 遍历
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        public void Foreach<T>(Action<T> callback) where T : UICell
        {
            cells.ForEach(c => callback((T)c));
        }

        /// <summary>
        /// 索引查找
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int FindIndex<T>(Predicate<T> predicate) where T : UICell
        {
            for (var i = 0; i < cells.Count; ++i)
            {
                if (predicate((T)cells[i]))
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// 索引查找
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T Find<T>(Predicate<T> predicate) where T : UICell
        {
            for (var i = 0; i < cells.Count; ++i)
            {
                var t = (T)cells[i];
                if (predicate(t))
                    return t;
            }
            return null;
        }


        /// <summary>
        /// 清理后必须做一些参数修改操作
        /// </summary>
        protected abstract void OnClear();

        /// <summary>
        /// 方向
        /// </summary>
        public enum Direction
        {
            Up,
            Right,
            Down,
            Left,
            Center
        }
        
    }
}