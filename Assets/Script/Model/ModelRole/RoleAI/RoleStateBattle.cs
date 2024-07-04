using Framework.Core;
using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 角色战斗状态
/// </summary>
public class RoleStateBattle : IRoleState, IDamage
{
    public ERoleSateType RoleSateType => ERoleSateType.Battle;
    public long ID { get; set; }
    public RoleData RoleData { get; set; }

    #region 本类持有
    private BattleActionData battleAction { get; set; }//自己的行动
    private bool isActionStarted { get; set; } = false;// 是否是行动开始了
    private Vector3 startPosition { get; set; }
    private float animSpeed { get; set; } = 10f;// 移动速度
    private RoleAttributes RoleAttributes => RoleData.RoleAttributes;
    private TeamData team => RoleData.Team;
    private GameObject roleGameObject { get; set; }
    public ERoleTurnState turnState { get; set; } = ERoleTurnState.PROCESSING;// 当前状态枚举
    public BattleData battle { get; set; }
    #endregion


    #region 初始化,必须调用
    /// <summary>
    /// 添加数据
    /// </summary>
    /// <param name="roleData">角色数据</param>
    public void SetBattleData(BattleData battleActual)
    {
        this.battle = battleActual;
    }
    #endregion


    #region 接口实现
    public void StateEnter()
    {
        RoleData.gameObject = roleGameObject = SceneBattleManager.Instance.InstantiateBattleRole(team.TeamPoint, RoleData);
        startPosition = roleGameObject.transform.position;
        turnState = ERoleTurnState.PROCESSING;
    }
    public void StateExit()
    {
    }
    public void StateUpdata()
    {
        switch (turnState)
        {
            case ERoleTurnState.PROCESSING:
                UpgradeProgressBar();
                break;
            case ERoleTurnState.CHOOSEACTION:
                switch (RoleData.RoleType)
                {
                    case ERoleOrTeamType.Player:
                        PlayerChooseAction();
                        break;
                    case ERoleOrTeamType.NPC:
                    case ERoleOrTeamType.Enemy:
                        NpcOrEnemyChooseAction();
                        break;
                    default:
                        break;
                }
                break;
            case ERoleTurnState.WAITING:
                break;
            case ERoleTurnState.ACTION:
                CoreBehaviour.AddCoroutine(TimeForAction());
                break;
            case ERoleTurnState.DEAD:
                Dead();
                break;
            default:
                EDebug.Error("行动错误");
                break;
        }
    }
    #endregion


    public void DoDamage()
    {
        int calc_damage = battleAction.AttackData.AttributeData.Damage;
        battleAction.TargetData.RoleState.GetRoleSate<RoleStateBattle>().TakeDamage(calc_damage);
    }
    public void TakeDamage(int getDamageAmount)
    {
        RoleAttributes.CurrentHP -= getDamageAmount;
        EDebug.Log($"{RoleData.Name}受到：{getDamageAmount}点伤害,剩余生命值：{RoleAttributes.CurrentHP}");
        if (RoleAttributes.CurrentHP >= 0) return;
        RoleAttributes.CurrentHP = 0;
        turnState = ERoleTurnState.DEAD;
    }


    #region 角色状态
    /// <summary>
    /// 进度条上升
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void UpgradeProgressBar()
    {
        RoleAttributes.CurColldown += Time.deltaTime;
        //Debug.Log($"{roleData.Name}进度条上升{RoleAttributes.CurColldown}");
        if (RoleAttributes.CurColldown < RoleAttributes.MaxColldown) return;//如果冷却时间到了
        RoleData.IsAlive = true;
        turnState = ERoleTurnState.CHOOSEACTION;
    }
    /// <summary>
    /// 现在是敌人状态,选择攻击方式和玩家或者NPC
    /// </summary>
    private void NpcOrEnemyChooseAction()
    {
        battleAction = new BattleActionData();
        //自己的攻击数据
        battleAction.AttackerData = RoleData;
        //敌人的攻击数据
        battleAction.TargetData = battle.RandomEnemyRole(team.TeamType);
        //选择攻击方式

        battleAction.AttackData = RoleData.GetSkill(ESkillType.NormalAttack, 1);
        //Debug.Log(this.gameObject.name + "选择了：" + myAttack.choosenAttack.attackName + "攻击方式,对" + myAttack.AttackersTarget.name + "造成" + (myAttack.choosenAttack.attackDamage + enemy.curAtk) + "伤害");
        //添加到战斗列表
        battle.AddBattleAction(battleAction);
        turnState = ERoleTurnState.WAITING;
    }
    private void PlayerChooseAction()
    {
    }
    private IEnumerator TimeForAction()
    {
        if (isActionStarted)
            yield break;//如果在行动,直接跳出协程
        isActionStarted = true;
        //播放接近动画
        Vector3 heroPostion = Vector3.zero;
        GameObject gameObject = battleAction.TargetData.gameObject;
        switch (team.TeamPoint)
        {
            case ETeamPoint.Left1:
            case ETeamPoint.Left2:
            case ETeamPoint.Left3:
            case ETeamPoint.Left4:
                heroPostion = new Vector3(gameObject.transform.position.x + 1.5f, gameObject.transform.position.y, gameObject.transform.position.z);
                break;
            case ETeamPoint.Right1:
            case ETeamPoint.Right2:
            case ETeamPoint.Right3:
            case ETeamPoint.Right4:
                heroPostion = new Vector3(gameObject.transform.position.x - 1.5f, gameObject.transform.position.y, gameObject.transform.position.z);
                break;
            default:
                break;
        }

        while (MoveTowrdsEnemy(heroPostion))//循环等待1帧
            yield return null;//这个是等待1帧的意思
        //等待
        yield return new WaitForSeconds(0.5f);
        //yield return new WaitForSeconds(0.5f);
        //伤害
        DoDamage();
        //回到起始位置的动画
        while (MoveTowrdsStart(startPosition))//循环等待1帧
            yield return null;//这个是等待1帧的意思
        //从行动列表移除
        battle.RemoveBattleAction(battleAction);
        //重置一场战斗变成等待,判断是否还有行动状态
        battle.BattleSate = EBattlePerformAction.WAIT;
        //结束协程
        isActionStarted = false;
        //重置状态(这个类是NPC用的)
        RoleAttributes.CurColldown = 0f;
        turnState = ERoleTurnState.PROCESSING;
    }
    /// <summary>
    /// 角色死亡
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    private void Dead()
    {
        if (!RoleData.IsAlive)
            return;
        //deactivate the selector 停用选择器 就是黄色的小物体
        //Selector.SetActive(false);
        //自己死了从行动列表删除自己，如果行动列表存在自己的话
        if (battle.ChackBattleActionExist(RoleData, out BattleActionData battleAction))
        {
            //if (battle.BattleActionList[0] == battleAction) return;
            if (battleAction.AttackerData == RoleData)
                battle.RemoveBattleAction(battleAction);
            if (battleAction.TargetData == RoleData)
                battleAction.TargetData = battle.RandomEnemyRole(team.TeamType);
        }
        //change color / play animation 改变颜色/播放动画
        //this.gameObject.GetComponent<MeshRenderer>().material.color = new Color32(105, 105, 105, 255);
        //设置为不存活
        RoleData.IsAlive = false;
        //重新生成敌人的按钮
        //BSM.EnemyButtons();
        //在一场战斗中检查敌人队伍或者自己人队伍是否还有存活的
        battle.BattleSate = EBattlePerformAction.CHECKALIVE;
    }
    #endregion


    #region 私有方法
    /// <summary>
    /// 移动敌人 如果敌人没移动到玩家坐标的时候  返回的就是false
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private bool MoveTowrdsEnemy(Vector3 target)
    {
        return target != (RoleData.gameObject.transform.position = Vector3.MoveTowards(RoleData.gameObject.transform.position, target, animSpeed * Time.deltaTime));
    }

    /// <summary>
    /// 回到原来的位置
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private bool MoveTowrdsStart(Vector3 target)
    {
        return target != (RoleData.gameObject.transform.position = Vector3.MoveTowards(RoleData.gameObject.transform.position, target, animSpeed * Time.deltaTime));
    }

    /// <summary>
    /// 技能选择算法
    /// </summary>
    private void SeleceSkill()
    {

    }
    #endregion
}