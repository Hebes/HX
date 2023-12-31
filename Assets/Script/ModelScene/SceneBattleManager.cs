using Core;
using UnityEngine;

public class SceneBattleManager : MonoBehaviour
{
    public static SceneBattleManager Instance;
    private UIComponent component;

    public UIComponent Component { get => component; }

    private void Awake()
    {
        Instance = this;
        component = GetComponent<UIComponent>();
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
