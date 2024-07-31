using UnityEngine;

public class PlayerFlashAttackAbility : CharacterState
{
	public override void Update()
	{
		if (this._target != null)
		{
			this._clearRate += Time.unscaledDeltaTime;
			if (this._clearRate >= 0.75f)
			{
				this._target = null;
			}
		}
		if (this._flashAtkStart)
		{
			this._recoverRate += Time.unscaledDeltaTime;
			if (this._recoverRate >= 1.5f)
			{
				this.PlayerRecover();
				this._flashAtkStart = false;
			}
		}
		if (this._recoverInvincible)
		{
			this.pab.hurt.Invincible = true;
			this._invincibleTime += Time.unscaledDeltaTime;
			if (this._invincibleTime > 0.6f)
			{
				this._invincibleTime = 0f;
				this.pab.hurt.Invincible = false;
				this._recoverInvincible = false;
			}
		}
	}

	public void FlashAttack(GameObject enemy)
	{
		if (this._target != null)
		{
			return;
		}
		if (enemy == null)
		{
			return;
		}
		this._clearRate = 0f;
		this._target = enemy.gameObject;
		this.StartQTE();
		R.Audio.PlayEffect(172, new Vector3?(this.pac.transform.position));
		this.AttackEnemy(this._target);
	}

	private void StartQTE()
	{
		SingletonMono<WorldTime>.Instance.TimeSlowByFrameOn60Fps(45, 0.15f);
	}

	public bool PressFlashAttack()
	{
		if (this._target == null)
		{
			return false;
		}
		if (this.stateMachine.currentState.IsInArray(PlayerAction.FlashAttackSta))
		{
			this.AttackEnemy(this._target);
			return true;
		}
		return false;
	}

	public bool CheckEnemy(GameObject enemy)
	{
		return enemy == this._target;
	}

	private void AttackEnemy(GameObject enemy)
	{
		this._recoverRate = 0f;
		this._flashAtkStart = true;
		this.listener.StopIEnumerator("FlashPositionSet");
		this.pac.ChangeState(PlayerAction.StateEnum.Disappear, 1f);
		SingletonMono<WorldTime>.Instance.TimeFrozenByFixedFrame(14, enemy);
		R.Audio.PlayEffect(200, new Vector3?(this.pac.transform.position));
		Transform transform = R.Effect.Generate(165, null, enemy.transform.position, default(Vector3), default(Vector3), true);
		transform.GetComponent<ShadeAttack>().Init(enemy);
		transform.localScale = new Vector3((float)(-(float)this.pAttr.faceDir), 1f, 1f);
		Vector3 pos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 2f, Camera.main.transform.parent.position.z + 3f);
		SingletonMono<CameraController>.Instance.CameraZoom(pos, 0.2f, 3f);
	}

	private void PlayerRecover()
	{
		this.pac.ChangeState(PlayerAction.StateEnum.EndAtk, 1f);
		SingletonMono<CameraController>.Instance.CameraZoomFinished();
		this._recoverInvincible = true;
	}

	private GameObject _target;

	private float _clearRate;

	private float _recoverRate;

	private bool _flashAtkStart;

	private bool _recoverInvincible;

	private float _invincibleTime;
}
