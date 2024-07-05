using Framework.Core;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 图片组件
/// </summary>
[RequireComponent(typeof(Image))]
public class ImageComponent : MonoBehaviour
{ 
    public Image image;
    public string key;

    private void Awake() => CoreImage.Add(this);

    public void Refresh() => image.sprite = CoreImage.GetSprite(key);
}