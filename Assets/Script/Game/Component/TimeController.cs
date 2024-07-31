using UnityEngine;

/// <summary>
/// 时间控制器
/// </summary>
[RequireComponent(typeof(SpineAnimationController))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyAttribute))]
public class TimeController : MonoBehaviour, IPlatformPhysics
{
    [Header("设置重力")]public float? currentGravity;
    [Header("设置速度")] public Vector2? currentSpeed;
    [Header("是否暂停")] [SerializeField] public bool isPause;
    [Header("敌人属性")]private EnemyAttribute eAttr;
    [Header("动画播放组件")] private SpineAnimationController animControl;
    [Header("下一次速度")] private Vector2? nextSpeed;
    private Rigidbody2D rigid;

    public Vector2 velocity
    {
        get => GetCurrentSpeed();
        set => SetSpeed(value);
    }

    public Vector2 position
    {
        get => rigid.position;
        set => rigid.position = value;
    }

    public bool isOnGround => eAttr.isOnGround;


    private void Awake()
    {
        animControl = base.GetComponent<SpineAnimationController>();
        rigid = base.GetComponent<Rigidbody2D>();
        eAttr = base.GetComponent<EnemyAttribute>();
    }

    private void OnEnable()
    {
        if (WorldTime.ApplicationIsQuitting) return;
        WorldTime.Instance.FrozenEvent += this.ClipFrozen;
        WorldTime.Instance.ResumeEvent += this.ClipResume;
    }

    private void OnDisable()
    {
        if (WorldTime.ApplicationIsQuitting) return;
        WorldTime.Instance.FrozenEvent -= this.ClipFrozen;
        WorldTime.Instance.ResumeEvent -= this.ClipResume;
    }
    
    private void FixedUpdate()
    {
        if (isPause) return;
        Vector2? vector = currentSpeed;
        if (vector != null)
        {
            rigid.velocity = currentSpeed.Value;
            currentSpeed = null;
        }

        float? num = currentGravity;
        if (num != null)
        {
            rigid.gravityScale = currentGravity.Value;
            currentGravity = null;
        }
    }

    /// <summary>
    /// 获取当前速度
    /// </summary>
    /// <returns></returns>
    public Vector2 GetCurrentSpeed()
    {
        Vector2? vector = currentSpeed;
        return vector ?? rigid.velocity;
    }
    
    /// <summary>
    /// 设置速度
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(Vector2 speed)
    {
        if (isPause)
            currentSpeed = new Vector2?(speed);
        else
            rigid.velocity = speed;
    }

    /// <summary>
    /// 设置重力
    /// </summary>
    /// <param name="scale"></param>
    public void SetGravity(float scale)
    {
        if (isPause)
            currentGravity = new float?(scale);
        else
            rigid.gravityScale = scale;
    }

    /// <summary>
    /// 播放冻结
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="e"></param>
    private void ClipFrozen(object obj, WorldTime.FrozenArgs e)
    {
        if (e.Type == WorldTime.FrozenArgs.FrozenType.Player) return;
        if (e.Type == WorldTime.FrozenArgs.FrozenType.Target && gameObject != e.Target) return;
        if (this.isPause) return;

        isPause = true;
        currentSpeed = new Vector2?(this.rigid.velocity);
        currentGravity = new float?(this.rigid.gravityScale);
        rigid.gravityScale = 0f;
        rigid.velocity = Vector2.zero;
        animControl.Pause();
    }

    /// <summary>
    /// 播放恢复
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="e"></param>
    private void ClipResume(object obj, WorldTime.FrozenArgs e)
    {
        if (!isPause) return;
        animControl.Resume();
        isPause = false;
    }
}