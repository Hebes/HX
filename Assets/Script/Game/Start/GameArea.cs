using UnityEngine;

/// <summary>
/// 游戏区
/// </summary>
[ExecuteInEditMode]
public class GameArea : SingletonMono<GameArea>
{
    public void Awake()
    {
        Init();
    }

    [ContextMenu("Init")]
    private void Init()
    {
        MapRange.min = bottomLeft.position;
        MapRange.max = TopRight.position;
        CameraRange = MapRange;
        PlayerRange.min = new Vector2(CameraRange.min.x - 2f, CameraRange.min.y);
        PlayerRange.max = new Vector2(CameraRange.max.x + 2f, CameraRange.max.y);
        EnemyRange = CameraRange;
    }

    public void OnDrawGizmos()
    {
        if (_showCamera)
            DebugX.DrawRect(CameraRange, _cameraRangeColor);
        if (_showPlayer)
            DebugX.DrawRect(PlayerRange, _playerRangeColor);
        if (_showEnemy)
            DebugX.DrawRect(EnemyRange, _enemyRangeColor);
        if (_showMap)
            DebugX.DrawRect(MapRange, _mapRangeColor);
    }

    [HideInInspector]
    public static Rect MapRange;

    [HideInInspector]
    public static Rect CameraRange;

    [HideInInspector]
    public static Rect PlayerRange;

    [HideInInspector]
    public static Rect EnemyRange;

    [SerializeField]
    private bool _showMap;

    [SerializeField]
    private Color _mapRangeColor = Color.red;

    [SerializeField]
    private bool _showCamera;

    [SerializeField]
    private Color _cameraRangeColor = Color.green;

    [SerializeField]
    private bool _showPlayer;

    [SerializeField]
    private Color _playerRangeColor = Color.cyan;

    [SerializeField]
    private bool _showEnemy;

    [SerializeField]
    private Color _enemyRangeColor = Color.yellow;

    [SerializeField]
    private Transform bottomLeft;

    [SerializeField]
    private Transform TopRight;
}