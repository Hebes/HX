using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ITask
{
    /// <summary>
    /// 任务名称
    /// </summary>
    public string taskName { get; set; }

    /// <summary>
    /// 任务说明
    /// </summary>
    public string taskDescription { get; set; }

    /// <summary>
    /// 任务类型
    /// </summary>
    public EnumTaskType taskType { get; set; }

    /// <summary>
    /// 任务进行的状态
    /// </summary>
    public EnumTaskStatus taskStatus { get; set; }
}
