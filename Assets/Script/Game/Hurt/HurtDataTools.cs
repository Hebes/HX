/// <summary>
/// 伤害数据工具
/// </summary>
public class HurtDataTools
{
    /// <summary>
    /// 反击
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="groundOnly"></param>
    /// <param name="actionInterrupt"></param>
    /// <param name="eAttr"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static bool Counterattack(int damage, bool groundOnly, ref bool actionInterrupt, ref EnemyAttribute eAttr, ref EnemyBaseAction action)
    {
        if (!eAttr.canCounterAttack)
        {
            return false;
        }

        if (damage == 0)
        {
            return false;
        }

        eAttr.currentCounterAttackValue += damage;
        bool flag = (float)UnityEngine.Random.Range(0, 100) < eAttr.currentCounterAttackProbPercentage;
        bool flag2 = eAttr.currentCounterAttackValue >= eAttr.counterAttack && flag && groundOnly && !action.IsInWeakSta();
        if (flag2)
        {
            eAttr.currentCounterAttackValue = 0;
            eAttr.currentCounterAttackProbPercentage = (float)eAttr.counterAttackProbPercentage;
            eAttr.currentActionInterruptPoint = 0;
            actionInterrupt = false;
        }
        else
        {
            eAttr.currentCounterAttackProbPercentage += (float)eAttr.counterAttackProbPercentage;
        }

        return flag2;
    }

    /// <summary>
    /// 添加动作中断点
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="atkName"></param>
    /// <param name="eAttr"></param>
    /// <param name="actionInterrupt"></param>
    public static void AddActionInterruptPoint(int damage, string atkName, ref EnemyAttribute eAttr, ref bool actionInterrupt)
    {
        if (!eAttr.hasActionInterrupt)
        {
            return;
        }

        if (atkName.IsInArray(PlayerAtkType.LightAttack))
        {
            return;
        }

        int currentActionInterruptPoint = eAttr.currentActionInterruptPoint;
        eAttr.currentActionInterruptPoint += damage;
        actionInterrupt =
            (currentActionInterruptPoint < eAttr.actionInterruptPoint && eAttr.currentActionInterruptPoint >= eAttr.actionInterruptPoint);
    }

    /// <summary>
    /// 计算怪物防御
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="defenceTrigger"></param>
    /// <param name="action"></param>
    /// <param name="eAttr"></param>
    /// <returns></returns>
    public static bool CalculateMonsterDefence(int damage, ref float defenceTrigger, ref EnemyBaseAction action, ref EnemyAttribute eAttr)
    {
        if (action.IsInWeakSta())
        {
            return false;
        }

        if (action.IsInDefenceState())
        {
            return false;
        }

        if (eAttr.monsterDefence <= 0)
        {
            return false;
        }

        if (damage == 0)
        {
            return false;
        }

        eAttr.currentDefence += damage;
        eAttr.startDefence = (eAttr.currentDefence >= eAttr.monsterDefence);
        bool flag = eAttr.startDefence && eAttr.isOnGround;
        bool flag2 = false;
        if (flag)
        {
            flag2 = ((float)UnityEngine.Random.Range(0, 100) < defenceTrigger);
            if (!flag2)
            {
                defenceTrigger += 20f;
            }
        }

        return flag && flag2;
    }

    /// <summary>
    /// 计算怪物侧步
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="sideStepTrigger"></param>
    /// <param name="action"></param>
    /// <param name="eAttr"></param>
    /// <returns></returns>
    public static bool CalculateMonsterSideStep(int damage, ref float sideStepTrigger, ref EnemyBaseAction action, ref EnemyAttribute eAttr)
    {
        if (action.IsInWeakSta())
        {
            return false;
        }

        if (eAttr.monsterSideStep <= 0)
        {
            return false;
        }

        if (damage == 0)
        {
            return false;
        }

        eAttr.currentSideStep += damage;
        bool flag = true;
        if (!eAttr.sideStepInAir)
        {
            flag = eAttr.isOnGround;
        }

        bool flag2 = eAttr.currentSideStep >= eAttr.monsterSideStep && flag;
        bool flag3 = false;
        if (flag2)
        {
            flag3 = ((float)UnityEngine.Random.Range(0, 100) < sideStepTrigger);
            if (!flag3)
            {
                sideStepTrigger += 20f;
            }
        }

        return flag2 && flag3;
    }

    /// <summary>
    /// 获得攻击等级
    /// </summary>
    /// <param name="atkName"></param>
    /// <returns></returns>
    public static int GetAtkLevel(string atkName)
    {
        if (atkName.IsInArray(PlayerAtkType.AirAttack))
        {
            return R.Player.EnhancementSaveData.AirAttack;
        }

        if (atkName.IsInArray(PlayerAtkType.AirAvatarAttack))
        {
            return R.Player.EnhancementSaveData.AirAvatarAttack;
        }

        if (atkName == PlayerAtkType.AirCombo1)
        {
            return R.Player.EnhancementSaveData.AirCombo1;
        }

        if (atkName == PlayerAtkType.AirCombo2)
        {
            return R.Player.EnhancementSaveData.AirCombo2;
        }

        if (atkName.IsInArray(PlayerAtkType.Attack))
        {
            return R.Player.EnhancementSaveData.Attack;
        }

        if (atkName.IsInArray(PlayerAtkType.AvatarAttack))
        {
            return R.Player.EnhancementSaveData.AvatarAttack;
        }

        if (atkName == PlayerAtkType.BladeStorm)
        {
            return R.Player.EnhancementSaveData.BladeStorm;
        }

        if (atkName == PlayerAtkType.Charging)
        {
            return R.Player.EnhancementSaveData.Charging;
        }

        if (PlayerAtkType.Chase == atkName)
        {
            return R.Player.EnhancementSaveData.Chase;
        }

        if (atkName == PlayerAtkType.Combo1)
        {
            return R.Player.EnhancementSaveData.Combo1;
        }

        if (atkName.IsInArray(PlayerAtkType.Combo2))
        {
            return R.Player.EnhancementSaveData.Combo2;
        }

        if (PlayerAtkType.HitGround == atkName)
        {
            return R.Player.EnhancementSaveData.HitGround;
        }

        if (PlayerAtkType.Knockout == atkName)
        {
            return R.Player.EnhancementSaveData.Knockout;
        }

        if (PlayerAtkType.ShadeAttack == atkName)
        {
            return R.Player.EnhancementSaveData.ShadeAttack;
        }

        if (atkName.IsInArray(PlayerAtkType.TripleAttack))
        {
            return R.Player.EnhancementSaveData.TripleAttack;
        }

        if (PlayerAtkType.UpperChop == atkName)
        {
            return R.Player.EnhancementSaveData.UpperChop;
        }

        return 0;
    }

    /// <summary>
    /// 追逐攻击
    /// </summary>
    /// <returns></returns>
    public static bool ChaseAttack()
    {
        int num = UnityEngine.Random.Range(0, 100);
        return num <= 4 && R.Player.EnhancementSaveData.Chase > 0;
    }

    /// <summary>
    /// 显示HP恢复
    /// </summary>
    public static void FlashHPRecover()
    {
        float num = 0f;
        int flashAttack = R.Player.EnhancementSaveData.FlashAttack;
        if (flashAttack != 1)
        {
            if (flashAttack != 2)
            {
                if (flashAttack == 3)
                {
                    num = 0.07f;
                }
            }
            else
            {
                num = 0.05f;
            }
        }
        else
        {
            num = 0.03f;
        }

        R.Player.Attribute.currentHP += (int)((float)R.Player.Attribute.maxHP * num);
    }
}