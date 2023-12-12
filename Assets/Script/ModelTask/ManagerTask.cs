using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*--------脚本描述-----------

描述:
	任务管理

-----------------------*/

public class ManagerTask : IModelInit
{
    public static ManagerTask Instance { get; private set; }

    private Dictionary<int, List<ITask>> _taskDic;
    public void Init()
    {
        Instance = this;
        _taskDic = new Dictionary<int, List<ITask>>();
    }

    public void RefreshTask()
    {

    }

    /// <summary>
    /// 添加任务
    /// </summary>
    public void AddTask(int id, ITask task)
    {
        if (Instance._taskDic.TryGetValue(id, out List<ITask> taskList))
            taskList.Add(task);
        Instance._taskDic.Add(id, new List<ITask>() { task });
    }


    public void RemoveTask(int id, ITask task)
    {
        if (Instance._taskDic.TryGetValue(id, out List<ITask> taskList))
            taskList.Remove(task);
    }
}
