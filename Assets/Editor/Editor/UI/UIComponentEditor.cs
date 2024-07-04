using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Framework.Core;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object; //Object并非C#基础中的Object，而是 UnityEngine.Object

/*--------脚本描述-----------

描述:
    修改UIComponent样式

    SerializedProperty  以完全通用的方式编辑对象上的属性（可自动处理撤销），同时还能调整预制件的 UI 样式
    https://docs.unity.cn/cn/2021.1/ScriptReference/SerializedProperty.html
    EditorGUILayout  EditorGUI 的自动布局版本
    https://docs.unity.cn/cn/2021.1/ScriptReference/EditorGUILayout.html
    SerializedObject     以完全通用的方式编辑 Unity 对象上的可序列化字段。
    https://docs.unity.cn/cn/2021.1/ScriptReference/SerializedObject.html

-----------------------*/

namespace ToolEditor
{
    /// <summary> 自定义ReferenceCollector类在界面中的显示与功能 </summary>
    [CustomEditor(typeof(UIComponent), true)]
    public class UIComponentEditor : Editor
    {
        private readonly string _prefix = "T_";
        private string _searchKey = string.Empty;
        private Object _heroPrefab;
        private UIComponent UI { get; set; }

        private string SearchKey
        {
            get => _searchKey;
            set
            {
                if (_searchKey == value) return;
                _searchKey = value;
                heroPrefab = aCManager.Get(searchKey);
            }
        }

        /// <summary> 组件列表 </summary>
        public List<string> components = new List<string>()
        {
             nameof(Button),
             nameof(Text),
             nameof(Image),
             nameof(GameObject),
             nameof(InputField),
        };

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("改前缀", EditorStyles.boldLabel);
            if (GUILayout.Button($"改前缀"))
                UIComponentTool.ShowUIComponentTool();
            EditorGUILayout.LabelField("按钮", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button($"获取{_prefix}物体"))
                OoGetKeyGos();
            if (GUILayout.Button("保存"))
            {
                foreach (var item in UI.dataList)
                {
                    Undo.RecordObject(item.gameObject, item.key);
                    EditorUtility.SetDirty(item.gameObject);
                }
            }

            if (GUILayout.Button("去除空格"))
                OnDelTrim();
            if (GUILayout.Button("删除全部"))
                UI.dataList.Clear();
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.LabelField("获取代码", EditorStyles.boldLabel);
            if (GUILayout.Button("获取脚本必要代码"))
                OnGetEssentialCode();


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("变量获取"))
                GetValue();
            if (GUILayout.Button("组件获取"))
                GetCode();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("添加图片组件"))
                AddComponent<Image, ImageComponent>();
            if (GUILayout.Button("添加文字组件"))
                AddComponent<Text, LanguageComponent>();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("内容", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            SearchKey = EditorGUILayout.TextField(SearchKey);
            _heroPrefab = EditorGUILayout.ObjectField(_heroPrefab, typeof(Object), false);
            if (GUILayout.Button("删除"))
                _heroPrefab = null;
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.Space(10);
            EditorGUILayout.BeginVertical();
            for (int i = 0; i < UI.dataList?.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                UIData tempData = UI.dataList[i];
                tempData.key = EditorGUILayout.TextField(tempData.key, GUILayout.Width(200));
                tempData.gameObject = EditorGUILayout.ObjectField(tempData.gameObject, typeof(Object), true);
                if (GUILayout.Button("删除"))
                    UI.dataList.Remove(tempData);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();


            EditorGUILayout.Space(10);
            EventType eventType = Event.current.type;
            //在Inspector 窗口上创建区域，向区域拖拽资源对象，获取到拖拽到区域的对象
            if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (eventType == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag(); //接受拖动操作。
                    foreach (var o in DragAndDrop.objectReferences)
                    {
                        var uIData = new UIData
                        {
                            key = o.name,
                            gameObject = o
                        };
                        UI.dataList?.Add(uIData);
                    }
                }

                Event.current.Use();
            }
        }
        
        /// <summary>
        /// 去除空白
        /// </summary>
        private void OnDelTrim()
        {
            foreach (var uiData in UI.dataList)
                uiData.key = uiData.gameObject.name = uiData.gameObject.name.Trim().Replace(" ", "");
        }

        /// <summary>
        /// 获取关键字开头的物体
        /// </summary>
        private void OoGetKeyGos()
        {
            //获取物体
            UI.dataList.Clear();
            List<GameObject> gameObjects = new List<GameObject>();
            GetKeywordGo(UI.gameObject.transform, _prefix, ref gameObjects);

            for (var i = 0; i < gameObjects?.Count; i++)
            {
                var uIData = new UIData
                {
                    key = gameObjects[i].name,
                    gameObject = gameObjects[i]
                };
                UI.dataList.Add(uIData);
            }
        }

        /// <summary>
        /// 查找物体(PS：包含隐藏版、关键词开头,Hierarchy面板)
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="keyValue"></param>
        /// <param name="goList"></param>
        private void GetKeywordGo(Transform transform, string keyValue, ref List<GameObject> goList)
        {
            if (transform.name.StartsWith(keyValue))
                goList.Add(transform.gameObject);
            for (var i = 0; i < transform?.childCount; i++)
                GetKeywordGo(transform.GetChild(i), keyValue, ref goList);
        }

        private void GetKeywordGo<T>(Transform transform, ref List<T> tList) where T : Component
        {
            var t = transform.GetComponent<T>();
            if (t) tList.Add(t);
            for (var i = 0; i < transform.childCount; i++)
                GetKeywordGo<T>(transform.GetChild(i), ref tList);
        }

        /// <summary>
        /// 获取脚本必要代码
        /// </summary>
        private void OnGetEssentialCode()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"UIComponent UIComponent = GetComponent<UIComponent>();");
            Copy(sb.ToString());
        }

        /// <summary>
        /// 获取变量
        /// </summary>
        private void GetValue()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in UI.dataList)
                sb.AppendLine($"public {item.gameObject.GetType().Name} {item.key};"); // {{get;set;}}
            UnityEngine.Debug.Log(sb.ToString());
            Copy(sb.ToString());
        }

        /// <summary>
        /// 添加图片组件
        /// </summary>
        private void AddComponent<T, TK>() where T : Component where TK : Component
        {
            var tList = new List<T>();
            GetKeywordGo(UI.transform, ref tList);
            foreach (var t in tList)
                DestroyImmediate(t.GetComponent<TK>());
            foreach (var t in tList)
                t.AddComponent<TK>();
        }

        /// <summary>
        /// 获取脚本
        /// </summary>
        private void GetCode()
        {
            StringBuilder sb = new StringBuilder();

            foreach (UIData item in UI.dataList)
            {
                item.gameObject.name = item.gameObject.name.Trim().Replace("-", "_").Replace(" ", "").Replace("\"", "");
                sb.AppendLine($"{item.gameObject.name} = UIComponent.Get<{item.gameObject.GetType().Name}>(\"{item.gameObject.name}\");");
            }

            UnityEngine.Debug.Log(sb.ToString());
            Copy(sb.ToString());
        }

        /// <summary>
        /// 反射命名空间
        /// </summary>
        /// <param name="className"></param>
        /// <param name="namespaceName"></param>
        /// <returns></returns>
        private Type ReflectClass(string className, string namespaceName = "UnityEngine.UI")
        {
            Assembly assem = Assembly.Load(namespaceName);
            Type type = assem.GetType($"{namespaceName}.{className}");
            return type;
        }

        /// <summary>
        /// /复制
        /// </summary>
        /// <param name="content"></param>
        private void Copy(string content)
        {
            UnityEngine.TextEditor te = new UnityEngine.TextEditor();
            te.text = content.ToString();
            te.SelectAll();
            te.Copy();
        }
    }
}