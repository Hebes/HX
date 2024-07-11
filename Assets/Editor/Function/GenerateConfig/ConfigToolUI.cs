using System;
using System.Reflection;
using Tool;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditorInternal;
using Debug = UnityEngine.Debug;

namespace ToolEditor
{
    public enum DataReadType
    {
        /// <summary> 普通带后缀 </summary>
        CommonSuffixation,

        /// <summary> 普通不带后缀 </summary>
        CommonNoSuffix,

        /// <summary> 全路径带后缀 </summary>
        AllPathSuffixation,

        /// <summary> 全路径不带后缀 </summary>
        AllPathNoSuffix,
    }

    public class ConfigToolUI : EditorWindow
    {
        [MenuItem("Tools/生成配置文件#C #C")]
        public static void ShowConfigToolUI()
        {
            if (!EditorWindow.HasOpenInstances<ConfigToolUI>())
            {
                GetWindow(typeof(ConfigToolUI), false, "生成配置文件").Show();
            }
            else
                GetWindow(typeof(ConfigToolUI)).Close();
        }

        #region 存读

        private void OnEnable() => Load();
        private void OnDisable() => Save();

        [MenuItem("Tools/保存数据 #S")]
        private void Save()
        {
            if (!EditorWindow.HasOpenInstances<ExcelWriteUI>()) return;
            Type type = GetType();
            var fieldsValue = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var data in fieldsValue)
            {
                if (data.FieldType == typeof(string))
                    PlayerPrefs.SetString($"{Application.productName}{data.Name}Save", (string)data.GetValue(this));
                if (data.FieldType == typeof(int))
                    PlayerPrefs.SetInt($"{Application.productName}{data.Name}Save", (int)data.GetValue(this));
            }

            Debug.LogError("保存成功");
        }

        private void Load()
        {
            Type type = GetType();
            var fieldsValue = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var data in fieldsValue)
            {
                if (data.FieldType == typeof(string))
                    data.SetValue(this, PlayerPrefs.GetString($"{Application.productName}{data.Name}Save"));
                if (data.FieldType == typeof(int))
                    data.SetValue(this, PlayerPrefs.GetInt($"{Application.productName}{data.Name}Save"));
            }
        }

        #endregion

        private string LabelFieldAndTextField(string labelText, string textFieldText, float textFieldWidth)
        {
            float labelWidth = EditorStyles.miniLabel.CalcSize(new GUIContent(labelText)).x;
            EditorGUILayout.LabelField(labelText, EditorStyles.miniLabel, GUILayout.Width(labelWidth));
            return EditorGUILayout.TextField(textFieldText, GUILayout.Width(textFieldWidth));
        }

        private bool _isOpenPreview = true;
        private static readonly string CommonPath = $"{Application.dataPath}\\";
        private string _content;
        private readonly string _loadPath = $"{CommonPath}/Resources/";
        private static string _creatPath = string.Empty;

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("打开文件夹", GUILayout.Width(100f))) ToolExpansion.OpenFolder(_loadPath);
            _creatPath = LabelFieldAndTextField("生成C#路径", _creatPath, 200f);
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
            if (GUILayout.Button("生成Tag数据预览")) _content = GenerateConfigTool.ReadTagData();
            if (GUILayout.Button("生成Tag数据文件"))
            {
                _content = GenerateConfigTool.ReadTagData();
                string creatFilePath = $"{_creatPath}/ConfigTag.cs";
                GenerateConfigTool.WriteData(_content, creatFilePath);
            }

            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成Layer数据预览")) _content = GenerateConfigTool.ReadLayerData();
            if (GUILayout.Button("生成Layer数据文件")) GenerateConfigTool.WriteData(_content, $"{_creatPath}/ConfigLayer.cs");
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("生成SortingLayer数据预览")) _content = GenerateConfigTool.ReadSortingLayerData();
            if (GUILayout.Button("生成SortingLayer数据文件")) GenerateConfigTool.WriteData(_content, $"{_creatPath}/ConfigSortingLayer.cs");
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


    public class GenerateConfigTool : EditorWindow
    {
        private static string namespaceName = "FieldEdge"; //命名空间

        private static string FilterKeyword(string str, params string[] filterSuffix)
        {
            foreach (string key in filterSuffix)
                str = str.Replace(key, "");
            return str;
        }

        public static void WriteData(string content, string creatFilePath)
        {
            //删除原来的文件
            if (File.Exists(creatFilePath))
            {
                Debug.Log("文件存在开始删除!");
                File.Delete(creatFilePath);
                Debug.Log("文件删除成功!");
            }

            File.WriteAllText(creatFilePath, content.ToString());
            Debug.Log("文件写入成功!");
            AssetDatabase.Refresh();
        }

        public static string ReadDataString(string path, string ConfigName, DataReadType dataReadType, params string[] filterSuffix)
        {
            //寻找需要的文件
            List<string> pathsList = new List<string>();
            foreach (string key in filterSuffix)
            {
                string[] strings = Directory.GetFiles(path, $"*{key}", SearchOption.AllDirectories);
                pathsList.AddRange(strings.ToList());
            }

            //拼接字符串
            StringBuilder sb = new StringBuilder();
            //sb.AppendLine($"namespace {namespaceName}\r\n{{");
            //sb.AppendLine($"    public class {ConfigName}\r\n    {{");
            sb.AppendLine($"public class {ConfigName}\r\n{{");
            foreach (string pathTemp in pathsList)
            {
                //文件名称
                string oldFileName = Path.GetFileNameWithoutExtension(pathTemp);
                string fileName = Path.GetFileNameWithoutExtension(pathTemp).Replace("@", "_").Replace("(", "").Replace(")", "").Replace("-", "_").Replace(" ", "");
                //文件路径
                string extendedName = Path.GetExtension(pathTemp); //如不需要请直接添加上去,这个是获取拓展名称
                string assetsPath = pathTemp.Replace(path, "").Replace("\\", "/"); //文件路径
                string extendedNameTemp = FilterKeyword(extendedName, ".");
                switch (dataReadType)
                {
                    case DataReadType.CommonSuffixation:
                        sb.AppendLine($"        public const string {extendedNameTemp}{fileName} = \"{oldFileName}{extendedName}\";");
                        break;
                    case DataReadType.CommonNoSuffix:
                        sb.AppendLine($"        public const string {extendedNameTemp}{fileName} = \"{oldFileName}\";");
                        break;
                    case DataReadType.AllPathSuffixation:
                        sb.AppendLine($"        public const string {extendedNameTemp}{fileName} = \"{assetsPath}\";");
                        break;
                    case DataReadType.AllPathNoSuffix:
                        sb.AppendLine($"        public const string {extendedNameTemp}{fileName} = \"{assetsPath.Replace(extendedName, "")}\";");
                        break;
                }

                //文件路径
            }

            sb.AppendLine("}");
            //sb.AppendLine("    }\r\n}");
            return sb.ToString();
        }

        public static string ReadTagData()
        {
            string[] tags = InternalEditorUtility.tags;
            StringBuilder sb = new StringBuilder();
            //sb.AppendLine("namespace ACFrameworkCore\r\n{");
            //sb.AppendLine("    public class ConfigTag\r\n    {");
            sb.AppendLine("public class ConfigTag\r\n{");
            foreach (string s in tags)
                sb.AppendLine($"        public const string Tag{s} = \"{s}\";");
            sb.AppendLine("}");
            //sb.AppendLine("    }\r\n}");
            return sb.ToString();
        }

        public static string ReadLayerData()
        {
            var tags = InternalEditorUtility.layers;

            StringBuilder sb = new StringBuilder();
            //sb.AppendLine("namespace ACFrameworkCore\r\n{");
            //sb.AppendLine("    public class ConfigLayer\r\n    {");
            sb.AppendLine("public class ConfigLayer\r\n{");

            foreach (string s in tags)
            {
                string tempstr = s;
                sb.AppendLine($"        public const string Layer{tempstr.Replace(" ", "").Trim()} = \"{tempstr}\";");
            }

            sb.AppendLine("}");
            //sb.AppendLine("    }\r\n}");
            return sb.ToString();
        }

        public static string ReadSortingLayerData()
        {
            Type internalEditorUtilityType = typeof(InternalEditorUtility);
            PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            string[] sortingLayers = (string[])sortingLayersProperty.GetValue(null, new object[0]);


            StringBuilder sb = new StringBuilder();
            //sb.AppendLine("namespace ACFrameworkCore\r\n{");
            //sb.AppendLine("    public class ConfigSortingLayer\r\n    {");
            sb.AppendLine("public class ConfigSortingLayer\r\n{");

            foreach (string s in sortingLayers)
            {
                string tempstr = s;
                sb.AppendLine($"        public const string SortingLayer{tempstr.Replace(" ", "").Trim()} = \"{tempstr}\";");
            }

            sb.AppendLine("}");
            //sb.AppendLine("    }\r\n}");
            return sb.ToString();
        }
    }
}