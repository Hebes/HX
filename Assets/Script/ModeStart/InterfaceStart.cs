using System.Collections;

/// <summary>
/// 描述
/// </summary>
public interface IDescribe
{
    /// <summary>
    /// 描述
    /// </summary>
    string Des { get; set; }
}

public interface IID
{
    /// <summary>
    /// ID
    /// </summary>
    public long ID { get; set; }
}

/// <summary>
/// 模块
/// </summary>
public interface IModel
{

    /// <summary>
    /// 模块进入
    /// </summary>
    public IEnumerator Enter();

    /// <summary>
    /// 模块退出
    /// </summary>
    public IEnumerator Exit();
}

/// <summary>
/// 名称
/// </summary>
public interface IName
{
    /// <summary>
    /// 名称
    /// </summary>
    string Name { get; set; }
}
