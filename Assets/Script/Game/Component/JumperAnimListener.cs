using Framework.Core;
using LitJson;
using UnityEngine;

public class JumperAnimListener : MonoBehaviour
{
	private void Start()
	{
		_eAction = GetComponent<JumperAction>();
		_eAttr = GetComponent<EnemyAttribute>();
		_enemyAtk = GetComponentInChildren<EnemyAtk>();
		_atkData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.跳跃者];
	}

	private void Update()
	{
		if (_eAttr.isDead)
		{
			return;
		}
		if (_eAttr.isFlyingUp)
		{
			bool flag = MaxFlyHeight > 0f && _eAttr.height >= MaxFlyHeight;
			if (flag)
			{
				Vector2 currentSpeed = _eAttr.timeController.GetCurrentSpeed();
				currentSpeed.y = 0f;
				_eAttr.timeController.SetSpeed(currentSpeed);
			}
			if (_eAttr.timeController.GetCurrentSpeed().y <= 0f)
			{
				_eAttr.isFlyingUp = false;
				_eAction.AnimChangeState(JumperAction.StateEnum.FlyToFall);
			}
		}
		if (!_eAttr.checkHitGround)
		{
			return;
		}
		if (!_eAttr.isOnGround)
		{
			return;
		}
		_eAttr.checkHitGround = false;
		if (_eAction.stateMachine.currentState == "HitFall")
		{
			MaxFlyHeight = 4f;
			_eAttr.timeController.SetSpeed(Vector2.up * 25f);
			_eAction.AnimChangeState(JumperAction.StateEnum.HitToFly);
		}
		else
		{
			_eAction.AnimChangeState(JumperAction.StateEnum.HitGround);
		}
	}

	public void ChangeState(JumperAction.StateEnum sta)
	{
		_eAction.AnimChangeState(sta);
	}

	public void StartPabody()
	{
		_eAttr.paBody = true;
	}

	public void EndPabody()
	{
		_eAttr.paBody = false;
	}

	public void Atk3End()
	{
		JumperAction.StateEnum stateEnum = (!_eAction.catchFollow) ? JumperAction.StateEnum.Atk3Fail : JumperAction.StateEnum.Atk3Success;
		if (!_eAction.catchFollow)
		{
			EndPabody();
		}
		_eAction.AnimChangeState(stateEnum);
	}

	public void Atk3Success()
	{
		_eAction.catchFollow = false;
		EndPabody();
		R.Player.Transform.localRotation = Quaternion.identity;
		PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(R.Player.GameObject, gameObject, gameObject, _eAttr.atk, Incrementor.GetNextId(), _atkData["Atk3Success"], true);
		EGameEvent.PlayerHurtAtk.Trigger((transform, args));
	}

	public void SetAtkData()
	{
		_enemyAtk.atkData = _atkData[_eAction.stateMachine.currentState];
	}

	public void SetAtkId()
	{
		_enemyAtk.atkId = Incrementor.GetNextId();
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

	public void FlyUp()
	{
		_eAttr.isFlyingUp = true;
	}

	public void HitGround()
	{
		_eAttr.checkHitGround = true;
	}

	public void GetUp()
	{
		if (_eAttr.isDead)
		{
			DestroySelf();
		}
		else
		{
			_eAction.AnimChangeState(JumperAction.StateEnum.GetUp);
		}
	}

	public void CameraShake(int id)
	{
		if (!R.Camera.IsInView(gameObject))
		{
			return;
		}
		Instantiate(CameraEffectProxyPrefabData.GetPrefab(id));
	}

	public void BackToIdle(JumperAction.StateEnum sta)
	{
		if (_eAction.IsInWeakSta())
		{
			_eAttr.enterWeakMod = false;
		}
		_eAction.AnimChangeState(sta);
	}

	public void DestroySelf()
	{
		RealDestroy();
	}

	private void RealDestroy()
	{
		Destroy(gameObject);
	}

	public void PlayHitGroundEffect()
	{
		R.Effect.Generate(40, transform, Vector3.zero, new Vector3(0f, 90 * _eAttr.faceDir, 0f));
	}

	public void DieBlock()
	{
		R.Effect.Generate(163, null, new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f), Vector3.zero);
	}

	public void HitGroundShake()
	{
		if (!R.Camera.IsInView(gameObject))
		{
			return;
		}
		Instantiate(CameraEffectProxyPrefabData.GetPrefab(13));
	}

	private JumperAction _eAction;

	private EnemyAttribute _eAttr;

	public float MaxFlyHeight;

	private JsonData1 _atkData;

	private EnemyAtk _enemyAtk;

	[SerializeField]
	private int[] moveSound;

	[SerializeField]
	private int[] hitGroundSound;
}
