using Core;

/// <summary>
/// 故事的接口（PS:就是剧情）
/// </summary>
public interface IStory : IID
{

}

/// <summary>
/// 故事的生命周期
/// </summary>
public interface IStoryBehaviour : IStory
{
    public void Trigger();
    public void Over();
}
