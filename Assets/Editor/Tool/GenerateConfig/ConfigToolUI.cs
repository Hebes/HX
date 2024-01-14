using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ACEditor
{
    public class ConfigToolUI : EditorWindow
    {
        private bool isOpenPreview = true;
        private static string CommonPath = $"{Application.dataPath}\\";
        private string content;
        private string LoadPath = $"{CommonPath}/Resources/";

        private string _creatPathKey = "保存配置文件路径";
        private string _creatPath = string.Empty;


        [MenuItem("Assets/生成配置文件#C #C")]
        [MenuItem("Tool/生成配置文件#C #C")]
        [MenuItem("GameObject/生成配置文件#C #C")]
        public static void ShowConfigToolUI()
        {
            if (!EditorWindow.HasOpenInstances<ConfigToolUI>())
                GetWindow(typeof(ConfigToolUI), false, "生成配置文件").Show();
            else
                GetWindow(typeof(ConfigToolUI)).Close();
        }
        private void Awake()
        {
            _creatPath = PlayerPrefs.GetString(_creatPathKey);
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            string _savePath = Application.dataPath.Replace("Assets", string.Empty);
            if (GUILayout.Button("打开文件夹", GUILayout.Width(100f)))
            {
                Process.Start(_creatPath);
            }
            if (GUILayout.Button("保存C#生成路径", GUILayout.Width(200f)))
            {
                PlayerPrefs.SetString(_creatPathKey, $"{_savePath}{_creatPath}");
                _creatPath = PlayerPrefs.GetString(_creatPathKey);
                GUIUtility.keyboardControl = 0;
            }
            _creatPath = EditorGUILayout.TextField(_creatPath);
            EditorGUILayout.EndHorizontal();

            Repaint();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成Prefab数据预览(全路径)"))
            {
                content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigPrefab", DataReadType.AllPathNoSuffix, ".prefab");
            }
            if (GUILayout.Button("生成Prefab数据文件(全路径)"))
            {
                content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigPrefab", DataReadType.AllPathNoSuffix, ".prefab");
                string creatFilePath = $"{_creatPath}/ConfigPrefab.cs";
                GenerateConfigTool.WriteData(content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("生成Material数据预览(全路径)"))
            {
                string LoadPath = $"{CommonPath}/Resources/";
                content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigMaterial", DataReadType.AllPathSuffixation, ".mat");
            }
            if (GUILayout.Button("生成Material数据文件(全路径)"))
            {
                string LoadPath = $"{CommonPath}/Resources/";
                content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigMaterial", DataReadType.AllPathSuffixation, ".mat");
                string creatFilePath = $"{_creatPath}/ConfigMaterial.cs"; 
                GenerateConfigTool.WriteData(content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成Tag数据预览"))
            {
                content = GenerateConfigTool.ReadTagData();
            }
            if (GUILayout.Button("生成Tag数据文件"))
            {
                content = GenerateConfigTool.ReadTagData();
                string creatFilePath = $"{_creatPath}/ConfigTag.cs";
                GenerateConfigTool.WriteData(content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成Layer数据预览"))
            {
                content = GenerateConfigTool.ReadLayerData();
            }
            if (GUILayout.Button("生成Layer数据文件"))
            {
                content = GenerateConfigTool.ReadLayerData();
                string creatFilePath = $"{_creatPath}/ConfigLayer.cs";
                GenerateConfigTool.WriteData(content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成SortingLayer数据预览"))
            {
                content = GenerateConfigTool.ReadSortingLayerData();
            }
            if (GUILayout.Button("生成SortingLayer数据文件"))
            {
                content = GenerateConfigTool.ReadSortingLayerData();
                string creatFilePath = $"{_creatPath}/ConfigSortingLayer.cs";
                GenerateConfigTool.WriteData(content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成Scenes数据预览"))
            {
                string LoadPath = $"{CommonPath}/";
                content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigScenes", DataReadType.CommonNoSuffix, ".unity");
            }
            if (GUILayout.Button("生成Scenes数据文件"))
            {
                string LoadPath = $"{CommonPath}/";
                content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigScenes", DataReadType.CommonNoSuffix, ".unity");
                string creatFilePath = $"{_creatPath}/ConfigScenes.cs";
                GenerateConfigTool.WriteData(content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成Audio数据预览"))
            {
                string LoadPath = $"{CommonPath}/Resources/";
                content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigAudio", DataReadType.AllPathNoSuffix, ".mp3", ".wav");
            }
            if (GUILayout.Button("生成Audio数据文件"))
            {
                string LoadPath = $"{CommonPath}/Resources/";
                content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigAudio", DataReadType.AllPathNoSuffix, ".mp3", ".wav");
                string creatFilePath = $"{_creatPath}/ConfigAudio.cs";
                GenerateConfigTool.WriteData(content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成Data数据预览"))
            {
                string LoadPath = $"{CommonPath}/Resources/";
                content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigData", DataReadType.AllPathNoSuffix, ".bytes");
            }
            if (GUILayout.Button("生成Data数据预览"))
            {
                string LoadPath = $"{CommonPath}/Resources/";
                content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigData", DataReadType.AllPathNoSuffix, ".bytes");
                string creatFilePath = $"{_creatPath}/ConfigData.cs";
                GenerateConfigTool.WriteData(content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();


            GUILayout.Space(5f);
            EditorGUILayout.LabelField("预览", EditorStyles.label);
            isOpenPreview = EditorGUILayout.ToggleLeft("是否开启预览", isOpenPreview, GUILayout.Width(130f));
            if (isOpenPreview && !string.IsNullOrEmpty(content))
                EditorGUILayout.TextArea(content);
        }
    }
}
