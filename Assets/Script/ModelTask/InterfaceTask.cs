using System.Collections.Generic;

public interface ITask : IID, IName, IDescribe
{
    /// <summary>
    /// 任务类型
    /// </summary>
    public EnumTask taskType { get; set; }

    /// <summary>
    /// 任务进行的状态
    /// </summary>
    public EnumTaskStatus taskStatus { get; set; }
}

/// <summary>
/// 任务的生命周期
/// </summary>
public interface ITaskBehaviour : IID
{
    /// <summary>
    /// 任务触发
    /// </summary>
    public void TaskTrigger();

    /// <summary>
    /// 任务结束
    /// </summary>
    public void TaskOver();
}

/// <summary>
/// 任务的持有者 每个NPC或者团队必须继承这个才有任务
/// </summary>
public interface ITaskCarrier : IID
{
    /// <summary>
    /// 任务列表
    /// </summary>
    public List<ITask> TaskList { get; set; }

    public static void AddTask(ITaskCarrier taskCarrier , ITask task)
    {
        if (taskCarrier.TaskList==null)
            taskCarrier.TaskList = new List<ITask>();
        taskCarrier.TaskList.Add(task);
    }

    public static void RemoveTask(ITaskCarrier taskCarrier, ITask task)
    {
        taskCarrier.TaskList.Remove(task);
    }
}

public static class HelperTask
{
    public static void AddTask(this ITaskCarrier taskCarrier, ITask task) => ITaskCarrier.AddTask(taskCarrier, task);
    public static void RemoveTask(this ITaskCarrier taskCarrier, ITask task) => ITaskCarrier.RemoveTask(taskCarrier, task);
}
