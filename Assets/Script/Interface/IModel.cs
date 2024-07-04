using System.Collections;

/// <summary>
/// 模块
/// </summary>
public interface IModel
{
    /// <summary>
    /// 模块初始化
    /// </summary>
    public void Init();
    
    /// <summary>
    /// 模块进入
    /// </summary>
    public IEnumerator AsyncEnter();

    /// <summary>
    /// 模块退出
    /// </summary>
    public IEnumerator Exit();
}


