using Farm2D;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace ACEditor
{
    [CustomEditor(typeof(LanguageText), true)]
    public class LanguageEditor : Editor
    {
        private LanguageText _languageText;

        private void OnEnable()
        {
            _languageText = (LanguageText)target;
            Text _text = _languageText.GetComponent<Text>();
            _languageText.key = _text.text;
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
