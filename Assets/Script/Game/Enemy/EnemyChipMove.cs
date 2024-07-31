using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class EnemyChipMove : MonoBehaviour
{
	private float deltaTime
	{
		get
		{
			return Time.time - this.startTime;
		}
	}

	private void OnEnable()
	{
		this.canPlay = false;
		this.startTime = Time.time;
		this.destinationPostion = new Vector3(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(1f, 4f), -0.02f);
		this.currentPostion = base.transform.position;
		this._sprite = base.GetComponent<SpriteRenderer>();
		base.StartCoroutine(this.PlayEffect());
	}

	private void Update()
	{
		if (this.canPlay)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.currentPostion + this.destinationPostion, 0.45f * this.speed * this.deltaTime);
			if (Vector3.Distance(base.transform.position, this.destinationPostion) < 0.05f)
			{
				EffectController.TerminateEffect(base.gameObject);
			}
		}
	}

	public IEnumerator PlayEffect()
	{
		yield return new WaitForSeconds(0.1f);
		float startTime = Time.time;
		if (R.Player != null)
		{
			PlayerAttribute pattr = R.Player.Attribute;
			while (Time.time - startTime < this.waitTime)
			{
				if (pattr.isInCharging)
				{
					base.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
					Transform transform = R.Effect.Generate(125, base.transform, default(Vector3), default(Vector3), default(Vector3), true);
					transform.GetComponent<ChildChipExplosion>().backToPlayer = true;
					EffectController.TerminateEffect(base.gameObject);
				}
				yield return new WaitForSeconds(0.1f);
			}
			yield return DOTween.To(() => this._sprite.color, delegate(Color x)
			{
				this._sprite.color = x;
			}, new Color(1f, 1f, 1f, 0f), 0.3f).WaitForCompletion();
			EffectController.TerminateEffect(base.gameObject);
		}
		yield break;
	}

	public bool canPlay;

	private float startTime;

	[SerializeField]
	private float speed = 1f;

	private Vector3 destinationPostion;

	private Vector3 currentPostion;

	public float waitTime = 2f;

	private SpriteRenderer _sprite;
}
