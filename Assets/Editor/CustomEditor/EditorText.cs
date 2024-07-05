﻿using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.UI;

namespace CustomEditorExpansion
{
    [CustomEditor(typeof(Text), true)]
    public class EditorText : Editor
    {
        private void Awake()
        {
            var value = target.GetComponent<LanguageComponent>();
            if (value) return;
            target.AddComponent<LanguageComponent>();
        }
    }
}