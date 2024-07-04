using System;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

namespace ToolEditor
{
    public class ConfigToolUI : EditorWindow
    {
        private bool _isOpenPreview = true;
        private static readonly string CommonPath = $"{Application.dataPath}\\";
        private string _content;
        private readonly string _loadPath = $"{CommonPath}/Resources/";
        private const string CreatPathKey = "保存配置文件路径";
        private static string _creatPath = string.Empty;


        [MenuItem("Tools/生成配置文件#C #C")]
        public static void ShowConfigToolUI()
        {
            if (!EditorWindow.HasOpenInstances<ConfigToolUI>())
            {
                GetWindow(typeof(ConfigToolUI), false, "生成配置文件").Show();
                _creatPath = LoadPath(CreatPathKey);
            }
            else
                GetWindow(typeof(ConfigToolUI)).Close();
        }

        /// <summary>
        /// 打开文件夹路径
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void OpenFolder()
        {
            if (!Directory.Exists(_creatPath))
                throw new Exception("当前文件夹路径错误");
            Process.Start(_creatPath);
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void SavePath()
        {
            
        }

        private static string LoadPath( string key) => PlayerPrefs.GetString(key);
        
        
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            var savePath = Application.dataPath.Replace("Assets", string.Empty);
            if (GUILayout.Button("打开文件夹", GUILayout.Width(100f)))
                OpenFolder();
            if (GUILayout.Button("保存C#生成路径", GUILayout.Width(200f)))
            {
                PlayerPrefs.SetString(CreatPathKey, $"{savePath}{_creatPath}");
                _creatPath = PlayerPrefs.GetString(CreatPathKey);
                GUIUtility.keyboardControl = 0;
            }
            _creatPath = EditorGUILayout.TextField(_creatPath);
            EditorGUILayout.EndHorizontal();

            Repaint();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成Prefab数据预览(全路径)"))
            {
                _content = GenerateConfigTool.ReadDataString(_loadPath, "ConfigPrefab", DataReadType.AllPathNoSuffix, ".prefab");
            }
            if (GUILayout.Button("生成Prefab数据文件(全路径)"))
            {
                _content = GenerateConfigTool.ReadDataString(_loadPath, "ConfigPrefab", DataReadType.AllPathNoSuffix, ".prefab");
                string creatFilePath = $"{_creatPath}/ConfigPrefab.cs";
                GenerateConfigTool.WriteData(_content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("生成Material数据预览(全路径)"))
            {
                string LoadPath = $"{CommonPath}/Resources/";
                _content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigMaterial", DataReadType.AllPathSuffixation, ".mat");
            }
            if (GUILayout.Button("生成Material数据文件(全路径)"))
            {
                string LoadPath = $"{CommonPath}/Resources/";
                _content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigMaterial", DataReadType.AllPathSuffixation, ".mat");
                string creatFilePath = $"{_creatPath}/ConfigMaterial.cs"; 
                GenerateConfigTool.WriteData(_content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成Tag数据预览"))
            {
                _content = GenerateConfigTool.ReadTagData();
            }
            if (GUILayout.Button("生成Tag数据文件"))
            {
                _content = GenerateConfigTool.ReadTagData();
                string creatFilePath = $"{_creatPath}/ConfigTag.cs";
                GenerateConfigTool.WriteData(_content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成Layer数据预览"))
            {
                _content = GenerateConfigTool.ReadLayerData();
            }
            if (GUILayout.Button("生成Layer数据文件"))
            {
                _content = GenerateConfigTool.ReadLayerData();
                string creatFilePath = $"{_creatPath}/ConfigLayer.cs";
                GenerateConfigTool.WriteData(_content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成SortingLayer数据预览"))
            {
                _content = GenerateConfigTool.ReadSortingLayerData();
            }
            if (GUILayout.Button("生成SortingLayer数据文件"))
            {
                _content = GenerateConfigTool.ReadSortingLayerData();
                string creatFilePath = $"{_creatPath}/ConfigSortingLayer.cs";
                GenerateConfigTool.WriteData(_content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成Scenes数据预览"))
            {
                string LoadPath = $"{CommonPath}/";
                _content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigScenes", DataReadType.CommonNoSuffix, ".unity");
            }
            if (GUILayout.Button("生成Scenes数据文件"))
            {
                string LoadPath = $"{CommonPath}/";
                _content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigScenes", DataReadType.CommonNoSuffix, ".unity");
                string creatFilePath = $"{_creatPath}/ConfigScenes.cs";
                GenerateConfigTool.WriteData(_content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成Audio数据预览"))
            {
                string LoadPath = $"{CommonPath}/Resources/";
                _content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigAudio", DataReadType.AllPathNoSuffix, ".mp3", ".wav");
            }
            if (GUILayout.Button("生成Audio数据文件"))
            {
                string LoadPath = $"{CommonPath}/Resources/";
                _content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigAudio", DataReadType.AllPathNoSuffix, ".mp3", ".wav");
                string creatFilePath = $"{_creatPath}/ConfigAudio.cs";
                GenerateConfigTool.WriteData(_content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成Data数据预览"))
            {
                string LoadPath = $"{CommonPath}/Resources/";
                _content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigData", DataReadType.AllPathNoSuffix, ".bytes");
            }
            if (GUILayout.Button("生成Data数据预览"))
            {
                string LoadPath = $"{CommonPath}/Resources/";
                _content = GenerateConfigTool.ReadDataString(LoadPath, "ConfigData", DataReadType.AllPathNoSuffix, ".bytes");
                string creatFilePath = $"{_creatPath}/ConfigData.cs";
                GenerateConfigTool.WriteData(_content, creatFilePath);
            }
            EditorGUILayout.EndHorizontal();


            GUILayout.Space(5f);
            EditorGUILayout.LabelField("预览", EditorStyles.label);
            _isOpenPreview = EditorGUILayout.ToggleLeft("是否开启预览", _isOpenPreview, GUILayout.Width(130f));
            if (_isOpenPreview && !string.IsNullOrEmpty(_content))
                EditorGUILayout.TextArea(_content);
        }
    }
}
