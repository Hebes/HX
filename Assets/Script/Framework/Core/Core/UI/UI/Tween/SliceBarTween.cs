using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// <summary>
// @Author: zrh
// @Date: 2023,01,16,18:32
// @Description:
// </summary>

namespace Game.UI
{
    /// <summary>
    /// 进度条动画
    /// </summary>
    public class SliceBarTween : MonoBehaviour
    {
        /// <summary>
        /// 归一化的值
        /// </summary>
        public class Data
        {
            /// <summary>
            /// 起始位置,归一化的值
            /// </summary>
            public float from;

            /// <summary>
            /// 结束位置,归一化的值
            /// </summary>
            public float to;

            /// <summary>
            /// 对应此次运动所需的时间,不需要外部传值,用于内部处理
            /// </summary>
            public float duration;

            /// <summary>
            /// 对应此次运动的速度,因为是匀速,避免多次运算,用这个变量记录下来,不需要外部传值
            /// </summary>
            public float v;

            /// <summary>
            /// 运动的时间
            /// </summary>
            private float t = 0f;

            /// <summary>
            /// 获取已经运动的时间
            /// </summary>
            /// <returns></returns>
            public float GetTime()
            {
                t += Time.deltaTime;
                t = Mathf.Min(t, duration);

                return t;
            }
        }

        /// <summary>
        /// 进度条的第几个版本
        /// </summary>
        private int progressVer = 1;

        /// <summary>
        /// 最大值,用于和归一化值计算出具体的值
        /// </summary>
        public float targetMaxWidth;

        /// <summary>
		/// 整条运动的时间
		/// </summary>
        public float duration = 1f;

        // 目标图片
        public RectTransform target;

        public Image fxImage = null;

        private UISlicedBar2 barVer2 = null;

        // 动画列表
        private Queue<Data> animationQueues = new Queue<Data>();

        void Awake()
        {
            barVer2 = GetComponent<UISlicedBar2>();
            if (barVer2 != null)
            {
                progressVer = 2;
            }
            else
            {
                progressVer = 1;
            }
        }

        // 更新
        void Update()
        {
            if (animationQueues.Count > 0)
            {
                SetActiveFxImage(true);

                Data tmpData = animationQueues.Peek();
                float v = tmpData.v;
                float tmpDuration = tmpData.duration;

                float t = tmpData.GetTime();

                float s = tmpData.from + v * t;

                if (progressVer == 2)
                {
                    float val = s * targetMaxWidth;
                    if (val <= 0)
                    {
                        barVer2.barLeft.gameObject.SetActive(false);
                        barVer2.valLayout.gameObject.SetActive(false);
                        barVer2.barRight.gameObject.SetActive(false);
                    }
                    else
                    {
                        barVer2.barLeft.gameObject.SetActive(true);
                        barVer2.valLayout.gameObject.SetActive(true);
                        barVer2.barRight.gameObject.SetActive(true);

                        barVer2.valLayout.preferredWidth = val;
                    }

                    if (fxImage != null)
                    {
                        float dt = 0;
                        if (Math.Abs(tmpDuration) >= 0.01f)
                        {
                            dt = t / tmpDuration;
                        }


                        //放到0-100，透明度0-100
                        var fxScale = fxImage.rectTransform.localScale;
                        fxScale.x = dt;
                        fxScale.y = dt;
                        fxImage.rectTransform.localScale = fxScale;

                        var fxColor = fxImage.color;
                        fxColor.a = dt;
                        fxImage.color = fxColor;
                    }
                }
                else
                {
                    Vector2 size = target.sizeDelta;
                    size.x = s * targetMaxWidth;
                    target.sizeDelta = size;
                }

                if (t >= tmpDuration)
                {
                    SetActiveFxImage(false);

                    animationQueues.Dequeue();
                }
            }
            else
            {
                SetActiveFxImage(false);

                enabled = false;
            }
        }

        private void SetActiveFxImage(bool enable)
        {
            if (fxImage != null)
            {
                fxImage.enabled = enable;
            }
        }

        /// <summary>
        /// 添加动画队列
        /// </summary>
        public void AppendQueue(List<Data> datas)
        {
            for (int i = 0; i < datas.Count; i++)
            {
                Data tmpData = datas[i];

                if (Mathf.Abs(tmpData.from) > 1f)
                {
                    tmpData.from = 1f;
                }
                if (Mathf.Abs(tmpData.to) > 1f)
                {
                    tmpData.to = 1f;
                }

                float v;
                float s = tmpData.to - tmpData.from;
                if (Math.Abs(s) < 0.01f)
                {
                    v = 0;
                }
                else if (s > 0)
                {
                    v = 1 / duration;
                }
                else
                {
                    v = -1 / duration;
                }

                tmpData.v = v;
                //几乎很小了，时间就算了
                if (Math.Abs(v) < 0.01f)
                {
                    tmpData.duration = 0;
                }
                else
                {
                    tmpData.duration = s / v;
                }


                animationQueues.Enqueue(datas[i]);
            }

            if (!enabled)
            {
                enabled = true;
            }
        }

        /// <summary>
        /// 清除所有动画队列
        /// </summary>
        public void ClearQueue()
        {
            int queueCount = animationQueues.Count;
            while (queueCount > 0)
            {
                animationQueues.Dequeue();
                queueCount--;
            }

            enabled = false;
        }

    }
}