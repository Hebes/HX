using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ITask
{
    /// <summary>
    /// 任务ID->重复的任务可能会用到
    /// </summary>
    public int TaskID { get; set; }

    /// <summary>
    /// 任务名称
    /// </summary>
    public string TaskName { get; set; }

    /// <summary>
    /// 任务说明
    /// </summary>
    public string TaskDescription { get; set; }

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
