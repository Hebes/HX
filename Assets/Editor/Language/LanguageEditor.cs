using UnityEditor;
using UnityEngine.UI;

namespace CustomEditorExpansion
{
    [CustomEditor(typeof(LanguageComponent), true)]
    public class LanguageEditor : Editor
    {
        private LanguageComponent _languageText;

        private void OnEnable()
        {
            _languageText = (LanguageComponent)target;
            var text = _languageText.GetComponent<Text>();
            _languageText.key = text.text;
        }

        //public override void OnInspectorGUI()
        //{
        //    EditorGUILayout.LabelField("按钮", EditorStyles.boldLabel);
        //    if(GUILayout.Button("获取文字"))
        //    {
        //        Text _text = _languageText.GetComponent<Text>();
        //        _languageText.key = _text.text;
        //    }
        //}
    }
}