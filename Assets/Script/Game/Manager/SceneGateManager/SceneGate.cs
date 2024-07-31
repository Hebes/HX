using Framework.Core;
using UnityEngine;

/// <summary>
/// 场景大门
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class SceneGate : MonoBehaviour
{
    private BoxCollider2D _trigger;
    public SwitchLevelGateData data;
    [SerializeField] private string _checkIdInSaveStorage;

    [SerializeField] [Header("允许进入的方式与方向")]
    public OpenType openType = OpenType.All;

    [SerializeField] [Header("允许出现在空中")] private bool InAir;

    private void Awake()
    {
        _trigger = GetComponent<BoxCollider2D>();
        if (_trigger == null)
            ("场景中没有Collider2D " + LevelManager.SceneName + " 大门 " + name).Error();
        else
            data.TriggerSize = Vector2.Scale(_trigger.size, transform.localScale);

        data.SelfPosition = transform.position;
        data.OpenType = openType;
        data.InAir = InAir;
    }

    public void OnEnable()
    {
        R.SceneGate.GatesInCurrentScene.Add(this);
    }

    public void OnDisable()
    {
        if (!SceneGateManager.ApplicationIsQuitting)
            R.SceneGate.GatesInCurrentScene.Remove(this);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!AllowEnterGate(collision)) return;
        Vector3 position = R.Player.Transform.position;
        float num = 0.5f;
        bool flag = openType == OpenType.All;
        bool flag2 = openType == OpenType.Left && position.x < transform.position.x + num && R.Player.Attribute.faceDir == 1;
        bool flag3 = openType == OpenType.Right && position.x > transform.position.x - num && R.Player.Attribute.faceDir == -1;
        bool flag4 = openType == OpenType.Top && position.y > transform.position.y - num;
        bool flag5 = openType == OpenType.Bottom && position.y < transform.position.y + num;
        if (flag || flag2 || flag3 || flag4 || flag5)
            R.SceneGate.Enter(data);
        if (openType == OpenType.PressKey && Input.Game.Search.OnClick)
            R.SceneGate.Enter(data);
    }

    /// <summary>
    /// 允许进入门
    /// </summary>
    /// <param name="collision"></param>
    /// <returns></returns>
    private bool AllowEnterGate(Collider2D collision)
    {
        bool flag = R.SceneGate.IsLocked || R.Player == null ||
                    !collision.CompareTag(ConfigTag.Player) || openType == OpenType.None ||
                    R.Mode.CheckMode(Mode.AllMode.Battle) || R.Player.Action.NotAllowPassSceneGate || !InputSetting.IsWorking() ||
                    (!string.IsNullOrEmpty(_checkIdInSaveStorage) && SaveStorage.Get(_checkIdInSaveStorage, false));
        return !flag;
    }

    public Coroutine Enter(bool needProgressBar = false)
    {
        return R.SceneGate.Enter(data, needProgressBar);
    }

    public Coroutine Exit(float groundDis = 0f, OpenType lastGateOpenType = OpenType.None)
    {
        return R.SceneGate.Exit(data, groundDis, lastGateOpenType);
    }

    /// <summary>
    /// 打开方式
    /// </summary>
    public enum OpenType
    {
        None = 1,

        /// <summary>
        /// 所有
        /// </summary>
        All,

        /// <summary>
        /// 左边
        /// </summary>
        Left,

        /// <summary>
        /// 右边
        /// </summary>
        Right,

        /// <summary>
        /// 顶部
        /// </summary>
        Top,

        /// <summary>
        /// 底部
        /// </summary>
        Bottom,

        /// <summary>
        /// 按键
        /// </summary>
        PressKey
    }
}