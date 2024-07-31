using UnityEngine;

/// <summary>
/// 玩家时间控制器
/// </summary>
public class PlayerTimeController : MonoBehaviour, IPlatformPhysics
{
	private void Start()
	{
		this.animControl = base.GetComponent<MultiSpineAnimationController>();
		this.platform = base.GetComponent<PlatformMovement>();
		this.pAttr = R.Player.Attribute;
	}

	private void OnEnable()
	{
		if (SingletonMono<WorldTime>.ApplicationIsQuitting)
		{
			return;
		}
		SingletonMono<WorldTime>.Instance.FrozenEvent += this.ClipFrozen;
		SingletonMono<WorldTime>.Instance.ResumeEvent += this.ClipResume;
	}

	private void OnDisable()
	{
		if (SingletonMono<WorldTime>.ApplicationIsQuitting)
		{
			return;
		}
		SingletonMono<WorldTime>.Instance.FrozenEvent -= this.ClipFrozen;
		SingletonMono<WorldTime>.Instance.ResumeEvent -= this.ClipResume;
	}

	private void OnDestroy()
	{
	}

	public Vector2 GetCurrentSpeed()
	{
		Vector2? vector = this.currentSpeed;
		return (vector == null) ? this.platform.velocity : vector.Value;
	}

	private void FixedUpdate()
	{
		if (this.isPause)
		{
			return;
		}
		Vector2? vector = this.currentSpeed;
		if (vector != null)
		{
			this.platform.velocity = this.currentSpeed.Value;
			this.currentSpeed = null;
		}
	}

	public void SetSpeed(Vector2 speed)
	{
		if (this.isPause)
		{
			this.currentSpeed = new Vector2?(speed);
		}
		else
		{
			this.platform.velocity = speed;
		}
	}

	public void NextPosition(Vector3 nextPos)
	{
		this.platform.position = nextPos;
	}

	private void ClipFrozen(object obj, WorldTime.FrozenArgs e)
	{
		if (e.Type == WorldTime.FrozenArgs.FrozenType.Enemy)
		{
			return;
		}
		if (e.Type == WorldTime.FrozenArgs.FrozenType.Target && base.gameObject != e.Target)
		{
			return;
		}
		if (this.isPause)
		{
			return;
		}
		this.isPause = true;
		this.currentSpeed = new Vector2?(this.platform.velocity);
		this.platform.velocity = Vector2.zero;
		this.platform.isKinematic = true;
		this.animControl.Pause();
	}

	private void ClipResume(object obj, WorldTime.FrozenArgs e)
	{
		if (e.Type == WorldTime.FrozenArgs.FrozenType.Enemy)
		{
			return;
		}
		if (e.Type == WorldTime.FrozenArgs.FrozenType.Target && base.gameObject != e.Target)
		{
			return;
		}
		if (!this.isPause)
		{
			return;
		}
		this.platform.isKinematic = false;
		this.animControl.Resume();
		this.isPause = false;
	}

	public Vector2 velocity
	{
		get
		{
			return this.GetCurrentSpeed();
		}
		set
		{
			this.SetSpeed(value);
		}
	}

	public Vector2 position
	{
		get
		{
			return this.platform.position;
		}
		set
		{
			this.platform.position = value;
		}
	}

	public bool isOnGround
	{
		get
		{
			return this.pAttr.isOnGround;
		}
	}

	private MultiSpineAnimationController animControl;

	private Vector2? currentSpeed;

	[SerializeField]
	public bool isPause;

	private PlatformMovement platform;

	private PlayerAttribute pAttr;

	private Vector2? nextSpeed;
}
