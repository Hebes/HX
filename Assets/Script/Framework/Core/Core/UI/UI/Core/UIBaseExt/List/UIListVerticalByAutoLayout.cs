using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2022,12,23,14:39
// @Description:
// </summary>

namespace Game.UI
{
    public class UIListVerticalByAutoLayout : UIListBaseByAutoLauout
    {
        protected override void Awake()
        {
            base.Awake();
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
        }

        public override void SetPosition(float postion)
        {
            root.anchoredPosition = new Vector2(0, postion);
        }

        public override void MoveTo(int index, Direction direction)
        {
            
        }

        protected override void OnClear()
        {
            
        }
        
    }
}