using OfficeOpenXml;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ACEditor
{
    /// <summary>
    /// https://blog.csdn.net/Xz616/article/details/128893023
    /// https://www.cnblogs.com/noteswiki/p/6095868.html
    /// https://blog.csdn.net/shuaiLS/article/details/107369443
    /// </summary>
    public class ExcelWriteUI : EditorWindow
    {

        public static string NewLoadExcelPath = string.Empty;   //新路径
        public static string OldLoadExcelPath = string.Empty;   //老路径
        private bool isSelectFile = true;                       //点击加载路径
        private Vector2 scrollPosition = Vector2.zero;          //滑动
        private Vector2 scrollExcel = Vector2.zero;             //滑动
        string[][] data = null;                                 //数据
        private string Message = string.Empty;                  //消息提示
        private Dictionary<string, string> DesDic;              //Excel描述信息

        private string _excelFolderPath;                        //Excel存放路径
        private string _excelFolderPathKey = "Excel存放路径Key";//Excel存放路径Key

        [SerializeField, Header("滑动调节"), Tooltip("滑动调节值"), Range(50f, 100f)]
        public float slideBox = 100f;

        private float _deleteButSice = 40f;

        private void Awake()
        {
            //读取说明文档
            DesDic = new Dictionary<string, string>()
            {
                { "ExcelDataItem.xlsx","建造数据"},
            };

            //显示保存路径
            BinaryData.binaryDataSavePath = PlayerPrefs.GetString(BinaryData.binaryDataSavePathKey);
            ClassData.CSharpSavePath = PlayerPrefs.GetString(ClassData.CSharpSavePathKey);
            _excelFolderPath = PlayerPrefs.GetString(_excelFolderPathKey);
        }

        [MenuItem("Tool/编辑Excel#E #E")]
        public static void BuildPackageVersions()
        {
            if (!EditorWindow.HasOpenInstances<ExcelWriteUI>())
                GetWindow(typeof(ExcelWriteUI), false, "Excel数据填充").Show();
            else
                GetWindow(typeof(ExcelWriteUI)).Close();
        }
        private void OnGUI()
        {
            GUILayout.Space(5f);
            EditorGUILayout.LabelField("加载单个Excel文件", EditorStyles.label);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("清空日志", GUILayout.Width(80f))) { ClearConsole(); }
            if (GUILayout.Button("Browse...", GUILayout.Width(80f))) { BrowseLoadFilePanel(); }
            isSelectFile = EditorGUILayout.ToggleLeft("点击加载文件路径", isSelectFile, GUILayout.Width(130f));
            EditorGUILayout.LabelField("选择的Excel文件路径:", EditorStyles.label, GUILayout.Width(130));
            NewLoadExcelPath = EditorGUILayout.TextField(NewLoadExcelPath);
            EditorGUILayout.EndHorizontal();


            GUILayout.Space(5f);
            EditorGUILayout.BeginVertical();
            string _savePath = Application.dataPath.Replace("Assets", string.Empty);
            //二进制文件路径
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("打开文件夹", GUILayout.Width(100f)))
            {
                Process.Start(BinaryData.binaryDataSavePath);
            }
            if (GUILayout.Button("保存二进制数据路径", GUILayout.Width(200f)))
            {
                PlayerPrefs.SetString(BinaryData.binaryDataSavePathKey, $"{_savePath}{BinaryData.binaryDataSavePath}");
                BinaryData.binaryDataSavePath = PlayerPrefs.GetString(BinaryData.binaryDataSavePathKey);
                GUIUtility.keyboardControl = 0;
            }
            BinaryData.binaryDataSavePath = EditorGUILayout.TextField(BinaryData.binaryDataSavePath);
            EditorGUILayout.EndHorizontal();
            //C#文件路径
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("打开文件夹", GUILayout.Width(100f)))
            {
                Process.Start(ClassData.CSharpSavePath);
            }
            if (GUILayout.Button("保存C#文件路径", GUILayout.Width(200f)))
            {
                PlayerPrefs.SetString(ClassData.CSharpSavePathKey, $"{_savePath}{ClassData.CSharpSavePath}");
                ClassData.CSharpSavePath = PlayerPrefs.GetString(ClassData.CSharpSavePathKey);
                GUIUtility.keyboardControl = 0;
            }
            ClassData.CSharpSavePath = EditorGUILayout.TextField(ClassData.CSharpSavePath);
            EditorGUILayout.EndHorizontal();
            //Excel文件夹读取路径
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("打开文件夹", GUILayout.Width(100f)))
            {
                Process.Start(_excelFolderPath);
            }
            if (GUILayout.Button("保存Excel文件夹读取路径", GUILayout.Width(200f)))
            {
                PlayerPrefs.SetString(_excelFolderPathKey, $"{_savePath}{_excelFolderPath}");
                _excelFolderPath = PlayerPrefs.GetString(_excelFolderPathKey);
                GUIUtility.keyboardControl = 0;
            }
            _excelFolderPath = EditorGUILayout.TextField(_excelFolderPath);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();



            slideBox = (int)EditorGUILayout.Slider("滑动调节值", slideBox, 50f, 100f);
            _deleteButSice = (int)EditorGUILayout.Slider("滑动调节值", _deleteButSice, 40f, 80f);



            scrollExcel = GUILayout.BeginScrollView(scrollExcel, GUILayout.Height(60));
            GUILayout.Space(10f);
            EditorGUILayout.BeginHorizontal();
            //读取Excel文件
            string[] paths = Directory.GetFiles(_excelFolderPath, "*.xlsx", SearchOption.AllDirectories);
            for (int i = 0; i < paths?.Length; i++)
            {
                string path = paths[i];
                if (path.EndsWith("meta") || path.EndsWith("txt")) continue;

                string[] strings = path.Split('\\');
                string btnName = string.Empty;
                if (DesDic.TryGetValue(strings[strings.Length - 1], out string value))
                    btnName = $"{strings[strings.Length - 1]}\t\n{value}";
                else
                    btnName = $"{strings[strings.Length - 1]}";
                if (GUILayout.Button(btnName))
                {
                    NewLoadExcelPath = path;
                    ReadData();
                }
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.EndScrollView();



            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("读取数据", GUILayout.Width(80f)))
            {
                ReadData();
            }
            if (GUILayout.Button("添加数据", GUILayout.Width(80f)))
            {
                if (data == null)
                {
                    Message = "请先读取数据";
                    return;
                }

                string[] strs = new string[data[0].Length];
                List<string[]> strings = data.ToList();
                strings.Add(strs);
                data = strings.ToArray();
                Message = "数据添加成功";
            }
            if (GUILayout.Button("写入数据", GUILayout.Width(80f)))
            {
                FileInfo _excelName = new FileInfo(NewLoadExcelPath);
                //通过ExcelPackage打开文件
                using (ExcelPackage package = new ExcelPackage(_excelName))
                {
                    //1表示第一个表
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    //Debug.Log(worksheet.Name);
                    for (int i = 0; i < data.Length; i++)
                    {
                        string[] item1 = data[i];
                        EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width));
                        for (global::System.Int32 j = 0; j < item1.Length; j++)
                        {
                            string item2 = item1[j];
                            if (IsInteger(item2, out int number1))
                                worksheet.SetValue(i + 1, j + 1, number1);
                            else if (IsFloateger(item2, out float number2))
                                worksheet.SetValue(i + 1, j + 1, number2);
                            else if (string.IsNullOrEmpty(item2))
                                worksheet.SetValue(i + 1, j + 1, null);
                            else
                                worksheet.Cells[i + 1, j + 1].Value = item2;
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    package.Save(); //储存
                }
                Message = "数据写入成功";
            }
            if (GUILayout.Button("清除消息", GUILayout.Width(80f)))
            {
                Message = string.Empty;
            }
            if (GUILayout.Button("Excel转换", GUILayout.Width(80f)))
            {
                ExcelChange.GenerateExcelInfo();
                Message = "转换成功";
            }
            if (GUILayout.Button("打开Excel", GUILayout.Width(80f)))
            {
                Application.OpenURL(NewLoadExcelPath);
            }
            EditorGUILayout.LabelField("消息提示:", GUILayout.Width(80f));
            EditorGUILayout.LabelField(Message, EditorStyles.label);
            EditorGUILayout.EndHorizontal();


            ClickFileLoadPath();
            RefreshData();
        }




        /// <summary>
        /// 打开文件
        /// </summary>
        private void BrowseLoadFilePanel()
        {
            string directory = EditorUtility.OpenFilePanel("选择Execl文件", NewLoadExcelPath, "xls,xlsx,csv");
            if (!string.IsNullOrEmpty(directory))
                NewLoadExcelPath = directory;
        }
        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData()
        {
            if (data == null) return;
            GUILayout.BeginScrollView(scrollPosition, false, false,
                GUILayout.Width(position.width - 20f), GUILayout.Height(90f));
            for (int i = 0; i < 3; i++)
            {
                string[] item1 = data[i];
                EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width));
                if (GUILayout.Button("删除", GUILayout.Width(_deleteButSice)))
                {
                    //删除内存数据
                    List<string[]> strings = data.ToList();
                    strings.Remove(item1);
                    data = strings.ToArray();
                    //删除实际数据
                    int deleteNumber = i;
                    FileInfo _excelName = new FileInfo(NewLoadExcelPath);
                    //通过ExcelPackage打开文件
                    using (ExcelPackage package = new ExcelPackage(_excelName))
                    {
                        //1表示第一个表
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                        for (int j = 0; j < data[0].Length; j++)
                            worksheet.SetValue(deleteNumber + 1, j + 1, null);
                        package.Save(); //储存
                    }
                    Message = "数据删除成功";

                    EditorGUILayout.EndHorizontal();
                    continue;
                }
                for (global::System.Int32 j = 0; j < item1.Length; j++)
                {
                    string item2 = item1[j];
                    data[i][j] = EditorGUILayout.TextField(item2, GUILayout.MinWidth(slideBox));
                }
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();

            //显示数据
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);//GUILayout.Width(400), GUILayout.Height(500)

            for (int i = 3; i < data.Length; i++)
            {
                string[] item1 = data[i];
                EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width));
                if (GUILayout.Button("删除", GUILayout.Width(_deleteButSice)))
                {
                    //删除内存数据
                    List<string[]> strings = data.ToList();
                    strings.Remove(item1);
                    data = strings.ToArray();
                    //删除实际数据
                    int deleteNumber = i;
                    FileInfo _excelName = new FileInfo(NewLoadExcelPath);
                    //通过ExcelPackage打开文件
                    using (ExcelPackage package = new ExcelPackage(_excelName))
                    {
                        //1表示第一个表
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                        for (int j = 0; j < data[0].Length; j++)
                            worksheet.SetValue(deleteNumber + 1, j + 1, null);
                        package.Save(); //储存
                    }
                    Message = "数据删除成功";

                    EditorGUILayout.EndHorizontal();
                    continue;
                }
                for (global::System.Int32 j = 0; j < item1.Length; j++)
                {
                    string item2 = item1[j];
                    data[i][j] = EditorGUILayout.TextField(item2, GUILayout.MinWidth(slideBox));
                }
                EditorGUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }

        private void ReadData()
        {
            data = NewLoadExcelPath.LoadExcel();//读取Excel数据
            Message = "数据读取成功";
        }
        private void ReadDataPathChange()
        {
            if (NewLoadExcelPath != OldLoadExcelPath)
            {
                OldLoadExcelPath = NewLoadExcelPath;
                ReadData();
            }
        }

        /// <summary>
        /// 点击文件加载路径,Unity专用
        /// </summary>
        private void ClickFileLoadPath()
        {
            if (isSelectFile == false) return;
            if (Selection.activeObject != null)
            {
                Repaint();
                string path;
                path = AssetDatabase.GetAssetPath(Selection.activeObject);//选择的文件的路径 
                if (path.Contains("xls") || path.Contains("xlsx"))
                {
                    path = path.Split("Assets")[1];
                    NewLoadExcelPath = string.Format($"{Application.dataPath}{path}");
                    ReadDataPathChange();
                }
            }
        }


        /// <summary>
        /// 清空日志
        /// </summary>
        public static void ClearConsole()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
            System.Type logEntries = assembly.GetType("UnityEditor.LogEntries");
            MethodInfo clearConsoleMethod = logEntries.GetMethod("Clear");
            clearConsoleMethod.Invoke(new object(), null);
        }


        private bool IsInteger(string input, out int number)
        {
            //string pattern = @"^-?\d+$";  
            //return IsMatch(input, pattern);  
            if (int.TryParse(input, out int i))
            {
                number = i;
                return true;
            }
            else
            {
                number = 0;
                return false;
            }
        }
        private bool IsFloateger(string input, out float number)
        {
            if (float.TryParse(input, out float i))
            {
                number = i;
                return true;
            }
            else
            {
                number = 0;
                return false;
            }
        }
    }
}
