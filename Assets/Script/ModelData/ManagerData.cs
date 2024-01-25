using Core;
using System.Collections;
using System.Collections.Generic;

public class ManagerData : IModel
{
    private Dictionary<string, List<IData>> dataDic = new Dictionary<string, List<IData>>();
    private static ManagerData Instance;

    public IEnumerator Enter()
    {
        Instance = this;
        CoreData.InitData<ExcelDataItem>(ConfigData.bytesDataItem);
        List<IData> dataList = CoreData.GetDataList<ExcelDataItem>();
        ChangerDataList<ExcelDataItem, DataItem>(dataList);
        yield return null;
    }

    public IEnumerator Exit()
    {
        yield return null;
    }

    private List<K> ChangerDataList<T, K>(List<IData> dataList) where T : class, IData where K : IData, new()
    {
        List<K> list = new List<K>();
        dataDic.Add(typeof(K).FullName, new List<IData>());
        foreach (IData item in dataList)
        {
            K k = CoreData.ExchangeData<T, K>(item as T);
            dataDic[typeof(K).FullName].Add(k);
        }
        return list;
    }

    /// <summary>
    /// 获取数据
    /// </summary>
    public static T GetData<T>(int id) where T : class, IData
    {
        IData data = Instance.dataDic[typeof(T).FullName].Find((data) => { return data.GetId() == id; });
        return data as T;
    }

    /// <summary>
    /// 获取数据
    /// </summary>
    /// <returns></returns>
    public static List<T> GetList<T>() where T : class, IData
    {
        return Instance.dataDic[typeof(T).FullName] as List<T>;
    }

    
}
