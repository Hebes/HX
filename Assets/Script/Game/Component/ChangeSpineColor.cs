using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 改变动画名称
/// </summary>
public class ChangeSpineColor : MonoBehaviour
{
    private void Awake()
    {
        _gameObjects = new GameObject[_renderers.Length];
        _materials = new Material[_renderers.Length];
        for (int i = 0; i < _renderers.Length; i++)
        {
            _gameObjects[i] = _renderers[i].gameObject;
            _materials[i] = _renderers[i].sharedMaterial;
        }

        ColorBecomeNormal();
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        ColorBecomeNormal();
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        ColorBecomeNormal();
    }

    private void OnSceneUnloaded(Scene arg0)
    {
        ColorBecomeNormal();
    }

    private void Update()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            if (_gameObjects[i].activeSelf && _renderers[i].enabled)
            {
                _materials[i].SetColor("_Color", _tint);
                _materials[i].SetFloat("_EmissionStrength", _emissionStrength);
            }
        }
    }

    private void OnDestroy()
    {
        _emissionStrength = 1f;
        _tint = DefaultColor;
    }

    private void ColorBecomeNormal()
    {
        _emissionStrength = 0f;
        _tint = DefaultColor;
    }

    public IEnumerator EnergyBallColorChange()
    {
        _tint = new Color(0.4f, 0.6f, 0.9f, 1f);
        yield return null;
        for (int i = 0; i < 30; i++)
        {
            _tint.a = Mathf.Lerp(1f, 0f, i / 30f);
            yield return null;
        }

        _tint = DefaultColor;
    }

    public void HurtChange(Color color)
    {
    }

    public void TurnOnEmission()
    {
        _breatheLightTweener?.Pause();
        _emissionStrength = 1f;
    }

    public void TurnOnBreatheLight()
    {
        _breatheLightTweener ??= DOTween.To(delegate(float s) { _emissionStrength = s; }, 1f, 0f, 1f).SetLoops(-1, LoopType.Yoyo);
        if (_breatheLightTweener.IsPlaying()) return;
        _breatheLightTweener.Restart();
    }

    public void TurnOffAll()
    {
        _breatheLightTweener.Pause();
        _emissionStrength = 0f;
    }

    private static readonly Color DefaultColor = new Color(1f, 1f, 1f, 0f);

    [SerializeField] private Color _tint = DefaultColor;

    [Range(0f, 1f)] [SerializeField] private float _emissionStrength;

    [SerializeField] private Renderer[] _renderers;

    private GameObject[] _gameObjects;

    private Material[] _materials;

    private Tweener _breatheLightTweener;
}