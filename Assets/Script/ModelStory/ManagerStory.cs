using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary> 故事管理器 </summary>
public class ManagerStory : IModelInit
{
    public static ManagerStory Instance { get; set; }
    public void Init()
    {
        Instance = this;
    }
}

