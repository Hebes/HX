using ACEditor;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;//Object并非C#基础中的Object，而是 UnityEngine.Object

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    修改UIComponent样式

    SerializedProperty  以完全通用的方式编辑对象上的属性（可自动处理撤销），同时还能调整预制件的 UI 样式
    https://docs.unity.cn/cn/2021.1/ScriptReference/SerializedProperty.html
    EditorGUILayout  EditorGUI 的自动布局版本
    https://docs.unity.cn/cn/2021.1/ScriptReference/EditorGUILayout.html
    SerializedObject     以完全通用的方式编辑 Unity 对象上的可序列化字段。
    https://docs.unity.cn/cn/2021.1/ScriptReference/SerializedObject.html

-----------------------*/

namespace Core
{
    /// <summary> 自定义ReferenceCollector类在界面中的显示与功能 </summary>
    [CustomEditor(typeof(UIComponent), true)]
    public class UIComponentEditor : Editor
    {
        private string Prefix = "T_";
        private string _searchKey = "";
        private Object heroPrefab;

        /// <summary> 输入在textfield中的字符串 </summary>
        private string searchKey
        {
            get
            {
                return _searchKey;
            }
            set
            {
                if (_searchKey != value)
                {
                    _searchKey = value;
                    heroPrefab = aCManager.Get<Object>(searchKey);
                }
            }
        }
        private UIComponent aCManager { get; set; }

        /// <summary> 组件列表 </summary>
        public List<string> components = new List<string>()
        {
             typeof(Button).Name,
             typeof(Text).Name,
             typeof(Image).Name,
             typeof(GameObject).Name,
             typeof(InputField).Name,
        };


        private void OnEnable()
        {
            aCManager = (UIComponent)target;
        }

        private void OnDestroy()
        {

        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("改前缀", EditorStyles.boldLabel);
            if (GUILayout.Button($"改前缀"))
            {
                UIComponentTool.ShowUIComponentTool();
            }
            EditorGUILayout.LabelField("按钮", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button($"获取{Prefix}物体"))
                OoGetKeyGos();
            if (GUILayout.Button("保存"))
            {
                foreach (UIData item in aCManager.dataList)
                {
                    Undo.RecordObject(item.gameObject, item.key);
                    EditorUtility.SetDirty(item.gameObject);
                }
            }
            if (GUILayout.Button("去除空格"))
                OnDelTrim();
            if (GUILayout.Button("删除全部"))
                aCManager.dataList.Clear();
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


            EditorGUILayout.LabelField("内容", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            searchKey = EditorGUILayout.TextField(searchKey);
            heroPrefab = EditorGUILayout.ObjectField(heroPrefab, typeof(Object), false);
            if (GUILayout.Button("删除"))
            {
                heroPrefab = null;
            }
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();



            EditorGUILayout.Space(10);
            EditorGUILayout.BeginVertical();
            for (int i = 0; i < aCManager.dataList?.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                UIData tempData = aCManager.dataList[i];
                tempData.key = EditorGUILayout.TextField(tempData.key, GUILayout.Width(200));
                tempData.gameObject = EditorGUILayout.ObjectField(tempData.gameObject, typeof(Object), true);
                if (GUILayout.Button("删除"))
                    aCManager.dataList.Remove(tempData);
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
                    DragAndDrop.AcceptDrag();//接受拖动操作。
                    foreach (var o in DragAndDrop.objectReferences)
                    {
                        UIData uIData = new UIData();
                        uIData.key = o.name;
                        uIData.gameObject = o;
                        aCManager.dataList.Add(uIData);
                    }
                }
                Event.current.Use();
            }

        }


        /// <summary>
        /// 去除空白
        /// </summary>
        /// <param name="dataProperty"></param>
        private void OnDelTrim()
        {
            aCManager.dataList?.ForEach(data =>
            {
                data.key = data.gameObject.name = data.gameObject.name.Trim().Replace(" ", "");
            });
        }


        /// <summary>
        /// 获取关键字开头的物体
        /// </summary>
        private void OoGetKeyGos()
        {
            //获取物体
            aCManager.dataList.Clear();
            List<GameObject> gameObjects = new List<GameObject>();
            ACLoopGetKeywordGO(aCManager.gameObject.transform, Prefix, ref gameObjects);

            for (int i = 0; i < gameObjects?.Count; i++)
            {
                UIData uIData = new UIData();
                uIData.key = gameObjects[i].name;
                uIData.gameObject = gameObjects[i];
                aCManager.dataList.Add(uIData);
            }
        }

        /// <summary>
        /// 查找物体(PS：包含隐藏版、关键词开头,Hierarchy面板)
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="keyValue"></param>
        /// <param name="goList"></param>
        public void ACLoopGetKeywordGO(Transform transform, string keyValue, ref List<GameObject> goList)
        {
            if (transform.name.StartsWith(keyValue))
                goList.Add(transform.gameObject);
            for (int i = 0; i < transform?.childCount; i++)
                ACLoopGetKeywordGO(transform.GetChild(i), keyValue, ref goList);
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
        /// <param name="dataProperty"></param>
        /// <param name="eTUITool_ClassName"></param>
        private void GetValue()
        {
            StringBuilder sb = new StringBuilder();
            foreach (UIData item in aCManager.dataList)
            {
                sb.AppendLine($"public {item.gameObject.GetType().Name} {item.key};");// {{get;set;}}
            }
            UnityEngine.Debug.Log(sb.ToString());
            Copy(sb.ToString());
        }

        /// <summary>
        /// 获取脚本
        /// </summary>
        private void GetCode()
        {
            StringBuilder sb = new StringBuilder();

            foreach (UIData item in aCManager.dataList)
            {
                item.gameObject.name = item.gameObject.name.Trim().Replace("-", "_").
                        Replace(" ", "").
                        Replace("\"", "");
                sb.AppendLine($"{item.gameObject.name} = UIComponent.Get<{item.gameObject.GetType().Name}>(\"{item.gameObject.name}\");");
            }
            UnityEngine.Debug.Log(sb.ToString());
            Copy(sb.ToString());
        }


        //其他
        /// <summary>
        /// 反射命名空间
        /// </summary>
        /// <param name="className"></param>
        /// <param name="namespaceName"></param>
        /// <returns></returns>
        private Type ACReflectClass(string className, string namespaceName = "UnityEngine.UI")
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
            TextEditor te = new TextEditor();
            te.text = content.ToString();
            te.SelectAll();
            te.Copy();
        }
    }
}