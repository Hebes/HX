using Framework.Core;
using LitJson;
using UnityEngine;

/// <summary>
/// 敌人子弹
/// </summary>
public class EnemyBullet : MonoBehaviour
{
	private bool _hitGround => Physics2D.OverlapBox(transform.position, Vector2.one, 0f, LayerManager.WallMask | LayerManager.GroundMask | LayerManager.OneWayGroundMask | LayerManager.CeilingMask | LayerManager.ObstacleMask);

	private void OnEnable()
	{
		player = null;
		beAtked = false;
	}

	private void Update()
	{
		if (_hitGround && !enableOnGround)
		{
			if (explosionEffect > 0)
			{
				R.Effect.Generate(explosionEffect, null, transform.position);
			}
			R.Audio.PlayEffect(Random.Range(136, 139), transform.position);
			EffectController.TerminateEffect(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "PlayerHurtBox")
		{
			PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(other.transform.parent.gameObject, 
				gameObject, origin, damage, Incrementor.GetNextId(), atkData);
			EGameEvent.PlayerHurtAtk.Trigger((transform,args));
			if (hitAudio > 0)
			{
				R.Audio.PlayEffect(hitAudio, transform.position);
			}
			if (!beAtked && type == BUlletType.Once)
			{
				player = other.transform.parent;
				HitBullet();
			}
			if (type == BUlletType.Stay)
			{
				Destroy(GetComponent<Collider2D>());
			}
		}
	}

	public void HitBullet()
	{
		if (beAtked)
		{
			if (explosionEffect > 0)
			{
				R.Effect.Generate(explosionEffect, null, transform.position);
			}
			R.Effect.Generate(49, null, transform.position);
			R.Audio.PlayEffect(Random.Range(136, 139), transform.position);
		}
		if (player)
		{
			if (explosionEffect > 0)
			{
				R.Effect.Generate(explosionEffect, null, new Vector3((transform.position.x + player.position.x) / 2f, transform.position.y, transform.position.z));
			}
			R.Audio.PlayEffect(Random.Range(136, 139), transform.position);
		}
		EffectController.TerminateEffect(gameObject);
	}

	private void OnDisable()
	{
		if (missAudio > 0)
		{
			R.Audio.PlayEffect(missAudio, transform.position);
		}
	}

	public void SetAtkData(JsonData1 jsonData)
	{
		atkData = jsonData;
	}

	private Transform player;

	public bool beAtked;

	public int damage;

	public JsonData1 atkData;

	[HideInInspector]
	public GameObject origin;

	[SerializeField]
	public int explosionEffect = 90;

	[SerializeField]
	private bool enableOnGround;

	public BUlletType type;

	public EnemyType EnemyTypeOfShooter;

	public int hitAudio;

	public int missAudio;

	public enum BUlletType
	{
		Once,
		Stay,
		Continue
	}
}
