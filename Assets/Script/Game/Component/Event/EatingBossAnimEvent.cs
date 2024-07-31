using System.Collections;
using Framework.Core;
using LitJson;
using UnityEngine;

/// <summary>
/// 暴食Boss的事件
/// </summary>
public class EatingBossAnimEvent : MonoBehaviour
{
	private Transform player => R.Player.Transform;

	private void Awake()
	{
		this._action = base.GetComponent<EatingBossAction>();
		this._eAttr = base.GetComponent<EnemyAttribute>();
		this._enemyAtk = base.GetComponentInChildren<EnemyAtk>();
	}

	private void Start()
	{
		this._atk2LoopTimes = 2;
		this._jsonData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.卡洛斯];
	}

	private void Update()
	{
		if (!this._cameraFollow)
		{
			return;
		}
		Vector3 position = R.Camera.Controller.MovableCamera.position;
		position.x = base.transform.position.x;
		Vector3 one = Vector3.one;
		Vector3.SmoothDamp(R.Camera.Controller.MovableCamera.position, position, ref one, 0.1f);
	}

	public void ChangeState(EatingBossAction.StateEnum sta)
	{
		this._action.AnimChangeState(sta, 1f);
	}

	public void Atk5Loop()
	{
		this.atk5Loop--;
		if (this.atk5Loop <= 0)
		{
			this.ChangeState(EatingBossAction.StateEnum.Atk5End);
		}
	}

	public void SetAtkData()
	{
		this._enemyAtk.atkData = this._jsonData[this._action.stateMachine.currentState];
		this.SetAtkId();
	}

	public void SetAtkId()
	{
		this._enemyAtk.atkId = Incrementor.GetNextId();
	}

	public void SummonEnemyOne()
	{
		EnemyType type = (R.GameData.Difficulty > 1) ? EnemyType.暴食 : EnemyType.斩轮式一型;
		if (R.Enemy.GetEnemyCountByType(type) < 1)
		{
			this.SummonEnemy(type);
		}
	}

	public void SummonEnemyTwo()
	{
		EnemyType type = (R.GameData.Difficulty > 1) ? EnemyType.炮击式一型 : EnemyType.蜜蜂;
		if (R.Enemy.GetEnemyCountByType(type) < 1)
		{
			this.SummonEnemy(type);
		}
	}

	public void SummonEnemy(EnemyType type)
	{
		Vector2 value = new Vector2(UnityEngine.Random.Range(GameArea.EnemyRange.xMin + 3f, GameArea.EnemyRange.xMax - 3f), base.transform.position.y);
		GameObject gameObject = Singleton<EnemyGenerator>.Instance.GenerateEnemy(type, new Vector2?(value), true, true);
		gameObject.GetComponent<EnemyAttribute>().playerInView = true;
	}

	public void Attack2Judge()
	{
		if (this._action.attack2Success)
		{
			this._action.AnimChangeState(EatingBossAction.StateEnum.Atk2Success, 1f);
		}
		else
		{
			this._action.AnimChangeState(EatingBossAction.StateEnum.Atk2End, 1f);
		}
	}

	public void Attack2End()
	{
		this._atk2LoopTimes--;
		if (this._atk2LoopTimes < 0)
		{
			this._atk2LoopTimes = 2;
			this._action.AnimChangeState(EatingBossAction.StateEnum.Atk2SuccessEnd, 1f);
		}
	}

	public void Attack2Release()
	{
		this._action.attack2Success = false;
		this.player.GetComponent<Collider2D>().enabled = false;
		this.player.localRotation = Quaternion.identity;
		PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(this.player.gameObject, base.gameObject, base.gameObject, this._eAttr.atk, Incrementor.GetNextId(), this._jsonData["Atk2Release"], true);
		EGameEvent.PlayerHurtAtk.Trigger((transform, args));
	}

	public void PlayBiteEffect()
	{
		R.Effect.Generate(134, base.transform, new Vector3(-1.36f, 1.8f, 0f), default(Vector3), default(Vector3), true);
	}

	public void Atk4Effect()
	{
		Transform transform = R.Effect.Generate(210, null, base.transform.position, default(Vector3), default(Vector3), true);
		Vector3 localScale = transform.localScale;
		localScale.x *= Mathf.Sign(base.transform.localScale.x);
		transform.localScale = localScale;
		EnemyBullet componentInChildren = transform.GetComponentInChildren<EnemyBullet>();
		componentInChildren.SetAtkData(this._jsonData[this._action.stateMachine.currentState]);
		componentInChildren.origin = base.gameObject;
		componentInChildren.damage = this._eAttr.atk;
	}

	public void BackToIdle()
	{
		if (this._action.IsInWeakSta())
		{
			this._eAttr.enterWeakMod = false;
			this._action.AnimChangeState(EatingBossAction.StateEnum.IdleToWeakMod, 1f);
		}
		else
		{
			this._action.AnimChangeState(EatingBossAction.StateEnum.Idle, 1f);
		}
	}

	public IEnumerator WeakOver()
	{
		for (int i = 0; i < 75; i++)
		{
			if (this._action.stateMachine.currentState == "WeakMod" && !this._action.IsInWeakSta())
			{
				this._action.AnimChangeState(EatingBossAction.StateEnum.WeakModToIdle, 1f);
				break;
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	private void RealDestroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void CameraZoomIn()
	{
		R.Camera.Controller.CameraZoom((this.player.position + base.transform.position) / 2f + Vector3.up * 1.5f, 0.3f, 3f);
	}

	public void CameraFollowStart()
	{
		this._cameraFollow = true;
	}

	public void QTEHurtShadeAtk()
	{
		Transform transform = R.Effect.Generate(177, null, base.transform.position + Vector3.up * 1.5f, default(Vector3), default(Vector3), true);
		Vector3 one = Vector3.one;
		one.x *= (float)this._eAttr.faceDir;
		transform.localScale = one;
	}

	public void QTEHurtShadeAtkBack()
	{
		Transform transform = R.Effect.Generate(177, null, base.transform.position + Vector3.up * 1.5f, default(Vector3), default(Vector3), true);
		Vector3 one = Vector3.one;
		one.x *= (float)(-(float)this._eAttr.faceDir);
		transform.localScale = one;
	}

	public void CameraShake(int frame)
	{
		R.Camera.Controller.CameraShake((float)frame / 60f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
	}

	public void QTEHurtEffect()
	{
		R.Audio.PlayEffect(254, new Vector3?(base.transform.position));
		EnemyBaseHurt component = base.GetComponent<EnemyBaseHurt>();
		component.HitEffect(component.center.localPosition, "Atk1");
	}

	public void PlayerRollAttack()
	{
		this.player.position += Vector3.up * 5f;
		R.Player.Action.ChangeState(PlayerAction.StateEnum.QTERoll, 1f);
		this.player.GetComponent<PlayerTimeController>().SetSpeed(Vector2.right * 4f * (float)R.Player.Attribute.faceDir);
	}

	public void PlayerRollAttackEffect()
	{
		R.Audio.PlayEffect(254, new Vector3?(base.transform.position));
		base.GetComponent<EnemyBaseHurt>().HitEffect(this.player.transform.position - base.transform.position, "Atk1");
		R.Camera.Controller.CameraShake(0.06666667f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
	}

	public void QTEFinalHurt()
	{
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(base.gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.QTEHurt);
		EGameEvent.EnemyHurtAtk.Trigger((player.gameObject, args));
	}

	public void PlayerEndAtk()
	{
		R.Player.Action.ChangeState(PlayerAction.StateEnum.EndAtk, 1f);
	}

	public void CameraFollowEnd()
	{
		this._cameraFollow = false;
	}

	public void ExecutePlayerPush()
	{
		R.Player.Action.ChangeState(PlayerAction.StateEnum.QTEPush, 1f);
	}

	public void ExecuteFinalHurt()
	{
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(base.gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.Execute, string.Empty);
		EGameEvent.EnemyHurtAtk.Trigger((player.gameObject, args));
	}

	public void DestroySelf()
	{
		base.gameObject.SetActive(false);
		base.Invoke("RealDestroy", Time.deltaTime);
	}

	public void PlayAudio(int id)
	{
		R.Audio.PlayEffect(id, new Vector3?(base.transform.position));
	}

	public void Play2DAudio(int id)
	{
		R.Audio.PlayEffect(id, null);
	}

	public void PlayMoveAudio()
	{
		int id = this.moveAudio[UnityEngine.Random.Range(0, this.moveAudio.Length)];
		R.Audio.PlayEffect(id, new Vector3?(base.transform.position));
	}

	private JsonData1 _jsonData;

	private EatingBossAction _action;

	private EnemyAttribute _eAttr;

	private EnemyAtk _enemyAtk;

	public int atk5Loop;

	private int _atk2LoopTimes;

	[SerializeField]
	private int[] moveAudio;

	private bool _cameraFollow;
}
