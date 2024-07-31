using System.Collections;
using Framework.Core;
using LitJson;
using UnityEngine;

public class JumperFooterAnimEvent : MonoBehaviour
{
	private GameObject player => R.Player.GameObject;

	private void Awake()
	{
		this._action = base.GetComponent<JumperFooterAction>();
		this._eAttr = base.GetComponent<EnemyAttribute>();
		this._enemyAtk = base.GetComponentInChildren<EnemyAtk>();
	}

	private void Start()
	{
		this._jsonData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.跳拳大脚组合];
	}

	public void ChangeState(JumperFooterAction.StateEnum sta)
	{
		this._action.AnimChangeState(sta, 1f);
	}

	public void SetAtkData()
	{
		this._enemyAtk.atkData = this._jsonData[this._action.stateMachine.currentState];
		this._enemyAtk.atkId = Incrementor.GetNextId();
	}

	public void Atk2Finish()
	{
		if (this._action.Atk2Success)
		{
			this._action.Atk2Result = true;
			this._action.AnimChangeState(JumperFooterAction.StateEnum.Atk2Success, 1f);
		}
		else
		{
			this._action.Atk2Result = false;
			this._action.AnimChangeState(JumperFooterAction.StateEnum.Atk2Fail, 1f);
		}
	}

	public void Atk2Release()
	{
		this._action.Atk2Success = false;
		R.Player.GetComponent<Collider2D>().enabled = false;
		R.Player.Transform.localRotation = Quaternion.identity;
		PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(R.Player.GameObject, base.gameObject, base.gameObject, this._eAttr.atk, Incrementor.GetNextId(), this._jsonData["Atk2Release"], true);
		EGameEvent.PlayerHurtAtk.Trigger((transform, args));
		Vector3 position = base.transform.position;
		position.z = LayerManager.ZNum.MMiddleE(this._eAttr.rankType);
		base.transform.position = position;
	}

	public void Atk2FailEnd()
	{
		int num = UnityEngine.Random.Range(0, 100);
		if (num < 40)
		{
			this._action.Defence();
		}
		else
		{
			this._action.AnimChangeState(JumperFooterAction.StateEnum.Idle, 1f);
		}
	}

	public void BackToIdle()
	{
		if (this._action.IsInWeakSta())
		{
			this._eAttr.enterWeakMod = false;
			this._action.AnimChangeState(JumperFooterAction.StateEnum.HitToWeakMod, 1f);
		}
		else
		{
			this._action.AnimChangeState(JumperFooterAction.StateEnum.Idle, 1f);
		}
	}

	public IEnumerator WeakOver()
	{
		for (int i = 0; i < 75; i++)
		{
			if (this._action.stateMachine.currentState == "WeakMod" && !this._action.IsInWeakSta())
			{
				this._action.AnimChangeState(JumperFooterAction.StateEnum.WeakModToIdle, 1f);
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	public void DestroySelf()
	{
		base.Invoke("RealDestroy", 2f);
		base.gameObject.SetActive(false);
	}

	private void RealDestroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void QTEHurtShadeAtk()
	{
		Transform transform = R.Effect.Generate(177, null, base.transform.position, default(Vector3), default(Vector3), true);
		Vector3 one = Vector3.one;
		one.x *= (float)this._eAttr.faceDir;
		transform.localScale = one;
	}

	public void QTEHurtPlayerHitGround()
	{
		this.player.transform.position = base.transform.position + Vector3.up * 5f + Vector3.right * (float)this._eAttr.faceDir;
		int dir = InputSetting.JudgeDir(this.player.transform.position, base.transform.position);
		R.Player.Action.TurnRound(dir);
		R.Player.Action.ChangeState(PlayerAction.StateEnum.QTEHitGround, 1f);
	}

	public void QTEFinalHurt()
	{
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(base.gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.QTEHurt);
		EGameEvent.EnemyHurtAtk.Trigger((player, args));
	}

	public void QTEEffect()
	{
		EnemyBaseHurt component = base.GetComponent<EnemyBaseHurt>();
		component.HitEffect(component.center.localPosition, "Atk1");
		R.Audio.PlayEffect(257, new Vector3?(base.transform.position));
	}

	public void QTEHurtShadeAtkBack()
	{
		Transform transform = R.Effect.Generate(177, null, base.transform.position, default(Vector3), default(Vector3), true);
		Vector3 one = Vector3.one;
		one.x *= (float)(-(float)this._eAttr.faceDir);
		transform.localScale = one;
	}

	public void QTEDiePush()
	{
		R.Player.Action.ChangeState(PlayerAction.StateEnum.QTEPush, 1f);
	}

	public void ExecuteFinalHurt()
	{
		EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(base.gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.Execute, string.Empty);
		EGameEvent.EnemyHurtAtk.Trigger((player.gameObject, args));
	}

	public void PlayAudio(int id)
	{
		R.Audio.PlayEffect(id, new Vector3?(base.transform.position));
	}

	private JumperFooterAction _action;

	private EnemyAttribute _eAttr;

	private EnemyAtk _enemyAtk;

	private JsonData1 _jsonData;
}
