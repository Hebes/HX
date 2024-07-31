using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// UI敌人点控制器
/// </summary>
public class UIEnemyPointController : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (enemy == null)
        {
            NGUITools.Destroy(gameObject);
            return;
        }
        enemyPosition = Camera.main.WorldToViewportPoint(enemy.transform.position).x;
        point.gameObject.SetActive(enemyPosition < 0f || enemyPosition > 1f);
        if (enemyPosition < 0f)
        {
            point.transform.localScale = new Vector3(-1f, 1f, 1f);
            point.transform.localPosition = new Vector3(-(float)UITools.ScreenWidth + 256, 1f / (enemyPosition / 100f - 2f / (float)UITools.ScreenHeight));
        }
        if (enemyPosition > 1f)
        {
            enemyPosition -= 1f;
            point.transform.localScale = Vector3.one;
            point.transform.localPosition = new Vector3(UITools.ScreenWidth - 256, 1f / (-enemyPosition / 100f - 2f / (float)UITools.ScreenHeight));
        }
        if (enemy.currentHp <= 0)
        {
            NGUITools.Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene arg0)
    {
        NGUITools.Destroy(gameObject);
    }

    [SerializeField]
    private Image point;

    [SerializeField]
    public EnemyAttribute enemy;

    [SerializeField]
    private float enemyPosition;
}