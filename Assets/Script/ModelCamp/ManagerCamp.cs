using System.Collections.Generic;


/*--------脚本描述-----------

描述:
	阵营管理器

-----------------------*/

public class ManagerCamp : IModelInit
{
    private Dictionary<EnumsCamp, List<ICamp>> _campDic;
    public static ManagerCamp Instance;

    public void Init()
    {
        Instance = this;
        _campDic = new Dictionary<EnumsCamp, List<ICamp>>();
    }
    /// <summary>
    /// 添加到指定阵营
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="id"></param>
    public static void AddCamp(EnumsCamp ecamp, ICamp camp)
    {
        if (Instance._campDic.TryGetValue(ecamp, out List<ICamp> campList))
            campList.Add(camp);
        Instance._campDic.Add(ecamp, new List<ICamp>() { camp });
    }

    /// <summary>
    /// 移除阵营中指定的
    /// </summary>
    /// <param name="id"></param>
    public static void RemoveCamp(EnumsCamp ecamp, ICamp camp)
    {
        if (Instance._campDic.TryGetValue(ecamp, out List<ICamp> campList))
            campList.Remove(camp);
    }

    public static ICamp GetCamp(EnumsCamp ecamp, ICamp camp)
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
    public static T GetCamp<T>(EnumsCamp ecamp, ICamp camp) where T : class, ICamp
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
    public static void ChangeCamp(EnumsCamp oldECamp, ICamp oldCamp, EnumsCamp newECamp, ICamp newCamp)
    {
        RemoveCamp(oldECamp, oldCamp);
        AddCamp(newECamp, newCamp);
    }


}
