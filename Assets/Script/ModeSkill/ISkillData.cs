public interface ISkillData : ISkill
{
    public uint Id { get; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; }
}
