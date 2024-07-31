using System;
using UnityEngine;

/// <summary>
/// 玩家拓展工具
/// </summary>
public class PlayerExecuteTools
{
	private PlayerAction player
	{
		get
		{
			return R.Player.Action;
		}
	}

	public void SpecicalEnemyQTE(Transform enemy)
	{
		EnemyType enemyType = EnemyGenerator.GetEnemyType(enemy.name.Replace("(Clone)", string.Empty));
		switch (enemyType)
		{
		case EnemyType.跳拳大脚组合:
			this.JumperFooterQTE(enemy);
			return;
		case EnemyType.锤子:
		case EnemyType.巨柱:
		case EnemyType.锤子精英版:
		case EnemyType.巨柱精英版:
			this.StickerHammerQTE(enemy);
			return;
		case EnemyType.巨型机器人:
			this.RiantRobotQTE(enemy);
			return;
		case EnemyType.骑兵改:
			goto IL_AE;
		case EnemyType.卡洛斯:
		case EnemyType.卡洛斯精英版:
			this.EatingBossQTE(enemy);
			return;
		default:
			switch (enemyType)
			{
			case EnemyType.骑兵:
				goto IL_AE;
			case EnemyType.蜜蜂:
				return;
			case EnemyType.达哈尔:
				break;
			case EnemyType.暴食:
				goto IL_A2;
			default:
				return;
			}
			break;
		case EnemyType.愚笨蜘蛛:
		case EnemyType.愚笨蜘蛛精英版:
			this.SpiderQTE(enemy);
			return;
		case EnemyType.杰克:
		case EnemyType.杰克精英版:
			this.JackQTE(enemy);
			return;
		case EnemyType.犹大:
			this.JudgesQTE(enemy);
			return;
		case EnemyType.达哈尔终极版:
		case EnemyType.达哈尔精英版:
			break;
		case EnemyType.暴食Boss版:
			goto IL_A2;
		}
		this.DahalQTE(enemy);
		return;
		IL_A2:
		this.BeelzebubQTE(enemy);
		return;
		IL_AE:
		this.RiderQTE(enemy);
	}

	private void RiderQTE(Transform rider)
	{
		EnemyBaseHurt component = rider.GetComponent<EnemyBaseHurt>();
		int currentPhase = component.currentPhase;
		if (currentPhase != 0)
		{
			if (currentPhase == 1)
			{
				this.SpecialEnemyExecute(rider, PlayerAction.StateEnum.RiderQTEHurt_1, 1f);
			}
		}
		else
		{
			this.SpecialEnemyQTEHurt(rider, PlayerAction.StateEnum.RiderQTEHurt_1, 1f);
		}
	}

	private void BeelzebubQTE(Transform beelzebub)
	{
		BeelzebubHurt component = beelzebub.GetComponent<BeelzebubHurt>();
		int currentPhase = component.currentPhase;
		if (currentPhase != 0)
		{
			if (currentPhase == 1)
			{
				this.SpecialEnemyExecute(beelzebub, PlayerAction.StateEnum.AtkHv4, 1f);
			}
		}
		else
		{
			this.SpecialEnemyQTEHurt(beelzebub, PlayerAction.StateEnum.BeelzebubQTECatch, 1f);
		}
	}

	private void DahalQTE(Transform dahal)
	{
		EnemyBaseHurt component = dahal.GetComponent<EnemyBaseHurt>();
		int currentPhase = component.currentPhase;
		if (currentPhase != 0)
		{
			if (currentPhase == 1)
			{
				this.SpecialEnemyExecute(dahal, PlayerAction.StateEnum.DahalAtkUpRising, 1f);
			}
		}
		else
		{
			this.SpecialEnemyQTEHurt(dahal, PlayerAction.StateEnum.Disappear, 1f);
		}
	}

	private void JumperFooterQTE(Transform jumperFooter)
	{
		JumperFooterHurt component = jumperFooter.GetComponent<JumperFooterHurt>();
		int currentPhase = component.currentPhase;
		if (currentPhase != 0)
		{
			if (currentPhase == 1)
			{
				this.SpecialEnemyExecute(jumperFooter, PlayerAction.StateEnum.Disappear, 1f);
			}
		}
		else
		{
			this.SpecialEnemyQTEHurt(jumperFooter, PlayerAction.StateEnum.Disappear, 1f);
		}
	}

	private void StickerHammerQTE(Transform stickOrHammer)
	{
		EnemyBaseHurt component = stickOrHammer.GetComponent<EnemyBaseHurt>();
		int currentPhase = component.currentPhase;
		if (currentPhase != 0)
		{
			if (currentPhase == 1)
			{
				this.SpecialEnemyExecute(stickOrHammer, PlayerAction.StateEnum.Disappear, 1f);
			}
		}
		else
		{
			this.SpecialEnemyQTEHurt(stickOrHammer, PlayerAction.StateEnum.Disappear, 1f);
		}
	}

	private void RiantRobotQTE(Transform giantRobot)
	{
		this.SpecialEnemyExecute(giantRobot, PlayerAction.StateEnum.Disappear, 1f);
	}

	private void EatingBossQTE(Transform eatingBoss)
	{
		EatingBossHurt component = eatingBoss.GetComponent<EatingBossHurt>();
		int currentPhase = component.currentPhase;
		if (currentPhase != 0)
		{
			if (currentPhase == 1)
			{
				this.SpecialEnemyExecute(eatingBoss, PlayerAction.StateEnum.AtkHv4, 1f);
			}
		}
		else
		{
			this.SpecialEnemyQTEHurt(eatingBoss, PlayerAction.StateEnum.Disappear, 1f);
		}
	}

	private void SpiderQTE(Transform spider)
	{
		SpiderBossHurt component = spider.GetComponent<SpiderBossHurt>();
		int currentPhase = component.currentPhase;
		if (currentPhase != 0)
		{
			if (currentPhase == 1)
			{
				this.SpecialEnemyExecute(spider, PlayerAction.StateEnum.Disappear, 1f);
			}
		}
		else
		{
			this.SpecialEnemyQTEHurt(spider, PlayerAction.StateEnum.BeelzebubQTEDie, 1f);
		}
	}

	private void JackQTE(Transform jack)
	{
		JackHurt component = jack.GetComponent<JackHurt>();
		int currentPhase = component.currentPhase;
		if (currentPhase != 0)
		{
			if (currentPhase == 1)
			{
				this.SpecialEnemyExecute(jack, PlayerAction.StateEnum.BeelzebubQTEDie, 1f);
			}
		}
		else
		{
			this.SpecialEnemyQTEHurt(jack, PlayerAction.StateEnum.BeelzebubQTEDie, 1f);
		}
	}

	private void JudgesQTE(Transform judges)
	{
		JudgesHurt component = judges.GetComponent<JudgesHurt>();
		switch (component.currentPhase)
		{
		case 0:
		case 1:
		case 2:
		case 3:
			this.SpecialEnemyQTEHurt(judges, PlayerAction.StateEnum.Disappear, 1f);
			break;
		case 4:
			this.SpecialEnemyExecute(judges, PlayerAction.StateEnum.QTECharge1Ready, 1f);
			break;
		}
	}

	private void SpecialEnemyExecute(Transform enemy, PlayerAction.StateEnum playerSta, float speed = 1f)
	{
		enemy.GetComponent<EnemyBaseAction>().AnimExecute();
		EnemyAttribute component = enemy.GetComponent<EnemyAttribute>();
		component.currentHp = 0;
		component.timeController.SetSpeed(Vector2.zero);
		this.player.ChangeState(playerSta, speed);
	}

	private void SpecialEnemyQTEHurt(Transform enemy, PlayerAction.StateEnum playerSta, float speed = 1f)
	{
		enemy.GetComponent<EnemyBaseAction>().AnimQTEHurt();
		enemy.GetComponent<EnemyAttribute>().timeController.SetSpeed(Vector2.zero);
		this.player.ChangeState(playerSta, speed);
	}
}
