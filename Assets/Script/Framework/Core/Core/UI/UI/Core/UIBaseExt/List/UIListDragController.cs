using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
// @Author: zrh
// @Date: 2022,12,22,17:30
// @Description:
// </summary>

namespace Game.UI
{
    public class UIListDragController
    {
        private UIListBaseByAutoLauout uiList;

        private int countForInit;

        private int countForAdd;

        private List<object> cachedData = new List<object>();

        private int currentIndex;

        public UIListDragController(UIListBaseByAutoLauout uiList, int countForInit, int countForAdd)
        {
            this.uiList = uiList;
            this.countForInit = countForInit;
            this.countForAdd = countForAdd;

            currentIndex = -1;
        }

        public void SetContent(IList data)
        {
            cachedData.Clear();
            for (var i = 0; i < data.Count; i++)
            {
                cachedData.Add(data[i]);
            }

            currentIndex = Mathf.Max(cachedData.Count - countForInit, 0);
            var initData = cachedData.GetRange(currentIndex, cachedData.Count - currentIndex);

            uiList.SetPosition(0);
            uiList.SetContent(initData.ToArray());
        }

        public void Add(object data)
        {
            cachedData.Add(data);
            uiList.Add(data);
        }

        public void RemoveAt(int index)
        {
            cachedData.RemoveAt(index);

            if (index >= currentIndex)
            {
                uiList.RemoveAt(index - currentIndex);
            }

            if (currentIndex > 0)
            {
                currentIndex--;
            }
        }

        public void Clear()
        {
            cachedData.Clear();
            uiList.Clear();

            currentIndex = -1;
        }

        public void DoUpdate()
        {
            if (uiList.scrollRect.content.anchoredPosition.y + uiList.scrollRect.content.rect.height < uiList.scrollRect.viewport.rect.height + 50)
            {
                AppendData();
            }
        }

        private void AppendData()
        {
            var tempCountForAdd = countForAdd;
            while (currentIndex > 0 && tempCountForAdd > 0)
            {
                currentIndex--;
                tempCountForAdd--;
                uiList.AddFirst(cachedData[currentIndex]);
            }
        }
        
    }
}