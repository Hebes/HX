using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection.Emit;
using System.Reflection;
using Unity.VisualScripting;
using System.Dynamic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Collections;

/// <summary>
/// https://blog.csdn.net/thick__fog/article/details/117995699
/// </summary>
public class Data : ISerializableData
{
    public List<string> data;

    public void init()
    {
        ManagerSerializableData.AddSerializableData(this);
        data = new List<string>();
        data.Add("1");
        data.Add("2");
        data.Add("3");
        data.Add("4");
        ManagerSerializableData.ShowSerializableData();
    }
}

/// <summary>
/// 管理序列化数据
/// </summary>
public class ManagerSerializableData
{
    public static ManagerSerializableData Instance;
    private SerializableDataScript serializableDataScript;

    public void Init()
    {
        Instance = this;
        GameObject gameObject = new GameObject("管理序列化数据");
        serializableDataScript = gameObject.AddComponent<SerializableDataScript>();
        GameObject.DontDestroyOnLoad(gameObject);
    }

    public static void AddSerializableData(ISerializableData serializableData)
    {
        Instance.serializableDataScript.AddSerializableData(serializableData);
    }
    public static void RemoveSerializableData(ISerializableData serializableData)
    {
        Instance.serializableDataScript.RemoveSerializableData(serializableData);
    }

    public static void ShowSerializableData()
    {
        Instance.serializableDataScript.ShowSerializableData();
    }
}

public class SerializableDataScript : MonoBehaviour
{

    List<ISerializableData> serializableDataScriptList;
    public static SerializableDataScript Instance;

    private void Awake()
    {
        Instance = this;
        serializableDataScriptList = new List<ISerializableData>();
    }

    public void AddSerializableData(ISerializableData serializableData)
    {
        Instance.serializableDataScriptList.Add(serializableData);
    }
    public void RemoveSerializableData(ISerializableData serializableData)
    {
        Instance.serializableDataScriptList.Remove(serializableData);
    }

    /// <summary>
    /// 显示一条序列化信息
    /// </summary>
    public void ShowSerializableData()
    {
        //获取父类
        Type parentType = typeof(MonoBehaviour);
        //实例类型反射
        Type type = serializableDataScriptList[0].GetType();
        //反射虚拟类型
        AssemblyName assemblyName = new AssemblyName("DynamicAssembly");
        AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");
        TypeBuilder typeBuilder = moduleBuilder.DefineType("ScriptClass", TypeAttributes.Public, parentType);

        Type scriptType = typeBuilder.CreateType();
        //Type scriptInstance = (Type)Activator.CreateInstance(scriptType);
        GameObject gameObject = new GameObject("测试脚本");
        Component scriptType1 = gameObject.AddComponent(scriptType);
        Debug.Log("测试脚本");
        // 创建脚本实例
        IEnumerable result = new ExpandoObject();
        //List<int> intList = example.CreateList<int>();

        //FieldInfo listField = scriptType.GetField("myList");
        //listField.SetValue(scriptType1, new List<int>());
    }
}

/// <summary>
/// 显示在unity面板方便查看数据
/// </summary>
public interface ISerializableData
{

}
