using Core;
using Farm2D;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

public class ModelRun
{
    public ModelRun()
    {
        ModelInit<ManagerLanguage>();   //数据
        ModelInit<ManagerData>();   //数据
        ModelInit<ManagerSave>();   //存档
        ModelInit<ManagerScene>();  //场景
    }

    /// <summary>
    /// 模块初始化
    /// </summary>
    private T ModelInit<T>() where T : IModelInit, new()
    {
        T t = new T();
        t.Init();
        return t;
    }
}
