using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Framework.Core;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : SingletonMono<AudioManager>
{
    public float EffectsVolume
    {
        get { return R.Settings.EffectsVolume; }
        set { R.Settings.EffectsVolume = value; }
    }

    public float BGMVolume
    {
        get { return R.Settings.BGMVolume; }
        set { R.Settings.BGMVolume = value; }
    }

    public bool IsEffectsMute
    {
        get { return R.Settings.IsEffectsMute; }
        set { R.Settings.IsEffectsMute = value; }
    }

    public bool IsBGMMute
    {
        get { return R.Settings.IsBGMMute; }
        set { R.Settings.IsBGMMute = value; }
    }

    private void Awake()
    {
        _bgmSource = _bgmSource1;
        CreatePool();
        Preload();
        AudioListener.volume = 0f;
        DOTween.To(delegate(float v) { AudioListener.volume = v; }, 0f, 1f, 1f).SetDelay(1f);
    }

    private void Update()
    {
        int num = 100;
        int num2 = -20;
        _mainAudioMixer.SetFloat("Effects",
            (!IsEffectsMute) ? (EffectsVolume * -(float)num2 / num + num2) : -80f);
        _mainAudioMixer.SetFloat("BGM", (!IsBGMMute) ? (BGMVolume * -(float)num2 / num + num2) : -80f);
    }

    private void OnEnable()
    {
        EffectsVolume = R.Settings.EffectsVolume;
        BGMVolume = R.Settings.BGMVolume;
    }

    private void OnDisable()
    {
        R.Settings.EffectsVolume = EffectsVolume;
        R.Settings.BGMVolume = BGMVolume;
        R.Settings.Save();
    }

    private static void Preload()
    {
        Instance._audioClipCache = new Dictionary<int, AudioClip>();
        Instance._bgmCache = new Dictionary<string, AudioClip>();
        foreach (AudioClipData audioClipData in DB.AudioClipData.Values)
        {
            if (audioClipData.type != AudioClipData.AudioClipDataType.Group)
            {
                AudioClip audioClip = Asset.LoadFromResources<AudioClip>(audioClipData.path, audioClipData.name);
                if (audioClip == null)
                {
                    string text = string.Format("位于 {0} 的{1}号音效{2}({3})无法被加载", audioClipData.path, audioClipData.id, audioClipData.name, audioClipData.desc);
                    if (audioClipData.name.Contains(" "))
                    {
                        (text + "，音效名包含空格").Warning();
                    }
                    else
                    {
                        text.Warning();
                    }
                }
                else
                {
                    if (audioClipData.pitchMax < audioClipData.pitchMin)
                    {
                        $"音效ID:{audioClipData.id}音调最大值 小于 音调最小值".Warning();
                    }

                    if (audioClipData.volumeMax < audioClipData.volumeMin)
                    {
                        $"音效ID:{audioClipData.id}音量最大值 小于 音量最小值".Warning();
                    }

                    if (audioClipData.type == AudioClipData.AudioClipDataType.BGM)
                    {
                        Instance._bgmCache.Add(audioClipData.path + audioClipData.name, audioClip);
                    }
                    else
                    {
                        Instance._audioClipCache.Add(audioClipData.id, audioClip);
                    }
                }
            }
        }
    }

    private void CreatePool()
    {
        for (int i = 0; i < _audioList.Count; i++)
        {
            AudioMap audioMap = _audioList[i];
            ObjectPool objectPool = new ObjectPool();
            StartCoroutine(objectPool.Init(audioMap.Prefab, PoolParent.gameObject, 5, 10));
            _audioPool.Add(audioMap.Name, objectPool);
        }
    }

    public AudioSource PlayEffect(int id, Vector3? position = null)
    {
        Vector3 vector = position ?? R.Camera.Transform.position;
        AudioClipData audioClipData = AudioClipData.FindById(id);
        switch (audioClipData.type)
        {
            case AudioClipData.AudioClipDataType.EnemyBoss:
                return PlayAudioClip(audioClipData, vector, "EnemyBoss", enemyBossSourcePrefab);
            case AudioClipData.AudioClipDataType.EnemyElite:
                return PlayAudioClip(audioClipData, vector, "EnemyElite", enemyEliteSourcePrefab);
            case AudioClipData.AudioClipDataType.EnemyNormal:
                return PlayAudioClip(audioClipData, vector, "EnemyNormal", enemyNormalSourcePrefab);
            case AudioClipData.AudioClipDataType.PlayerMove:
                return PlayAudioClip(audioClipData, vector, "PlayerMove", playerMoveSourcePrefab);
            case AudioClipData.AudioClipDataType.PlayerAtk:
                return PlayAudioClip(audioClipData, vector, "PlayerAtk", playerAtkSourcePrefab);
            case AudioClipData.AudioClipDataType.PlayerMaterial:
                return PlayAudioClip(audioClipData, vector, "PlayerMaterial", playerMaterialSourcePrefab);
            case AudioClipData.AudioClipDataType.UI:
                return PlayAudioClip2D(audioClipData, _uiSource);
            case AudioClipData.AudioClipDataType.Group:
            {
                int?[] array = StringTool.ParseIntArray(audioClipData.path, ';');
                int num = Random.Range(0, array.Length);
                int? num2 = array[num];
                if (num2 != null && num2.Value != id)
                {
                    return PlayEffect(num2.Value, vector);
                }

                return null;
            }
            case AudioClipData.AudioClipDataType.Scene:
                return PlayAudioClip(audioClipData, vector, "Scene", sceneSourcePrefab);
            case AudioClipData.AudioClipDataType.Special:
                return PlayAudioClip(audioClipData, vector, "Special", specialSourcePrefab);
            case AudioClipData.AudioClipDataType.Video:
                return PlayAudioClip(audioClipData, vector, "Video", videoSourcePrefab);
            default:
                $"有个未被分类的音效ID是{id}，是怎么回事呢".Warning();
                return PlayAudioClip(audioClipData, vector, "UnSorted", unSortedSourcePrefab);
        }
    }

    private AudioSource PlayAudioClip2D(AudioClipData data, [NotNull] AudioSource source)
    {
        AudioClip clip = _audioClipCache[data.id];
        source.clip = clip;
        source.Play();
        return source;
    }

    public AudioSource PlayAudioClip2D(AudioClip clip, AudioClipData.AudioClipDataType type)
    {
        AudioSource audioSource;
        if (type != AudioClipData.AudioClipDataType.UI)
        {
            if (type != AudioClipData.AudioClipDataType.Video)
            {
                throw new ArgumentNullException(nameof(type), "非2D类型");
            }

            audioSource = Instantiate(videoSourcePrefab).GetComponent<AudioSource>();
        }
        else
        {
            audioSource = _uiSource;
        }

        audioSource.clip = clip;
        audioSource.Play();
        return audioSource;
    }

    private AudioSource PlayAudioClip(AudioClipData data, Vector3 pos, string type, [NotNull] GameObject sourcePrefab)
    {
        AudioClip clip = _audioClipCache[data.id];
        GameObject gameObject = LoadFromPool(type);
        GameObject gameObject2 = (!(gameObject != null)) ? Instantiate(sourcePrefab) : gameObject;
        AudioSource component = gameObject2.GetComponent<AudioSource>();
        if (!gameObject2.activeSelf)
        {
            gameObject2.SetActive(true);
        }

        component.clip = clip;
        component.pitch = Random.Range(data.pitchMin, data.pitchMax);
        component.volume = Random.Range(data.volumeMin, data.volumeMax);
        component.Play();
        component.transform.position = pos;
        return component;
    }

    private GameObject LoadFromPool(string type)
    {
        ObjectPool objectPool;
        return (!_audioPool.TryGetValue(type, out objectPool)) ? null : objectPool.GetObject();
    }

    public YieldInstruction PlayBGM(int id, bool loop = true)
    {
        AudioClipData audioClipData = AudioClipData.FindById(id);
        if (audioClipData.type != AudioClipData.AudioClipDataType.BGM)
        {
            $"ID{id}的 Clip，其类型不是BGM".Warning();
        }

        AudioClip audioClip = null;
        AudioClip audioClip2;
        if (audioClipData.name.Contains("Intro"))
        {
            audioClip = Instance._bgmCache[audioClipData.path + audioClipData.name];
            audioClip2 = Instance._bgmCache[audioClipData.path + audioClipData.name.Replace("Intro", "Loop")];
        }
        else if (audioClipData.name.Contains("Loop"))
        {
            audioClip = Instance._bgmCache[audioClipData.path + audioClipData.name.Replace("Loop", "Intro")];
            audioClip2 = Instance._bgmCache[audioClipData.path + audioClipData.name];
        }
        else
        {
            audioClip2 = Instance._bgmCache[audioClipData.path + audioClipData.name];
        }

        if (audioClip == null)
        {
            return PlayBGM(audioClip2, loop);
        }

        StopCoroutine(PlayBgmWithIntroCoroutine(null, null));
        return PlayBgmWithIntro(audioClip, audioClip2);
    }

    public void PauseBGM()
    {
        _bgmSource.Pause();
    }

    public void UnPauseBGM()
    {
        _bgmSource.UnPause();
    }

    public YieldInstruction StopBGM(bool fade = true)
    {
        StopCoroutine(PlayBgmWithIntroCoroutine(null, null));
        if (fade)
        {
            return AudioSourceFadeOut(_bgmSource);
        }

        _bgmSource.Stop();
        _bgmSource.clip = null;
        return null;
    }

    private YieldInstruction PlayBGM(AudioClip clip, bool loop = true)
    {
        if (!_bgmSource.isPlaying)
        {
            _bgmSource.Stop();
            _bgmSource.volume = 1f;
            _bgmSource.loop = loop;
            _bgmSource.clip = clip;
            _bgmSource.Play();
            return WaitForEndOfPlaying(_bgmSource);
        }

        if (_bgmSource.clip == clip)
        {
            _bgmSource.loop = loop;
            return null;
        }

        return PlayBgmWithCrossFade(clip, loop);
    }

    private YieldInstruction PlayBgmWithIntro(AudioClip introClip, AudioClip loopClip)
    {
        return StartCoroutine(PlayBgmWithIntroCoroutine(introClip, loopClip));
    }

    private IEnumerator PlayBgmWithIntroCoroutine(AudioClip introClip, AudioClip loopClip)
    {
        if (_bgmSource.clip == introClip || _bgmSource.clip == loopClip)
        {
            yield break;
        }

        yield return PlayBGM(introClip, false);
        if (_bgmSource.clip == introClip)
        {
            yield return PlayBGM(loopClip);
        }
    }

    private Coroutine PlayBgmWithCrossFade(AudioClip nextClip, bool isNextClipLoop = true)
    {
        AudioSource bgmSource = _bgmSource;
        _bgmSource = ((!(_bgmSource == _bgmSource1)) ? _bgmSource1 : _bgmSource2);
        _bgmSource.loop = isNextClipLoop;
        _bgmSource.clip = nextClip;
        _bgmSource.Play();
        AudioSourceFadeOut(bgmSource);
        _bgmSource.volume = 0f;
        AudioSourceFadeIn(_bgmSource);
        return WaitForEndOfPlaying(_bgmSource);
    }

    private Coroutine WaitForEndOfPlaying(AudioSource source)
    {
        return StartCoroutine(WaitForEndOfPlayingCoroutine(source));
    }

    private IEnumerator WaitForEndOfPlayingCoroutine(AudioSource source)
    {
        while (source.isPlaying)
        {
            yield return null;
        }
    }

    private YieldInstruction AudioSourceFadeOut(AudioSource source)
    {
        if (source.isPlaying)
        {
            return source.DOFade(0f, 0.5f).OnComplete(delegate { source.clip = null; }).WaitForCompletion();
        }

        return null;
    }

    private YieldInstruction AudioSourceFadeIn(AudioSource source)
    {
        if (source.isPlaying)
        {
            return source.DOFade(1f, 0.5f).WaitForCompletion();
        }

        return null;
    }

    public YieldInstruction PlayVoiceOver(string key, Action callback = null, bool hideSubtitle = false)
    {
        StopVoiceOver();
        YieldInstruction result;
        try
        {
            VoiceOver[] voiceOvers = VoiceOver.FindByKey(key);
            _playVoiceOverReturn = StartCoroutine(PlayVoiceOverCoroutine(voiceOvers, callback, hideSubtitle));
            result = _playVoiceOverReturn;
        }
        catch (KeyNotFoundException)
        {
            // if (Log.CurrentLevel <= Log.LogLevel.Debug)
            // {
            //     throw;
            // }

            result = null;
        }

        return result;
    }

    public void PauseVoiceOver()
    {
        if (_playVoiceOverReturn != null)
        {
            _voiceOverSource.Pause();
        }
    }

    public void ResumeVoiceOver()
    {
        if (_playVoiceOverReturn != null)
        {
            _voiceOverSource.UnPause();
        }
    }

    public void StopVoiceOver()
    {
        if (_playVoiceOverReturn != null)
        {
            StopCoroutine(_playVoiceOverReturn);
            _voiceOverSource.Stop();
            R.Ui.UISubtitle.Hide();
        }
    }

    private IEnumerator PlayVoiceOverCoroutine(VoiceOver[] voiceOvers, Action callback, bool hideSubtitle)
    {
        string currentSceneName = LevelManager.SceneName;
        for (var i = 0; i < voiceOvers.Length; i++)
        {
            if (currentSceneName == LevelManager.SceneName)
            {
                //Log.Info(string.Format("Play {0}:{1}", voiceOvers[i].Key, voiceOvers[i].LocalizedSubtitle));
                "开始播放语音".Warning();
                //yield return base.StartCoroutine(this.PlayVoiceOverSingleCoroutine(voiceOvers[i], hideSubtitle));
            }
        }

        if (callback != null)
        {
            callback();
        }

        yield break;
    }

    /// <summary>
    /// 在单个协同程序上播放语音
    /// </summary>
    /// <param name="voiceOver"></param>
    /// <param name="hideSubtitle"></param>
    /// <returns></returns>
    private IEnumerator PlayVoiceOverSingleCoroutine(VoiceOver voiceOver, bool hideSubtitle)
    {
        //string path = $"Audio/VoiceOver/{UIAudioLanguage.CurrentAudioLanguage.Name}/{voiceOver.FilePath}";
        string path = $"Audio/VoiceOver/{1}/{voiceOver.FilePath}";
        string clipName = voiceOver.FileName;
        if (string.IsNullOrEmpty(voiceOver.FileName))
        {
            clipName = voiceOver.Key;
        }

        AudioClip clip = clipName.Combine(path).Load<AudioClip>();
        _voiceOverSource.clip = clip;
        _voiceOverSource.Play();
        if (R.Settings.SubtitleVisiable && !hideSubtitle)
        {
            R.Ui.UISubtitle.FadeIn(voiceOver.LocalizedSubtitle);
        }

        bool isComplate;
        bool isSkip;
        do
        {
            yield return null;
            isComplate = (_voiceOverSource.timeSamples == 0 && !_voiceOverSource.isPlaying);
            isSkip = false;
            // if (Log.CurrentLevel == Log.LogLevel.Debug)
            // {
                isSkip = UnityEngine.Input.GetKey(KeyCode.N);
            //}
        } while (!isComplate && !isSkip);

        if (isSkip)
        {
            _voiceOverSource.Stop();
        }

        if (R.Settings.SubtitleVisiable && !hideSubtitle)
        {
            R.Ui.UISubtitle.FadeOut();
        }

        yield break;
    }

    public Coroutine PlayVoiceOverArray(string[] array, float interval, Action callback = null, float delay = 0f)
    {
        StopVoiceOver();
        return StartCoroutine(PlayVoiceOverArrayCoroutine(array, interval, callback, delay));
    }

    private IEnumerator PlayVoiceOverArrayCoroutine(string[] array, float interval, Action callback, float delay = 0f)
    {
        string currentSceneName = LevelManager.SceneName;
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < array.Length; i++)
        {
            if (currentSceneName == LevelManager.SceneName)
            {
                yield return PlayVoiceOver(array[i]);
                yield return null;
                yield return new WaitForSeconds(interval);
            }
        }

        if (callback != null)
        {
            callback();
        }
    }

    [SerializeField] private AudioMixer _mainAudioMixer;

    [SerializeField] private AudioSource _bgmSource;

    [SerializeField] private AudioSource _bgmSource1;

    [SerializeField] private AudioSource _bgmSource2;

    [SerializeField] private AudioSource _voiceOverSource;

    [SerializeField] private AudioSource _uiSource;

    [SerializeField] private GameObject enemyBossSourcePrefab;

    [SerializeField] private GameObject enemyEliteSourcePrefab;

    [SerializeField] private GameObject enemyNormalSourcePrefab;

    [SerializeField] private GameObject playerMoveSourcePrefab;

    [SerializeField] private GameObject playerAtkSourcePrefab;

    [SerializeField] private GameObject playerMaterialSourcePrefab;

    [SerializeField] private GameObject unSortedSourcePrefab;

    [SerializeField] private GameObject sceneSourcePrefab;

    [SerializeField] private GameObject specialSourcePrefab;

    [SerializeField] private GameObject videoSourcePrefab;

    public Transform PoolParent;

    private Dictionary<int, AudioClip> _audioClipCache;

    private Dictionary<string, AudioClip> _bgmCache;

    private Dictionary<string, ObjectPool> _audioPool = new Dictionary<string, ObjectPool>();

    [SerializeField] private List<AudioMap> _audioList = new List<AudioMap>();

    private const float FadeDuration = 0.5f;

    private Coroutine _playVoiceOverReturn;

    [Serializable]
    private struct AudioMap
    {
        public string Name;

        public GameObject Prefab;
    }
}