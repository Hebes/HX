using System.Collections.Generic;

/*--------脚本描述-----------

描述:
	任务管理

-----------------------*/

public class ManagerTask : IModelInit
{
    public static ManagerTask Instance { get; private set; }

    private List<ITaskCarrier> _taskCarrierList;
    public void Init()
    {
        Instance = this;
        _taskCarrierList = new List<ITaskCarrier>();
    }

    public void RefreshTask()
    {

    }

    /// <summary>
    /// 添加任务
    /// </summary>
    public static void AddTask(ITaskCarrier taskCarrier, ITask task)
    {
        //taskCarrier.TaskList.Add(task);
        //task.TaskTrigger();
    }


    public static void RemoveTask(ITaskCarrier taskCarrier, ITask task)
    {
        //if (!taskCarrier.TaskList.Contains(task)) return;
        //taskCarrier.TaskList.Remove(task);
        //task.TaskOver();
    }

    private static void TaskAddCarrier(ITaskCarrier taskCarrier)
    {
        //if (!Instance._taskCarrierList.Contains(taskCarrier))
        //    Instance._taskCarrierList.Add(taskCarrier);
    }
}
