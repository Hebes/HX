using Tool;
using UnityEditor;
using UnityEngine;

namespace ToolEditor
{
    public class UIComponentTool : EditorWindow
    {
        private string _prefix1 = "T_";

        public static void ShowUIComponentTool()
        {
            if (!EditorWindow.HasOpenInstances<UIComponentTool>())
                GetWindow(typeof(UIComponentTool), false, "Excel数据填充").Show();
            else
                GetWindow(typeof(UIComponentTool)).Close();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(5f); EditorGUILayout.LabelField("Hierarchy前缀工具", EditorStyles.boldLabel);
            EditorGUILayout.Space(5f); EditorGUILayout.LabelField("请输入组件查找前缀:", EditorStyles.largeLabel);


            EditorGUILayout.BeginHorizontal();
            {
                _prefix1 = EditorGUILayout.TextField("请输入组件查找前缀", _prefix1);
                if (GUILayout.Button("复制", EditorStyles.miniButtonMid))
                    _prefix1.ACCopyWord();
                if (GUILayout.Button("保存修改", EditorStyles.miniButtonMid))
                    ToolExpansion_Find.ACGetObjs().ACSaveModification();
                if (GUILayout.Button("清除", EditorStyles.miniButtonMid))
                    _prefix1 = string.Empty;
            }
            EditorGUILayout.EndHorizontal();



            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("获取前缀", EditorStyles.miniButtonMid))
                    _prefix1 = ToolExpansion_Find.ACGetObj().ACGetPrefix();
                if (GUILayout.Button("前缀添加", EditorStyles.miniButtonMid))
                    ToolExpansion_Find.ACGetObjs().ACChangePrefixLoop(_prefix1);
                if (GUILayout.Button("去除前缀", EditorStyles.miniButtonMid))
                    ToolExpansion_Find.ACGetObjs().ACChangePrefixLoop(_prefix1, false);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
