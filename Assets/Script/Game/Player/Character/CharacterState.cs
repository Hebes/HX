using System.Collections;
using UnityEngine;

/// <summary>
/// 特征状态
/// </summary>
public abstract class CharacterState
{
	public void Init()
	{
		this.pab = R.Player.Abilities;
		this.pac = R.Player.Action;
		this.pAttr = R.Player.Attribute;
		this.stateMachine = R.Player.StateMachine;
		this.weapon = R.Player.GetComponent<Claymore>();
		this.msac = R.Player.GetComponent<MultiSpineAnimationController>();
		this.listener = R.Player.GetComponent<PlayerAnimEventListener>();
	}

	public virtual void Start()
	{
	}

	public virtual void OnEnable()
	{
	}

	public virtual void OnDisable()
	{
	}

	public virtual void OnDestroy()
	{
	}

	public virtual void Update()
	{
	}

	public virtual void FixedUpdate()
	{
	}

	public virtual void OnStateMachineStateTransfer(object sender, StateMachine.TransferEventArgs args)
	{
	}

	public virtual void OnStateMachineStateEnter(object sender, StateMachine.StateEventArgs args)
	{
	}

	public virtual void OnStateMachineStateExit(object sender, StateMachine.StateEventArgs args)
	{
	}

	public Coroutine StartCoroutine(IEnumerator routine)
	{
		return this.pab.StartCoroutine(routine);
	}

	public Coroutine StartCoroutine(string methodName)
	{
		return this.pab.StartCoroutine(methodName);
	}

	public Coroutine StartCoroutine(string methodName, object value)
	{
		return this.pab.StartCoroutine(methodName, value);
	}

	public Coroutine StartCoroutine_Auto(IEnumerator routine)
	{
		return this.pab.StartCoroutine(routine);
	}

	public void StopAllCoroutines()
	{
		this.pab.StopAllCoroutines();
	}

	public void StopCoroutine(Coroutine routine)
	{
		this.pab.StopCoroutine(routine);
	}

	public void StopCoroutine(IEnumerator routine)
	{
		this.pab.StopCoroutine(routine);
	}

	public void StopCoroutine(string methodName)
	{
		this.pab.StopCoroutine(methodName);
	}

	protected PlayerAction pac;

	protected PlayerAttribute pAttr;

	protected StateMachine stateMachine;

	protected Claymore weapon;

	protected MultiSpineAnimationController msac;

	protected PlayerAnimEventListener listener;

	protected PlayerAbilities pab;

	protected const int LEFT = -1;

	protected const int RIGHT = 1;

	protected const int UP = 2;

	protected const int DOWN = -2;

	protected const int RIGHT_DOWN = -4;

	protected const int LEFT_DOWN = -5;

	protected const int RIGHT_UP = 4;

	protected const int LEFT_UP = 5;

	protected const int STOP = 0;

	protected const int CURRENT = 3;
}
