﻿using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace CustomEditorExpansion
{
    /// <summary>
    /// 多语言工具
    /// </summary>
    [CustomEditor(typeof(LanguageComponent), true)]
    public class EditorLanguage : Editor
    {
        private LanguageComponent _languageText;

        private void OnEnable()
        {
            _languageText = (LanguageComponent)target;
            _languageText.text = _languageText.GetComponent<Text>();
            _languageText.key = _languageText.GetComponent<Text>().text;
        }

        public override void OnInspectorGUI()
        {
            base.DrawDefaultInspector();
            if (Application.isPlaying) return;
            GUILayout.Label("Key", EditorStyles.boldLabel);
            _languageText.key = EditorGUILayout.TextArea(string.IsNullOrEmpty(_languageText.text.text) ? string.Empty : _languageText.text.text, GUILayout.Height(40));
        }
    }
}