using System;
using System.Collections.Generic;
using Framework.Core;

/// <summary>
/// 单独AI拥有
/// </summary>
public interface IAI:IID
{
    public Dictionary<EAIType, List<EAIExecute>> AIDic { get; set; }
}

/// <summary>
/// AI状态-》给每个AI状态继承的
/// </summary>
public class AiStata
{

}

/// <summary>
/// AI思想接口
/// </summary>
public interface EAIExecute
{
    public long AIExecuteType { get; set; }//AI执行事件的类型,比如买东西
}

public interface EAIExecuteTry : EAIExecute
{
    /// <summary>
    /// 尝试委托
    /// </summary>
    //public delegate bool AIExecuteTry();
}

public interface EAIExecuteAction : EAIExecute
{
    /// <summary>
    /// 执行委托
    /// </summary>
    //public delegate void AIExecuteAction();
}
