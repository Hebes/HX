/// <summary>
/// 流程状态机节点
/// </summary>
public interface IProcessStateNode
{
    /// <summary>
    /// 创建出来的时候会执行
    /// </summary>
    /// <param name="obj"></param>
    void OnCreate(ProcessFsmSystem obj);
    
    /// <summary>
    /// 状态切换的时候会执行
    /// </summary>
    void OnEnter(object obj);

    /// <summary>
    /// 轮询
    /// </summary>
    void OnUpdate();

    /// <summary>
    /// 退出
    /// </summary>
    void OnExit();
}