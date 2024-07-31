using System.Collections;
using System.Collections.Generic;
using Framework.Core;
using UnityEngine;

/// <summary>
/// 杰克动画事件
/// </summary>
public class JackAnimEvent : MonoBehaviour
{
	private Transform player
	{
		get
		{
			return R.Player.Transform;
		}
	}

	private void Awake()
	{
		_action = GetComponent<JackAction>();
		_eAttr = GetComponent<EnemyAttribute>();
		_enemyAtk = GetComponentInChildren<EnemyAtk>();
	}

	private void Start()
	{
		jsonData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.杰克];
	}

	private void Update()
	{
		if (_eAttr.isDead)
		{
			return;
		}
		if (_eAttr.isFlyingUp)
		{
			bool flag = maxFlyHeight > 0f && _eAttr.height >= maxFlyHeight;
			if (flag)
			{
				Vector2 currentSpeed = _eAttr.timeController.GetCurrentSpeed();
				currentSpeed.y = 0f;
				_eAttr.timeController.SetSpeed(currentSpeed);
			}
			if (_eAttr.timeController.GetCurrentSpeed().y <= 0f)
			{
				_eAttr.isFlyingUp = false;
				_action.AnimChangeState(JackAction.StateEnum.FlyToFall);
			}
		}
		if (_eAttr.checkHitGround && _eAttr.isOnGround)
		{
			_eAttr.checkHitGround = false;
			R.Effect.Generate(6, transform);
			_action.AnimChangeState(DaoAction.StateEnum.FallHitGround);
		}
	}

	public void ChangeState(JackAction.StateEnum sta)
	{
		_action.AnimChangeState(sta);
	}

	public void FlyUp()
	{
		_eAttr.isFlyingUp = true;
	}

	public void CheckHitGround()
	{
		_eAttr.checkHitGround = true;
	}

	public void SetAtkData()
	{
		_enemyAtk.atkData = jsonData[_action.stateMachine.currentState];
		SetAtkID();
	}

	public void SetGivenAtkData(string state)
	{
		_enemyAtk.atkData = jsonData[state];
		SetAtkID();
	}

	public void SetAtkID()
	{
		_enemyAtk.atkId = Incrementor.GetNextId();
	}

	public void PlaySound(int id)
	{
		R.Audio.PlayEffect(id, transform.position);
	}

	public void LunchBulletLeft(float angle)
	{
		Transform transform = Instantiate(bulletPrefab, leftGun.position, Quaternion.identity);
		transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, _eAttr.faceDir * (angle - 90f));
		Rigidbody2D component = transform.GetComponent<Rigidbody2D>();
		Vector2 vector = new Vector2(_eAttr.faceDir * Mathf.Cos(angle * 0.0174532924f), Mathf.Sin(angle * 0.0174532924f));
		component.velocity = vector.normalized * 15f;
		EnemyBullet component2 = transform.GetComponent<EnemyBullet>();
		component2.damage = _eAttr.atk;
		component2.origin = gameObject;
		component2.SetAtkData(jsonData["Atk1"]);
	}

	public void LunchBulletRight(float angle)
	{
		Transform transform = Instantiate(bulletPrefab, rightGun.position, Quaternion.identity);
		transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, _eAttr.faceDir * (angle - 90f));
		Rigidbody2D component = transform.GetComponent<Rigidbody2D>();
		Vector2 vector = new Vector2(_eAttr.faceDir * Mathf.Cos(angle * 0.0174532924f), Mathf.Sin(angle * 0.0174532924f));
		component.velocity = vector.normalized * 15f;
		EnemyBullet component2 = transform.GetComponent<EnemyBullet>();
		component2.damage = _eAttr.atk;
		component2.origin = gameObject;
		component2.SetAtkData(jsonData["Atk1"]);
	}

	public void PlayAimEffect()
	{
		StartCoroutine(Aim());
	}

	public IEnumerator Aim()
	{
		_realList = new List<Transform>();
		for (int j = 0; j < 3; j++)
		{
			_realList.Add(Instantiate(aimReal));
			_realList[j].GetComponent<JackAimReal>().Jack = transform;
			_realList[j].gameObject.SetActive(false);
		}
		for (int i = 0; i < 7; i++)
		{
			Transform effect = R.Effect.Generate(200, null, (player.position + transform.position) / 2f + new Vector3(Random.Range(-7f, 5f), Random.Range(-0.5f, 4f), -0.01f));
			effect.GetComponent<SkeletonAnimation>().state.SetAnimation(0, "ShowDisappear", false);
			R.Audio.PlayEffect(320, effect.position);
			if (i == 2)
			{
				StartCoroutine(AimReal());
			}
			yield return new WaitForSeconds(0.03f);
		}
	}

	public IEnumerator AimReal()
	{
		for (int i = 0; i < 3; i++)
		{
			_realList[i].transform.position = player.position + Vector3.up;
			R.Audio.PlayEffect(320, _realList[i].transform.position);
			_realList[i].gameObject.SetActive(true);
			_realList[i].GetComponent<Animation>().Play("ShowShootAppear");
			yield return new WaitForSeconds(0.35f);
		}
	}

	public void SpwanEffect(string anim)
	{
		GameObject gameObject = Instantiate(jackEffect, transform.position + Vector3.back * 0.01f, Quaternion.identity);
		gameObject.GetComponent<SkeletonAnimation>().state.SetAnimation(0, anim, false);
		gameObject.transform.localScale = transform.localScale;
		gameObject.AddComponent<AutoDestroy>();
		Object.Destroy(gameObject.GetComponent<EnemyEffectSync>());
	}

	public void BackToIdle()
	{
		if (_action.IsInWeakSta())
		{
			_eAttr.enterWeakMod = false;
			_action.AnimChangeState(JackAction.StateEnum.IdleToWeakMod);
		}
		else
		{
			_action.AnimChangeState(JackAction.StateEnum.Idle);
		}
	}

	public IEnumerator WeakOver()
	{
		for (int i = 0; i < 65; i++)
		{
			if (_action.stateMachine.currentState == "WeakMod" && !_action.IsInWeakSta())
			{
				_action.AnimChangeState("WeakModToIdle");
			}
			yield return new WaitForFixedUpdate();
		}
	}

	public void DestroySelf()
	{
		RealDestroy();
	}

	private void RealDestroy()
	{
		Destroy(gameObject);
	}

	public void QTEHurtPlayerPush()
	{
		R.Player.Action.ChangeState(PlayerAction.StateEnum.QTEPush);
	}

	public void QTEFinalHurt()
	{
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.QTEHurt);
		EGameEvent.EnemyHurtAtk.Trigger((player.gameObject, args));
	}

	public void QTEHurtEffect()
	{
		EnemyBaseHurt component = GetComponent<EnemyBaseHurt>();
		component.HitEffect(component.center.localPosition);
		R.Camera.Controller.CameraShake(0.2f);
		R.Audio.PlayEffect(153, transform.position);
	}

	public void ExecutePlayerPush()
	{
		R.Player.Action.ChangeState(PlayerAction.StateEnum.BeelzebubQTEDie);
	}

	public void ExecuteFinalHurt()
	{
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.Execute, string.Empty);
		EGameEvent.EnemyHurtAtk.Trigger((player.gameObject, args));
	}

	public float maxFlyHeight;

	private JackAction _action;

	private EnemyAttribute _eAttr;

	public Transform aimReal;

	public JsonData1 jsonData;

	private List<Transform> _realList;

	[SerializeField]
	private Transform leftGun;

	[SerializeField]
	private Transform rightGun;

	[SerializeField]
	private Transform bulletPrefab;

	[SerializeField]
	private GameObject jackEffect;

	private EnemyAtk _enemyAtk;
}
