using UnityEngine;

/// <summary>
/// 玩家伤害计算
/// </summary>
public static class PlayerDamageCalculate
{
    private static PlayerAttribute pAttr => R.Player.Attribute;

    /// <summary>
    /// 获得最终伤害
    /// </summary>
    /// <param name="attackPercent"></param>
    /// <param name="atkLevel"></param>
    /// <param name="atkName"></param>
    /// <returns></returns>
    public static int GetFinalDamage(float attackPercent, int atkLevel, string atkName = "")
    {
        float num = (float)PlayerDamageCalculate.PlayerAtk() * attackPercent * Mathf.Pow(1.5f, (float)Mathf.Clamp(atkLevel - 1, 0, int.MaxValue));
        float num2 = UnityEngine.Random.Range(0.9f, 1.1f) * num;
        if (atkName != "Charge1EndLevel1")
        {
            return (int)num2;
        }

        float num3 = 1f + 0.5f * (float)(Mathf.Clamp(R.Player.Action.absorbNum - 3, 0, int.MaxValue) / 3);
        num2 = Mathf.Clamp(num2 * num3, 0f, 999f);
        return (int)num2;
    }

    /// <summary>
    /// 玩家攻击
    /// </summary>
    /// <returns></returns>
    private static int PlayerAtk()
    {
        float num = 1f;
        if (R.GameData.Difficulty == 0)
        {
            num = 1.5f;
        }

        float num2 = (float)PlayerDamageCalculate.pAttr.baseAtk * num;
        if (R.SceneData.assessmentData.CurrentComboNum >= 120)
        {
            return (int)(num2 * 1.5f);
        }

        if (R.SceneData.assessmentData.CurrentComboNum >= 80)
        {
            return (int)(num2 * 1.2f);
        }

        if (R.SceneData.assessmentData.CurrentComboNum >= 40)
        {
            return (int)(num2 * 1.1f);
        }

        return (int)num2;
    }
}