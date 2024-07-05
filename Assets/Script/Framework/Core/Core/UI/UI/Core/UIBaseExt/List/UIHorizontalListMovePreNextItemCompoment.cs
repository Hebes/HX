using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [RequireComponent(typeof(UIListHorizontal))]
    public class UIHorizontalListMovePreNextItemCompoment : MonoBehaviour
    {
        private UIListHorizontal listHorizontal;
        private ScrollRect scrollRect;

        public Button preButton;
        public Button nextButton;

        private void Awake()
        {
            listHorizontal = GetComponent<UIListHorizontal>();
            scrollRect = GetComponentInParent<ScrollRect>();

            preButton.AppendClick(OnClickedPreButton);
            nextButton.AppendClick(OnClickedNextButton);
        }

        private void OnClickedPreButton()
        {
            Move(-1);
        }

        private void OnClickedNextButton()
        {
            Move(1);
        }

        private void Move(int step)
        {
            if (scrollRect != null && scrollRect.content != null)
            {
                var rootRectTransform = scrollRect.content.transform.GetComponent<RectTransform>();
                var localPos = rootRectTransform.anchoredPosition;
                var rectTransformCell = listHorizontal.GetChildComponent<RectTransform>("Cell");
                var space = listHorizontal.space;

                var aCellSize = (rectTransformCell.rect.width + space);

                var firstIndex = (int)(Mathf.Abs(rootRectTransform.localPosition.x) / aCellSize);

                localPos.x = -(rectTransformCell.rect.width + space) * (firstIndex + step);

                rootRectTransform.anchoredPosition = localPos;
            }
        }
    }
}