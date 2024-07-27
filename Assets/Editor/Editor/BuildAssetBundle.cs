// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using UnityEditor;
// using UnityEngine;
//
// public class BuildAssetBundleWindow : EditorWindow
// {
//     [MenuItem("Tools/Tool")]
//     public static void BuildAssetBundles()
//     {
//         GetWindow<BuildAssetBundleWindow>().Show();
//     }
//
//     //ab包存放的目录
//     string _pathParent = Application.streamingAssetsPath + @"\AssetBundles";
//
//     private AssetBundleBuildType AssetBundleBuildType { get; set; }
//
//     private int selected;
//
//     private BuildAnimation BuildAnimation;
//
//     private void OnEnable()
//     {
//         if (!Directory.Exists(_pathParent))
//             Directory.CreateDirectory(_pathParent);
//         //设置固定界面大小
//         maxSize = minSize = new Vector2(500, 500);
//         BuildAnimation = new BuildAnimation();
//     }
//
//     private bool IsUncompressedAssetBundle;
//
//     /// <summary>
//     /// 要设置贴图的路径
//     /// </summary>
//     private string setTexturePath;
//
//     private void OnGUI()
//     {
//         if (selected == 0)
//         {
//             GUI.Label(new Rect(200, 0, 200, 30), "打包文件的类型");
//             if (GUI.Toggle(new Rect(200, 30, 100, 20), AssetBundleBuildType == AssetBundleBuildType.Animation,
//                 AssetBundleBuildType.Animation.ToString()))
//                 AssetBundleBuildType = AssetBundleBuildType.Animation;
//             if (GUI.Toggle(new Rect(300, 30, 100, 20), AssetBundleBuildType == AssetBundleBuildType.Model,
//                 AssetBundleBuildType.Model.ToString()))
//                 AssetBundleBuildType = AssetBundleBuildType.Model;
//
//             GUI.Label(new Rect(200, 70, 200, 30), "打包文件的压缩方式");
//             if (GUI.Toggle(new Rect(200, 100, 200, 20), !IsUncompressedAssetBundle,
//                 BuildAssetBundleOptions.None.ToString()))
//                 IsUncompressedAssetBundle = false;
//             if (GUI.Toggle(new Rect(300, 100, 100, 20), IsUncompressedAssetBundle,
//                 BuildAssetBundleOptions.UncompressedAssetBundle.ToString()))
//                 IsUncompressedAssetBundle = true;
//             Button(new Rect(0, 0, 200, 200), "打包当前选择的文件为ab包", BuildAssetBundle);
//         }
//
//         if (selected == 1)
//         {
//             BuildAnimation.OnGUI();
//         }
//
//         if (selected == 2)
//         {
//             Button(new Rect(0, 50, 200, 50), "清空所有标签", () =>
//             {
//                 if (EditorUtility.DisplayDialog("Unity", "确定要删除所有AssetBundle标签么", "确定", "取消"))
//                     RemoveAllAssetBundleName();
//             });
//         }
//
//         selected = GUI.Toolbar(new Rect(80, 400, 340, 30), selected, new[] {"AB包工具", "动画", "杂"});
//     }
//
//     void BuildAssetBundle()
//     {
//         var pathParent = "";
//         switch (AssetBundleBuildType)
//         {
//             case AssetBundleBuildType.Animation:
//                 pathParent = _pathParent + @"\Animation";
//                 break;
//             case AssetBundleBuildType.Model:
//                 EditorUtility.DisplayDialog("Unity", "选择了Model", "确定");
//                 return;
//         }
//
//         if (pathParent == "")
//         {
//             EditorUtility.DisplayDialog("Unity", "请选择要打包文件的类型", "确定");
//             return;
//         }
//
//         if (!Directory.Exists(pathParent))
//             Directory.CreateDirectory(pathParent);
//
//         //拿到当前选中的物体
//         var selectionObjectList = Selection.objects.ToList();
//         //过滤无用的资源
//         for (var i = 0; i < selectionObjectList.Count; i++)
//             if (selectionObjectList[i] is DefaultAsset)
//             {
//                 selectionObjectList.RemoveAt(i);
//                 i--;
//             }
//
//         if (selectionObjectList.Count < 1)
//         {
//             EditorUtility.DisplayDialog("Unity", "请重新选择要打包AssetBundle的资源", "确定");
//             Selection.objects = null;
//             return;
//         }
//
//         RemoveAllAssetBundleName();
//         var targetPath = "";
//         for (var i = 0; i < selectionObjectList.Count; i++)
//         {
//             //要打包的文件路径
//             var path = AssetDatabase.GetAssetOrScenePath(selectionObjectList[i]); //获取objecct所在的路径
//             var strs = path.Split('/');
//             var str = strs[strs.Length - 1].Split('.');
//             var assetImporter = AssetImporter.GetAtPath(path); //通过该API获取路径资源(该路径是unity项目内部的路径)
//             assetImporter.assetBundleName = $"{str[0]}"; //设置包名
//             assetImporter.assetBundleVariant = "lf"; //后缀名
//             //ab包存放的目录
//             if (i == 0)
//                 targetPath = pathParent + $@"\{strs[strs.Length - 2]}";
//         }
//
//         if (!Directory.Exists(targetPath))
//             Directory.CreateDirectory(targetPath);
//         BuildPipeline.BuildAssetBundles(targetPath,
//             IsUncompressedAssetBundle
//                 ? BuildAssetBundleOptions.UncompressedAssetBundle
//                 : BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
//
//         if (EditorUtility.DisplayDialog("打包成功", $"共打包{selectionObjectList.Count}个", "打开文件夹", "确定"))
//             System.Diagnostics.Process.Start(pathParent);
//     }
//
//     /// <summary>
//     /// 删除所有AssetBundle标签
//     /// </summary>
//     void RemoveAllAssetBundleName()
//     {
//         var allAssetBundleNames = AssetDatabase.GetAllAssetBundleNames();
//         foreach (var abName in allAssetBundleNames)
//             AssetDatabase.RemoveAssetBundleName(abName, true);
//         /// 批量清空所选文件夹下资源的AssetBundleName
//         UnityEngine.Object[] selObj = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Unfiltered);
//         foreach (UnityEngine.Object item in selObj)
//         {
//             var objPath = AssetDatabase.GetAssetPath(item);
//             var dirInfo = new DirectoryInfo(objPath);
//
//             var filePath = dirInfo.FullName.Replace('\\', '/');
//             filePath = filePath.Replace(Application.dataPath, "Assets");
//             var ai = AssetImporter.GetAtPath(filePath);
//             ai.assetBundleName = "None";
//             ai.assetBundleVariant = "None";
//         }
//
//         AssetDatabase.Refresh();
//         Debug.Log("******批量清除AssetBundle名称成功******");
//     }
//
//     /// <summary>
//     /// 修改Anim文件夹中所有图片的最大尺寸为512
//     /// </summary>
//     void AnimationTextureSizeSwitch()
//     {
//         var path = Application.dataPath + "\\" + setTexturePath;
//         EditorUtility.DisplayProgressBar("修改", "查找中", 0);
//         var aa = new List<string>();
//         a(path, aa, "png");
//         EditorUtility.DisplayProgressBar("修改", "修改中", 0);
//         try
//         {
//             for (var i = 0; i < aa.Count; i++)
//                 aa[i] = aa[i].Remove(0, Application.dataPath.Length - ("Assets").Length);
//             for (var i = 0; i < aa.Count; i++)
//             {
//                 var info = aa[i].Split('\\');
//                 EditorUtility.DisplayProgressBar("修改", "修改中" + info[info.Length - 1], (float) i / (float) aa.Count);
//                 var obj = AssetImporter.GetAtPath(aa[i]);
//                 var TextureImporter = (TextureImporter) obj;
//                 TextureImporter.maxTextureSize = 512;
//             }
//
//             EditorUtility.ClearProgressBar();
//         }
//         catch (Exception)
//         {
//             EditorUtility.DisplayDialog("修改失败", "修改失败", "ok");
//             EditorUtility.ClearProgressBar();
//         }
//     }
//
//     /// <summary>
//     /// 修改文件夹中所有图片的类型为sprite
//     /// </summary>
//     void AnimationTextureTypeSwitch()
//     {
//         var path = Application.dataPath + "\\" + setTexturePath;
//         EditorUtility.DisplayProgressBar("修改", "查找中", 0);
//         var aa = new List<string>();
//         a(path, aa, "png");
//         EditorUtility.DisplayProgressBar("修改", "修改中", 0);
//         try
//         {
//             for (var i = 0; i < aa.Count; i++)
//                 aa[i] = aa[i].Remove(0, Application.dataPath.Length - ("Assets").Length);
//             for (var i = 0; i < aa.Count; i++)
//             {
//                 var info = aa[i].Split('\\');
//                 EditorUtility.DisplayProgressBar("修改", "修改中" + info[info.Length - 1], (float) i / (float) aa.Count);
//                 var obj = AssetImporter.GetAtPath(aa[i]);
//                 var TextureImporter = (TextureImporter) obj;
//                 TextureImporter.textureType = TextureImporterType.Sprite;
//             }
//
//             EditorUtility.ClearProgressBar();
//         }
//         catch (Exception)
//         {
//             EditorUtility.DisplayDialog("修改失败", "修改失败", "ok");
//             EditorUtility.ClearProgressBar();
//         }
//     }
//
//     void a(string path, List<string> fileList, string fileType)
//     {
//         var paths = Directory.GetDirectories(path);
//         var files = Directory.GetFiles(path);
//         foreach (var item in files)
//         {
//             if (!fileList.Contains(item) && item.EndsWith($".{fileType}"))
//                 fileList.Add(item);
//         }
//
//         if (paths.Length > 0)
//         {
//             for (var i = 0; i < paths.Length; i++)
//                 a(paths[i], fileList, fileType);
//         }
//     }
//
//     /// <summary>
//     /// 按钮
//     /// </summary>
//     /// <param name="rect"></param>
//     /// <param name="info"></param>
//     /// <param name="clickedAction"></param>
//     void Button(Rect rect, string info, Action clickedAction = null)
//     {
//         if (GUI.Button(rect, info))
//             clickedAction?.Invoke();
//     }
// }
//
// enum AssetBundleBuildType
// {
//     Animation = 1,
//     Model = 2,
// }