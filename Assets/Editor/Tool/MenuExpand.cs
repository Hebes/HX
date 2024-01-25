using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 菜单扩展
/// </summary>
public class MenuExpand
{
    [RuntimeInitializeOnLoadMethod]
    public static void NewMenuItem1()
    {
        //顶部菜单
    }

    [MenuItem("GameObject/NewMenuItem", priority = 11)]
    public static void NewMenuItem2()
    {
        //顶部和hierarchy菜单
    }

    /// <summary>
    ///Project菜单
    /// </summary>
    [MenuItem("Assets/创建State对应的Data", false, 80)]
    public static void CreateStateData()
    {
        var a = GetPath("AiStateManagerData").Replace("Assets", "");
        
        
        var dataPath = Application.dataPath;
        var combine = dataPath + a;
        if (!File.Exists(combine))
            throw new Exception("AiStateManagerData脚本不存在");
        var aiStateManagerDataStr = File.ReadAllText(combine);
        var nowSelection = Selection.objects;
        var stateType = StateDataTool.Assembly.GetType("State");
        var stateTypeList = new List<Type>();

        for (var i = 0; i < nowSelection.Length; i++)
        {
            if (!(nowSelection[i] is MonoScript)) continue;
            var type = StateDataTool.Assembly.GetType(nowSelection[i].name);
            if (type.IsAbstract) continue;
            if (type.BaseType == stateType || type.BaseType.BaseType == stateType)
            {
                var stateData = StateDataTool.Assembly.GetType(type.Name + "Data");
                if (stateData != default)
                    throw new Exception("已经包含的状态数据：" + type.Name);
                stateTypeList.Add(type);
            }
        }

        for (var i = 0; i < stateTypeList.Count; i++)
        {
            StateDataTool.CreateStateData(stateTypeList[i]);
            //字符串替换
            aiStateManagerDataStr = aiStateManagerDataStr.Replace("//AutoTool",
                $"public List<{stateTypeList[i].Name}Data> {stateTypeList[i].Name}DataList;\n\r//AutoTool");
        }

        //写入AiStateManagerData脚本
        File.WriteAllText(combine, aiStateManagerDataStr);

        var info = "StateData创建完毕";
        for (var i = 0; i < stateTypeList.Count; i++)
            info += "\n" + stateTypeList[i].Name;
        EditorUtility.DisplayDialog("CreateStateData", info, "ok");
    }

    [MenuItem("Assets/输出当前选中代码的信息", false, 80)]
    public static void PrintNowSelectionScriptInfo()
    {
        var fileNameList = new List<string>();
        foreach (var path in Selection.objects.Select(t => AssetDatabase.GetAssetPath(t)))
        {
            if (Directory.Exists(path))
                fileNameList.AddRange(Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories));
            else if (File.Exists(path))
            {
                var fileInfo = new FileInfo(path);
                if (fileInfo.Extension == ".cs")
                    fileNameList.Add(path);
            }
        }

        var totalLine = 0; //总行数
        var totalWord = 0; //总字数
        foreach (var temp in fileNameList)
        {
            var nowLine = 0;
            var sr = new StreamReader(temp);
            var readLine = sr.ReadLine();
            while (readLine != null)
            {
                nowLine++;
                totalWord += readLine.Length;
                readLine = sr.ReadLine();
            }

            //文件名+文件行数
            //Debug.Log($"{temp}——{nowLine}");
            totalLine += nowLine;
        }

        EditorUtility.DisplayDialog("Unity",
            $"总代码行数：{totalLine}\n" +
            $"总代码字数：{totalWord}\n" +
            $"总cs文件数量：{fileNameList.Count}", "ok");
    }

    /// <summary>
    /// ab包名字矫正
    /// </summary>
    [MenuItem("Assets / ab包名字矫正")]
    public static void AnimationABName()
    {
        var nowSelectionObj = Selection.objects.First();
        if (!(nowSelectionObj is DefaultAsset)) return;
        var animationShapeType = nowSelectionObj.name;
        var animationShapePath = $"{Application.streamingAssetsPath}/AssetBundles/Animation/{animationShapeType}";
        if (!Directory.Exists(animationShapePath))
            throw new Exception("错误的类型" + animationShapeType);
        var fileInfoList = new List<FileInfo>();
        foreach (var filePath in Directory.GetFiles(animationShapePath))
            fileInfoList.Add(new FileInfo(filePath));
        for (var i = 0; i < fileInfoList.Count; i++)
        {
            if (fileInfoList[i].Extension == ".meta")
            {
                //删除所有的meta文件（等待重新刷新）
                fileInfoList[i].Delete();
            }
            else if (false) //多点了一次矫正之后使用，出现两个前缀，删除第一个前缀
            {
                var oldPath = fileInfoList[i].FullName;
                var paths = oldPath.Split('\\');
                var newPath = "";
                for (var j = 0; j < paths.Length - 1; j++)
                    newPath += $"{paths[j]}/";
                newPath +=
                    $"{fileInfoList[i].Name.Split('.')[0].Remove(0, "AlloyArmor_".Length)}{fileInfoList[i].Extension}";
                fileInfoList[i].MoveTo(newPath);
            }
            else
            {
                var oldPath = fileInfoList[i].FullName;
                var paths = oldPath.Split('\\');
                var newPath = "";
                for (var j = 0; j < paths.Length - 1; j++)
                    newPath += $"{paths[j]}/";
                newPath += $"{animationShapeType}_{fileInfoList[i].Name.Split('.')[0]}{fileInfoList[i].Extension}";
                fileInfoList[i].MoveTo(newPath);
            }
        }

        AssetDatabase.Refresh();
    }

    [MenuItem("Tools / 创建常用文件夹")]
    public static void CreateFolder()
    {
        return;
        string path = Application.dataPath + "/";
        Directory.CreateDirectory(path + "Resources");
        Directory.CreateDirectory(path + "Assets");
        Directory.CreateDirectory(path + "Plugins");
        Directory.CreateDirectory(path + "StreamingAssets");
        Directory.CreateDirectory(path + "Editor");
        Directory.CreateDirectory(path + "Scenes");
        Directory.CreateDirectory(path + "Scripts");
    }

    [MenuItem("Assets/动画ab包数量检测", false, 80)]
    public static void AnimationABFileCheck()
    {
        var selection = Selection.objects.First();
        if (!selection)
            return;
        AnimationExamine(selection.name);
    }

    /// <summary>
    /// shader引用查找
    /// </summary>
    [MenuItem("Assets / shader引用查找")]
    public static void ShaderCiteFind()
    {
        Shader selectedShader = Selection.activeObject as Shader;
        if (selectedShader != null)
        {
            string[] guids = AssetDatabase.FindAssets("t:Material");
            bool use = false;
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (material.shader == selectedShader)
                {
                    use = true;
                    Debug.Log("Material " + material.name + " 在项目中的路径是：" + path);
                }
            }

            if (!use)
                Debug.Log("选中的" + selectedShader + " 未被任何材质球使用");
        }
        else
        {
            Debug.Log("请在Project面板中选中一个Shader");
        }
    }

    /// <summary>
    /// 获取当前项目某个脚本的路径
    /// </summary>
    /// <param name="scriptName">脚本的名字</param>
    /// <returns></returns>
    static string GetPath(string scriptName)
    {
        var paths = AssetDatabase.FindAssets(scriptName);
        if (paths.Length > 1)
        {
            Debug.LogError("有同名文件" + scriptName + "获取路径失败");
            return null;
        }

        //将字符串中得脚本后缀去除掉
        return AssetDatabase.GUIDToAssetPath(paths[0]);
    }

    /// <summary>
    /// 动画数量检测
    /// </summary>
    static void AnimationExamine(string animationShapeType)
    {
        var AnimationShapeType = StateDataTool.Assembly.GetType("AnimationShapeType");
        var animationShapeTypes = AnimationShapeType.GetFields();

        var contains = false;
        for (var i = 0; i < animationShapeTypes.Length; i++)
        {
            if (animationShapeTypes[i].Name == animationShapeType)
            {
                contains = true;
                break;
            }
        }

        if (!contains)
        {
            EditorUtility.DisplayDialog("Unity", $"错误的类型:{animationShapeType}", "ok");
            return;
        }

        //动画ab包获取
        var directoryPath = Application.streamingAssetsPath + @"\AssetBundles\Animation\" + animationShapeType;
        var abFilePathS = Directory.GetFiles(directoryPath);
        var fileList = new List<string>();
        var extension = ".lf";
        foreach (var abFile in abFilePathS)
        {
            var abFileInfo = new FileInfo(abFile);
            if (abFileInfo.Extension == extension)
                fileList.Add(abFileInfo.Name.Split('.')[0]);
        }

        //转小写
        fileList = fileList.Select(file => file.ToLower()).ToList();

        //动画枚举获取
        var spriteAnimation_Type = StateDataTool.Assembly.GetType($"SpriteAnimation_{animationShapeType}Type");
        var spriteAnimation_TypeList = spriteAnimation_Type.GetFields()
            .Select(spriteAnimation_Type => $"{animationShapeType}_" + spriteAnimation_Type.Name)
            .Where(spriteAnimation_Type => spriteAnimation_Type.Split('_').Last() != "reverse")
            .ToList();
        spriteAnimation_TypeList.RemoveAt(0);

        //转小写
        spriteAnimation_TypeList =
            spriteAnimation_TypeList.Select(spriteAnimation => spriteAnimation.ToLower()).ToList();

        for (var i = 0; i < fileList.Count; i++)
        {
            if (spriteAnimation_TypeList.Contains(fileList[i]))
            {
                spriteAnimation_TypeList.Remove(fileList[i]);
                fileList.RemoveAt(i);
                i--;
            }
        }

        //多余的枚举（没有对应动画）
        var typeStr = "";
        //多余的动画（没有对应的枚举）
        var fileStr = "";
        foreach (var spriteAnimationType in spriteAnimation_TypeList)
            typeStr += spriteAnimationType + "\n";

        foreach (var file in fileList)
            fileStr += file + "\n";

        EditorUtility.DisplayDialog("Unity",
            $"多余的枚举:\n{typeStr}\n多余的动画:\n{fileStr}",
            "ok");
    }
}