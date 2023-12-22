using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ACEditor
{
    public class Vlue : EditorWindow
    {

        [MenuItem("Tool/生成配置文件#P #P")]
        public static void ShowConfigToolUI()
        {
            if (!EditorWindow.HasOpenInstances<Vlue>())
                GetWindow(typeof(Vlue), false, "查看").Show();
            else
                GetWindow(typeof(Vlue)).Close();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("测试", GUILayout.Width(100f)))
            {
                //Assembly assem = Assembly.GetExecutingAssembly();
                Assembly assem = Assembly.Load("Assembly-CSharp");
                Type type = assem.GetType("Newlifecycle");
                FieldInfo[] t1 = type.GetFields();
                foreach (var item in t1)
                {
                    if (item.GetValue() is List<int> ttt)
                    {
                        foreach (var item1 in ttt)
                        {
                            Debug.Log(item1);
                        }
                    }

                }
            }
        }
    }
}
