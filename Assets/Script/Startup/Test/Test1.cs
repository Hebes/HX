using Framework.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Test1 : IPool// MonoBehaviour,
{
    public float DesMilliseconds => 1000;

    public void Get(object valueTuple)
    {
        Debug.Log("获取");
    }

    public void Push()
    {
        Debug.Log("推入");
    }

    //public int ID => throw new System.NotImplementedException();

    //public List<BuffData> BuffList { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    //public void GetAfter()
    //{
    //    UnityEngine.Debug.Log($"Test1出了对象池");
    //    CoreBehaviour.Add(this);
    //}

    //public void PushBefore()
    //{
    //    UnityEngine.Debug.Log($"Test1进了对象池");
    //    CoreBehaviour.Remove(this);
    //}

    //public void OnUpdata()
    //{
    //    BuffData1 buffData1 = new BuffData1();
    //    UnityEngine.Debug.Log($"Test1   Updata");
    //    //IBuffCarrier.Remove(buffData1);
    //}

    private void Awake()
    {

        // 获取当前程序集
        //Assembly assembly = Assembly.GetExecutingAssembly();

        //Debug.Log(assembly.FullName);
        // 获取程序集中的所有类型
        //Type[] types = assembly.GetTypes();

        //// 遍历所有类型
        //foreach (Type type in types)
        //{
        //    Debug.Log(type.FullName);
        //}

        //Type type = GetType();
        //System.Reflection.FieldInfo[] t1 = type.GetFields();
        //System.Reflection.MethodInfo[] ttt = type.GetMethods();
        //foreach (var item in t1)
        //{
        //    Debug.Log(item.Name);
        //}
    }

    IEnumerator MyCoroutine(Transform target)
    {
        Debug.Log("开启了协程");
        yield return null;
    }
}
