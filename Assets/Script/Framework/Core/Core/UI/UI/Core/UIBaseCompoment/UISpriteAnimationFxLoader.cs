
using System.Collections.Generic;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

namespace zhaorh.UI
{
    public class UISpriteAnimationFxLoader : MonoBehaviour
    {
        public Event onEnd = new Event();
        /// <summary>
        /// 针对特效覆盖在一张图片上的情况，在这里设置被覆盖的图片，一般是“升级成功”之类的美术字
        /// </summary>
        //public Sprite titleImage;
        public string titleImageName;

        public string spritePrefabName;

        public Vector2 offset = Vector2.zero;

        public bool autoPlayOption = true;
        public bool loopOption = true;
        public bool keepTheLastFrame;
        public float rate = 0.1f;

        public Vector2 scale = Vector2.one;

        public float imageAlpha = 1f;

        private SpriteAnimation spriteAnimation;

        /// <summary>
        /// 是否自动附加UI叠加特效
        /// </summary>
        public bool needUIAdditveFx = true;

        /// <summary>
        /// 在基础上叠加的特效
        /// </summary>
        private List<SpriteAnimation> additionalSpriteAnimations = new List<SpriteAnimation>();

        /// <summary>
        /// 临时生成的标题图片
        /// </summary>
        [System.NonSerialized]
        public Image tImg = null;

        public RectTransform rectTransform
        {
            get { return GetComponent<RectTransform>(); }
        }

        public bool IsPlaying
        {
            get
            {
                if (spriteAnimation != null)
                {
                    return spriteAnimation.IsPlaying;
                }
                else
                {
                    return false;
                }
            }
        }

        public void SetParent(RectTransform rect)
        {
            rectTransform.SetParent(rect);
            rectTransform.Normalize();
            rectTransform.anchoredPosition = offset;
        }

        private void OnEnable()
        {
            LoadSpriteAnimationFx();

            //每次enable的时候，判断是否自动播放 
            if (autoPlayOption)
            {
                Play();
            }
            else
            {
                if (IsPlaying == false)
                {
                    Stop();
                }
            }
        }

        private void OnDisable()
        {
            ReleaseSpriteAnimationFx();
        }

        [ContextMenu("Play")]
        public void Play()
        {
            if (spriteAnimation == null)
            {
                LoadSpriteAnimationFx();
            }

            if (spriteAnimation != null)
            {
                if (tImg != null)
                {
                    tImg.enabled = true;
                }
                // 防止默认的自动播放SpriteAnimation干预了顶层的设定不要自动播放
                if (autoPlayOption == false && spriteAnimation.auto == true && spriteAnimation.IsPlaying)
                {
                    spriteAnimation.Stop();
                }
                spriteAnimation.auto = autoPlayOption;
                spriteAnimation.loop = loopOption;
                spriteAnimation.rate = rate;
                spriteAnimation.leaveToLastFrame = keepTheLastFrame;
                spriteAnimation.SetOffset(offset);
                spriteAnimation.imageAlpha = imageAlpha;
                spriteAnimation.Play();

                if (needUIAdditveFx)
                {
                    var spriteImage = spriteAnimation.GetComponent<Image>();
                    if (spriteImage)
                    {
                        spriteImage.material = Tools.UIFxAdditiveMaterial;
                    }
                }

            }
        }

        [ContextMenu("PlayAdditional")]
        public void PlayAdditional(string effectName)
        {
            bool gotFromPast = false;

            if (!string.IsNullOrEmpty(effectName))
            {
                SpriteAnimation spriteAnimation = null;

                for (int i = 0; i < transform.childCount; i++)
                {
                    if(transform.GetChild(i).name == effectName)
                    {
                        SpriteAnimation spriteAnimation1 = transform.GetChild(i).GetComponent<SpriteAnimation>();

                        spriteAnimation = spriteAnimation1;
                        gotFromPast = true;
                    }
                }

                if (!gotFromPast)
                {
                    spriteAnimation = LoadSpriteAnimationFxCore(effectName);
                }

                if (spriteAnimation != null)
                {
                    if (tImg != null)
                    {
                        tImg.enabled = true;
                    }
                    // 防止默认的自动播放SpriteAnimation干预了顶层的设定不要自动播放
                    if (autoPlayOption == false && spriteAnimation.auto == true && spriteAnimation.IsPlaying)
                    {
                        spriteAnimation.Stop();
                    }
                    spriteAnimation.auto = autoPlayOption;
                    spriteAnimation.loop = loopOption;
                    spriteAnimation.rate = rate;
                    spriteAnimation.leaveToLastFrame = keepTheLastFrame;
                    spriteAnimation.SetOffset(offset);
                    spriteAnimation.imageAlpha = imageAlpha;
                    spriteAnimation.PlayWithRandomStart();

                    if (!additionalSpriteAnimations.Contains(spriteAnimation))
                    {
                        additionalSpriteAnimations.Add(spriteAnimation);
                    }
                }
            }
        }

        [ContextMenu("Stop")]
        public void Stop()
        {
            if (spriteAnimation != null)
            {
                if (tImg != null && !keepTheLastFrame)
                {
                    tImg.enabled = false;
                }
                spriteAnimation.Stop();
            }

            for (int i = 0; i < additionalSpriteAnimations.Count; i++)
            {
                additionalSpriteAnimations[i].Stop();
            }
        }

        [ContextMenu("LoadFx")]
        public void LoadSpriteAnimationFx()
        {
            if (this != null)
            {
                var tmp = GetComponentInChildren<SpriteAnimation>();
                if ((tmp == null || spriteAnimation == null) && string.IsNullOrEmpty(spritePrefabName) == false)
                {
                    spriteAnimation = LoadSpriteAnimationFxCore(spritePrefabName);
                }
            }
        }

        public bool isLoading = false;
        /// <summary>
        /// 加载帧动画对象
        /// </summary>
        /// <param name="loadName"></param>
        private SpriteAnimation LoadSpriteAnimationFxCore(string loadName)
        {
            SpriteAnimation spriteAnimation = null;
            GameObject assetsGo = null;
            //如果是加载界面则从 Resources直接加载 否则都从ab加载
            if (isLoading)
            {
                assetsGo = Resources.Load<GameObject>("UI/Fx/loading_weijier");
            }
            else
            {
                //TODO 需要修改
                //assetsGo = ResMgr.Instance.GetAsset<GameObject>(loadName);
            }
            var spriteAnimGO = Instantiate(assetsGo);
            if (spriteAnimGO != null)
            {
                spriteAnimation = spriteAnimGO.GetComponent<SpriteAnimation>();
                spriteAnimGO.name = loadName;
            }
            else
            {
                Debug.LogError("LoadSpriteAnimationFxCore 加载帧动画失败!!");
            }
            
            if (spriteAnimation != null)
            {
                spriteAnimation.transform.SetParent(this.transform);
                spriteAnimation.transform.Normalize();
                spriteAnimation.gameObject.name = loadName;
                spriteAnimation.rectTransform.ResetAnchor();//重置描点
                spriteAnimation.transform.localScale = new Vector3(scale.x, scale.y, 1f);

                // 防止默认的自动播放SpriteAnimation干预了顶层的设定不要自动播放
                if (autoPlayOption == false && spriteAnimation.auto == true && spriteAnimation.IsPlaying)
                {
                    spriteAnimation.Stop();
                }
                spriteAnimation.auto = autoPlayOption;
                spriteAnimation.loop = loopOption;
                spriteAnimation.rate = rate;
                spriteAnimation.leaveToLastFrame = keepTheLastFrame;
                spriteAnimation.SetOffset(offset);

                spriteAnimation.SetOnEnd(onEnd);

                if (string.IsNullOrEmpty(titleImageName) == false)
                {
                    //加载一个标题图片
                    GameObject titleGo = new GameObject("fxTitle");
                    tImg = titleGo.AddComponent<Image>();
                    //TODO 需要修改
                    //tImg.sprite = ResMgr.Instance.GetSprite(titleImageName);
                    tImg.raycastTarget = false;
                    Transform tmpTr = tImg.transform;
                    //避免影响整个特效的size，放到兄弟，然后中心对其
                    tmpTr.SetParent(this.transform);
                    tmpTr.Normalize();
                    tmpTr.position = this.transform.position;
                    tmpTr.SetAsFirstSibling();//设置为第一个层级
                    tImg.enabled = false;
                    
                    titleGo.SetLayerInChildren("UI");//设置层级
                    tImg.SetNativeSize();
                }
            }

            return spriteAnimation;
        }


        [ContextMenu("ReleaseFx")]
        public void ReleaseSpriteAnimationFx()
        {
            if (spriteAnimation != null)
            {
                spriteAnimation.ClearOnEndEvent();
                Destroy(spriteAnimation.gameObject);
                spriteAnimation = null;
            }
            for (int i = 0; i < additionalSpriteAnimations.Count; i++)
            {
                additionalSpriteAnimations[i].ClearOnEndEvent();
                additionalSpriteAnimations[i].Stop();
            }
            additionalSpriteAnimations.Clear();
            if (tImg != null)
            {
                Destroy(tImg.gameObject);
                tImg = null;
            }
        }

        /// <summary>
        /// 影藏之前额外加载的shader
        /// </summary>
        public void HideAdditionalEffect()
        {
            for (int i = 0; i < additionalSpriteAnimations.Count; i++)
            {
                //additionalSpriteAnimations[i].ClearOnEndEvent();
                additionalSpriteAnimations[i].Stop();
            }
        }
    }
}
