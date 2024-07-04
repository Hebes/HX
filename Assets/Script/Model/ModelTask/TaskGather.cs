using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 采集任务
/// </summary>
public class TaskGather : ITask
{
    public EnumTask taskType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public EnumTaskStatus taskStatus { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public long ID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public string Des { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void TaskOver()
    {
    }

    public void TaskTrigger()
    {
    }
}
