/// <summary>
/// 战斗评估数据
/// </summary>
public class BattleAssessmentData
{
    public BattleAssessmentData.BattleComboData ComboData = new BattleAssessmentData.BattleComboData();

    /// <summary>
    /// 连击清空
    /// </summary>
    public void ComboClear()
    {
        ComboData.SameComboNum = 0;
        ComboData.AirComboNum = 0;
        ComboData.FlashAttackSuccessNum = 0;
        ComboData.CoreBreakNum = 0;
        ComboData.AllDamagePercent = 0f;
    }

    /// <summary>
    /// 战斗数据清空
    /// </summary>
    public void BattleClear()
    {
        FlashAttackSuccessNum = 0;
        FlashAttackNum = 0;
    }

    /// <summary>
    /// 闪攻击成功次数
    /// </summary>
    public int FlashAttackSuccessNum;

    /// <summary>
    /// 闪攻击次数
    /// </summary>
    public int FlashAttackNum;

    /// <summary>
    /// 不受伤害
    /// </summary>
    public bool NotHurt = true;

    /// <summary>
    /// 当前组合数
    /// </summary>
    public int CurrentComboNum;

    /// <summary>
    /// 战斗组合数据
    /// </summary>
    public class BattleComboData
    {
        /// <summary>
        /// 相同连击数
        /// </summary>
        public int SameComboNum;

        /// <summary>
        /// 空中组合数
        /// </summary>
        public int AirComboNum;

        /// <summary>
        /// 闪攻击成功次数
        /// </summary>
        public int FlashAttackSuccessNum;

        /// <summary>
        /// 核心破坏数
        /// </summary>
        public int CoreBreakNum;

        /// <summary>
        /// 所有伤害百分比
        /// </summary>
        public float AllDamagePercent;
    }
}