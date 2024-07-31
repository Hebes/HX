using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Framework.Core;
using UnityEngine;

/// <summary>
/// 世界时间
/// </summary>
public class WorldTime : SingletonNewMono<WorldTime>
{
    public static bool IsPausing { get; private set; }

    public static float LevelTime => Time.time - _sceneStartTime;

    private static int _pauseNum => _timeScaleStack.Count;

    public static float Fps => Mathf.Clamp(WorldTime._fps.Fps, 1f, float.MaxValue);

    public static float PhysicsFps => _fps.PhysicsFps;

    //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public event EventHandler<WorldTime.FrozenArgs> FrozenEvent;

    //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public event EventHandler<WorldTime.FrozenArgs> ResumeEvent;

    /// <summary>
    /// 是否是冻结的
    /// </summary>
    public bool IsFrozen { get; private set; }

    public bool IsSlow { get; private set; }

    private void Start()
    {
        WorldTime._sceneStartTime = Time.time;
        WorldTime._fps.Start();
    }

    private void Update()
    {
        _fps.Update();
    }

    private void FixedUpdate()
    {
        WorldTime._fps.FixedUpdate();
        if (this.IsFrozen && this._autoRecover)
        {
            if (WorldTime.IsPausing)
            {
                return;
            }

            if (this._frozeFrame > 0)
            {
                this._frozeFrame--;
            }
            else
            {
                this._autoRecover = false;
                this.FrozenResume();
            }
        }

        float num = this._slowEnd - Time.time;
        if (this.IsSlow && num < 0f)
        {
            Time.timeScale = Mathf.Clamp01(Time.timeScale + this._slowRecover);
            if (Math.Abs(Time.timeScale - 1f) < 1.401298E-45f)
            {
                this.IsSlow = false;
                this._slowEnd = 0f;
            }
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        WorldTime.Reset();
    }

    public static void Pause()
    {
        ("时间暂停, 推入 " + Time.timeScale).Log();
        WorldTime._timeScaleStack.Push(Time.timeScale);
        Time.timeScale = 0f;
        WorldTime.IsPausing = true;
    }

    public static void Resume()
    {
        if (WorldTime._timeScaleStack.Count == 0)
        {
            return;
        }

        ("Time Resume, Pop " + WorldTime._timeScaleStack.Peek()).Log();
        Time.timeScale = WorldTime._timeScaleStack.Pop();
        WorldTime.IsPausing = false;
    }

    public void TimeSlow(float slowTime, float slowScale)
    {
        if (WorldTime.IsPausing)
        {
            return;
        }

        this._slowEnd = Time.time + slowTime * slowScale;
        this.IsSlow = true;
        Time.timeScale = slowScale;
        this._slowRecover = (1f - slowScale) / 7f;
    }

    public void TimeSlowByFrameOn60Fps(int slowFrame, float slowScale)
    {
        this.TimeSlow((float)slowFrame / 60f, slowScale);
    }

    public void TimeFrozen(float second, WorldTime.FrozenArgs.FrozenType type = WorldTime.FrozenArgs.FrozenType.All,
        bool autoRecover = true)
    {
        this.TimeFrozenByFixedFrame(WorldTime.FixedSecondToFrame(second), type, autoRecover);
    }

    /// <summary>
    /// 时间被固定帧冻结
    /// </summary>
    /// <param name="frame"></param>
    /// <param name="type"></param>
    /// <param name="autoRecover"></param>
    public void TimeFrozenByFixedFrame(int frame,
        WorldTime.FrozenArgs.FrozenType type = WorldTime.FrozenArgs.FrozenType.All, bool autoRecover = true)
    {
        if (frame == 0)
        {
            return;
        }

        this._frozeFrame = frame;
        if (!this.IsFrozen && this.FrozenEvent != null)
        {
            this._frozenArgs = new WorldTime.FrozenArgs(type, null);
            this.FrozenEvent(this, this._frozenArgs);
        }

        this._autoRecover = autoRecover;
        this.IsFrozen = true;
    }

    public void TimeFrozenByFixedFrame(int frame, GameObject frozenTarget)
    {
        if (frame == 0)
        {
            return;
        }

        this._frozeFrame = frame;
        if (!this.IsFrozen && this.FrozenEvent != null)
        {
            this._frozenArgs = new WorldTime.FrozenArgs(WorldTime.FrozenArgs.FrozenType.Target, frozenTarget);
            this.FrozenEvent(this, this._frozenArgs);
        }

        this._autoRecover = true;
        this.IsFrozen = true;
    }

    public void FrozenResume()
    {
        if (!this.IsFrozen) return;
        IsFrozen = false;
        ResumeEvent?.Invoke(this, this._frozenArgs);
    }

    public static float FrameToSecond(int frame)
    {
        return (float)frame / ((WorldTime.Fps <= 30f) ? 60f : WorldTime.Fps);
    }

    public static float FrameToFixedSecond(int frame)
    {
        return (float)frame * Time.fixedDeltaTime;
    }

    public static int SecondToFrame(float second)
    {
        if (second < 0f)
        {
            "秒不能是负数".Error();
            return 0;
        }

        return (int)(second * ((WorldTime.Fps <= 30f) ? 60f : WorldTime.Fps));
    }

    /// <summary>
    /// 固定秒到帧
    /// </summary>
    /// <param name="second"></param>
    /// <returns></returns>
    public static int FixedSecondToFrame(float second)
    {
        if (second < 0f)
        {
            "秒不能是负数".Error();
            return 0;
        }

        return (int)(second / Time.fixedDeltaTime);
    }

    /// <summary>
    /// 等待秒签时间刻度
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public static Coroutine WaitForSecondsIgnoreTimeScale(float seconds)
    {
        return WaitForSecondsIgnoreTimeScaleCoroutine(seconds).StartCoroutine();

        static IEnumerator WaitForSecondsIgnoreTimeScaleCoroutine(float seconds)
        {
            for (var totalSeconds = 0f; totalSeconds < seconds; totalSeconds += Time.unscaledDeltaTime)
                yield return null;
        }
    }


    public static void Reset()
    {
        WorldTime.IsPausing = false;
        WorldTime._timeScaleStack.Clear();
        Time.timeScale = 1f;
    }

    private static float _sceneStartTime;

    private static Stack<float> _timeScaleStack = new Stack<float>();

    private static readonly WorldTime.FramesPerSecond _fps = new WorldTime.FramesPerSecond();

    public const float TargetFps = 60f;

    private WorldTime.FrozenArgs _frozenArgs;

    private int _frozeFrame;

    private bool _autoRecover;

    private float _slowEnd;

    private float _slowRecover;

    public class FrozenArgs : EventArgs
    {
        public FrozenArgs(WorldTime.FrozenArgs.FrozenType type, GameObject target)
        {
            this.Type = type;
            this.Target = target;
        }

        public readonly WorldTime.FrozenArgs.FrozenType Type;

        public readonly GameObject Target;

        public enum FrozenType
        {
            Enemy,
            Player,
            All,
            Target
        }
    }

    private class FramesPerSecond
    {
        internal void Start()
        {
            this._lastInterval = Time.realtimeSinceStartup;
            this._physicsLastInterval = Time.realtimeSinceStartup;
            this._frames = 0;
            this._physicsFrames = 0;
        }

        internal void Update()
        {
            this._frames++;
            if (Time.realtimeSinceStartup > this._lastInterval + 0.5f)
            {
                this.Fps = (float)this._frames / (Time.realtimeSinceStartup - this._lastInterval);
                this._frames = 0;
                this._lastInterval = Time.realtimeSinceStartup;
            }
        }

        internal void FixedUpdate()
        {
            this._physicsFrames++;
            if (Time.realtimeSinceStartup > this._physicsLastInterval + 0.5f)
            {
                this.PhysicsFps = (float)this._physicsFrames / (Time.realtimeSinceStartup - this._physicsLastInterval);
                this._physicsFrames = 0;
                this._physicsLastInterval = Time.realtimeSinceStartup;
            }
        }

        private const float UpdateInterval = 0.5f;

        private float _lastInterval;

        private int _frames;

        internal float Fps;

        private const float PhysicsUpdateInterval = 0.5f;

        private float _physicsLastInterval;

        private int _physicsFrames;

        internal float PhysicsFps;
    }
}