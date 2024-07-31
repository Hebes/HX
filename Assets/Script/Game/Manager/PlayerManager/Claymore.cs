using System;
using LitJson;
using UnityEngine;

/// <summary>
/// 大剑
/// </summary>
public class Claymore : MonoBehaviour
{
	/// <summary>
	/// 攻击重置
	/// </summary>
	public bool aitAtkReset => !IsInvoking("AttackReset");

	/// <summary>
	/// 最大充电时间
	/// </summary>
	private float maxChargeTime => this.pAttr.maxChargeTime;

	/// <summary>
	/// 可冲锋攻击
	/// </summary>
	public bool canChargeAttack => this.chargeTime >= 2.5f;

	private void Start()
	{
		this.execute = new PlayerExecuteTools();
		this.pAttr = R.Player.Attribute;
		this.listener = base.GetComponent<PlayerAnimEventListener>();
		this.player = base.GetComponent<PlayerAction>();
		this.comboConfig = null;// JsonMapper.ToObject(this.weaponConfigure.text);
		this.AirAttackReset();
	}

	private void Update()
	{
		this.UpdateCharge();
	}

	public void HandleAttack(bool cirt)
	{
		JsonData1 jsonData = this.comboConfig["normalAttack"];
		if (this.player.stateMachine.currentState.IsInArray(PlayerAction.AttackSta))
		{
			this.continueAttack = true;
			this.cirtAttack = cirt;
		}
		else
		{
			if (cirt)
			{
				this.player.ChangeState(jsonData[9.ToString()].Get<string>("anim", null), 1f);
				this.attackID = 9;
			}
			else
			{
				this.player.ChangeState(jsonData[1.ToString()].Get<string>("anim", null), 1f);
				this.attackID = 1;
			}
			this.continueAttack = false;
			this.cirtAttack = false;
		}
	}

	public void CirtAttackHold()
	{
		JsonData1 jsonData = this.comboConfig["normalAttack"];
		this.player.ChangeState(jsonData[12.ToString()].Get<string>("anim", null), 1f);
		this.attackID = 12;
	}

	private void PlayNextAttackAnim(bool cirt)
	{
		this.canChangeAtkAnim = false;
		this.continueAttack = false;
		this.player.TurnRound(this.player.tempDir);
		JsonData1 jsonData = this.comboConfig["normalAttack"];
		string key = (!cirt) ? "nextID" : "nextCirtID";
		int nextID = jsonData[this.attackID.ToString()].Get<int>(key, 0);
		if (!cirt && this.attackID == 4)
		{
			nextID = this.ComboCheck(nextID);
		}
		if (cirt)
		{
			nextID = this.CirtComboCheck(nextID);
		}
		if (!jsonData.Contains(nextID.ToString()))
		{
			this.continueAttack = false;
			this.canChangeAtkAnim = false;
			return;
		}
		JsonData1 jsonData2 = jsonData[nextID.ToString()];
		this.player.ChangeState(jsonData2.Get<string>("anim", null), 1f);
		this.attackID = nextID;
	}

	public void CanPlayNextAttack()
	{
		if (this.canChangeAtkAnim)
		{
			this.PlayNextAttackAnim(this.cirtAttack);
			return;
		}
		if (this.continueAttack)
		{
			this.PlayNextAttackAnim(this.cirtAttack);
		}
		else
		{
			this.canChangeAtkAnim = true;
		}
	}

	public void AttackFinish()
	{
		this.cirtAttack = false;
		this.continueAttack = false;
		this.canChangeAtkAnim = false;
	}

	private int CirtComboCheck(int nextID)
	{
		if (this.attackID == 1 && nextID == 15 && R.Player.EnhancementSaveData.UpperChop == 0)
		{
			return -1;
		}
		if (this.attackID == 2 && nextID == 5 && R.Player.EnhancementSaveData.Combo2 == 0)
		{
			return -1;
		}
		if (this.attackID == 3 && nextID == 19 && R.Player.EnhancementSaveData.AvatarAttack == 0)
		{
			return -1;
		}
		if (this.attackID == 4 && nextID == 18 && R.Player.EnhancementSaveData.Knockout == 0)
		{
			return -1;
		}
		return nextID;
	}

	private int ComboCheck(int nextID)
	{
		int attack = R.Player.EnhancementSaveData.Attack;
		if (attack == 2)
		{
			return 16;
		}
		if (attack != 3)
		{
			return nextID;
		}
		return 16;
	}

	public void HandleAirAttack(bool cirt)
	{
		JsonData1 jsonData = this.comboConfig["airAttack"];
		if (this.player.stateMachine.currentState.IsInArray(PlayerAction.AirAttackSta) && this.player.stateMachine.currentState != "AirAtkRoll")
		{
			this.continueAirAttack = true;
			this.cirtAttack = cirt;
		}
		else
		{
			if (cirt)
			{
				if (!this.airAttackReset)
				{
					return;
				}
				this.player.ChangeState(jsonData[4.ToString()].Get<string>("anim", null), 1f);
				this.airAttackReset = false;
				this.attackID = 4;
			}
			else
			{
				if (!this.airAttackReset)
				{
					return;
				}
				this.player.ChangeState(jsonData[1.ToString()].Get<string>("anim", null), 1f);
				this.airAttackReset = false;
				this.attackID = 1;
			}
			this.continueAirAttack = false;
			this.cirtAttack = false;
		}
	}

	public void AirCirtAttackHold()
	{
		JsonData1 jsonData = this.comboConfig["airAttack"];
		this.player.ChangeState(jsonData[10.ToString()].Get<string>("anim", null), 1f);
		this.attackID = 10;
	}

	private void PlayNextAirAttackAnim(bool cirt)
	{
		this.continueAirAttack = false;
		this.canChangeAirAtkAnim = false;
		R.Player.TimeController.SetSpeed(Vector2.zero);
		this.player.TurnRound(this.player.tempDir);
		JsonData1 jsonData = this.comboConfig["airAttack"];
		string key = (!cirt) ? "nextID" : "nextCirtID";
		int nextID = jsonData[this.attackID.ToString()].Get<int>(key, 0);
		if (cirt)
		{
			nextID = this.AirCirtComboCheck(nextID);
		}
		if (!jsonData.Contains(nextID.ToString()))
		{
			this.continueAirAttack = false;
			this.canChangeAirAtkAnim = false;
			return;
		}
		JsonData1 jsonData2 = jsonData[nextID.ToString()];
		this.player.ChangeState(jsonData2.Get<string>("anim", null), 1f);
		this.attackID = nextID;
	}

	public void CanPlayNextAirAttack()
	{
		if (this.pAttr.isOnGround)
		{
			this.AirAttackReset();
			this.player.ChangeState(PlayerAction.StateEnum.GetUp, 1f);
			return;
		}
		if (this.canChangeAirAtkAnim)
		{
			this.PlayNextAirAttackAnim(this.cirtAttack);
			return;
		}
		if (this.continueAirAttack)
		{
			this.PlayNextAirAttackAnim(this.cirtAttack);
		}
		else
		{
			this.canChangeAirAtkAnim = true;
		}
	}

	public void AirAttackFinish()
	{
		this.cirtAttack = false;
		this.continueAirAttack = false;
		this.canChangeAirAtkAnim = false;
		this.player.ChangeState(PlayerAction.StateEnum.Fall1, 1f);
	}

	public void AirAttackRecover()
	{
		this.cirtAttack = false;
		this.AirAttackReset();
	}

	private int AirCirtComboCheck(int nextID)
	{
		if (this.attackID == 1 && nextID == 13 && R.Player.EnhancementSaveData.AirCombo2 == 0)
		{
			return -1;
		}
		if (this.attackID == 2 && nextID == 11 && R.Player.EnhancementSaveData.AirAvatarAttack == 0)
		{
			return -1;
		}
		if (this.attackID == 3 && nextID == 7 && R.Player.EnhancementSaveData.AirCombo1 == 0)
		{
			return -1;
		}
		return nextID;
	}

	public void HandleUpRising()
	{
		this.listener.PhysicReset();
		this.AirAttackReset();
		PlayerAction.StateEnum sta = PlayerAction.StateEnum.UpRising;
		this.player.ChangeState(sta, 1f);
	}

	public void HandleHitGround()
	{
		this.player.ChangeState(PlayerAction.StateEnum.HitGround, 1f);
	}

	public void HandleExecute(bool inAir, EnemyAttribute eAttr)
	{
		if (eAttr.rankType == EnemyAttribute.RankType.Normal)
		{
			this.NormalEnemyExecute(inAir, eAttr);
		}
		else
		{
			this.listener.isFalling = false;
			this.listener.checkFallDown = false;
			this.listener.airAtkDown = false;
			this.listener.checkHitGround = false;
			this.listener.PhysicReset();
			base.transform.position = base.transform.position.SetY(eAttr.transform.position.y);
			this.execute.SpecicalEnemyQTE(eAttr.transform);
		}
		eAttr.GetComponent<EnemyBaseHurt>().QTECameraStart();
		this.listener.StopIEnumerator("FlashPositionSet");
	}

	private void NormalEnemyExecute(bool inAir, EnemyAttribute eAttr)
	{
		if (inAir)
		{
			PlayerAction.StateEnum sta = (UnityEngine.Random.Range(0, 2) != 0) ? PlayerAction.StateEnum.NewExecuteAir2_1 : PlayerAction.StateEnum.NewExecuteAir1_1;
			this.player.ChangeState(sta, 1f);
		}
		else
		{
			PlayerAction.StateEnum sta2 = (UnityEngine.Random.Range(0, 2) != 0) ? PlayerAction.StateEnum.NewExecute2_0 : PlayerAction.StateEnum.NewExecute1_1;
			if (!eAttr.accpectAirExecute)
			{
				sta2 = PlayerAction.StateEnum.NewExecute1_1;
			}
			this.player.ChangeState(sta2, 1f);
			if (eAttr.isOnGround)
			{
				eAttr.stiffTime = 1f;
				eAttr.GetComponent<EnemyBaseAction>().AnimReady();
			}
		}
	}

	private void UpdateCharge()
	{
		if (this.startCharge)
		{
			this.chargeTime = Mathf.Clamp(this.chargeTime + Time.deltaTime, 0f, this.maxChargeTime);
			bool flag = this.player.stateMachine.currentState == "AirCharging";
			if (this.lastFrameTime < 2.5f && this.chargeTime >= 2.5f)
			{
				Input.Vibration.Vibrate(2);
				if (flag)
				{
					this.airRelease = 5f;
					this.listener.chargeAnim.ChargeOneOverAir();
				}
				else
				{
					this.listener.chargeAnim.ChargeOneOver();
				}
			}
			this.lastFrameTime = this.chargeTime;
		}
		if (this.airRelease > 0f)
		{
			this.airRelease = Mathf.Clamp(this.airRelease - Time.unscaledDeltaTime, 0f, float.MaxValue);
			if (Math.Abs(this.airRelease) < 1.401298E-45f)
			{
				this.ReleaseCharge(true);
			}
		}
	}

	public void StartCharge(bool inAir)
	{
		this.startCharge = true;
		this.listener.charge = false;
		this.player.ChangeState((!inAir) ? PlayerAction.StateEnum.Charge1Ready : PlayerAction.StateEnum.AirCharging, 1f);
	}

	public void ReleaseCharge(bool inAir)
	{
		this.startCharge = false;
		this.listener.charge = true;
		this.listener.ChargeEffectDisappear();
		PlayerAction.StateEnum sta = (!inAir) ? PlayerAction.StateEnum.Charge1End : PlayerAction.StateEnum.AirChargeEnd;
		this.player.ChangeState(sta, 1f);
		this.player.pab.charge.ChargeReset();
		this.chargeTime = 0f;
		this.airRelease = 0f;
	}

	public void ChargeCancel()
	{
		this.listener.ChargeEffectDisappear();
		this.startCharge = false;
		this.chargeTime = 0f;
		this.airRelease = 0f;
	}

	public void AddChargeLevel()
	{
		this.chargeTime = Mathf.Clamp(this.chargeTime + 1.5f, 0f, this.maxChargeTime);
	}

	public void HandleShadeAttack()
	{
		R.Player.TimeController.SetSpeed(Vector2.zero);
		R.Player.Rigidbody2D.gravityScale = 0f;
		this.listener.isFalling = false;
		this.listener.checkFallDown = false;
		this.listener.checkHitGround = false;
		this.player.ChangeState(PlayerAction.StateEnum.Disappear, 1f);
		Transform transform = R.Effect.Generate(191, null, base.transform.position, default(Vector3), default(Vector3), true);
		Vector3 localScale = transform.localScale;
		localScale.x *= (float)(-(float)this.pAttr.faceDir);
		transform.localScale = localScale;
	}

	public void HandleBladeStorm()
	{
		R.Player.TimeController.SetSpeed(Vector2.zero);
		this.player.ChangeState(PlayerAction.StateEnum.RollGround, 1f);
	}

	public void AirAttackReset()
	{
		if (!this.airAttackReset)
		{
			this.airAttackReset = true;
		}
	}

	private PlayerAttribute pAttr;

	private int attackID;

	private const int comboID = 1;

	private const int cirtComboID = 9;

	private const int cirtHoldComboID = 12;

	private const int airComboID = 1;

	private const int airCirtComboID = 4;

	private const int airCirtHoldComboID = 10;

	private bool continueAttack;

	private bool continueAirAttack;

	public bool cirtAttack;

	[SerializeField]
	private TextAsset weaponConfigure;

	private JsonData1 comboConfig;

	private PlayerAction player;

	private PlayerAnimEventListener listener;

	private bool airAttackReset;

	[HideInInspector]
	public bool canChangeAtkAnim;

	[HideInInspector]
	public bool canChangeAirAtkAnim;

	private float chargeTime;

	private bool startCharge;

	private float lastFrameTime;

	private float airRelease;

	private PlayerExecuteTools execute;
}
