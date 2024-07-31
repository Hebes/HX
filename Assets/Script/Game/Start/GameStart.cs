using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Framework.Core;
using UnityEngine;

/// <summary>
/// 游戏开始
/// </summary>
public class GameStart
{
    /// <summary>
    /// 游戏启动器
    /// </summary>
    public void GameRun()
    {
        CoreIEnumeratorInit().StartCoroutine();
        return;

        IEnumerator CoreIEnumeratorInit()
        {
            yield return new CoreRun().FrameworkCoreRun();//启动框架核心
            yield return GameStartRun(); //启动游戏管理器
            
            //开启UI
            CoreUI.Instance.PreLoadWindow<UIStart>();
            CoreUI.Instance.PreLoadWindow<UIOption>();
            CoreUI.Instance.PreLoadWindow<UICutTo>();
            CoreUI.Instance.PreLoadWindow<UIPause>();

            //显示窗口
            CoreUI.Instance.PopUpWindow<UIStart>();
        }
    }

    private IEnumerator GameStartRun()
    {
        var instanceList = new List<CreateCore>();
        var assembly = Assembly.GetExecutingAssembly();
        var types = assembly.GetTypes();
        // 遍历所有类型
        foreach (var type in types)
        {
            if (!Attribute.IsDefined(type, typeof(CreateCore))) continue;
            var attribute = Attribute.GetCustomAttribute(type, typeof(CreateCore));
            instanceList.Add((CreateCore)attribute);
        }

        //排序
        instanceList.Sort();
        //执行
        foreach (var instanceValue in instanceList)
        {
            if (!typeof(ICore).IsAssignableFrom(instanceValue.Type))
                throw new Exception($"{instanceValue.Type.Name}请继承ICore接口");
            var instance = Activator.CreateInstance(instanceValue.Type);
            Debug.Log($"{instanceValue.Type.Name}初始化,序列号为{instanceValue.NumberValue}");
            var type = instance.GetType();
            //普通方法
            var init = type.GetMethod("Init");
            init?.Invoke(instance, new object[] { });
            //协程方法
            var asyncInit = type.GetMethod("AsyncInit");
            yield return asyncInit?.Invoke(instance, new object[] { });
        }
    }
}