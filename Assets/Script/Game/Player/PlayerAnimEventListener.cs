using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Framework.Core;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 玩家动画事件监听器
/// </summary>
public class PlayerAnimEventListener : MonoBehaviour
{
	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event EventHandler OnPlayerDead;

	private void Awake()
	{
		_box = GetComponent<BoxCollider2D>();
	}

	private void Start()
	{
		getUp = true;
		stateMachine = GetComponent<StateMachine>();
		pAction = GetComponent<PlayerAction>();
		pAttr = R.Player.Attribute;
		onion = GetComponent<OnionCreator>();
		atkData = null;// JsonMapper.ToObject(atkParm.text);
		_pAtk = atkBox.GetComponent<PlayerAtk>();
	}

	private void Update()
	{
		if (R.Player.TimeController.isPause)
		{
			return;
		}
		if (checkFallDown && R.Player.TimeController.GetCurrentSpeed().y <= 3f)
		{
			checkFallDown = false;
			pAction.ChangeState(PlayerAction.StateEnum.Fall1);
			isFalling = true;
		}
		if (isFalling && pAttr.isOnGround)
		{
			isFalling = false;
			checkFallDown = false;
			PhysicReset();
			PlayerSound(21);
			pAction.ChangeState(PlayerAction.StateEnum.GetUp);
		}
		if (flyHitFlag && R.Player.TimeController.GetCurrentSpeed().y <= 0f && stateMachine.currentState.IsInArray(PlayerAction.HurtSta))
		{
			flyHitFlag = false;
			pAction.ChangeState(PlayerAction.StateEnum.UnderAtkFlyToFall);
			flyHitGround = true;
		}
		if (flyHitGround && pAttr.isOnGround)
		{
			hitJump = false;
			flyHitGround = false;
			PhysicReset();
			Vector2 currentSpeed = R.Player.TimeController.GetCurrentSpeed();
			currentSpeed.x /= 2f;
			R.Player.TimeController.SetSpeed(currentSpeed);
			pAction.ChangeState(PlayerAction.StateEnum.UnderAtkHitGround);
		}
		UpdateFlashEnd();
	}

	// public void SetAtkData()
	// {
	// 	if (atkData.Contains(stateMachine.currentState))
	// 	{
	// 		_pAtk.SetData(atkData[stateMachine.currentState], Incrementor.GetNextId());
	// 	}
	// }

	public void SetAttackId()
	{
		_pAtk.attackId = Incrementor.GetNextId();
	}

	public void FallDown()
	{
		checkFallDown = true;
	}

	public void HitGroundCheck()
	{
		checkFallDown = false;
		isFalling = false;
		airAtkDown = false;
		checkHitGround = true;
	}

	public void Falling()
	{
		isFalling = true;
	}

	public void AirAtkDown()
	{
		isFalling = false;
		checkFallDown = false;
		checkHitGround = false;
		airAtkDown = true;
	}

	public void PlayAnim(PlayerAction.StateEnum sta)
	{
		pAction.ChangeState(sta);
	}

	public void CanChangeState()
	{
		pAction.canChangeAnim = true;
	}

	// public void Speed(string speed)
	// {
	// 	JsonData jsonData = JsonMapper.ToObject(speed);
	// 	float num = jsonData.Get<float>("x", 0f);
	// 	float y = jsonData.Get<float>("y", 0f);
	// 	Vector2 speed2 = new Vector2(transform.localScale.x * -num, y);
	// 	R.Player.TimeController.SetSpeed(speed2);
	// }

	public void PhysicReset()
	{
		R.Player.Rigidbody2D.WakeUp();
		R.Player.Rigidbody2D.gravityScale = 1f;
	}

	public void AirPhysic(float gravityScale)
	{
		R.Player.TimeController.SetSpeed(Vector2.zero);
		R.Player.Rigidbody2D.gravityScale = gravityScale;
	}

	public void MoveBoxSize(float xSize)
	{
		Vector2 offset = _box.offset;
		offset.x = -xSize / 2f;
		_box.offset = offset;
		Vector2 size = _box.size;
		size.x = xSize;
		_box.size = size;
	}

	public void BoxSizeRecover()
	{
		_box.offset = new Vector2(0f, 1f);
		_box.size = new Vector2(1f, 2f);
	}

	public IEnumerator FlashPositionSet()
	{
		float x = 0f;
		float y = 0f;
		PlayerAction.StateEnum nextSta = PlayerAction.StateEnum.Flash2;
		int num = flashDir;
		switch (num + 5)
		{
		case 0:
		case 1:
			x = 3.535f;
			y = -3.535f;
			break;
		case 3:
			y = -5f;
			nextSta = PlayerAction.StateEnum.FlashDown2;
			break;
		case 4:
		case 6:
			x = 5f;
			nextSta = PlayerAction.StateEnum.Flash2;
			break;
		case 7:
			y = 4f;
			nextSta = PlayerAction.StateEnum.FlashUp2;
			break;
		case 9:
		case 10:
			x = 3.535f;
			y = 3.535f;
			break;
		}
		Vector2 deltaPos = new Vector3(transform.localScale.x * -x, y);
		int clip = 8;
		for (int i = 0; i < clip; i++)
		{
			if (i == clip / 2)
			{
				float num2;
				if (deltaPos.x == 0f)
				{
					num2 = 1.57079637f * Mathf.Sign(deltaPos.y);
				}
				else
				{
					num2 = Mathf.Atan(deltaPos.y / deltaPos.x);
				}
				Vector3 position = Vector3.zero;
				if (y == 0f)
				{
					position = Vector3.up * 0.5f;
				}
				R.Effect.Generate(48, transform, position, new Vector3(0f, 0f, num2 * 180f / 3.14159274f));
			}
			Vector3 nextPos = transform.position + new Vector3(deltaPos.x / 8f, deltaPos.y / 8f, 0f);
			R.Player.TimeController.NextPosition(nextPos);
			yield return new WaitForFixedUpdate();
		}
		atkBox.localScale = Vector3.zero;
		atkBox.localPosition = Vector3.zero;
		R.Player.Rigidbody2D.gravityScale = 1f;
		if (pAttr.isOnGround)
		{
			pAction.ChangeState(PlayerAction.StateEnum.FlashGround);
		}
		else
		{
			Vector2 speed = new Vector2(5 * pAttr.faceDir, 10f);
			if (flashDir == 2 || flashDir == -2)
			{
				speed.x = 0f;
			}
			R.Player.TimeController.SetSpeed(speed);
			pAction.ChangeState(nextSta, 1.5f);
		}
	}

	public void PlayerSound(int audioIndex)
	{
		R.Audio.PlayEffect(audioIndex, transform.position);
	}

	public void RandomPlaySound(int audioIndex)
	{
		if (Random.Range(0, 100) > 60)
		{
			return;
		}
		PlayerSound(audioIndex);
	}

	public void PlayRunSound()
	{
		R.Audio.PlayEffect(runSound[Random.Range(0, runSound.Length)], transform.position);
	}

	public void PlayFlashSound()
	{
		R.Audio.PlayEffect(flashSound[Random.Range(0, flashSound.Length)], transform.position);
	}

	public void PlayShoutSoundLight()
	{
	}

	public void PlayShoutSoundHeavy()
	{
	}

	public void PlayHurtSoundLight()
	{
	}

	public void PlayHurtSoundHeavy()
	{
	}

	public void PlayAtkSound()
	{
		R.Audio.PlayEffect(atkSound[Random.Range(0, atkSound.Length)], transform.position);
	}

	public void PlayExecuteSound()
	{
	}

	public void FlyHit()
	{
		checkFallDown = false;
		isFalling = false;
		checkHitGround = false;
		flyHitFlag = true;
	}

	public void HitGetUp()
	{
		if (!pAttr.isDead && getUp)
		{
			getUp = true;
			pAction.ChangeState(PlayerAction.StateEnum.UnderAtkGetUp);
		}
	}

	public IEnumerator HitJumpBack()
	{
		yield return new WaitForSeconds(0.2f);
		hitJump = true;
	}

	public IEnumerator PlayerDieEnumerator()
	{
		R.Ui.Pause.Enabled = false;
		yield return new WaitForSeconds(2f);
		if (OnPlayerDead != null)
		{
			OnPlayerDead(this, null);
		}
	}

	public void PlayEffect(int effectId)
	{
		if (stateMachine.currentState != "QTECharge1End" && stateMachine.currentState != "Charge1End" && stateMachine.currentState != "AirChargeEnd")
		{
			R.Effect.Generate(effectId, this.transform);
			return;
		}
		AtkEffector component = R.Effect.fxData[effectId].effect.GetComponent<AtkEffector>();
		Vector3 pos = component.pos;
		if (component.CanHitGround)
		{
			RaycastHit2D raycastHit2D = Physics2D.Raycast(this.transform.position, -Vector2.up, 100f, LayerManager.GroundMask);
			pos = new Vector3(pos.x, pos.y - Mathf.Clamp(raycastHit2D.distance, 0f, float.PositiveInfinity), pos.z);
		}
		Transform transform = R.Effect.Generate(effectId, this.transform, pos);
		transform.localScale = new Vector3(transform.localScale.x * -Mathf.Sign(this.transform.localScale.x), transform.localScale.y, transform.localScale.z);
		if (component.UseAtkData)
		{
			if (charge)
			{
				charge = false;
				Vector3 position = Camera.main.transform.position.SetZ(-0.2f);
				transform.position = position;
				transform.GetComponent<AtkEffector>().SetData(atkData["Charge1EndLevel1"], Incrementor.GetNextId());
			}
			else
			{
				transform.GetComponent<AtkEffector>().SetData(atkData[stateMachine.currentState], Incrementor.GetNextId());
			}
		}
	}

	public void CloseEnemyEffect()
	{
		R.Effect.Generate(152, transform, Vector3.zero);
	}

	public void CloseEnemyBlast()
	{
		R.Effect.Generate(153, transform, new Vector3(pAttr.faceDir * 2.5f, 1f, 0f));
	}

	public void FlashStart()
	{
		_flashEndCount = WorldTime.SecondToFrame(0.2f);
		pAttr.flashFlag = true;
	}

	private void UpdateFlashEnd()
	{
		if (_flashEndCount > 0)
		{
			_flashEndCount--;
			if (_flashEndCount <= 0)
			{
				pAttr.flashFlag = false;
			}
		}
	}

	public void GetExecuteEnemy()
	{
		Vector2 zero = Vector2.zero;
		for (int i = 0; i < executeEnemyList.Count; i++)
		{
			EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(executeEnemyList[i].gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.ExecuteFollow);
			EGameEvent.EnemyHurtAtk.Trigger((gameObject, args));
			zero.x = (transform.position.x + executeEnemyList[i].transform.position.x) / 2f;
		}
		zero.y = transform.position.y + 2f;
		R.Camera.Controller.CameraShake(0.25f, 0.6f);
		SingletonMono<CameraController>.Instance.OpenMotionBlur(0.13333334f, 1f, transform.position);
		SingletonMono<CameraController>.Instance.CameraZoom(new Vector3(zero.x, zero.y, Camera.main.transform.parent.position.z + 3f), 0.166666672f);
	}

	public void StartExecute()
	{
		for (int i = 0; i < executeEnemyList.Count; i++)
		{
			EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(executeEnemyList[i], EnemyHurtAtkEventArgs.HurtTypeEnum.Execute, stateMachine.currentState);
			EGameEvent.EnemyHurtAtk.Trigger((gameObject, args));
		}
		executeEnemyList.Clear();
		pAction.QTEHPRecover();
	}

	public void ExecuteTimeSlow()
	{
		SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(30, 0.5f);
	}

	public void Execute2_1Hit()
	{
		for (int i = 0; i < executeEnemyList.Count; i++)
		{
			EnemyBaseHurt component = executeEnemyList[i].GetComponent<EnemyBaseHurt>();
			component.StopFollowLeftHand();
			Vector3 position = executeEnemyList[i].transform.position;
			position.y = Mathf.Clamp(position.y + 1f, position.y, LayerManager.YNum.GetGroundHeight(executeEnemyList[i].gameObject) + 4f);
			executeEnemyList[i].transform.position = position;
			EnemyHurtAtkEventArgs.PlayerNormalAtkData attackData = new EnemyHurtAtkEventArgs.PlayerNormalAtkData(atkData[stateMachine.currentState], true)
			{
				camShakeFrame = 0,
				shakeStrength = 0f
			};
			Vector3 position2 = component.center.position;
			EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(executeEnemyList[i].gameObject, gameObject, Incrementor.GetNextId(), position2, HurtCheck.BodyType.Body, attackData);
			EGameEvent.EnemyHurtAtk.Trigger((gameObject, args));
			R.Camera.Controller.OpenMotionBlur(0.13333334f, 1f, transform.position);
			R.Camera.Controller.ZoomFinished();
			R.Camera.Controller.CameraMoveToBySpeed(Camera.main.transform.parent.position + Vector3.up * 4f, 20f, false, Ease.InOutExpo);
			R.Camera.Controller.CameraShake(0.266666681f, 0.4f, CameraController.ShakeTypeEnum.Horizon);
		}
	}

	public IEnumerator Execute2_1ChangeState()
	{
		GameObject enemy = executeEnemyList[0].gameObject;
		EnemyAttribute attr = enemy.GetComponent<EnemyAttribute>();
		while (attr.timeController.GetCurrentSpeed().y > 0f)
		{
			yield return null;
		}
		attr.timeController.SetSpeed(Vector2.zero);
		AirPhysic(0f);
		Vector3 pos = transform.position;
		pos.y = enemy.transform.position.y;
		transform.position = pos;
		pAction.ChangeState(PlayerAction.StateEnum.NewExecute2_2);
	}

	public void AirExecute1_2Hit()
	{
		for (int i = 0; i < executeEnemyList.Count; i++)
		{
			executeEnemyList[i].GetComponent<EnemyAttribute>().timeController.SetSpeed(Vector2.down * 60f);
			Vector3 position = Camera.main.transform.parent.position;
			position.y = LayerManager.YNum.GetGroundHeight(executeEnemyList[i]) + 2f;
			R.Camera.Controller.ZoomFinished();
			R.Camera.Controller.CameraMoveToBySpeed(position, 40f, false, Ease.InOutExpo);
			R.Camera.Controller.CameraShake(0.167f, 0.4f, CameraController.ShakeTypeEnum.Rect, true);
		}
	}

	public void QTECameraFinish()
	{
	}

	public void ChargeEffectAppear()
	{
		if (chargeAnim.gameObject.activeSelf)
		{
			return;
		}
		chargeAnim.gameObject.SetActive(true);
		chargeAnim.ChargeZeroToOne();
	}

	public void ChargeEffectDisappear()
	{
		chargeAnim.gameObject.SetActive(false);
	}

	public void ChargeEnemyFrozen()
	{
		SingletonMono<WorldTime>.Instance.TimeFrozenByFixedFrame(10, WorldTime.FrozenArgs.FrozenType.Enemy);
	}

	public void StopRoll()
	{
		for (int i = 0; i < 4; i++)
		{
			GameObject gameObject = GameObject.Find("NewRoll(Clone)");
			if (gameObject != null)
			{
				gameObject.SetActive(false);
			}
		}
	}

	public void StopIEnumerator(string name)
	{
		StopCoroutine(name);
	}

	public void OpenOnion()
	{
		onion.Open(true, 0.3f, onionObj);
	}

	public Transform atkBox;

	public bool checkFallDown;

	public bool isFalling;

	public bool airAtkDown;

	public int flashDir;

	private PlayerAction pAction;

	private PlayerAttribute pAttr;

	public bool checkHitGround;

	public bool flyHitFlag;

	public bool flyHitGround;

	public bool hitJump;

	private float hitJumpTime;

	[SerializeField]
	private TextAsset atkParm;

	private BoxCollider2D _box;

	public JsonData1 atkData;

	private OnionCreator onion;

	[SerializeField]
	private GameObject[] onionObj;

	private StateMachine stateMachine;

	public List<GameObject> executeEnemyList;

	public ChargeController chargeAnim;

	public bool getUp;

	public bool charge;

	private PlayerAtk _pAtk;

	private int _flashEndCount;

	[SerializeField]
	private int[] runSound;

	[SerializeField]
	private int[] flashSound;

	[SerializeField]
	private int[] hurtSoundLight;

	[SerializeField]
	private int[] hurtSoundHeavy;

	[SerializeField]
	private int[] atkSound;

	[SerializeField]
	private int[] executeSound;
}
