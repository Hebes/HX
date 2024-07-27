// using System;
// using System.Collections.Generic;
// using System.Data;
// using System.Diagnostics;
// using System.IO;
// using System.Linq;
// using System.Reflection;
// using System.Text;
// using Excel;
// using OfficeOpenXml;
// using UnityEditor;
// using UnityEditor.U2D;
// using UnityEngine;
// using UnityEngine.U2D;
// using Debug = UnityEngine.Debug;
// using Object = UnityEngine.Object;
//
// namespace Editor
// {
//     /// <summary>
//     /// 动画工具
//     /// </summary>
//     public class AnimationTool : EditorWindow
//     {
//         private string mainGameTablePath; //主程序的GameTable路径
//         private int index; //options的选择号
//         private static string abFolderPath; //AB包文件夹路径
//         private static string animationPath;
//         private string animationTypeName;
//         private static string spriteFolderPath;
//         private static string loadExcelPath;
//         private static string animFolderPath;
//         private static string folder1;
//         private string prefix; //改名前缀
//         private string removeNameStr; //需要移除的字段
//         private static string changeNamePath; //选择一个文件夹就可以了(会根据自己所在的文件夹命名)
//         private static string animationType;
//         private string codeExcelStartRow;
//         private string codeExcelEndRow;
//         private static string moveOldPath; //选中的老路径
//         private static string moveTargetPath; //目标路径
//         private static string moveAction; //动作比如about_die
//         private static string message; //消息提示
//         private Vector2 textAreaScrollPosition;
//         private string reNameNewKey;
//         private string reNamePath;
//         private string reNameOldKey;
//
//         private List<(string, string)> options = new List<(string, string)>()
//         {
//             ("BoyQ", "男轻甲"),
//             ("BoyZ", "男中甲"),
//             ("GirlQ", "女轻甲"),
//             ("GirlZ", "女中甲"),
//             ("Currency", "通用"),
//             ("CurrencyZ", "重甲通用"),
//             ("BoyAborigine", "男土著"),
//             ("GirlAborigine", "女土著"),
//             ("NativeAmericanBusinessman", "土著商人"),
//             ("CamelBeast", "骆兽_土著商人的"),
//             ("GirlMainOne", "女一"),
//             ("GirlMainTwo", "女二"),
//             ("Spider1", "蜘蛛1"),
//             ("Spider2", "蜘蛛2"),
//             ("BoyResearcher", "男科研员/研究员"),
//             ("GirlResearcher", "女科研员/研究员"),
//             ("GodFather", "神父"),
//             ("GodFollower", "信徒/追随者/邪教徒"),
//             ("Bear", "熊"),
//             ("SmallBear", "小熊"),
//             ("AlloyArmor", "壮汉/重力甲（合金装甲/四级甲）"),
//             ("Effect", "特效"),
//             ("Pig", "猪"),
//             ("Rabbit", "兔子"),
//             ("Deer", "鹿"),
//             ("SweepingRobot", "扫地机器人"),
//             ("CamelBeast_Free", "驼兽_野生的/自由的"),
//             ("Smuggler", "走私者"),
//             ("Beast", "异能兽"),
//             ("FlyingSlave", "飞行奴隶"),
//             ("AlienSlave", "异星人奴隶"),
//             ("Maid", "女仆"),
//             ("WaifBoy", "无业男"),
//             ("WaifGirl", "无业女"),
//             ("Official", "官员"),
//             ("CurrencyAborigineZ", "土著重甲通用"),
//         };
//
//         
//
//         [MenuItem("Tools/动画工具#Q #Q")]
//         public static void BuildPackageVersions()
//         {
//             if (!EditorWindow.HasOpenInstances<AnimationTool>())
//                 GetWindow(typeof(AnimationTool), false, "动画工具").Show();
//             else
//                 GetWindow(typeof(AnimationTool)).Close();
//         }
//
//         #region 存读
//
//         private void OnEnable() => Load();
//
//         private void OnDisable() => Save();
//
//         [MenuItem("Tools/保存数据 #S")]
//         private void Save()
//         {
//             if (!EditorWindow.HasOpenInstances<AnimationTool>()) return;
//             Type type = GetType();
//             var fieldsValue = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
//             foreach (var data in fieldsValue)
//             {
//                 if (data.FieldType == typeof(string))
//                     PlayerPrefs.SetString($"{Application.productName}{data.Name}Save", (string)data.GetValue(this));
//                 if (data.FieldType == typeof(int))
//                     PlayerPrefs.SetInt($"{Application.productName}{data.Name}Save", (int)data.GetValue(this));
//             }
//
//             Debug.LogError("保存成功");
//         }
//
//         private void Load()
//         {
//             Type type = GetType();
//             var fieldsValue = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
//             foreach (var data in fieldsValue)
//             {
//                 if (data.FieldType == typeof(string))
//                     data.SetValue(this, PlayerPrefs.GetString($"{Application.productName}{data.Name}Save"));
//                 if (data.FieldType == typeof(int))
//                     data.SetValue(this, PlayerPrefs.GetInt($"{Application.productName}{data.Name}Save"));
//             }
//         }
//
//         #endregion
//
//         private void OnGUI()
//         {
//             EditorGUILayout.BeginHorizontal();
//             EditorGUILayout.LabelField("当前动画类型：", EditorStyles.label, GUILayout.Width(100f));
//             var optionsTemp = new string[options.Count];
//             for (var i = 0; i < options.Count; i++)
//                 optionsTemp[i] = options[i].Item1;
//             index = EditorGUILayout.Popup(index, optionsTemp);
//             animationType = EditorGUILayout.TextField(options[index].Item1);
//             animationTypeName = EditorGUILayout.TextField(options[index].Item2);
//             EditorGUILayout.EndHorizontal();
//
//
//             EditorGUILayout.BeginHorizontal();
//             EditorGUILayout.LabelField("动画AB包路径：", EditorStyles.label, GUILayout.Width(100f));
//             abFolderPath = EditorGUILayout.TextField(abFolderPath);
//             EditorGUILayout.EndHorizontal();
//
//
//             EditorGUILayout.BeginHorizontal();
//             EditorGUILayout.LabelField("GameTable路径：", EditorStyles.label, GUILayout.Width(100f));
//             mainGameTablePath = EditorGUILayout.TextField(mainGameTablePath, GUILayout.Width(250f));
//             if (GUILayout.Button("获取主程序的Excel实际路径", GUILayout.Width(200f)))
//             {
//                 var temp = GetFiles(mainGameTablePath);
//                 foreach (var s in temp)
//                 {
//                     if (string.IsNullOrEmpty(animationType))
//                         throw new Exception("请先输入动画类型");
//                     if (!s.Contains(animationType)) continue;
//                     loadExcelPath = s;
//                 }
//             }
//             loadExcelPath = EditorGUILayout.TextField(loadExcelPath);
//             EditorGUILayout.EndHorizontal();
//             
//             
//             EditorGUILayout.BeginHorizontal();
//             EditorGUILayout.LabelField("改名目录(会根据文件的文件夹命名):", EditorStyles.label, GUILayout.Width(200f));
//             changeNamePath = EditorGUILayout.TextField(changeNamePath);
//             if (GUILayout.Button("改名", GUILayout.Width(150f))) ChangeName(changeNamePath);
//             EditorGUILayout.EndHorizontal();
//             
//             
//             GUILayout.Space(10f);
//             EditorGUILayout.BeginHorizontal();
//             if (GUILayout.Button("清理所有文件", GUILayout.Width(200f))) DeleteAllFile();
//             if (GUILayout.Button("清空日志", GUILayout.Width(200f))) ClearConsole();
//             if (GUILayout.Button("执行Bat", GUILayout.Width(200f))) RunBat("D:/Preject/client/GameTable/Generated.bat");
//             if (GUILayout.Button("保存数据 Shift+S", GUILayout.Width(200f))) Save();
//             if (GUILayout.Button("打开excel", GUILayout.Width(150f))) Application.OpenURL(loadExcelPath);
//             EditorGUILayout.EndHorizontal();
//
//             GUILayout.Space(10f);
//             EditorGUILayout.BeginHorizontal();
//             if (GUILayout.Button("打开远端动画文件夹", GUILayout.Width(200f))) Application.OpenURL("\\\\Sc-202010130846\\动画提交\\000000动画整理");
//             if (GUILayout.Button("打开新文件夹动画存放", GUILayout.Width(200f))) Application.OpenURL("D:\\Preject\\动画工具");
//             if (GUILayout.Button("打开主工程ab资源目录", GUILayout.Width(200f))) Application.OpenURL("D:\\Preject\\client\\Assets\\StreamingAssets\\AssetBundles\\Animation");
//             if (GUILayout.Button("打开主工程GameTable", GUILayout.Width(200f))) Application.OpenURL(mainGameTablePath);
//             EditorGUILayout.EndHorizontal();
//
//             GUILayout.Space(10f);
//             EditorGUILayout.BeginHorizontal();
//             if (GUILayout.Button("执行动画导出exe", GUILayout.Width(200f)))
//                 RunExe("D:\\Preject\\动画工具\\DragonAnimationBuildTool\\" +
//                        "DragonAnimationBuildTool\\obj\\Debug\\DragonAnimationBuildTool.exe");
//             if (GUILayout.Button("打包图集(请先选中所有文件夹)", GUILayout.Width(200f))) BuildSpriteAtlas_();
//             if (GUILayout.Button("图集转AB(请先选中所有图集)", GUILayout.Width(200f))) GetWindow<BuildAssetBundleWindow>().Show();
//             EditorGUILayout.EndHorizontal();
//
//             GUILayout.Space(10f);
//             EditorGUILayout.BeginHorizontal();
//             EditorGUILayout.LabelField("创建类型脚本", EditorStyles.label, GUILayout.Width(130f));
//             if (GUILayout.Button("创建类型脚本", GUILayout.Width(150f))) CreatCode();
//             EditorGUILayout.LabelField($"当前创建类型的路径：{loadExcelPath}", EditorStyles.miniLabel);
//             EditorGUILayout.EndHorizontal();
//
//             GUILayout.Space(10f);
//             EditorGUILayout.BeginHorizontal();
//             EditorGUILayout.LabelField("重命名", EditorStyles.label, GUILayout.Width(130f));
//             if (GUILayout.Button("重命名", GUILayout.Width(150f))) ReName(reNameOldKey, reNameNewKey, reNamePath);
//             reNameOldKey = LabelFieldAndTextField("老关键字：", reNameOldKey, EditorStyles.miniLabel, 150f);
//             reNameNewKey = LabelFieldAndTextField("新关键字：", reNameNewKey, EditorStyles.miniLabel, 150f);
//             reNamePath = LabelFieldAndTextField("重命名路径：", reNamePath, EditorStyles.miniLabel, 400f);
//             EditorGUILayout.EndHorizontal();
//
//             GUILayout.Space(10f);
//             EditorGUILayout.LabelField("数据校验", EditorStyles.label, GUILayout.Width(150f));
//             EditorGUILayout.BeginHorizontal();
//             if (GUILayout.Button("打包后数量比对", GUILayout.Width(150f))) CountComparison(spriteFolderPath);
//             spriteFolderPath = LabelFieldAndTextField("动画文件和打包文件比对路径:：", spriteFolderPath, EditorStyles.label, 400f);
//             EditorGUILayout.EndHorizontal();
//             EditorGUILayout.BeginHorizontal();
//             if (GUILayout.Button("和主程序AB包比对", GUILayout.Width(150f))) CheckMainAb(abFolderPath);
//             EditorGUILayout.LabelField($"校验数据Excel路径：{loadExcelPath}");
//             EditorGUILayout.LabelField($"校验数据动画路径：{abFolderPath}");
//             EditorGUILayout.EndHorizontal();
//
//             // GUILayout.Space(30f);
//             // EditorGUILayout.LabelField($"先通过校验数据的数据再移动,Copy文件(老的路径):{animFolderPath}", EditorStyles.label);
//             // EditorGUILayout.BeginHorizontal();
//             // if (GUILayout.Button("获取动画文件实际路径", GUILayout.Width(150f))) GetAnimationFolder();
//             // animationPath = EditorGUILayout.TextField(animationPath);
//             // EditorGUILayout.EndVertical();
//             // EditorGUILayout.BeginHorizontal();//D:\Preject\ab包打包\Assets\Anim
//             // EditorGUILayout.LabelField("Copy文件(目标路径)", EditorStyles.label, GUILayout.Width(150f));
//             // moveTargetPath = EditorGUILayout.TextField(moveTargetPath, GUILayout.Width(200f));
//             // EditorGUILayout.LabelField("动作比如about_die", EditorStyles.label, GUILayout.Width(120f));
//             // moveAction = EditorGUILayout.TextField(moveAction);
//             // EditorGUILayout.EndHorizontal();
//             // EditorGUILayout.BeginVertical();
//             // if (GUILayout.Button("移动(Copy版本)", GUILayout.Width(150f))) MoveCopy();
//             // EditorGUILayout.EndVertical();
//
//             if (string.IsNullOrEmpty(message)) return;
//             if (GUILayout.Button("拷贝", GUILayout.Height(40f))) Copy(message);
//             textAreaScrollPosition = EditorGUILayout.BeginScrollView(textAreaScrollPosition, GUILayout.Height(300f));
//             EditorGUILayout.TextArea(message, GUILayout.ExpandHeight(true));
//             EditorGUILayout.EndScrollView();
//             //Repaint();
//         }
//
//         private void ReName(string oldKey, string newKey, string path)
//         {
//             var fileArray = GetFiles(path);
//             foreach (var data in fileArray)
//             {
//                 var fileName = Path.GetFileNameWithoutExtension(data); //获取文件名
//                 fileName = fileName.Replace(oldKey, newKey); //改名
//                 var pathValue = Path.GetDirectoryName(data); //获取路径
//                 var fileExtension = Path.GetExtension(data); //获取后缀拓展
//                 if (string.IsNullOrEmpty(pathValue)) throw new Exception("当前路径错误");
//                 var newFileName = $"{fileName}{fileExtension}"; //新的名称
//                 var newFilePath = Path.Combine(pathValue, newFileName); //拼接
//                 File.Move(data, newFilePath);
//             }
//             AssetDatabase.Refresh();
//         }
//
//         private string LabelFieldAndTextField(string labelText, string textFieldText, GUIStyle style, float textFieldWidth)
//         {
//             float labelWidth = EditorStyles.miniLabel.CalcSize(new GUIContent(labelText)).x;
//             EditorGUILayout.LabelField(labelText, style, GUILayout.Width(labelWidth));
//             return EditorGUILayout.TextField(textFieldText, GUILayout.Width(textFieldWidth));
//         }
//
//         /// <summary>
//         /// AB包和excel比对
//         /// </summary>
//         private void CheckMainAb(string abPath)
//         {
//             if (string.IsNullOrEmpty(abPath))
//                 throw new Exception("请检查路径");
//             var path1 = new List<string>();
//             var temp1 = GetFiles(abPath);
//             foreach (var s in temp1)
//             {
//                 if (s.Contains(".meta")) continue;
//                 if (!s.Contains(animationType)) continue;
//                 var str = Path.GetFileName(s).Replace(Path.GetExtension(s), string.Empty);
//                 var sTemp = str.Replace($"{animationType}_", string.Empty);
//                 path1.Add(sTemp.ToLower());
//             }
//
//             //获取Excel中的动画名称
//             string[][] excelData = LoadExcelData(loadExcelPath);
//             var dataTemp = new List<string>();
//             foreach (var t in excelData)
//             {
//                 if (t[3].Contains("reverse")) continue;
//                 dataTemp.Add(t[3].ToLower()); //获取Excel的第四行
//             }
//
//             var sb = new StringBuilder();
//             sb.AppendLine($"当前动画类型：{animationType}   动画名称：{animationTypeName}");
//             foreach (var s in dataTemp)
//             {
//                 var isExist = false;
//                 foreach (var sData in path1)
//                 {
//                     if (s != sData) continue;
//                     isExist = true;
//                     break;
//                 }
//
//                 if (isExist) continue;
//                 sb.AppendLine($"Excel为主,Excel中存在，但是AB不存在,资源包中的名称是  {s}");
//             }
//
//             foreach (var s in path1)
//             {
//                 var isExist = false;
//                 foreach (var sData in dataTemp)
//                 {
//                     if (s != sData) continue;
//                     isExist = true;
//                     break;
//                 }
//
//                 if (isExist) continue;
//                 sb.AppendLine($"AB为主,AB中存在，但是Excel不存在,资源包中的名称是  {s}");
//             }
//
//             message = sb.ToString();
//         }
//
//         /// <summary>
//         /// 获取动画实际路径
//         /// </summary>
//         /// <exception cref="Exception"></exception>
//         private void GetAnimationFolder()
//         {
//             var temp = GetDirectories(animationPath);
//             if (string.IsNullOrEmpty(animationType))
//                 throw new Exception("请先输入动画类型");
//             foreach (var s in temp)
//             {
//                 if (!s.Contains(animationType)) continue;
//                 animFolderPath = s;
//                 break;
//             }
//         }
//
//         /// <summary>
//         /// 弹框显示消息
//         /// </summary>
//         /// <param name="titleValue"></param>
//         /// <param name="message"></param>
//         /// <param name="ok"></param>
//         /// <param name="action"></param>
//         private static void ShowMessage(string message, string ok, string cancel, Action action = null)
//         {
//             bool isOk = EditorUtility.DisplayDialog("消息提示", message, ok, cancel);
//             if (!isOk) return;
//             action?.Invoke();
//         }
//
//         /// <summary>
//         /// 执行Bat
//         /// </summary>
//         /// <param name="batPath">bat的exe路径</param>
//         private static void RunBat(string batPath)
//         {
//             // 设置要执行的.bat文件和工作目录
//             var startInfo = new ProcessStartInfo();
//             startInfo.FileName = batPath;
//             var directoryPath = Path.GetDirectoryName(startInfo.FileName);
//             if (string.IsNullOrEmpty(directoryPath))
//                 throw new Exception("当前路径为空");
//             startInfo.WorkingDirectory = directoryPath;
//
//             // 创建进程对象
//             var process = new Process();
//             process.StartInfo = startInfo;
//             // // 设置进程文件路径
//             // process.StartInfo.FileName = "cmd.exe";
//             // // 设置传递给 cmd.exe 的参数，包括批处理文件路径和执行参数（如果有）
//             // process.StartInfo.Arguments = $"/C {batchFilePath}";
//
//             // 隐藏命令行窗口
//             //process.StartInfo.CreateNoWindow = true;
//             //process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
//
//             process.Start(); // 启动进程
//             process.WaitForExit(); // 等待进程完成执行
//             var exitCode = process.ExitCode; // 获取批处理文件的退出码
//             process.Close(); // 关闭进程
//             ShowMessage(exitCode == 0 ? "批处理文件执行成功." : $"批处理文件执行失败。退出代码: {exitCode}", "Ok", "Close"); // 检查退出码以确定批处理文件的执行结果
//         }
//
//         /// <summary>
//         /// 执行Exe
//         /// </summary>
//         /// <param name="str"></param>
//         private static void RunExe(string str)
//         {
//             var process = new Process();
//             process.StartInfo.FileName = str;
//             process.Start();
//
//             // // 可选：等待进程执行完成
//             // process.WaitForExit();
//             //
//             // // 关闭进程
//             // process.Close();
//         }
//
//         /// <summary>
//         /// /复制
//         /// </summary>
//         /// <param name="content"></param>
//         private static void Copy(string content)
//         {
//             var te = new TextEditor();
//             te.text = content;
//             te.SelectAll();
//             te.Copy();
//         }
//
//         /// <summary>
//         /// 清空日志
//         /// </summary>
//         private static void ClearConsole()
//         {
//             Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
//             System.Type logEntries = assembly.GetType("UnityEditor.LogEntries");
//             MethodInfo clearConsoleMethod = logEntries.GetMethod("Clear");
//             clearConsoleMethod?.Invoke(new object(), null);
//             message = string.Empty;
//         }
//
//         /// <summary>
//         /// 读取Excel数据并保存为字符串锯齿数组(如果读取不出数据，请把ICSharpCode.SharpZipLib.dll也加入进来)
//         /// </summary>
//         /// <param name="filePath"></param>
//         /// <returns></returns>
//         private static string[][] LoadExcelData(string filePath)
//         {
//             System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
//             using FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
//             DataSet dataSet = fileInfo.Extension == ".xlsx"
//                 ? ExcelReaderFactory.CreateOpenXmlReader(stream).AsDataSet()
//                 : ExcelReaderFactory.CreateBinaryReader(stream).AsDataSet();
//
//             DataRowCollection rows = dataSet.Tables[0].Rows;
//             string[][] data = new string[rows.Count][];
//             for (var i = 0; i < rows.Count; ++i)
//             {
//                 var columnCount = rows[i].ItemArray.Length;
//                 string[] columnArray = new string[columnCount];
//                 for (var j = 0; j < columnArray.Length; ++j)
//                     columnArray[j] = rows[i].ItemArray[j].ToString();
//                 data[i] = columnArray;
//             }
//
//             return data;
//         }
//
//         /// <summary>
//         /// 移动
//         /// </summary>
//         private static void MoveCopy()
//         {
//             animationType = animationType.Trim();
//             moveAction = moveAction.Trim();
//             var destinationFolder = $"{moveTargetPath}\\{animationType}\\{moveAction}";
//             if (!Directory.Exists(destinationFolder))
//                 Directory.CreateDirectory(destinationFolder);
//             var files = Directory.GetFiles($"{moveOldPath}\\{moveAction}", "*", SearchOption.AllDirectories); //所有文件
//             foreach (var s in files)
//                 File.Copy(s, Path.Combine(destinationFolder, Path.GetFileName(s)));
//             AssetDatabase.Refresh();
//         }
//
//         /// <summary>
//         /// 删除文件
//         /// </summary>
//         private static void DeleteAllFile()
//         {
//             Remove("D:\\Preject\\ab包打包\\Assets\\Anim"); //帧文件夹
//             Remove("D:\\Preject\\ab包打包\\Assets\\SpriteAtlas"); //图集文件夹
//             Remove("D:\\Preject\\ab包打包\\Assets\\StreamingAssets\\AssetBundles\\Animation"); //AB包文件夹
//             AssetDatabase.Refresh();
//             return;
//
//             void Remove(string path)
//             {
//                 var strTemp1 = GetFiles(path);
//                 foreach (var s in strTemp1)
//                     File.Delete(s);
//
//                 var strTemp2 = GetDirectories(path);
//                 foreach (var s in strTemp2)
//                     Directory.Delete(s);
//                 AssetDatabase.Refresh();
//             }
//         }
//
//         /// <summary>
//         /// 改名
//         /// </summary>
//         /// <param name="changeNamePathValue"></param>
//         private void ChangeName(string changeNamePathValue)
//         {
//             //删除指定文件后缀
//             var str = GetFiles(changeNamePathValue);
//
//             //删除指定文件
//             foreach (var s in str)
//             {
//                 var fileName = Path.GetFileName(s);
//                 if (fileName.Equals(animationType) || fileName.Equals($"{animationType}_{animationType}"))
//                     File.Delete(s);
//             }
//
//             foreach (var s in str)
//             {
//                 var fileName = Path.GetFileName(s);
//                 if (s.Contains(".manifest"))
//                     File.Delete(s);
//             }
//
//             //获取文件
//             var pathList = new List<string>();
//             var temp = GetDirectories(changeNamePathValue);
//             foreach (var value in temp)
//             {
//                 var subfolders = Directory.GetDirectories(value);
//                 if (subfolders.Length > 0) continue; //文件夹中还有其他文件夹
//                 var files = Directory.GetFiles(value, "*", SearchOption.AllDirectories);
//                 pathList.AddRange(files);
//             }
//
//             var openPath = string.Empty;
//             //改名
//             foreach (var data in pathList)
//             {
//                 var fileName = Path.GetFileNameWithoutExtension(data); //获取文件名
//                 var fileExtension = Path.GetExtension(data); //获取后缀拓展
//                 var path = Path.GetDirectoryName(data);
//                 openPath = path;
//                 var previousFolderName = Path.GetFileName(path);
//                 var newFileName = $"{previousFolderName}_{fileName}{fileExtension}";
//                 if (fileName.Contains($"{previousFolderName}_")) continue;
//                 var newFilePath = Path.Combine(path, newFileName); //拼接
//                 File.Move(data, newFilePath);
//             }
//
//             ShowMessage("改名成功", "OK", "Close", () => { Application.OpenURL(openPath); });
//             AssetDatabase.Refresh();
//         }
//
//         /// <summary>
//         /// 动画图集打包
//         /// </summary>
//         private static void BuildSpriteAtlas_()
//         {
//             //拿到当前选中的object
//             var selectionObjectList = Selection.objects.ToList();
//             //过滤无用的资源
//             for (var i = 0; i < selectionObjectList.Count; i++)
//                 if (!(selectionObjectList[i] is DefaultAsset))
//                 {
//                     selectionObjectList.RemoveAt(i);
//                     i--;
//                 }
//
//             if (selectionObjectList.Count < 1)
//             {
//                 EditorUtility.DisplayDialog("Unity", "请重新选择要打包SpriteAtlas的文件夹", "确定");
//                 return;
//             }
//
//             foreach (var t in selectionObjectList)
//             {
//                 var path = AssetDatabase.GetAssetOrScenePath(t); //获取objecct所在的路径
//                 path = Path.Combine(Application.dataPath.Remove(Application.dataPath.Length - "Assets".Length, "Assets".Length), path);
//                 CreateAnimClip(path);
//             }
//
//             AssetDatabase.Refresh();
//             return;
//
//             void CreateAnimClip(string path)
//             {
//                 var images = Directory.CreateDirectory(path).GetFiles("*.png");
//                 var spriteAtlas = new SpriteAtlas();
//                 Object[] sprites = new Object[images.Length];
//                 for (var i = 0; i < images.Length; i++)
//                 {
//                     var str = images[i].FullName.Split('\\');
//                     var o = "";
//                     for (var j = 3; j < str.Length; j++)
//                     {
//                         if (j == str.Length - 1)
//                             o += str[j];
//                         else
//                             o += str[j] + "\\";
//                     }
//
//                     sprites[i] = AssetDatabase.LoadAssetAtPath<Sprite>(o);
//                     if (!sprites[i])
//                         throw new Exception("获取失败+" + o);
//                 }
//
//                 spriteAtlas.Add(sprites);
//
//                 var settings = spriteAtlas.GetPlatformSettings("Default");
//                 settings.maxTextureSize = 4096;
//                 settings.crunchedCompression = true;
//                 settings.compressionQuality = 50;
//                 spriteAtlas.SetPlatformSettings(settings);
//
//                 var st = path.Split('/');
//                 var p = Application.dataPath + "\\SpriteAtlas" + "";
//                 var nowTragetPath = p + "\\" + st[st.Length - 2];
//                 if (!Directory.Exists(nowTragetPath))
//                     Directory.CreateDirectory(nowTragetPath);
//                 //图集所在目录
//                 var nowTragetFilePath =
//                     "Assets\\SpriteAtlas" + "\\" + st[st.Length - 2] + "\\" + st[st.Length - 1] + ".spriteatlas";
//                 AssetDatabase.CreateAsset(spriteAtlas, nowTragetFilePath);
//             }
//         }
//
//         /// <summary>
//         /// 创建代码
//         /// </summary>
//         private void CreatCode()
//         {
//             using var fs = new FileStream(loadExcelPath, FileMode.Open, FileAccess.Read, FileShare.Read);
//             using var excel = new ExcelPackage(fs);
//             ExcelWorksheets worksheets = excel.Workbook.Worksheets;
//             ExcelWorksheet excelWorksheet = worksheets[1];
//             ExcelRange excelRange = excelWorksheet.Cells;
//             var targetStr = "";
//             targetStr += "public enum " + "SpriteAnimation_" + animationType + "Type\n{";
//
//             //var startCol = int.Parse(codeExcelStartRow); 
//             // var endCol = int.Parse(codeExcelEndRow);
//             var startCol = 5;
//             var endCol = excelRange.Columns;
//
//             for (var i = startCol; i <= endCol; i++)
//             {
//                 if (string.IsNullOrEmpty(excelRange[i, 3].Text)) continue;
//                 targetStr += "\n";
//                 targetStr += "/// <summary>\n";
//                 targetStr += "/// ";
//                 targetStr += excelRange[i, 3].Text;
//                 targetStr += "\n";
//                 targetStr += "/// </summary>\n";
//                 targetStr += excelRange[i, 4].Text;
//                 targetStr += "=";
//                 targetStr += excelRange[i, 2].Text;
//                 targetStr += ",";
//                 targetStr += "\n";
//             }
//
//             targetStr += "\n}";
//             Copy(targetStr);
//             message = targetStr;
//             ShowMessage($"当前创建的脚本代码Excel是{loadExcelPath}", "OK", "Close");
//         }
//
//         /// <summary>
//         /// 检查动画文件和excel匹配
//         /// </summary>
//         private static void CheckData()
//         {
//             //获取动画文件夹名称
//             var animPathName = new List<(string, string)>();
//             var pathArray = GetDirectories(animFolderPath);
//             foreach (var path in pathArray)
//             {
//                 var value = (Path.GetFileName(path).ToLower(), Path.GetFileName(path));
//                 animPathName.Add(value);
//             }
//
//             //获取Excel中的动画名称
//             string[][] excelData = LoadExcelData(loadExcelPath);
//             //获取Excel的第四行
//             var dataTemp = new List<string>();
//             foreach (var t in excelData)
//                 dataTemp.Add(t[3].ToLower());
//
//             StringBuilder sb = new StringBuilder();
//
//             //以动画文件夹为主
//             foreach (var animTemp in animPathName)
//             {
//                 var isExist = false;
//                 foreach (var sData in dataTemp)
//                 {
//                     if (animTemp.Item1 != sData) continue;
//                     isExist = true;
//                     break;
//                 }
//
//                 if (isExist) continue;
//                 sb.AppendLine($"以AB包为主,AB中存在，但是Excel不存在{animTemp.Item1},资源包中的名称是  {animTemp.Item2}");
//             }
//
//             if (string.IsNullOrEmpty(sb.ToString()))
//                 ShowMessage("未发现Excel数据丢失", "OK", "Close");
//             else
//                 Debug.LogError(sb.ToString());
//         }
//
//         /// <summary>
//         /// AB包和文件比对
//         /// </summary>
//         /// <exception cref="NotImplementedException"></exception>
//         private static void CountComparison(string abPath)
//         {
//             message = string.Empty;
//             var name1 = new List<string>();
//             var folderArray = GetDirectories($"{abPath}\\{animationType}"); //文件夹
//             foreach (var s in folderArray)
//             {
//                 var path = Path.GetFileName(s);
//                 path = $"{animationType}_{path}";
//                 name1.Add(path.ToLower());
//             }
//
//             var name2 = new List<string>();
//             var fileArray = GetNeedFiles(abFolderPath, animationType); //ab包
//             foreach (var s in fileArray)
//             {
//                 if (s.Contains(".meta")) continue;
//                 var path = Path.GetFileNameWithoutExtension(s);
//                 name2.Add(path.ToLower());
//             }
//
//             StringBuilder sb = new StringBuilder();
//             sb.AppendLine(string.Empty);
//             sb.AppendLine($"目标数量{name1.Count()}   打包数量{name2.Count()}");
//             foreach (var f1 in name1)
//             {
//                 bool isExist = false;
//                 foreach (var f2 in name2)
//                 {
//                     if (f1 != f2) continue;
//                     isExist = true;
//                     break;
//                 }
//
//                 if (isExist) continue;
//                 //sb.AppendLine($"不存在{f1}");
//             }
//
//             message = sb.ToString();
//         }
//
//         #region 通用代码
//
//         /// <summary>
//         /// 获取需要的文件
//         /// </summary>
//         /// <param name="simplePath">简单的路径</param>
//         /// <param name="specifiedFolder">指定的文件夹</param>
//         private static IEnumerable<string> GetNeedFiles(string simplePath, string specifiedFolder)
//         {
//             var folderArray = Directory.GetDirectories(simplePath, "*", SearchOption.AllDirectories);
//             var folderPath = string.Empty;
//             foreach (var s in folderArray)
//             {
//                 if (!s.Contains(specifiedFolder)) continue;
//                 folderPath = s;
//                 break;
//             }
//
//             return GetFiles(folderPath);
//         }
//
//         /// <summary>
//         /// 获取指定路径下的所有文件夹
//         /// </summary>
//         /// <param name="pathValue"></param>
//         private static IEnumerable<string> GetDirectories(string pathValue)
//         {
//             return Directory.GetDirectories(pathValue, "*", SearchOption.AllDirectories);
//         }
//
//         /// <summary>
//         /// 获取指定路径下的文件
//         /// </summary>
//         /// <param name="pathValue"></param>
//         /// <returns></returns>
//         private static IEnumerable<string> GetFiles(string pathValue)
//         {
//             return Directory.GetFiles(pathValue, "*", SearchOption.AllDirectories);
//         }
//
//         #endregion
//     }
// }