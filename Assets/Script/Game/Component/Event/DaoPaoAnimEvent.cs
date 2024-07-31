using System.Collections;
using UnityEngine;

/// <summary>
/// 刀炮物体事件
/// </summary>
public class DaoPaoAnimEvent : MonoBehaviour
{
	private Transform player => R.Player.Transform;

	private void Start()
	{
		_rollLoopTimes = 2;
		_eAction = GetComponent<DaoPaoAction>();
		_eAttr = GetComponent<EnemyAttribute>();
		_atkData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.刀炮混合];
		_onion = GetComponent<OnionCreator>();
		_rigidbody2D = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (_eAttr.isDead)
		{
			return;
		}
		if (_rollAttackFly && _eAttr.isOnGround && _rigidbody2D.velocity.y <= 0f)
		{
			_rollAttackFly = false;
			_eAction.AnimChangeState(DaoPaoAction.StateEnum.HitGround);
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
			if (_rigidbody2D.velocity.y <= 0f)
			{
				_eAttr.isFlyingUp = false;
				_eAction.AnimChangeState(DaoPaoAction.StateEnum.HitToFly3);
			}
		}
		if (_eAttr.checkHitGround && _eAttr.isOnGround)
		{
			_eAttr.checkHitGround = false;
			R.Effect.Generate(6, transform);
			if (_eAction.stateMachine.currentState == "HitFall")
			{
				maxFlyHeight = 4f;
				_eAttr.timeController.SetSpeed(Vector2.up * 25f);
				_eAction.AnimChangeState(DaoPaoAction.StateEnum.HitToFly1);
			}
			else
			{
				_eAction.AnimChangeState(DaoPaoAction.StateEnum.FallHitGround);
			}
		}
	}

	public void ChangeState(DaoPaoAction.StateEnum sta)
	{
		_eAction.AnimChangeState(sta.ToString());
	}

	public void PlaySound(int index)
	{
		R.Audio.PlayEffect(index, transform.position);
	}

	public void PlayMoveSound()
	{
		R.Audio.PlayEffect(moveSound[Random.Range(0, moveSound.Length)], transform.position);
	}

	public void PlayHitGroundSound()
	{
		R.Audio.PlayEffect(hitGroundSound[Random.Range(0, hitGroundSound.Length)], transform.position);
	}

	public void RollStart()
	{
		if (_rollOver)
		{
			_rollOver = false;
			_rollLoopTimes = 2;
		}
	}

	public void Atk2Start()
	{
		_rollOver = true;
		_eAttr.paBody = true;
	}

	public void RollOver()
	{
		_rollLoopTimes--;
		if (_rollLoopTimes <= 0)
		{
			if (_eAction.stateMachine.currentState == "DaoAtk6_2" && Random.Range(0, 100) < 50)
			{
				_eAction.FaceToPlayer();
				atk.atkId = Incrementor.GetNextId();
				_eAction.AnimChangeState(DaoPaoAction.StateEnum.DaoAtk4);
			}
			else
			{
				_eAction.AnimChangeState(DaoPaoAction.StateEnum.Idle);
			}
			_rollOver = true;
		}
	}

	public void Atk6_1Over()
	{
		if (Random.Range(0, 100) < 50)
		{
			_eAction.FaceToPlayer();
			atk.atkId = Incrementor.GetNextId();
			_eAction.AnimChangeState(DaoPaoAction.StateEnum.DaoAtk6_2Ready);
		}
		else
		{
			_eAction.AnimChangeState(DaoPaoAction.StateEnum.Idle);
		}
	}

	public void HitGround()
	{
		_rollAttackFly = false;
		_eAttr.checkHitGround = true;
	}

	public void FlyUp()
	{
		_rollAttackFly = false;
		_eAttr.isFlyingUp = true;
	}

	public void SetAtkData()
	{
		atk.atkData = _atkData[_eAction.stateMachine.currentState];
	}

	public void SwordFightStart()
	{
		_rollAttackFly = true;
	}

	public void RunAwayEnd()
	{
		BackToIdle();
		if (!_eAction.IsInNormalState())
		{
			return;
		}
		if (Random.Range(0, 2) == 0)
		{
			int dir = InputSetting.JudgeDir(transform.position, player.position);
			_eAction.Attack3(dir);
		}
	}

	public void GetUp()
	{
		if (_eAttr.isDead)
		{
			DieBlock();
			DestroySelf();
		}
		else
		{
			_eAction.AnimChangeState(DaoPaoAction.StateEnum.GetUp);
		}
	}

	public void BackToIdle()
	{
		if (_eAction.IsInWeakSta())
		{
			_eAttr.enterWeakMod = false;
		}
		_eAction.AnimChangeState(DaoAction.StateEnum.Idle);
	}

	public void WeakOver()
	{
		if (!_eAction.IsInWeakSta())
		{
			_eAction.AnimChangeState(DaoPaoAction.StateEnum.Idle);
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

	public void Shoot()
	{
		Transform transform = Instantiate(bullet);
		EnemyBullet component = transform.GetComponent<EnemyBullet>();
		component.damage = _eAttr.atk;
		component.origin = gameObject;
		transform.position = gunPos.position;
		Vector2 from = gunPos.position - gunAssistant.position;
		if (Vector2.Angle(from, Vector2.right) < 3f)
		{
			from = Vector2.right;
		}
		if (Vector2.Angle(from, -Vector2.right) < 3f)
		{
			from = -Vector2.right;
		}
		transform.GetComponent<Rigidbody2D>().velocity = from.normalized * 12f;
		component.SetAtkData(_atkData["Bullet"]);
	}

	public IEnumerator TargetingPlayer()
	{
		float angle = Vector2.Angle(player.position + Vector3.up - transform.position, Vector2.up);
		if (angle >= 45f)
		{
			gun.GetComponent<SkeletonUtilityBone>().mode = SkeletonUtilityBone.Mode.Override;
			Vector3 startEuler = gun.localEulerAngles;
			float targetAngle = Mathf.Clamp(angle - 8f, 37f, 128f);
			for (int i = 0; i < 40; i++)
			{
				gun.localEulerAngles = Vector3.Lerp(startEuler, new Vector3(0f, 0f, targetAngle), i / 39f);
				yield return null;
			}
		}
	}

	public IEnumerator TargetingRecover()
	{
		if (gun.GetComponent<SkeletonUtilityBone>().mode == SkeletonUtilityBone.Mode.Override)
		{
			Vector3 startEuler = gun.localEulerAngles;
			int clips = (int)(startEuler.z / 2f);
			if (clips > 1)
			{
				for (int i = 0; i < clips; i++)
				{
					gun.localEulerAngles = Vector3.Lerp(startEuler, new Vector3(0f, 0f, 80f), i * 1f / (clips - 1));
					yield return null;
				}
			}
		}
		gun.GetComponent<SkeletonUtilityBone>().mode = SkeletonUtilityBone.Mode.Follow;
	}

	public void PlayHitGroundEffect()
	{
		R.Effect.Generate(40, transform, Vector3.zero, new Vector3(0f, 90 * _eAttr.faceDir, 0f));
	}

	public void DieBlock()
	{
		R.Effect.Generate(163, null, new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f), Vector3.zero);
	}

	public void ChargingDieEffect()
	{
		if (!R.Camera.IsInView(gameObject))
		{
			return;
		}
		GameObject prefab = CameraEffectProxyPrefabData.GetPrefab(9);
		Instantiate(prefab, _eAttr.bounds.center, Quaternion.identity);
	}

	public void OpenOnion()
	{
		_onion.Open(true, 0.7f, onionObj);
	}

	[SerializeField]
	private EnemyAtk atk;

	private JsonData1 _atkData;

	[SerializeField]
	private Transform bullet;

	private DaoPaoAction _eAction;

	private EnemyAttribute _eAttr;

	[SerializeField]
	private Transform gun;

	[SerializeField]
	private Transform gunAssistant;

	[SerializeField]
	private Transform gunPos;

	[SerializeField]
	private int[] hitGroundSound;

	public float maxFlyHeight;

	[SerializeField]
	private int[] moveSound;

	private OnionCreator _onion;

	[SerializeField]
	private GameObject[] onionObj;

	private bool _rollAttackFly;

	private int _rollLoopTimes;

	private bool _rollOver;

	private Rigidbody2D _rigidbody2D;
}
