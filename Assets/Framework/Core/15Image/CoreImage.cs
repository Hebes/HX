using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class CoreImage : ICore
    {
        private List<ImageData> imagesList;
        private List<ImageResLoadd> imageResLoaddsList;
        private int imageIndex = 0;

        public IEnumerator AsyncInit()
        {
            yield return null;
        }

        public void Init()
        {
            imagesList = new List<ImageData>();
            imageResLoaddsList = new List<ImageResLoadd>();
        }

        public void AddInterfaceToList(ImageResLoadd imageResLoadd)
        {
            if (imageResLoaddsList.Contains(imageResLoadd))
            {
                this.Error($"已经添加{imageResLoadd}");
                return;
            }
            imageResLoaddsList.Add(imageResLoadd);
        }

        public ImageData GetImage(EImageType imageType)
        {
            foreach (ImageData image in imagesList)
            {
                if (image.imageType == imageType)
                    return image;
            }
            return Load(imageType);
        }

        private ImageData Load(EImageType imageType)
        {
            ImageData imageData = default;
            imageData.imageType = imageType;
            imageData.sprite = CoreResource.Load<Sprite>(imageType.ToString());
            imagesList.Add(imageData);
            return imageData;
        }

        private IEnumerator LoadAsync(EImageType imageType)
        {
            ImageData imageData = default;
            imageData.imageType = imageType;
            yield return CoreResource.LoadAsync<Sprite>(imageType.ToString(), LoadOverImage);
            void LoadOverImage(Sprite sprite)
            {
                imageData.sprite = sprite;
                imagesList.Add(imageData);
                imageIndex = imagesList.Count - 1;
            }
        }

        private void ReLoadImage()
        {
            foreach (ImageResLoadd item in imageResLoaddsList)
                item.ReloadImage();
        }
    }

    public interface ImageResLoadd
    {
        public void ReloadImage();
    }

    public struct ImageData
    {
        public EImageType imageType;
        public Sprite sprite;
    }

    public enum EImageType
    {
        None,
    }
}
