// using UnityEngine;
// using UnityEditor;
// using System.IO;
// public class BuildAnimation
// {
//     private static float _frameRate = 24;
//     private static float _frameTime = 1f;
//     
//     /// <summary>
//     /// 序列帧路径
//     /// </summary>
//     private string sourcePath = "";
//     /// <summary>
//     /// 输出的目标路径
//     /// </summary>
//     private string targetPath = "";
//
//     private bool start;
//     private DirectoryInfo[] diArr;
//     private int index=0;
//
//     public void OnGUI()
//     {
//         //不包含Asste文件夹
//         //格式例如：@"\***\****"
//         //格式例如："\\***\\****"
//         if (!start)
//         {
//             sourcePath = GUI.TextField(new Rect(10, 180, 280, 20), sourcePath);
//             if (sourcePath == "" || sourcePath == " ")
//                 sourcePath = "序列帧路径";
//             targetPath = GUI.TextField(new Rect(10, 220, 280, 20), targetPath);
//             if (targetPath == "" || targetPath == " ")
//                 targetPath = "输出路径";
//             if (GUI.Button(new Rect(300, 200, 100, 30), "创建Anim"))
//             {
//                 var st = Application.dataPath+"\\" + sourcePath;
//                 if (Directory.Exists(st)) //判断当前文件路径是否存在
//                 {
//                     diArr = new DirectoryInfo(st).GetDirectories();
//                     start = true;
//                     index = 0;
//                 }
//                 else
//                 {
//                     EditorUtility.DisplayDialog("错误",
//                         "输入的文件路径不存在！查看路径格式是否正确！\n如：格式例如：@“\\Ass文件下的一个文件名\\存放每组序列帧动画的父文件夹名”\n例如：“\\\\***\\\\”", "OK");
//                     AssetDatabase.Refresh();
//                 }
//             }
//         }
//         else
//         {
//             GUI.Label(new Rect(100, 100, 100, 100), $"{index}\\{diArr.Length}");
//             CreateAnimClip(diArr[index]);
//             index++;
//             if (index >= diArr.Length)
//             {
//                 start = false;
//                 EditorUtility.DisplayDialog("完成", "Animation生成成功", "OK");
//                 AssetDatabase.Refresh(); //刷新Assets文件目录 
//             }
//         }
//     }
//     
//     void CreateAnimClip(DirectoryInfo a)
//     {
//         var images = a.GetFiles("*.png");
//         var keyFrames = new ObjectReferenceKeyframe[images.Length];
//         var time = _frameTime / _frameRate;
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
//             var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(o);
//             keyFrames[i] = new ObjectReferenceKeyframe {time = time * i, value = sprite};
//         }
//
//         var clip = new AnimationClip {frameRate = _frameRate};
//
//         var clipSettings = AnimationUtility.GetAnimationClipSettings(clip);
//         clipSettings.loopTime = true;
//         AnimationUtility.SetAnimationClipSettings(clip, clipSettings);
//         var curveBinding = new EditorCurveBinding
//         {
//             type = typeof(SpriteRenderer), propertyName = "m_Sprite"
//         };
//         AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyFrames);
//
//         var stri = a.FullName.Split('\\');
//         var path = Application.dataPath+"\\" + targetPath+"\\" + stri[5] + "\\Clip";
//         if (!Directory.Exists(path))
//         {
//             Directory.CreateDirectory(path);
//         }
//         AssetDatabase.CreateAsset(clip, stri[3] + "\\"+targetPath+"\\" + stri[5] + "\\Clip\\" + a.Name + ".anim");
//     }
// }