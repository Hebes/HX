using UnityEditor;
using UnityEngine.UI;

namespace CustomEditorExpansion
{
    /// <summary>
    /// 图片工具
    /// </summary>
    [CustomEditor(typeof(ImageComponent), true)]
    public class EditorImage : Editor
    {
        private void OnEnable()
        {
            var imageComponent = (ImageComponent)target;
            var nameValue = imageComponent.name.Replace("T_", string.Empty);
            imageComponent.key = nameValue;
            imageComponent.image=imageComponent.GetComponent<Image>();
        }
    }
}