using System.Collections;
using LitJson;
using UnityEngine;

public class DaoEnemyAnimListener : MonoBehaviour
{
	private Transform player
	{
		get
		{
			return R.Player.Transform;
		}
	}

	private void Start()
	{
		this._rollLoopTimes = 2;
		this._eAction = base.GetComponent<DaoAction>();
		this._eAttr = base.GetComponent<EnemyAttribute>();
		this._atkData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.斩轮式一型];
		this._onion = base.GetComponent<OnionCreator>();
		this._rigidbody2D = base.GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (this._eAttr.isDead)
		{
			return;
		}
		if (this._rollAttackFly && this._eAttr.isOnGround && this._rigidbody2D.velocity.y <= 0f)
		{
			this._rollAttackFly = false;
			this._eAction.AnimChangeState(DaoAction.StateEnum.HitGround, 1f);
		}
		if (this._eAttr.isFlyingUp)
		{
			bool flag = this.maxFlyHeight > 0f && this._eAttr.height >= this.maxFlyHeight;
			if (flag)
			{
				Vector2 currentSpeed = this._eAttr.timeController.GetCurrentSpeed();
				currentSpeed.y = 0f;
				this._eAttr.timeController.SetSpeed(currentSpeed);
			}
			if (this._rigidbody2D.velocity.y <= 0f)
			{
				this._eAttr.isFlyingUp = false;
				this._eAction.AnimChangeState(DaoAction.StateEnum.HitToFly3, 1f);
			}
		}
		if (this._eAttr.checkHitGround && this._eAttr.isOnGround)
		{
			this._eAttr.checkHitGround = false;
			R.Effect.Generate(6, base.transform, default(Vector3), default(Vector3), default(Vector3), true);
			if (this._eAction.stateMachine.currentState == "HitFall")
			{
				this.maxFlyHeight = 4f;
				this._eAttr.timeController.SetSpeed(Vector2.up * 25f);
				this._eAction.AnimChangeState(DaoAction.StateEnum.HitToFly1, 1f);
			}
			else
			{
				this._eAction.AnimChangeState(DaoAction.StateEnum.FallHitGround, 1f);
			}
		}
	}

	public void ChangeState(DaoAction.StateEnum sta)
	{
		this._eAction.AnimChangeState(DaoAction.State.StateArray[(int)sta], 1f);
	}

	public void PlaySound(int index)
	{
		R.Audio.PlayEffect(index, new Vector3?(base.transform.position));
	}

	public void PlayMoveSound()
	{
		R.Audio.PlayEffect(this.moveSound[UnityEngine.Random.Range(0, this.moveSound.Length)], new Vector3?(base.transform.position));
	}

	public void PlayHitGroundSound()
	{
		R.Audio.PlayEffect(this.hitGroundSound[UnityEngine.Random.Range(0, this.hitGroundSound.Length)], new Vector3?(base.transform.position));
	}

	public void RollStart()
	{
		if (this._rollOver)
		{
			this._rollOver = false;
			this._rollLoopTimes = 2;
		}
	}

	public void Atk2Start()
	{
		this._rollOver = true;
		this._eAttr.paBody = true;
	}

	public void RollOver()
	{
		this._rollLoopTimes--;
		if (this._rollLoopTimes <= 0)
		{
			if (this._eAction.stateMachine.currentState == "DaoAtk6_2" && UnityEngine.Random.Range(0, 100) < 50)
			{
				this._eAction.FaceToPlayer();
				this.atk.atkId = Incrementor.GetNextId();
				this._eAction.AnimChangeState(DaoAction.StateEnum.DaoAtk2Ready, 1f);
			}
			else
			{
				this._eAction.AnimChangeState(DaoAction.StateEnum.Idle, 1f);
			}
			this._rollOver = true;
		}
	}

	public void Atk6_1Over()
	{
		if (UnityEngine.Random.Range(0, 100) < 50)
		{
			this._eAction.FaceToPlayer();
			this.atk.atkId = Incrementor.GetNextId();
			this._eAction.AnimChangeState(DaoAction.StateEnum.DaoAtk6_2Ready, 1f);
		}
		else
		{
			this._eAction.AnimChangeState(DaoAction.StateEnum.Idle, 1f);
		}
	}

	public void HitGround()
	{
		this._rollAttackFly = false;
		this._eAttr.checkHitGround = true;
	}

	public void FlyUp()
	{
		this._rollAttackFly = false;
		this._eAttr.isFlyingUp = true;
	}

	public void SetAtkData()
	{
		this.atk.atkData = this._atkData[this._eAction.stateMachine.currentState];
	}

	public void SwordFightStart()
	{
		this._rollAttackFly = true;
	}

	public void RunAwayEnd()
	{
		this.BackToIdle();
		if (!this._eAction.IsInNormalState())
		{
			return;
		}
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			int dir = InputSetting.JudgeDir(base.transform.position, this.player.position);
			this._eAction.Attack3(dir);
		}
	}

	public void GetUp()
	{
		if (this._eAttr.isDead)
		{
			this.DieBlock();
			this.DestroySelf();
		}
		else
		{
			this._eAction.AnimChangeState(DaoAction.StateEnum.GetUp, 1f);
		}
	}

	public void BackToIdle()
	{
		if (this._eAction.IsInWeakSta())
		{
			this._eAttr.enterWeakMod = false;
		}
		this._eAction.AnimChangeState(DaoAction.StateEnum.Idle, 1f);
	}

	public void WeakOver()
	{
		if (!this._eAction.IsInWeakSta())
		{
			this._eAction.AnimChangeState(DaoAction.StateEnum.Idle, 1f);
		}
	}

	public void DestroySelf()
	{
		this.RealDestroy();
	}

	private void RealDestroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void Shoot()
	{
		Transform transform = UnityEngine.Object.Instantiate<Transform>(this.bullet);
		EnemyBullet component = transform.GetComponent<EnemyBullet>();
		component.damage = this._eAttr.atk;
		component.origin = base.gameObject;
		transform.position = this.gunPos.position;
		Vector2 from = this.gunPos.position - this.gunAssistant.position;
		if (Vector2.Angle(from, Vector2.right) < 3f)
		{
			from = Vector2.right;
		}
		if (Vector2.Angle(from, -Vector2.right) < 3f)
		{
			from = -Vector2.right;
		}
		transform.GetComponent<Rigidbody2D>().velocity = from.normalized * 12f;
		component.SetAtkData(this._atkData["Bullet"]);
	}

	public IEnumerator TargetingPlayer()
	{
		float angle = Vector2.Angle(this.player.position + Vector3.up - base.transform.position, Vector2.up);
		if (angle >= 45f)
		{
			this.gun.GetComponent<SkeletonUtilityBone>().mode = SkeletonUtilityBone.Mode.Override;
			Vector3 startEuler = this.gun.localEulerAngles;
			float targetAngle = Mathf.Clamp(angle - 8f, 37f, 128f);
			for (int i = 0; i < 40; i++)
			{
				this.gun.localEulerAngles = Vector3.Lerp(startEuler, new Vector3(0f, 0f, targetAngle), (float)i / 39f);
				yield return null;
			}
		}
		yield break;
	}

	public IEnumerator TargetingRecover()
	{
		if (this.gun.GetComponent<SkeletonUtilityBone>().mode == SkeletonUtilityBone.Mode.Override)
		{
			Vector3 startEuler = this.gun.localEulerAngles;
			int clips = (int)(startEuler.z / 2f);
			if (clips > 1)
			{
				for (int i = 0; i < clips; i++)
				{
					this.gun.localEulerAngles = Vector3.Lerp(startEuler, new Vector3(0f, 0f, 80f), (float)i * 1f / (float)(clips - 1));
					yield return null;
				}
			}
		}
		this.gun.GetComponent<SkeletonUtilityBone>().mode = SkeletonUtilityBone.Mode.Follow;
		yield break;
	}

	public void PlayHitGroundEffect()
	{
		R.Effect.Generate(40, base.transform, Vector3.zero, new Vector3(0f, (float)(90 * this._eAttr.faceDir), 0f), default(Vector3), true);
	}

	public void DieBlock()
	{
		R.Effect.Generate(163, null, new Vector3(base.transform.position.x, base.transform.position.y, base.transform.position.z - 0.1f), Vector3.zero, default(Vector3), true);
	}

	public void ChargingDieEffect()
	{
		if (!R.Camera.IsInView(base.gameObject))
		{
			return;
		}
		GameObject prefab = CameraEffectProxyPrefabData.GetPrefab(9);
		UnityEngine.Object.Instantiate<GameObject>(prefab, this._eAttr.bounds.center, Quaternion.identity);
	}

	public void Die2Spark()
	{
		R.Effect.Generate(144, base.transform, new Vector3((float)(-(float)this._eAttr.faceDir), 1.2f, LayerManager.ZNum.Fx), Vector3.zero, default(Vector3), true);
	}

	public void Die2Explosion()
	{
		if (!R.Camera.IsInView(base.gameObject))
		{
			return;
		}
		UnityEngine.Object.Instantiate<GameObject>(CameraEffectProxyPrefabData.GetPrefab(12));
		R.Effect.Generate(9, base.transform, new Vector3((float)(-(float)this._eAttr.faceDir), 1.2f, LayerManager.ZNum.Fx), Vector3.zero, default(Vector3), true);
	}

	public void OpenOnion()
	{
		this._onion.Open(true, 0.7f, this.onionObj);
	}

	public float maxFlyHeight;

	private DaoAction _eAction;

	private EnemyAttribute _eAttr;

	private bool _rollOver;

	private bool _rollAttackFly;

	private int _rollLoopTimes;

	[SerializeField]
	private EnemyAtk atk;

	private JsonData1 _atkData;

	[SerializeField]
	private Transform bullet;

	[SerializeField]
	private Transform gun;

	[SerializeField]
	private Transform gunPos;

	[SerializeField]
	private Transform gunAssistant;

	private OnionCreator _onion;

	[SerializeField]
	private GameObject[] onionObj;

	private Rigidbody2D _rigidbody2D;

	[SerializeField]
	private int[] moveSound;

	[SerializeField]
	private int[] hitGroundSound;
}
