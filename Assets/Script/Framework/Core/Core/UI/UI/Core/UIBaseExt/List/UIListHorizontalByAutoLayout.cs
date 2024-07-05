using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// <summary>
// @Author: zrh
// @Date: 2023,01,27,15:46
// @Description:
// </summary>

namespace Game.UI
{
    /// <summary>
    /// 水平格子
    /// </summary>
    public sealed class UIListHorizontalByAutoLayout : UIListBaseByAutoLauout
    {
        private float maskSize;

        protected override void Awake()
        {
            base.Awake();
            scrollRect.horizontal = true;
            scrollRect.vertical = false;
        }

        private void Start()
        {
            maskSize = mask.sizeDelta.x;
        }

        public override void SetPosition(float postion)
        {
            root.anchoredPosition = new Vector2(postion, 0);
        }

        public override void MoveTo(int index, Direction direction)
        {
            var factor = -1;
            if (direction != Direction.Right)
            {
                factor = 1;
            }

            var widthOfCell = 0F;
            if (Cells.Count > 0)
            {
                widthOfCell = Cells[0].Size.x;
            }

            var layout = root.GetComponent<HorizontalLayoutGroup>();
            var moveDistance = (widthOfCell * index + layout.spacing * index);
            var maxRootOffset = widthOfCell * Cells.Count + layout.spacing * (Cells.Count - 1) - maskSize;
            moveDistance = Mathf.Clamp(moveDistance, 0, maxRootOffset);
            root.anchoredPosition = new Vector2(moveDistance * factor, 0);
        }

        protected override void OnClear() { }
    }
}