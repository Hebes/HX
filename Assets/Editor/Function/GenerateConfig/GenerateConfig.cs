using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Tool;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class GenerateConfig : EditorWindow
{
    [MenuItem("Tools/生成配置文件")]
    public static void ShowConfigToolUI()
    {
        if (!EditorWindow.HasOpenInstances<GenerateConfig>())
            GetWindow(typeof(GenerateConfig), false, "生成配置文件").Show();
        else
            GetWindow(typeof(GenerateConfig)).Close();
    }

    #region 存读

    private void OnEnable() => Load();
    private void OnDisable() => Save();

    private void Save()
    {
        if (!EditorWindow.HasOpenInstances<GenerateConfig>()) return;
        Type type = GetType();
        var fieldsValue = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                                         BindingFlags.NonPublic);
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
        var fieldsValue = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                                         BindingFlags.NonPublic);
        foreach (var data in fieldsValue)
        {
            if (data.FieldType == typeof(string))
                data.SetValue(this, PlayerPrefs.GetString($"{Application.productName}{data.Name}Save"));
            if (data.FieldType == typeof(int))
                data.SetValue(this, PlayerPrefs.GetInt($"{Application.productName}{data.Name}Save"));
        }
    }

    #endregion

    private string _prefabPath;
    private string _scenePath;
    private string _content;

    private void OnGUI()
    {
        Repaint();
        GUILayout.Space(10f);
        _prefabPath = Template1("Prefab数据", 100f, GeneratePrefab, "Prefab数据路径", _prefabPath);
        _scenePath = Template1("Scene数据", 100f, GenerateScene, "Scene数据路径", _scenePath);

        GUILayout.Space(10f);
        if (GUILayout.Button("Tag数据"))
        {
            string[] tags = InternalEditorUtility.tags;
            List<(string, string)> strList = new List<(string, string)>();
            foreach (var t in tags)
                strList.Add((t, t));
            GetConfig("ConfigTag", strList);
        }

        if (GUILayout.Button("Layer数据"))
        {
            string[] layers = InternalEditorUtility.layers;
            List<(string, string)> strList = new List<(string, string)>();
            foreach (var t in layers)
                strList.Add((t, t));
            GetConfig("ConfigLayer", strList);
        }

        if (GUILayout.Button("sortingLayer数据"))
        {
            Type internalEditorUtilityType = typeof(InternalEditorUtility);
            PropertyInfo sortingLayersProperty =
                internalEditorUtilityType.GetProperty("sortingLayerNames",
                    BindingFlags.Static | BindingFlags.NonPublic);
            string[] sortingLayers = (string[])sortingLayersProperty.GetValue(null, new object[0]);

            List<(string, string)> strList = new List<(string, string)>();
            foreach (var t in sortingLayers)
                strList.Add((t, t));
            GetConfig("ConfigSortingLayer", strList);
        }

        GUILayout.Space(10f);
        EditorGUILayout.LabelField("预览", EditorStyles.label);
        if (!string.IsNullOrEmpty(_content))
            EditorGUILayout.TextArea(_content);
    }


    /// <summary>
    /// 模板1
    /// </summary>
    /// <param name="btnName"></param>
    /// <param name="btnWidth"></param>
    /// <param name="btnAction"></param>
    /// <param name="labelText"></param>
    /// <param name="textFieldText"></param>
    /// <returns></returns>
    private string Template1(string btnName, float btnWidth, Action btnAction, string labelText, string textFieldText)
    {
        float labelWidth = EditorStyles.miniLabel.CalcSize(new GUIContent(labelText)).x;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(btnName, GUILayout.Width(btnWidth))) btnAction?.Invoke();
        EditorGUILayout.LabelField(labelText, EditorStyles.miniLabel, GUILayout.Width(labelWidth));
        var textFieldTextTemp = EditorGUILayout.TextField(textFieldText); //GUILayout.Width(textFieldWidth)
        EditorGUILayout.EndHorizontal();
        return textFieldTextTemp;
    }

    private void GeneratePrefab()
    {
        string[] strings = Directory.GetFiles(_prefabPath, $"*.prefab", SearchOption.AllDirectories);
        List<(string, string)> strList = new List<(string, string)>();
        foreach (var s in strings)
        {
            string fileName = Path.GetFileNameWithoutExtension(s);
            strList.Add((fileName, fileName));
        }

        GetConfig("ConfigPrefab", strList);
    }

    private void GenerateScene()
    {
        string[] strings = Directory.GetFiles(_scenePath, $"*.unity", SearchOption.AllDirectories);
        List<(string, string)> strList = new List<(string, string)>();
        foreach (var s in strings)
        {
            string fileName = Path.GetFileNameWithoutExtension(s);
            strList.Add((fileName, fileName));
        }

        GetConfig("ConfigScene", strList);
    }

    private void GetConfig(string className, List<(string, string)> contentListValue)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"public class {className}");
        sb.AppendLine("{");
        foreach (var c in contentListValue)
            sb.AppendLine($"\tpublic const string {ReplaceStr(c.Item1)} = \"{c.Item2}\";");
        sb.AppendLine("}");
        sb.ToString().Copy();
        _content = sb.ToString();
    }

    private string ReplaceStr(string str)
    {
        return str.Replace(" ",String.Empty);
    }
}