using UnityEngine;

/// <summary>
/// 动画移动控制器
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(StateMachine))]
public class AnimMoveController : MonoBehaviour
{
    private void Awake()
    {
        this._isPlayer = base.gameObject.CompareTag("Player");
        if (this._isPlayer)
        {
            this.platform = base.GetComponent<PlatformMovement>();
        }
    }

    private void Start()
    {
        this.position = Vector3.zero;
        this.lastposition = this.position;
        this.state = base.GetComponent<StateMachine>();
        this.state.OnTransfer += this.OnStateTransfer;
    }

    private void OnStateTransfer(object sender, StateMachine.TransferEventArgs args)
    {
        this.position = Vector3.zero;
        this.lastposition = this.position;
    }

    public void AnimMoveLoopReset()
    {
        this.position = Vector3.zero;
        this.lastposition = this.position;
    }

    private void Update()
    {
        Vector3 vector = this.position - this.lastposition;
        if (vector.x == 0f && vector.y == 0f)
        {
            return;
        }
        vector.Scale((!this.isLocal) ? Vector3.one : base.transform.localScale);
        if (!Application.isPlaying)
        {
            Vector3 vector2 = vector + base.transform.position;
            base.transform.position = vector2;
        }
        else if (this._isPlayer)
        {
            Vector2 vector3 = (Vector2)vector + this.platform.position;
            this.platform.position = vector3;
        }
        else
        {
            Vector3 vector4 = vector + base.transform.position;
            vector4.x = Mathf.Clamp(vector4.x, GameArea.EnemyRange.xMin, GameArea.EnemyRange.xMax);
            if (vector.y < 0f)
            {
                vector4.y = Mathf.Clamp(vector4.y, LayerManager.YNum.GetGroundHeight(base.transform.gameObject), float.MaxValue);
            }
            base.transform.position = vector4;
        }
        this.lastposition = this.position;
    }

    public void Clear()
    {
        this.position = Vector3.zero;
    }

    public bool isLocal = true;

    [SerializeField]
    private Vector3 position;

    private Vector3 lastposition;

    private Animation lastAnim;

    private StateMachine state;

    private PlatformMovement platform;

    private bool _isPlayer;
}