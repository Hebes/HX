using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

public class ModelRun
{
    public static ModelRun Instance { get; set; }

    public List<IModel> modelList;

    public ModelRun()
    {
        Instance = this;
        modelList = new List<IModel>();
        Start().Forget();
    }

    private async UniTask Start()
    {
        await ModeEnter<ManagerLanguage>();   //数据
        await ModeEnter<ManagerData>();       //数据
        await ModeEnter<ManagerSave>();       //存档
        await ModeEnter<ManagerScene>();      //场景
        await ModeEnter<ManagerRPGBattle>();      //战斗
    }

    /// <summary>
    /// 模块进入->初始化
    /// </summary>
    private async UniTask<T> ModeEnter<T>() where T : IModel, new()
    {
        T t = new T();
        modelList.Add(t);
        await t.Enter();
        return t;
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
