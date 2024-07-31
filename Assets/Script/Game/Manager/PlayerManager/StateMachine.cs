using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Framework.Core;
using UnityEngine;

/// <summary>
/// 状态机
/// </summary>
public class StateMachine : MonoBehaviour
{
    //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public event EventHandler<StateMachine.StateEventArgs> OnEnter;

    //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public event EventHandler<StateMachine.StateEventArgs> OnExit;

    //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public event EventHandler<StateMachine.StateEventArgs> StateUpdate;

    //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public event EventHandler<StateMachine.TransferEventArgs> OnTransfer;

    public string currentState { get; private set; }

    private void Awake()
    {
        string.Intern("__Empty__");
        this.AddState("__Empty__");
        this.currentState = "__Empty__";
    }

    private void Update()
    {
        StateMachine.State state;
        if (!string.IsNullOrEmpty(this.currentState) && this.currentState != "__Empty__" &&
            this.states.TryGetValue(this.currentState, out state))
        {
            this.OnTrigger(this.StateUpdate, state.name);
            this.OnTrigger(state.StateUpdate, state.name);
        }
    }

    public void AddStates(Type type)
    {
        this.AddStates(Enum.GetNames(type));
    }

    public StateMachine.State AddState(Enum state)
    {
        return this.AddState(state.ToString());
    }

    private StateMachine.State AddState(string name)
    {
        if (this.states.ContainsKey(name))
        {
            this.Info("在状态机中添加了同名方法" + name);
        }

        StateMachine.State state = new StateMachine.State
        {
            name = name
        };
        this.states[name] = state;
        return state;
    }

    public void AddStates(string[] names)
    {
        for (int i = 0; i < names.Length; i++)
        {
            this.AddState(names[i]);
        }
    }

    public StateMachine.State GetState(string name)
    {
        return this.states[name];
    }

    public StateMachine.State GetState(Enum name)
    {
        return this.GetState(name.ToString());
    }

    public void SetState(Enum nextState)
    {
        this.SetState(nextState.ToString());
    }

    public void SetState(string nextState)
    {
        this.Info(string.Format("当前状态为{0},准备转换状态为{1}", this.currentState, nextState));
        if (this.currentState == "__Empty__")
        {
            this.OnTrigger(this.states[nextState].OnEnter, this.states[nextState].name);
            this.OnTrigger(this.OnEnter, this.states[nextState].name);
            this.currentState = nextState;
            return;
        }

        this.OnTrigger(this.states[this.currentState].OnExit, this.states[this.currentState].name);
        this.OnTrigger(this.OnExit, this.states[this.currentState].name);
        this.OnTrigger(this.OnTransfer, this.currentState, nextState);
        try
        {
            this.OnTrigger(this.states[nextState].OnEnter, this.states[nextState].name);
        }
        catch (KeyNotFoundException)
        {
            nextState.Error();
            throw;
        }

        this.OnTrigger(this.OnEnter, this.states[nextState].name);
        this.currentState = nextState;
    }

    public void SetStateDelay(Enum nextState, float time)
    {
        this._nextState = nextState;
        base.StartCoroutine(this.SetStateDelayCoroutine(time));
    }

    private IEnumerator SetStateDelayCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        this.SetState(this._nextState);
        yield break;
    }

    private void OnTrigger(EventHandler<StateMachine.StateEventArgs> handler, string lastState)
    {
        if (handler != null)
        {
            StateMachine.StateEventArgs e = new StateMachine.StateEventArgs(lastState);
            handler(this, e);
        }
    }

    private void OnTrigger(EventHandler<StateMachine.TransferEventArgs> handler, string lastState, string nextState)
    {
        if (handler != null)
        {
            StateMachine.TransferEventArgs e = new StateMachine.TransferEventArgs(lastState, nextState);
            handler(this, e);
        }
    }

    private void Info(object obj)
    {
        obj.Log();
    }

    public Dictionary<string, StateMachine.State> states = new Dictionary<string, StateMachine.State>();

    private const string EmptyState = "__Empty__";

    private Enum _nextState;

    public class State
    {
        public EventHandler<StateMachine.StateEventArgs> OnEnter;


        public EventHandler<StateMachine.StateEventArgs> OnExit;


        public EventHandler<StateMachine.StateEventArgs> StateUpdate;


        public string name;
    }

    public class TransferEventArgs : EventArgs
    {
        public TransferEventArgs(string lastState, string nextState)
        {
            this.lastState = lastState;
            this.nextState = nextState;
        }

        public readonly string lastState;

        public readonly string nextState;
    }

    public class StateEventArgs : EventArgs
    {
        public StateEventArgs(string state)
        {
            this.state = state;
        }

        public readonly string state;
    }
}