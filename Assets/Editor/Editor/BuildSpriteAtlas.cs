// using System;
// using System.IO;
// using System.Linq;
// using UnityEditor;
// using UnityEditor.U2D;
// using UnityEngine;
// using UnityEngine.U2D;
// using Object = UnityEngine.Object;
//
// public class BuildSpriteAtlas
// {
//     private static readonly string targetPath = Application.dataPath + "\\SpriteAtlas";
//
//     [MenuItem("Tools/BuildSpriteAtlas")]
//     public static void BuildSpriteAtlas_()
//     {
//         //拿到当前选中的object
//         var selectionObjectList = Selection.objects.ToList();
//         //过滤无用的资源
//         for (var i = 0; i < selectionObjectList.Count; i++)
//             if (!(selectionObjectList[i] is DefaultAsset))
//             {
//                 selectionObjectList.RemoveAt(i);
//                 i--;
//             }
//
//         if (selectionObjectList.Count < 1)
//         {
//             EditorUtility.DisplayDialog("Unity", "请重新选择要打包SpriteAtlas的文件夹", "确定");
//             return;
//         }
//
//         for (var i = 0; i < selectionObjectList.Count; i++)
//         {
//             var path = AssetDatabase.GetAssetOrScenePath(selectionObjectList[i]); //获取objecct所在的路径
//             path = Path.Combine(
//                 Application.dataPath.Remove(Application.dataPath.Length - "Assets".Length, "Assets".Length), path);
//             CreateAnimClip(path);
//         }
//         AssetDatabase.Refresh();
//     }
//
//     static void CreateAnimClip(string path)
//     {
//         var images = Directory.CreateDirectory(path).GetFiles("*.png");
//         var spriteAtlas = new SpriteAtlas();
//         Object[] sprites = new Object[images.Length];
//         for (var i = 0; i < images.Length; i++)
//         {
//             var str = images[i].FullName.Split('\\');
//             var o = "";
//             for (var j = 3; j < str.Length; j++)
//             {
//                 if (j == str.Length - 1)
//                     o += str[j];
//                 else
//                     o += str[j] + "\\";
//             }
//
//             sprites[i] = AssetDatabase.LoadAssetAtPath<Sprite>(o);
//             if(!sprites[i])
//                 throw new Exception("获取失败+"+o);
//         }
//
//         spriteAtlas.Add(sprites);
//         
//         var settings = spriteAtlas.GetPlatformSettings("Default");
//         settings.maxTextureSize = 4096;
//         settings.crunchedCompression = true;
//         settings.compressionQuality = 50;
//         spriteAtlas.SetPlatformSettings(settings);
//         
//         var st = path.Split('/');
//         var p = targetPath + "";
//         var nowTragetPath = p + "\\" + st[st.Length - 2];
//         if (!Directory.Exists(nowTragetPath))
//             Directory.CreateDirectory(nowTragetPath);
//         //图集所在目录
//         var nowTragetFilePath =
//             "Assets\\SpriteAtlas" + "\\" + st[st.Length - 2] + "\\" + st[st.Length - 1] + ".spriteatlas";
//         AssetDatabase.CreateAsset(spriteAtlas, nowTragetFilePath);
//     }
// }