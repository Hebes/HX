using System.Collections;
using System.Collections.Generic;
using Framework.Core;
using UnityEngine;

/*--------脚本描述-----------

描述:
    音频模块

-----------------------*/


namespace Framework.Core
{
    /// <summary> 音乐类型 </summary>
    public enum EAudioSourceType
    {
        /// <summary> 背景音乐 </summary>
        BGM,
        /// <summary> 音效 </summary>
        SFX,
    }

    [System.Serializable]
    public struct AudioData
    {
        public AudioClip audioClip;
        public float volume;
    }

    [CreateCore(typeof(CoreAduio), 6)]
    public class CoreAduio : ICore
    {
        public static CoreAduio Instance;
        public Dictionary<string, AudioData> audioClipDic;         //音效列表
        public Dictionary<string, AudioSource> sudioSourceDic;     //音乐组件列表

        public IEnumerator AsyncEnter()
        {
            yield break;
        }

        public IEnumerator Exit()
        {
            yield break;
        }

        public void Init()
        {
            Instance = this;
            audioClipDic = new Dictionary<string, AudioData>();
            sudioSourceDic = new Dictionary<string, AudioSource>();

            var audioManagerGo = new GameObject("音乐");
            audioManagerGo.AddComponent<AudioListener>();
            Object.DontDestroyOnLoad(audioManagerGo);
            

            sudioSourceDic.Add(EAudioSourceType.BGM.ToString(), audioManagerGo.AddComponent<AudioSource>());
            sudioSourceDic.Add(EAudioSourceType.SFX.ToString(), audioManagerGo.AddComponent<AudioSource>());
        }
    }
}


public static class HelperAduio
{
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="audioSourceType"></param>
    /// <param name="isLoop"></param>
    public static void Play(this AudioData audioClip, EAudioSourceType audioSourceType, bool isLoop = false)
    {
        AudioSource audioSource = null;
        switch (audioSourceType)
        {
            case EAudioSourceType.BGM:
                if (CoreAduio.Instance.sudioSourceDic.TryGetValue(audioSourceType.ToString(), out audioSource))
                {
                    audioSource.clip = audioClip.audioClip;
                    audioSource.loop = isLoop;
                    audioSource.volume = audioClip.volume;
                    audioSource.Play();
                }
                break;
            case EAudioSourceType.SFX:
                if (CoreAduio.Instance.sudioSourceDic.TryGetValue(audioSourceType.ToString(), out audioSource))
                {
                    audioSource.loop = isLoop;
                    audioSource.PlayOneShot(audioClip.audioClip, audioClip.volume);
                }
                break;
        }
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="audioClipName">音效名称</param>
    /// <param name="audioSourceType">音效的类型</param>
    /// <param name="isLoop">是否循环</param>
    /// <returns></returns>
    public static IEnumerator PlayAudioSource(this string audioClipName, EAudioSourceType audioSourceType, bool isLoop = false)
    {
        if (CoreAduio.Instance.audioClipDic.TryGetValue(audioClipName, out AudioData audioClip))
            Play(audioClip, audioSourceType, isLoop);
        else
            yield return CoreResource.LoadAsync<AudioClip>(audioClipName, LoadOkOver);

        void LoadOkOver(AudioClip audioClip)
        {
            AudioData audioData = new AudioData();
            audioData.audioClip = audioClip;
            audioData.volume = 1;
            CoreAduio.Instance.audioClipDic.Add(audioClipName, audioData);
            audioData.Play(audioSourceType, isLoop);
        }
    }

    /// <summary>
    /// 暂停播放
    /// </summary>
    /// <param name="audioSourceType"></param>
    public static void StopAudioSource(EAudioSourceType audioSourceType)
    {
        if (CoreAduio.Instance.sudioSourceDic.TryGetValue(audioSourceType.ToString(), out AudioSource audioSource))
            audioSource.Stop();
    }

    /// <summary>
    /// 改变音量
    /// </summary>
    /// <param name="v"></param>
    public static void ChangeAudioSourceValue(EAudioSourceType audioSourceType, float v)
    {
        if (CoreAduio.Instance.sudioSourceDic.TryGetValue(audioSourceType.ToString(), out AudioSource audioSource))
            audioSource.volume = v;
    }
}