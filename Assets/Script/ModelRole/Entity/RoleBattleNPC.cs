﻿using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = Core.Debug;

/// <summary>
/// 战斗的NPC类型
/// </summary>
public class RoleBattleNPC : IRoleActual, ISkillCarrier, IAttributes, IRoleAttackCount
{
    private uint _id;
    private string _name;
    private ERoleBattlePoint _roleBattlePoint = ERoleBattlePoint.Point2;//玩家默认右边，以后会改位置//被偷袭可能会在左边
    private ETurnState _turnState = ETurnState.PROCESSING;
    private ERoleType _roleType = ERoleType.NPC;
    private Dictionary<ESkillType, List<ISkill>> _skillDataDic;
    private int _maxHP;
    private int _currentHP;
    private int _maxATK;
    private int _currentATK;
    private int _attackCount;
    private GameObject _go;
    private float _max_colldown;//最大的冷却时间



    public float Max_colldown { get => _max_colldown; set => _max_colldown = value; }
    public ERoleType RoleType { get => _roleType; set => _roleType = value; }
    public ERoleBattlePoint RoleBattlePoint { get => _roleBattlePoint; set => _roleBattlePoint = value; }
    public ETurnState TurnState { get => _turnState; set => _turnState = value; }
    public uint ID { get => _id; set => _id = value; }
    public string Name { get => _name; set => _name = value; }
    public Dictionary<ESkillType, List<ISkill>> SkillDataDic { get => _skillDataDic; set => _skillDataDic = value; }
    public int MaxHP { get => _maxHP; set => _maxHP = value; }
    public int CurrentHP { get => _currentHP; set => _currentHP = value; }
    public int AttackCount { get => _attackCount; set => _attackCount = value; }
    public GameObject Go { get => _go; set => _go = value; }
    public int MaxATK { get => _maxATK; set => _maxATK = value; }
    public int CurrentATK { get => _currentATK; set => _currentATK = value; }


    /// <summary>
    /// 当前的冷却时间
    /// </summary>
    private float _cur_colldown;

    /// <summary>
    /// 一场战斗接口
    /// </summary>
    private IBattleActual _battle;

    /// <summary>
    /// 自己的队伍
    /// </summary>
    private ITeamActual _team;

    /// <summary>
    /// 角色的移动速度
    /// </summary>
    private float animSpeed = 5f;

    /// <summary>
    /// 是否正在行动
    /// </summary>
    private bool isActionStarted = false;

    /// <summary>
    /// 战斗的执行
    /// </summary>
    private IBattleAction _battleAction;

    /// <summary>
    /// 是否存活
    /// </summary>
    private bool isAlive = true;

    /// <summary>
    /// 玩家初始站的位置
    /// </summary>
    private Vector2 _startPosition;


    /// <summary>
    /// 添加数据
    /// </summary>
    public void AddData(IBattleActual battle, ITeamActual team)
    {
        this._battle = battle;
        this._team = team;
    }

    public void RoleInit()
    {
        _startPosition = _go.transform.position;
    }
    public void RoleRemove()
    {
    }
    public void RoleUpdata()
    {
        switch (TurnState)
        {
            case ETurnState.PROCESSING:
                UpgradeProgressBar();
                break;
            case ETurnState.CHOOSEACTION:
                ChooseAction();
                break;
            case ETurnState.WAITING:
                break;
            case ETurnState.ACTION:
                CoreBehaviour.AddCoroutine(TimeForAction());
                break;
            case ETurnState.DEAD:
                Dead();
                break;
            default:
                Debug.Error("角色状态错误");
                break;
        }
    }

    /// <summary>
    /// 进度条上升
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void UpgradeProgressBar()
    {
        _max_colldown += Time.deltaTime;
        if (_cur_colldown >= _max_colldown)//如果冷却时间到了
            _turnState = ETurnState.CHOOSEACTION;
    }
    /// <summary>
    /// 现在是敌人状态 选择英雄行动
    /// </summary>
    private void ChooseAction()
    {
        _battleAction = new BattleAction();
        BattleAction battleAction = new BattleAction();
        _battleAction = battleAction;
        //自己的攻击数据
        battleAction.AttackerData = this;
        //敌人的攻击数据
        battleAction.TargetData = _battle.RandomEnemyRole(_team.TeamType);
        //选择攻击方式
        AttackWay attackWay = new AttackWay();
        attackWay.Skill = _skillDataDic[ESkillType.NormalAttack][0];
        battleAction.Attack = attackWay;
        //int num = Random.Range(0, enemy.attacks.Count);
        //myAttack.choosenAttack = enemy.attacks[num];
        //伤害公式=emeny的enemy.curAtk+选择攻击方式的一种的伤害-对方的防御
        //Debug.Log(this.gameObject.name + "选择了：" + myAttack.choosenAttack.attackName + "攻击方式,对" + myAttack.AttackersTarget.name + "造成" + (myAttack.choosenAttack.attackDamage + enemy.curAtk) + "伤害");
        //添加到战斗列表
        _battle.AddBattle(battleAction);
        _turnState = ETurnState.WAITING;
    }
    /// <summary>
    /// 行动的时间到了
    /// </summary>
    /// <returns></returns>
    private IEnumerator TimeForAction()
    {
        if (isActionStarted)
            yield break;//如果在行动,直接跳出协程
        isActionStarted = true;
        //播放敌人接近英雄的攻击动画
        Vector3 heroPostion = new Vector3(
            _battleAction.TargetData.Go.transform.position.x - 1.5f,
            _battleAction.TargetData.Go.transform.position.y,
            _battleAction.TargetData.Go.transform.position.z);
        while (MoveTowrdsEnemy(heroPostion))//循环等待1帧
            yield return null;//这个是等待1帧的意思
        //等待
        yield return new WaitForSeconds(0.5f);
        //伤害
        DoDamage();
        //回到起始位置的动画
        while (MoveTowrdsStart(_startPosition))//循环等待1帧
            yield return null;//这个是等待1帧的意思
        //从行动列表移除
        _battle.RemoveBattleAction(_battleAction);
        //重置一场战斗变成等待,判断是否还有行动状态
        _battle.BattleSate = EBattlePerformAction.WAIT;
        //结束协程
        isActionStarted = false;
        //重置状态(这个类是NPC用的)
        _cur_colldown = 0f;
        _turnState = ETurnState.PROCESSING;
    }
    /// <summary>
    /// 角色死亡
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    private void Dead()
    {
        if (!isAlive)
            return;
        //change tag 切换标签
        //this.gameObject.tag = "DeadEnemy";
        //自己死亡了,队伍列表删除自己
        _team.RemoveRole(this);
        //deactivate the selector 停用选择器 就是黄色的小物体
        //Selector.SetActive(false);
        //从行动列表删除自己，如果行动列表存在自己的话
        if (_team.RoleList.Count > 0)
        {
            for (int i = 0; i < _battle.BattleActionList.Count; i++)
            {
                if (i == 0) continue;
                IBattleAction battleAction = _battle.BattleActionList[i];
                //行动的被攻击者是我的话,随机自己方一个人
                if (battleAction.AttackerData == this)
                    _battle.RemoveBattleAction(battleAction);
                if (battleAction.TargetData == this)
                    battleAction.TargetData = _battle.RandomOwnRole(_team.TeamType);
            }
        }
        //change color / play animation 改变颜色/播放动画
        //this.gameObject.GetComponent<MeshRenderer>().material.color = new Color32(105, 105, 105, 255);
        //设置为不存活
        isAlive = false;
        //重新生成敌人的按钮
        //BSM.EnemyButtons();
        //在一场战斗中检查敌人队伍或者自己人队伍是否还有存活的
        _battle.BattleSate = EBattlePerformAction.CHECKALIVE;
    }




    /// <summary>
    /// 移动敌人 如果敌人没移动到玩家坐标的时候  返回的就是false
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private bool MoveTowrdsEnemy(Vector3 target)
    {
        return target != (_go.transform.position = Vector3.MoveTowards(_go.transform.position, target, animSpeed * Time.deltaTime));
    }
    /// <summary>
    /// 回到原来的位置
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private bool MoveTowrdsStart(Vector3 target)
    {
        return target != (_go.transform.position = Vector3.MoveTowards(_go.transform.position, target, animSpeed * Time.deltaTime));
    }
    /// <summary>
    /// 给与伤害
    /// </summary>
    private void DoDamage()
    {
        int calc_damage = CurrentATK + _battleAction.Attack.Skill.CurrentATK;
        _battleAction.TargetData.TakeDamage(calc_damage);
    }
    /// <summary>
    /// 遭受伤害
    /// </summary>
    public void TakeDamage(int getDamageAmount)
    {
        CurrentHP -= getDamageAmount;
        Debug.Log($"{Name}受到：{getDamageAmount}点伤害,剩余生命值：{CurrentHP}");
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            _turnState = ETurnState.DEAD;
        }
    }
}
