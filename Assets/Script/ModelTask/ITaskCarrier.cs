using System.Collections.Generic;

public interface ITaskCarrier
{
    /// <summary>
    /// 角色人
    /// </summary>
    public int CarrierID { get; set; }

    /// <summary>
    /// 任务列表
    /// </summary>
    public List<ITask> TaskList { get; set; }
}
