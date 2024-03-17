using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

public class ModelRun
{
    public static ModelRun Instance { get; set; }

    public List<IModel> modelList;

    public IEnumerator ModelInit()
    {
        Instance = this;
        modelList = new List<IModel>();

        //await ModeEnter<ManagerData>();           //数据
        yield return ModeEnter<ManagerSave>();      //存档
        yield return ModeEnter<ManagerScene>();     //场景
        yield return ModeEnter<ManagerRPGBattle>(); //战斗
    }

    /// <summary>
    /// 模块进入->初始化
    /// </summary>
    private IEnumerator ModeEnter<T>() where T : IModel, new()
    {
        T t = new T();
        modelList.Add(t);
        yield return t.Enter();
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
