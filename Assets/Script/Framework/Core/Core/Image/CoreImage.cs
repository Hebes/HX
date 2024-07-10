using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Core
{
    /// <summary>
    /// 图片核心
    /// </summary>
    [CreateCore(typeof(CoreImage), 2)]
    public class CoreImage : ICore
    {
        public static CoreImage Instance;
        private Dictionary<string, ImageComponent> ImageDic { get; set; }
        private Dictionary<string, Sprite> SpriteDic { get; set; }

        public void Init()
        {
            Instance = this;
            ImageDic = new Dictionary<string, ImageComponent>();
            SpriteDic = new Dictionary<string, Sprite>();
        }

        public IEnumerator AsyncEnter()
        {
            yield return null;
        }

        public IEnumerator Exit()
        {
            yield break;
        }

        public static void Add(ImageComponent imageComponent)
        {
            if (!Instance.ImageDic.TryAdd(imageComponent.name, imageComponent))
                throw new Exception("当前组件已有重复名称");
        }

        public static void Add(Sprite sprite)
        {
            if (!Instance.SpriteDic.TryAdd(sprite.name, sprite))
                throw new Exception("当前已有重复组件");
        }

        public static Sprite GetSprite(string name)
        {
            return Instance.SpriteDic.GetValueOrDefault(name);
        }

        public static void Refresh(string name)
        {
            if (Instance.ImageDic.TryGetValue(name, out var imageComponent))
                imageComponent.Refresh();
        }

        

        //private ImageData Load(EImageType imageType, IImageResLoadd imageResLoadd)
        //{
        //    ImageData imageData = default;
        //    imageData.imageType = imageType;
        //    imageData.sprite = CoreResource.Load<Sprite>(imageType.ToString());
        //    imageData.imageResLoadd = imageResLoadd;
        //    imageList.Add(imageData);
        //    return imageData;
        //}

        // public static IEnumerator LoadAsync(IImageResLoadd imageResLoadd, Image image)
        // {
        //     yield return CoreResource.LoadAsync<Sprite>($"{Instance.imagePathDic[imageResLoadd.imageGroup]}/{image.name}", LoadOverImage);
        //
        //     void LoadOverImage(Sprite sprite)
        //     {
        //         image.sprite = sprite;
        //     }
        // }
    }
}