using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2023,01,18,19:22
// @Description:
// </summary>

namespace Game.UI
{
    /// <summary>
    ///  水平列表
    ///   author:sy
    /// </summary>
    public class UIListHorizontal : UIListBase
    {
        #region Field
        // content height
        private float x;
        #endregion

        // init
        protected override void Awake()
        {
            base.Awake();
            // --- in child
            scrollRect.horizontal = true;
            scrollRect.vertical = false;
            x = border;
        }

        void Start()
        {
            maskSize = mask.rect.width;
        }


        /// <summary>
        ///     添加格子
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override UICell Add(params object[] args)
        {
            var cell = GetCellTemplate();
            cell.SetContent(args);
            cell.name = Cells.Count.ToString();
            x += Cells.Count > 0 ? space : 0; // 如果不是第一个需要加一个空格 +space
            Cells.Add(cell);
            var rect = cell.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(x, 0);
            x += cell.Size.x; // + cell width
            var temp = root.sizeDelta;
            temp.x = x + border;
            root.sizeDelta = temp;
            scrollRect.enabled = temp.x > maskSize;
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
            x -= cell.Size.x;
            x -= Cells.Count > 1 ? space : 0;
            var xUnit = rect.anchoredPosition.x;
            cells.RemoveAt(index);
            //if (cache)
            //{
            //    cell.gameObject.SetActive(false);
            //    cellCaches.Add(cell);
            //}
            //else
                Destroy(cell.gameObject);
            // resort
            for (var i = index; i < Cells.Count; ++i)
            {
                var c = Cells[i];
                var uRect = c.GetComponent<RectTransform>();
                uRect.anchoredPosition = new Vector2(xUnit, 0);
                xUnit += c.Size.x + space;
            }
            var temp = root.sizeDelta;
            temp.x = x + border;
            root.sizeDelta = temp;
            scrollRect.enabled = temp.x > maskSize;
        }
        /// <summary>
        ///     排序
        /// </summary>
        /// <param name="comparer"></param>
        public override void Sort(Comparison<UICell> comparer)
        {
            cells.Sort(comparer);
            x = border;
            for (var i = 0; i < cells.Count; ++i)
            {
                var cell = cells[i];
                var rect = cell.GetComponent<RectTransform>();
                x += i > 0 ? space : 0; // +space
                rect.anchoredPosition = new Vector2(x, 0);
                x += cell.Size.x; // + width
            }
            var temp = root.sizeDelta;
            temp.x = x + border;
            root.sizeDelta = temp;
            scrollRect.enabled = temp.x > maskSize;
        }

        /// <summary>
        ///  设置位置
        /// </summary>
        /// <param name="postion"></param>
        public override void SetPosition(float postion)
        {
            root.anchoredPosition = new Vector2(postion, 0);
        }

        /// <summary>
        /// 查询是否可以移动
        /// </summary>
        /// <returns></returns>
        public override bool CanMove()
        {
            return x - maskSize > 0;
        }

        /// <summary>
        /// 移动到指定位置
        /// </summary>
        /// <param name="index">从0开始</param>
        public override void MoveTo(int index)
        {
            if (index < 0 || index >= cells.Count) throw new ArgumentOutOfRangeException("index");

            var diff = x - maskSize;
            if (diff > 0 && cells.Count > 0)
            {
                SetPosition(Mathf.Min((cells[0].Size.x + space) * index, diff));
            }
        }

        /// <summary>
        /// 移动到下一个位置
        /// </summary>
        public override void MoveNext()
        {
            var diff = x - maskSize;
            if (diff > 0 && cells.Count > 0)
            {
                var currentPostion = root.anchoredPosition.x;
                SetPosition(Mathf.Min(currentPostion + cells[0].Size.x + space, diff));
            }
        }

        /// <summary>
        /// 移动到最后
        /// </summary>
        public override void MoveToLast()
        {
            var diff = x - maskSize;
            if (diff > 0)
            {
                SetPosition(diff);
            }
        }

        /// <summary>
        ///  参数处理
        /// </summary>
        protected override void OnClear()
        {
            x = border;
        }
    }
}