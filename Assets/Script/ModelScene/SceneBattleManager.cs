using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 战斗管理器
/// </summary>
public class SceneBattleManager : MonoBehaviour, IPool
{
    public static SceneBattleManager Instance;
    private UIComponent component;
    private GameObject SceneBattleManagerGameObject;

    public UIComponent Component { get => component; }

    public void GetAfter()
    {
        Instance = this;
        if (component == null)
            component = GetComponent<UIComponent>();
        if (SceneBattleManagerGameObject == null)
            SceneBattleManagerGameObject = new GameObject("SceneBattle战斗场景物体寻找");

        SceneManager.UnloadSceneAsync(ConfigScenes.unityScenePersistent);

    }

    public void PushBefore()
    {
        Instance = null;
    }
}
