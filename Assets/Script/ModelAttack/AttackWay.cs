﻿/// <summary>
/// 攻击方式
/// </summary>
public class AttackWay : IAttack
{
    private ISkill _skill;

    public ISkill Skill { get => _skill; set => _skill = value; }
}
