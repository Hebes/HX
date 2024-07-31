using Framework.Core;
using LitJson;
using UnityEngine;

/// <summary>
/// 敌人子弹发射器
/// </summary>
public class EnemyBulletLaucher : MonoBehaviour
{
	private bool isOnGround => Physics2D.Raycast(transform.position, -Vector2.up, 0.36f, LayerManager.GroundMask).collider != null;

	private void OnEnable()
	{
		player = null;
		beAtked = false;
	}

	private void OnDisable()
	{
		if (disablePlayEffect != -1)
		{
			R.Effect.Generate(disablePlayEffect, null, transform.position);
		}
	}

	private void Update()
	{
		if (isOnGround && groundEffect != -1)
		{
			R.Effect.Generate(groundEffect, null, transform.position + new Vector3(0f, -0.3f, 0f));
			EffectController.TerminateEffect(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "PlayerHurtBox")
		{
			PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(R.Player.GameObject, gameObject, 
				attacker.gameObject, damage, Incrementor.GetNextId(), atkData);
			EGameEvent.PlayerHurtAtk.Trigger((transform,args));
			if (!beAtked)
			{
				player = R.Player.Transform;
			}
			HitBullet();
		}
	}

	public void HitBullet()
	{
		if (beAtked && hitEffect != -1)
		{
			R.Effect.Generate(hitEffect, null, transform.position);
			R.Effect.Generate(49, null, transform.position);
		}
		if (player && hitEffect != -1)
		{
			R.Effect.Generate(hitEffect, null, new Vector3((transform.position.x + player.position.x) / 2f, transform.position.y, transform.position.z));
		}
		EffectController.TerminateEffect(gameObject);
	}

	public void SetVelocity(float speed, float angle)
	{
		GetComponent<Rigidbody2D>().velocity = new Vector2(speed * Mathf.Sin(angle * 0.0174532924f), speed * Mathf.Cos(angle * 0.0174532924f));
		transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -angle));
	}

	public void SetAtkData(JsonData1 data)
	{
		atkData = data;
	}

	[SerializeField]
	public float speed;

	[SerializeField]
	public float angle;

	[SerializeField]
	private bool canThrough;

	[SerializeField]
	private int hitEffect = -1;

	[SerializeField]
	private int groundEffect = -1;

	[SerializeField]
	private int disablePlayEffect = -1;

	private JsonData1 atkData;

	[SerializeField]
	public bool beAtked;

	public int damage;

	private Transform player;

	public Transform attacker;
}
