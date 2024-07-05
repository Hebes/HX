using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;
using zhaorh.UI;

// <summary>
// @Author: zrh
// @Date: 2022,12,24,11:23
// @Description:
// </summary>

namespace Game.UI
{
    /// <summary>
    ///     UI 列表
    ///     author:sy
    /// </summary>
    public abstract class UIListBase : MonoBehaviour
    {
        #region Field
        // 格子之间的间距
        public float space;
        // 格子到四周的间距
        public float border;

        // 格子
        protected readonly List<UICell> cells = new List<UICell>();
        // cell template
        protected UICell cellTemplate;
        // mask height
        protected float maskSize;
        // root rect
        protected RectTransform root;
        // rect
        public ScrollRect scrollRect { get; protected set; }
        // mask
        protected RectTransform mask;

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
            root = mask.GetChildComponent<RectTransform>(0);
            cellTemplate = transform.GetChildComponent<UICell>("Cell");
            
            if (cellTemplate == null)
            {
                //TODO 需要修改
                //cellTemplate = ResMgr.Instance.ResLoad<UICell>(cellPrefabName);
            }

            cellTemplate.gameObject.SetActive(false);
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
                throw new IndexOutOfRangeException();
#endif
            return (T)cells[index];
        }

        //protected void OnValueChanged(Vector2 vec)
        //{
        //    var rootY = root.anchoredPosition.y;
        //    if (rootY > yOffsetMax || rootY < yOffsetMin)
        //    {
        //        auto = true;
        //        scrollRect.enabled = !auto;
        //        scrollRect.inertia = !auto;
        //        isMaxOffset = rootY > yOffsetMax;
        //    }
        //}

        /// <summary>
        /// 添加一组数据
        /// </summary>
        /// <param name="array"></param>
        public void AddRange(Array array)
        {
            for (var i = 0; i < array.Length; ++i)
                Add(array.GetValue(i));
        }

        /// <summary>
        ///     添加格子
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public abstract UICell Add(params object[] args);

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
        public abstract void RemoveAt(int index);

        /// <summary>
        ///     排序
        /// </summary>
        /// <param name="comparer"></param>
        public abstract void Sort(Comparison<UICell> comparer);

        /// <summary>
        ///  设置位置
        /// </summary>
        /// <param name="postion"></param>
        public abstract void SetPosition(float postion);

        /// <summary>
        /// 查询是否可以移动
        /// </summary>
        /// <returns></returns>
        public abstract bool CanMove();

        /// <summary>
        /// 移动到指定位置
        /// </summary>
        /// <param name="index">从0开始</param>
        public abstract void MoveTo(int index);

        /// <summary>
        /// 移动到下一个位置
        /// </summary>
        public abstract void MoveNext();

        /// <summary>
        /// 移动到最后
        /// </summary>
        public abstract void MoveToLast();

        /// <summary>
        /// 获取一个模版
        /// </summary>
        /// <returns></returns>
        protected UICell GetCellTemplate()
        {
            var cell = Instantiate(cellTemplate);
            cell.gameObject.SetActive(true);
            cell.transform.SetParent(root);
            cell.transform.localScale = Vector3.one;
            cell.transform.localPosition = Vector3.zero;
            return cell;
        }

        /// <summary>
        /// 清楚所有格子 与数据
        /// </summary>
        public void Clear()
        {
            for (var i = cells.Count - 1; i >= 0; --i)
            {
                DestroyImmediate(cells[i].gameObject);
            }
            cells.Clear();
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
        /// 设置单元格中心点
        /// </summary>
        /// <param name="pivot"></param>
        public void SetPivot(Vector2 pivot)
        {
            cells.ForEach(c =>
             {
                 // 需要计算 中心改变后的位置 保证格子的位置不会改变
                 var rect = c.GetComponent<RectTransform>();
                 var x0 = rect.anchoredPosition.x - rect.pivot.x * rect.sizeDelta.x;
                 var y0 = rect.anchoredPosition.y - rect.pivot.y * rect.sizeDelta.y;
                 rect.anchoredPosition = new Vector2(x0 + pivot.x * rect.sizeDelta.x, y0 + pivot.y * rect.sizeDelta.y);
                 rect.pivot = pivot;
             });
        }

        /// <summary>
        /// 清理后必须做一些参数修改操作
        /// </summary>
        protected abstract void OnClear();

        /// <summary>
        /// 遮罩层尺寸
        /// </summary>
        public float MaskSize
        {
            get { return maskSize; }
        }
    }
}