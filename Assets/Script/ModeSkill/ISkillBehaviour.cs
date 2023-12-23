using Core;

public interface ISkillBehaviour : IUpdata
{
    /// <summary>
    /// 表现效果
    /// </summary>
    void Trigger();

    /// <summary>
    /// 技能结束
    /// </summary>
    void Over();
}
