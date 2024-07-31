using Framework.Core;
using LitJson;
using UnityEngine;

public class Asset
{
    public static bool SerializeToFile(string path, string name, object obj)
    {
        string str = JsonMapper.ToJson(obj);
        return SaveToFile(path, name, str);
    }

    public static T DeserializeFromFile<T>(string path, string name)
    {
        string json = LoadFromFile(path, name);
        return JsonMapper.ToObject<T>(json);
    }

    public static T LoadFromResources<T>(string path, string name) where T : UnityEngine.Object
    {
        return Resources.Load<T>(System.IO.Path.Combine(path, name));
    }

    public struct Path
    {
        public const string Config = "Conf/";
    }
    
    public static bool SaveToFile(string path, string name, string str)
    {
        UnityEngine.Debug.LogError(path + name + " 不能在非编辑器下保存");
        return false;
    }

    public static string LoadFromFile(string path, string name)
    {
        string result = string.Empty;
        TextAsset textAsset = Resources.Load<TextAsset>(System.IO.Path.Combine(path, name));
        if (textAsset != null)
        {
            result = textAsset.text;
        }
        else
        {
            (System.IO.Path.Combine(path, name) + " 文件不存在").Error();
        }
        return result;
    }

    public static bool IsFileExist(string path, string name)
    {
        UnityEngine.Object x = Resources.Load(System.IO.Path.Combine(path, name));
        return x != null;
    }
}