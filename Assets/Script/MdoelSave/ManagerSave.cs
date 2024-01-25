using System.Collections;
using System.Collections.Generic;
using Core;

/*--------脚本描述-----------

描述:
	存读档

-----------------------*/

public class ManagerSave : IModel
{
    public static ManagerSave Instance { get; private set; }
    private List<ISave> _saveList;
    public IEnumerator Enter()
    {
        Instance = this;
        _saveList = new List<ISave>();
        yield return null;
    }

    public IEnumerator Exit()
    {
        yield return null;
    }

    /// <summary>
    /// 注册保存
    /// </summary>
    public static void RegisterSavea(ISave save)
    {
        if (Instance._saveList.Contains(save))
        {
            Debug.Log($"保存的已经存在{save.GetType().FullName}");
            return;
        }
        Instance._saveList.Add(save);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    public static void Save()
    {
        foreach (ISave save in Instance._saveList)
            save.Save();
    }

    /// <summary>
    /// 加载数据
    /// </summary>
    public static void Load()
    {
        //先加载数据，然后把数据传递进去
        SaveData saveData = null;
        foreach (ISave save in Instance._saveList)
            save.Load(saveData);
    }

   
}
