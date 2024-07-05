using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zhaorh.UI;

// <summary>
// @Author: zrh
// @Date: 2023,01,16,18:31
// @Description:
// </summary>

namespace Game.UI
{
    /// <summary>
    /// 进度条
	/// 旧的进度条的显示不合理,更改为使用这个,使用统一的节点结构,不使用WidthTween做动画
    /// </summary>
	[DisallowMultipleComponent]
    public class UISlicedBar2 : MonoBehaviour
    {
        // 动画周期
        [SerializeField]
        private float tweenDuration = .2f;
        
		// 最大值
        [SerializeField]
        private long valueMax = 100;
		// 前景宽度追加值
		
		/// <summary>
		/// 当前值
		/// </summary>
        private long curValue;

		/// <summary>
		/// 进度条图片
		/// </summary>
		//RectTransform valRect;
		public LayoutElement valLayout;

		/// <summary>
		/// 左边的边缘,当进度为0时隐藏
		/// </summary>
		public Transform barLeft;

		/// <summary>
		/// 右边的边缘,当进度为0时隐藏
		/// </summary>
		public Transform barRight;
        
		// 进度条最大宽度
        private float maxWidth;
        
		// 系数
        private float k;
        
		/// <summary>
		/// 进度条动画
		/// </summary>
		public SliceBarTween sbTween = null;

		private Transform tran = null;

        void Awake()
        {
			tran = transform;

			var progressRoot = tran.GetChildComponent<Transform>("ProgressRoot");
			
			barLeft = progressRoot.GetChildComponent<Transform>("BarLeftImage");
			
			var valRect = progressRoot.GetChildComponent<RectTransform>("BarImage");
			valLayout = valRect.GetComponent<LayoutElement>();

			barRight = progressRoot.GetChildComponent<Transform>("BarRightImage");
			
			maxWidth = transform.GetComponent<RectTransform>().sizeDelta.x;
            k = maxWidth / ValueMax;
			
			sbTween = gameObject.AddComponent<SliceBarTween>();
			sbTween.enabled = false;
			sbTween.duration = tweenDuration;
			sbTween.target = valRect;
			sbTween.targetMaxWidth = maxWidth;

			sbTween.fxImage = barRight.GetChildComponent<Image>("TweenFx");
		}

		/// <summary>
		/// 设置进度条的值,并且带有动画,一次的动画使用这个,如果是多个的,需要自行组织TweenData
		/// </summary>
		/// <param name="val"></param>
		public void SetValue(long val)
		{
			long from = Value;
			List<SliceBarTween.Data> moveQueue = new List<SliceBarTween.Data>();
			moveQueue.Add(new SliceBarTween.Data() { from = 1f * from / ValueMax, to = 1f * val / ValueMax });

			Value = val;

			sbTween.ClearQueue();
			sbTween.AppendQueue(moveQueue);
		}

        /// <summary>
        ///  设置进度条的值
        /// </summary>
        public long Value
        {
            set
            {
                curValue = value; // 保证为实际值
                var val = value > valueMax ? valueMax : value;
				
				if (val == 0)
                {
					barLeft.gameObject.SetActive(false);
					valLayout.gameObject.SetActive(false);
					barRight.gameObject.SetActive(false);
                }
				else
				{
					barLeft.gameObject.SetActive(true);
					valLayout.gameObject.SetActive(true);
					barRight.gameObject.SetActive(true);
					
					valLayout.preferredWidth = val * k;
				}
            }
            get { return curValue; }
        }
		
        /// <summary>
        /// 最大值
        /// </summary>
        public long ValueMax
        {
            get { return valueMax; }
            set
            {
                valueMax = value;
                k = maxWidth / ValueMax;
            }
        }
		
    }
}