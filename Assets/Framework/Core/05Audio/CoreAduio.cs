using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*--------�ű�����-----------

����:
    ��Ƶģ��

-----------------------*/


namespace Core
{
    /// <summary> �������� </summary>
    public enum EAudioSourceType
    {
        /// <summary> �������� </summary>
        BGM,
        /// <summary> ��Ч </summary>
        SFX,
    }

    [System.Serializable]
    public struct AudioData
    {
        public AudioClip audioClip;
        public float volume;
    }

    public class CoreAduio : ICore
    {
        public static CoreAduio Instance;
        public Dictionary<string, AudioData> audioClipDic;         //��Ч�б�
        public Dictionary<string, AudioSource> sudioSourceDic;     //��������б�

        public IEnumerator AsyncInit()
        {
            yield break;
        }

        public void Init()
        {
            Instance = this;
            audioClipDic = new Dictionary<string, AudioData>();
            sudioSourceDic = new Dictionary<string, AudioSource>();

            GameObject AudioManagerGo = new GameObject("����");
            GameObject.DontDestroyOnLoad(AudioManagerGo);

            sudioSourceDic.Add(EAudioSourceType.BGM.ToString(), AudioManagerGo.AddComponent<AudioSource>());
            sudioSourceDic.Add(EAudioSourceType.SFX.ToString(), AudioManagerGo.AddComponent<AudioSource>());
            UnityEngine.Debug.Log("��Ƶģ���ʼ���ɹ�!");
        }

    }
}


public static class HelperAduio
{
    /// <summary>
    /// ������Ч
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
    /// ������Ч
    /// </summary>
    /// <param name="audioClipName">��Ч����</param>
    /// <param name="audioSourceType">��Ч������</param>
    /// <param name="isLoop">�Ƿ�ѭ��</param>
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
    /// ��ͣ����
    /// </summary>
    /// <param name="audioSourceType"></param>
    public static void StopAudioSource(EAudioSourceType audioSourceType)
    {
        if (CoreAduio.Instance.sudioSourceDic.TryGetValue(audioSourceType.ToString(), out AudioSource audioSource))
            audioSource.Stop();
    }

    /// <summary>
    /// �ı�����
    /// </summary>
    /// <param name="v"></param>
    public static void ChangeAudioSourceValue(EAudioSourceType audioSourceType, float v)
    {
        if (CoreAduio.Instance.sudioSourceDic.TryGetValue(audioSourceType.ToString(), out AudioSource audioSource))
            audioSource.volume = v;
    }
}