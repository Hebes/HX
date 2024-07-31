using Framework.Core;
using LitJson;
using UnityEngine;

public class EnemyAtk : MonoBehaviour
{
	public JsonData1 atkData
	{
		private get => _atkData;
		set
		{
			_atkData = value;
			hitTimes = value.Get<int>("hitTimes", 0);
			hitInterval = value.Get<float>("hitInterval", 0f);
			hitType = value.Get<int>("hitType", 1);
		}
	}

	private void Start()
	{
		eAttr = transform.parent.GetComponent<EnemyAttribute>();
	}

	private void Update()
	{
		if (atkStart)
		{
			hitInterval = Mathf.Clamp(hitInterval - Time.deltaTime, 0f, float.PositiveInfinity);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name == "PlayerHurtBox")
		{
			PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(other.transform.parent.gameObject, transform.parent.gameObject, transform.parent.gameObject, eAttr.atk, atkId, atkData);
			EGameEvent.PlayerHurtAtk.Trigger((transform, args));
			atkStart = true;
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (SingletonMono<WorldTime>.Instance.IsFrozen)
		{
			return;
		}
		if (atkData == null)
		{
			return;
		}
		if (other.name == "PlayerHurtBox")
		{
			if (hitType == 0)
			{
				UnlimitedAttack(other);
			}
			else if (hitType == 1)
			{
				LimitedAttack(other);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.name == "PlayerHurtBox")
		{
			atkStart = false;
		}
	}

	private void UnlimitedAttack(Collider2D other)
	{
		if (hitInterval <= 0f)
		{
			atkId = Incrementor.GetNextId();
			hitInterval = atkData.Get<float>("hitInterval", 0f);
			PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(other.transform.parent.gameObject, transform.parent.gameObject, transform.parent.gameObject, eAttr.atk, atkId, atkData);
			EGameEvent.PlayerHurtAtk.Trigger((transform, args));
		}
	}

	private void LimitedAttack(Collider2D other)
	{
		if (hitTimes > 0)
		{
			hitInterval -= Time.deltaTime;
			if (hitInterval <= 0f)
			{
				atkId = Incrementor.GetNextId();
				hitInterval = atkData.Get<float>("hitInterval", 0f);
				PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(other.transform.parent.gameObject, transform.parent.gameObject, transform.parent.gameObject, eAttr.atk, atkId, atkData);
				EGameEvent.PlayerHurtAtk.Trigger((transform, args));
				hitTimes--;
			}
		}
	}

	private JsonData1 _atkData;

	private EnemyAttribute eAttr;

	public int atkId;

	private int hitTimes;

	private float hitInterval;

	private int hitType;

	public bool atkStart;
}
