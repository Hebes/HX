using System.Collections.Generic;

public interface ITask : IID, IName, IDescribe
{
    /// <summary>
    /// 任务名称
    /// </summary>
    //public string TaskName { get; set; }

    /// <summary>
    /// 任务说明
    /// </summary>
    //public string TaskDescription { get; set; }

    /// <summary>
    /// 任务类型
    /// </summary>
    public EnumTask taskType { get; set; }

    /// <summary>
    /// 任务进行的状态
    /// </summary>
    public EnumTaskStatus taskStatus { get; set; }

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
/// 每个NPC或者团队必须继承这个才有任务
/// </summary>
public interface ITaskCarrier : IID
{
    /// <summary>
    /// 任务列表
    /// </summary>
    public List<ITask> TaskList { get; set; }
}
