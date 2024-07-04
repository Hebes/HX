using System;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Framework.Core;
using UnityEngine;

public class ModelRun
{
    public static ModelRun Instance { get; set; }

    public List<IModel> modelList;

    public IEnumerator ModelInit()
    {
        Instance = this;
        modelList = new List<IModel>();

        var creatModels = Utility.Reflection.GetAttribute<ModelCreat>();
        creatModels.Sort();
        foreach (var creatModel in creatModels)
        {
            var model = (IModel)Activator.CreateInstance(creatModel.Type);
            Debug.Log($"{creatModel.Type.Name}初始化,序列号为{creatModel.Num}");
            var type = model.GetType();
            var init = type.GetMethod("Init");
            init?.Invoke(model, new object[] { });
            var asyncInit = type.GetMethod("AsyncInit");
            yield return asyncInit?.Invoke(model, new object[] { });
        }

        //await ModeEnter<ManagerData>();           //数据
        yield return ModeEnter<ManagerSave>(); //存档
        yield return ModeEnter<ManagerScene>(); //场景
        yield return ModeEnter<ManagerRPGBattle>(); //战斗
    }

    /// <summary>
    /// 模块进入->初始化
    /// </summary>
    private IEnumerator ModeEnter<T>() where T : IModel, new()
    {
        T t = new T();
        modelList.Add(t);
        yield return t.AsyncEnter();
    }

    /// <summary>
    /// 模块退出
    /// </summary>
    public static IEnumerator ModelExit()
    {
        foreach (IModel item in Instance.modelList)
            yield return item.Exit();
    }
}