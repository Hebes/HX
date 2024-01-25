using System.Collections;
using System.Collections.Generic;


/*--------脚本描述-----------

描述:
	阵营管理器

-----------------------*/

public class ManagerCamp : IModel
{
    private Dictionary<ECamp, List<ICamp>> _campDic;
    public static ManagerCamp Instance;

    public IEnumerator Enter()
    {
        Instance = this;
        _campDic = new Dictionary<ECamp, List<ICamp>>();
        yield return null;
    }

    public IEnumerator Exit()
    {
        yield return null;
    }

    /// <summary>
    /// 添加到指定阵营
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="id"></param>
    public static void AddCamp(ECamp ecamp, ICamp camp)
    {
        if (Instance._campDic.TryGetValue(ecamp, out List<ICamp> campList))
            campList.Add(camp);
        Instance._campDic.Add(ecamp, new List<ICamp>() { camp });
    }

    /// <summary>
    /// 移除阵营中指定的
    /// </summary>
    /// <param name="id"></param>
    public static void RemoveCamp(ECamp ecamp, ICamp camp)
    {
        if (Instance._campDic.TryGetValue(ecamp, out List<ICamp> campList))
            campList.Remove(camp);
    }

    public static ICamp GetCamp(ECamp ecamp, ICamp camp)
    {
        if (Instance._campDic.TryGetValue(ecamp, out List<ICamp> campList))
        {
            for (int i = 0; i < campList.Count; i++)
            {
                if (campList[i] == camp)
                    return campList[i];
            }
        }
        return null;
    }
    public static T GetCamp<T>(ECamp ecamp, ICamp camp) where T : class, ICamp
    {
        if (Instance._campDic.TryGetValue(ecamp, out List<ICamp> campList))
        {
            for (int i = 0; i < campList.Count; i++)
            {
                if (campList[i] == camp)
                    return campList[i] as T;
            }
        }
        return null;
    }


    /// <summary>
    /// 改变阵营
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="id"></param>
    /// <param name="oldCamp"></param>
    /// <param name="newCamp"></param>
    public static void ChangeCamp(ECamp oldECamp, ICamp oldCamp, ECamp newECamp, ICamp newCamp)
    {
        RemoveCamp(oldECamp, oldCamp);
        AddCamp(newECamp, newCamp);
    }
}
