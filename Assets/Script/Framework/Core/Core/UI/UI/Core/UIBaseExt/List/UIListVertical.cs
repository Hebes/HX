using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2022,12,24,11:22
// @Description:
// </summary>

namespace Game.UI
{
    /// <summary>
    ///     UI 列表 - 垂直
    ///     author:sy
    /// </summary>
    public class UIListVertical : UIListBase
    {
        #region Field
        // content height
        protected float y;
        #endregion
        // init
        protected override void Awake()
        {
            base.Awake();
            // --- in child
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            y = border;
        }

        void Start()
        {
            maskSize = mask.rect.height;
        }

        /// <summary>
        ///     添加格子
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override UICell Add(params object[] args)
        {
            var cell = GetCellTemplate();
            cell.gameObject.SetActive(true);
            cell.transform.SetParent(root);
            cell.transform.localScale = Vector3.one;
            cell.transform.localPosition = Vector3.zero;
            cell.SetContent(args);
            cell.name = cells.Count.ToString();
            cells.Add(cell);

            y = 0;

            for (var i = 0; i < cells.Count; i++)
            {
                y += (cells[i].Size.y + space);
            }
            
            y = y + 2 * border - space;

            var rect = cell.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, -(y - cell.Size.y - border));

            var temp = root.sizeDelta;
            temp.y = y;
            root.sizeDelta = temp;

            if (scrollRect.verticalScrollbar)
                scrollRect.verticalScrollbar.gameObject.SetActive(temp.y > maskSize);

            return cell;
        }

        /// <summary>
        ///     移除指定位置的格子
        /// </summary>
        /// <param name="index"></param>
        public override void RemoveAt(int index)
        {
            var cell = cells[index];
            var rect = cell.GetComponent<RectTransform>();
            y -= rect.sizeDelta.y;
            y -= cells.Count > 1 ? space : 0;
            var yUnit = -rect.anchoredPosition.y;
            cells.RemoveAt(index);
            
            Destroy(rect.gameObject);
            // resort
            for (var i = index; i < cells.Count; ++i)
            {
                var c = Cells[i];
                var uRect = c.GetComponent<RectTransform>();
                uRect.anchoredPosition = new Vector2(0, -yUnit);
                yUnit += c.Size.y + space;
            }
            var temp = root.sizeDelta;
            temp.y = y + border;
            root.sizeDelta = temp;
            //scrollRect.enabled = temp.y > maskSize;
            if (scrollRect.verticalScrollbar)
                scrollRect.verticalScrollbar.gameObject.SetActive(temp.y > maskSize);
        }

        /// <summary>
        ///     排序
        /// </summary>
        /// <param name="comparer"></param>
        public override void Sort(Comparison<UICell> comparer)
        {
            Cells.Sort(comparer);
            y = border;
            for (var i = 0; i < Cells.Count; ++i)
            {
                var cell = Cells[i];
                var rect = cell.GetComponent<RectTransform>();
                y += i > 0 ? space : 0; // +space
                rect.anchoredPosition = new Vector2(0, -y);
                y += cell.Size.y; // + width
            }
            var temp = root.sizeDelta;
            temp.y = y + border;
            root.sizeDelta = temp;
            //scrollRect.enabled = temp.y > maskSize;
            if (scrollRect.verticalScrollbar)
                scrollRect.verticalScrollbar.gameObject.SetActive(temp.y > maskSize);
        }

        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="postion"></param>
        public override void SetPosition(float postion)
        {
            if (root != null)
            {
                root.anchoredPosition = new Vector2(0, postion);
            }
        }

        /// <summary>
        /// 查询是否可以移动
        /// </summary>
        /// <returns></returns>
        public override bool CanMove()
        {
            return y - maskSize > 0;
        }

        /// <summary>
        /// 移动到指定位置(使用此方法建议space>=border，不然表现怪异)
        /// </summary>
        /// <param name="index">从0开始</param>
        public override void MoveTo(int index)
        {
            if (index < 0 || index >= cells.Count) throw new ArgumentOutOfRangeException("index");

            var diff = y - maskSize;
            if (diff > 0 && cells.Count > 0)
            {
                SetPosition(Mathf.Min((cells[0].Size.y + space) * index, diff));
            }
        }

        /// <summary>
        /// 移动到下一个位置
        /// </summary>
        public override void MoveNext()
        {
            var diff = y - maskSize;
            if (diff > 0 && cells.Count > 0)
            {
                var currentPostion = root.anchoredPosition.y;
                SetPosition(Mathf.Min(currentPostion + cells[0].Size.y + space, diff));
            }
        }

        /// <summary>
        /// 移动到最后
        /// </summary>
        public override void MoveToLast()
        {
            var diff = y - maskSize;
            if (diff > 0)
            {
                SetPosition(diff);
            }
        }

        /// <summary>
        ///  重置 y
        /// </summary>
        protected override void OnClear()
        {
            y = border;
            if (scrollRect != null && scrollRect.verticalScrollbar)
                scrollRect.verticalScrollbar.gameObject.SetActive(false);
        }

        /// <summary>
        /// 重新设置一个Cell的高度
        /// 从 index 开始布局
        /// </summary>
        /// <param name="index"></param>
        /// <param name="oldValue">原始值</param>
        /// <param name="newValue">新值</param>
        public void ResizeCell(int index, float oldValue, float newValue)
        {
            y += newValue - oldValue;
            var yUnit = cells[index].GetComponent<RectTransform>().anchoredPosition.y;
            // relayout
            for (var i = index; i < cells.Count; ++i)
            {
                var cell = cells[i];
                var rect = cell.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(0, yUnit);
                yUnit -= cell.Size.y + space;
            }
            var temp = root.sizeDelta;
            temp.y = y;
            root.sizeDelta = temp;
            scrollRect.enabled = temp.y > maskSize;
            if (scrollRect.verticalScrollbar)
                scrollRect.verticalScrollbar.gameObject.SetActive(scrollRect.enabled);
        }

        /// <summary>
        /// 重新设置一个Cell的高度
        /// 从 index 开始布局
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        public void ResizeCell(UICell cell, float oldValue, float newValue)
        {
            var index = cells.FindIndex(c => cell == c);
            ResizeCell(index, oldValue, newValue);
        }

        /// <summary>
        /// 重新布局
        /// </summary>
        public void Relayout()
        {
            y = border;
            for (var i = 0; i < cells.Count; ++i)
            {
                var cell = cells[i];
                var rect = cell.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(0, -y);
                y += cell.Size.y + space;
            }
            var temp = root.sizeDelta;
            temp.y = y + border;
            root.sizeDelta = temp;
            //scrollRect.enabled = temp.y > maskSize;
        }

        /// <summary>
        /// 高度
        /// </summary>
        public float height
        {
            get { return y; }
        }
    }
}