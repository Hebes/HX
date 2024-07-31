using System;
using System.Diagnostics;
using LitJson;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 动画播放组件
/// </summary>
[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(TimeController))]
[RequireComponent(typeof(SkeletonAnimation))]
public class SpineAnimationController : MonoBehaviour
{
    /// <summary>
    /// 时间尺度
    /// </summary>
    public float TimeScale => _skeletonAnimation.timeScale;

    /// <summary>
    /// 图片名称->皮肤
    /// </summary>
    public string Skin
    {
        get => _skeletonAnimation.skeleton.skin.name;
        set => _skeletonAnimation.skeleton.SetSkin(value);
    }

    /// <summary>
    /// 动画变化
    /// </summary>
    //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public event EventHandler<SpineAnimationController.EffectArgs> OnAnimChange;

    /// <summary>
    /// 动画变化速度
    /// </summary>
    //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public event EventHandler<SpineAnimationController.EffectArgs> OnAnimSpeedChange;

    private void Awake()
    {
        this._timeController = base.GetComponent<TimeController>();
        SkeletonAnimation skeletonAnimation;
        if ((skeletonAnimation = _skeletonAnimation) == null)
            skeletonAnimation = _skeletonAnimation = GetComponent<SkeletonAnimation>();

        _skeletonAnimation = skeletonAnimation;
        _animation = base.GetComponent<Animation>();
        CurrentUnityAnim = string.Empty;
        CurrentSpineAnim = string.Empty;
        //_mappingData = _animMapping == null ? null : JsonMapper.ToObject(this._animMapping.text);
    }

    /// <summary>
    /// 播放
    /// </summary>
    /// <param name="animName">动画名称</param>
    /// <param name="loop">是否循环</param>
    /// <param name="forceChange"></param>
    /// <param name="animSpeed">动画速度</param>
    public void Play(string animName, bool loop = false, bool forceChange = false, float animSpeed = 1f)
    {
        _lastAnimSpeed = animSpeed;
        if (loop && CurrentUnityAnim == animName && MathfX.isInMiddleRange(_lastAnimSpeed, animSpeed, 0.1f))
            return;

        try
        {
            _resumeScale = animSpeed;
            if (WorldTime.Instance.IsFrozen) //是冻结的话
            {
                animSpeed = 0f;
                if (_timeController != null)
                    _timeController.isPause = true;
            }

            this._skeletonAnimation.timeScale = animSpeed;
            this._animation[animName].speed = animSpeed;
            if (this.OnAnimSpeedChange != null)
            {
                this.OnAnimSpeedChange(this, new SpineAnimationController.EffectArgs(animSpeed));
            }

            if (forceChange || this.CurrentUnityAnim != animName)
            {
                this._animation.Stop();
                this._animation[animName].wrapMode = ((!loop) ? WrapMode.Default : WrapMode.Loop);
                this._animation.Play(animName, PlayMode.StopAll);
                this.CurrentUnityAnim = animName;
                animName = _mappingData != null ? _mappingData.Get<string>(animName, animName) : animName;
                this._skeletonAnimation.state.SetAnimation(0, animName, loop);
                this._skeletonAnimation.skeleton.SetToSetupPose();
                this._skeletonAnimation.Update(0f);
                this.OnAnimChange?.Invoke(this, new SpineAnimationController.EffectArgs(animName, loop));
                this.CurrentSpineAnim = animName;
            }

            this.AnimLoop = loop;
            this.AnimForceChange = forceChange;
        }
        catch (IndexOutOfRangeException)
        {
            UnityEngine.Debug.LogError("动画 " + animName + " 不存在");
            throw;
        }
        catch (NullReferenceException)
        {
            UnityEngine.Debug.LogError("动画 " + animName + " 不存在");
            throw;
        }
    }

    public void AddAnim(string animName, bool loop)
    {
        if (this._animation[animName] == null)
        {
            UnityEngine.Debug.LogError("动画 " + animName + " 不存在");
            return;
        }

        this._animation[animName].wrapMode = ((!loop) ? WrapMode.Default : WrapMode.Loop);
        this._animation.PlayQueued(animName, QueueMode.CompleteOthers);
        animName = ((this._mappingData != null) ? this._mappingData.Get<string>(animName, animName) : animName);
        this._skeletonAnimation.state.AddAnimation(0, animName, loop, 0f);
        this._skeletonAnimation.Update(0.01f);
        this.CurrentUnityAnim = animName;
        this.AnimLoop = loop;
    }

    public void Pause()
    {
        this.ChangeTimeScale(0f);
    }

    public void Resume()
    {
        if (Math.Abs(this._resumeScale) < 1.401298E-45f)
        {
            this._resumeScale = 1f;
        }

        this.ChangeTimeScale(this._resumeScale);
    }

    public void Resume(float animSpeed)
    {
        if (this._animation[this.CurrentUnityAnim] != null)
        {
            this._animation[this.CurrentUnityAnim].speed = animSpeed;
        }

        this._skeletonAnimation.timeScale = animSpeed;
    }

    private void ChangeTimeScale(float animSpeed)
    {
        if (this._animation[this.CurrentUnityAnim] != null)
        {
            this._animation[this.CurrentUnityAnim].speed = animSpeed;
        }

        if (this.OnAnimSpeedChange != null)
        {
            this.OnAnimSpeedChange(this, new SpineAnimationController.EffectArgs(animSpeed));
        }

        this._skeletonAnimation.timeScale = animSpeed;
    }

    public bool IsPlaying(string animName)
    {
        return this._animation.IsPlaying(animName);
    }

    public void Play(Enum stateEnum, bool loop = false, bool forceChange = false, float animSpeed = 1f)
    {
        this.Play(stateEnum.ToString(), loop, forceChange, animSpeed);
    }

    [SerializeField] [FormerlySerializedAs("animMapping")]
    private TextAsset _animMapping;

    /// <summary>
    /// 最后的动画速度
    /// </summary>
    private float _lastAnimSpeed;

    private Animation _animation;

    private JsonData1 _mappingData;

    /// <summary>
    /// 动画速度
    /// </summary>
    private float _resumeScale;

    [SerializeField] [FormerlySerializedAs("m_skeletonAnimation")]
    private SkeletonAnimation _skeletonAnimation;

    /// <summary>
    /// 时间控制器
    /// </summary>
    private TimeController _timeController;

    /// <summary>
    /// 动画力的改变
    /// </summary>
    public bool AnimForceChange;

    /// <summary>
    /// 动画是否循环
    /// </summary>
    public bool AnimLoop;

    /// <summary>
    /// 当前Spine动画
    /// </summary>
    public string CurrentSpineAnim;

    /// <summary>
    /// 当前Unity动画
    /// </summary>
    public string CurrentUnityAnim;

    /// <summary>
    /// 影响参数
    /// </summary>
    public class EffectArgs : EventArgs
    {
        public EffectArgs(string effectName, bool _loop)
        {
            this.EffectName = effectName;
            this.Loop = _loop;
        }

        public EffectArgs(float _animSpeed)
        {
            this.AnimSpeed = _animSpeed;
        }

        public readonly string EffectName;

        public readonly bool Loop;

        public float AnimSpeed;
    }
}