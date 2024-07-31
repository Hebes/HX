using System;
using UnityEngine;

/// <summary>
/// 玩家属性
/// </summary>
public class PlayerAttribute
{
	/// <summary>
	/// 当前生命值
	/// </summary>
	public int currentHP
	{
		get => _currentHP;
		set => _currentHP = Mathf.Clamp(value, 0, maxHP);
	}

	/// <summary>
	/// 当前能量
	/// </summary>
	public int currentEnergy
	{
		get => _currentEnergy;
		set => _currentEnergy = Mathf.Clamp(value, 0, maxEnergy);
	}

	public int flashTimes => flashLevel < 2 ? 3 : 5;

	/// <summary>
	/// 正在充电
	/// </summary>
	public bool isInCharging => R.Player.Action.stateMachine.currentState == "Charging1" || R.Player.Action.stateMachine.currentState == "Charge1Ready" || R.Player.Action.stateMachine.currentState == "AirCharging";

	public bool isOnGround => platform.State.IsDetectedGround;

	/// <summary>
	/// 是否死亡
	/// </summary>
	public bool isDead => currentHP <= 0;

	/// <summary>
	/// 玩家碰撞盒
	/// </summary>
	public Bounds bounds => R.Player.GetComponent<Collider2D>().bounds;

	private PlatformMovement platform
	{
		get
		{
			PlatformMovement result;
			if ((result = _platform) == null)
				result = _platform = R.Player.GetComponent<PlatformMovement>();
			return result;
		}
	}

	public void ResetData()
	{
		SetBaseLevelData();
		AllAttributeRecovery();
	}

	private void SetBaseLevelData()
	{
		maxHP = DB.Enhancements["maxHP"].GetEnhanceEffect(R.Player.EnhancementSaveData.MaxHp);
		baseAtk = ((!Debug.isDebugBuild) ? 40 : ((!R.Settings.CheatMode) ? 40 : 9999));
		maxEnergy = ((R.GameData.Difficulty != 3) ? 10 : 1);
		moveSpeed = 9f;
		currentFlashTimes = flashTimes;
		maxChargeTime = 2.5f;
	}

	public void AllAttributeRecovery()
	{
		maxHP = DB.Enhancements["maxHP"].GetEnhanceEffect(R.Player.EnhancementSaveData.MaxHp);
		currentHP = maxHP;
		currentEnergy = maxEnergy;
		currentFlashTimes = flashTimes;
		R.Ui.Flash.RecoverAll(flashTimes);
	}

	public const int maxExecuteNum = 1;

	public const float ChargeTime = 2.5f;

	private const float BaseMoveSpeed = 9f;

	public int faceDir;

	public int maxHP;

	[SerializeField]
	private int _currentHP;

	public int maxEnergy;

	[SerializeField]
	private int _currentEnergy;

	public float moveSpeed;

	public int flashLevel = 1;

	public int currentFlashTimes;

	public int FlashCd;

	public bool flashFlag;

	[HideInInspector]
	public float maxChargeTime;

	public int maxChargeEndureDamage;

	public int baseAtk;

	[NonSerialized]
	private PlatformMovement _platform;
}
