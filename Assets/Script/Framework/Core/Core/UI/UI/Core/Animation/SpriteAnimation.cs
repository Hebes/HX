using Game.UI;
using UnityEngine;
using UnityEngine.UI;

namespace zhaorh.UI
{
    /// <summary>
    /// UI帧动画
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class SpriteAnimation : MonoBehaviour
    {
        /// <summary>
        /// 结束回调
        /// </summary>
        public zhaorh.UI.Event onEnd = new zhaorh.UI.Event();

        public void SetOnEnd(Event onEndParam)
        {
            this.onEnd = onEndParam;
        }

        public void ClearOnEndEvent()
        {
            this.onEnd = null;
        }

        // 变换
        public RectTransform rectTransform { get; set; }

        public bool IsPlaying
        {
            get { return isPlaying; }
        }
        // 相对位置偏移
        [SerializeField]
        private Vector2 offset;

        // 自动播放
        public bool auto = false;
        // 停留到最后一帧
        public bool leaveToLastFrame = false;
        // 重复播放
        public bool loop = true;
        // 图片的alpha值
        public float imageAlpha = 1f;
        //每次播放完之后等待的时间
        public float timeInternal = 0f;
        // 图片
        private Image image;
        // 帧
        public Sprite[] frames;
        // 更换频率
        public float rate = .25f;
        // 当前图片索引
        private int index;
        // 计时器
        private float timer;

        /// <summary>
        /// Model数据，用以判断当前是否在播放
        /// </summary>
        private bool isPlaying = false;


        public void SetOffset(Vector2 offset)
        {
            this.offset = offset;
            rectTransform.anchoredPosition = offset;
        }

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            image = GetComponent<Image>();
            if (frames != null && frames.Length > 0)
            {
                image.sprite = frames[0];
            }
            else
            {
                Debug.LogError("框架。Length ==0，至少需要2帧" + name);
            }
            index = 0;
            if (auto)
            {
                Play();
            }
            else
            {
                if (image.enabled)
                {
                    Stop();
                }
            }
        }

        protected void OnEnable()
        {
            image.enabled = true;
        }


        protected void OnDisable()
        {
            image.enabled = false;

            isPlaying = false;
        }

        /// <summary>
        /// 获取周期
        /// </summary>
        /// <returns></returns>
        public float GetDuration()
        {
            return rate * frames.Length;
        }

        public float GetTimer()
        {
            return timer;
        }

        public void SetTimer(float timerParam)
        {
            timer = timerParam;
        }

        public int GetIndex()
        {
            return index;
        }

        public void SetIndex(int indexParam)
        {
            index = indexParam;
        }

        /// <summary>
        /// 设置父节点
        /// </summary>
        /// <param name="rect"></param>
        public void SetParent(RectTransform rect)
        {
            rectTransform.SetParent(rect);
            rectTransform.Normalize();
            rectTransform.anchoredPosition = offset;
        }


#if UNITY_EDITOR
        [ContextMenu("Play")]
#endif
        // 开始
        public void Play()
        {
            if(!image.enabled)
            {
                image.enabled = enabled = true;
            }
            image.enabled = enabled = true;
            image.color = new Color(image.color.r, image.color.g, image.color.b, imageAlpha);
            index = 0;
            timer = Time.unscaledTime;

            isPlaying = true;
        }

        // 开始
        public void PlayWithRandomStart()
        {
            if(!image.enabled)
            {
                image.enabled = enabled = true;
            }
            image.color = new Color(image.color.r, image.color.g, image.color.b, imageAlpha);
            index = Random.Range(0, frames.Length);
            timer = Time.unscaledTime;
            isPlaying = true;
        }

        public void Stop()
        {
            enabled = false;

            isPlaying = false;

            if (image != null)
            {
                image.enabled = leaveToLastFrame;
            }

            if (!leaveToLastFrame)
            {
                //潜规则，隐藏上层节点下面的标题图片
                if (this != null && this.transform != null)
                {
                    Transform parentTransform = this.transform.parent;
                    if (parentTransform)
                    {
                        UISpriteAnimationFxLoader loader = parentTransform.GetComponent<UISpriteAnimationFxLoader>();
                        if (loader != null && loader.tImg != null)
                        {
                            loader.tImg.enabled = false;
                        }
                    }
                }

            }
            if (onEnd != null)
            {
                onEnd.Invoke();
            }
        }

        // 更新
        void Update()
        {
            if (index == frames.Length - 1 && Time.unscaledTime - timer < timeInternal)
            {
                return;
            }

            if (Time.unscaledTime - timer > rate)
            {
                timer = Time.unscaledTime;
                index = index == frames.Length - 1 ? 0 : index + 1;
                image.sprite = frames[index];
                if (!loop && index == frames.Length - 1)
                {
                    Stop();
                }
            }
        }
    }
}
