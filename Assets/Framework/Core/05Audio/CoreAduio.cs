using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

/*--------�ű�����-----------
				
�������䣺
	1607388033@qq.com
����:
	����
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
    public class AudioData
    {
        public AudioClip audioClip;
        public float volume;
    }

    public class CoreAduio : ICore
    {
        public static CoreAduio Instance;
        private Dictionary<string, AudioData> audioClipDic;         //��Ч�б�
        private Dictionary<string, AudioSource> sudioSourceDic;     //��������б�


        public void ICoreInit()
        {
            Instance = this;
            audioClipDic = new Dictionary<string, AudioData>();
            sudioSourceDic = new Dictionary<string, AudioSource>();

            GameObject AudioManagerGo = new GameObject("ModelAudio");
            GameObject.DontDestroyOnLoad(AudioManagerGo);

            sudioSourceDic.Add(EAudioSourceType.BGM.ToString(), AudioManagerGo.AddComponent<AudioSource>());
            sudioSourceDic.Add(EAudioSourceType.SFX.ToString(), AudioManagerGo.AddComponent<AudioSource>());
            Debug.Log("��Ƶģ���ʼ���ɹ�!");
        }


        /// <summary>
        /// ������Ч
        /// </summary>
        /// <param name="audioClipName">��Ч����</param>
        /// <param name="audioSourceType">��Ч������</param>
        /// <param name="isLoop">�Ƿ�ѭ��</param>
        /// <returns></returns>
        public static async UniTask PlayAudioSource(string audioClipName, EAudioSourceType audioSourceType, bool isLoop = false)
        {
            if (Instance.audioClipDic.TryGetValue(audioClipName, out AudioData audioClip))
            {
                Play(audioClip, audioSourceType, isLoop);
                return;
            }
            audioClip.audioClip = await CoreResource.LoadAsync<AudioClip>(audioClipName);
            Instance.audioClipDic.Add(audioClipName, audioClip);
            Play(audioClip, audioSourceType, isLoop);
        }

        /// <summary>
        /// ��ͣ����
        /// </summary>
        /// <param name="audioSourceType"></param>
        public static void StopAudioSource(EAudioSourceType audioSourceType)
        {
            if (Instance.sudioSourceDic.TryGetValue(audioSourceType.ToString(), out AudioSource audioSource))
                audioSource.Stop();
        }

        /// <summary>
        /// �ı�����
        /// </summary>
        /// <param name="v"></param>
        public static void ChangeAudioSourceValue(EAudioSourceType audioSourceType, float v)
        {
            if (Instance.sudioSourceDic.TryGetValue(audioSourceType.ToString(), out AudioSource audioSource))
                audioSource.volume = v;
        }

        /// <summary>
        /// ������Ч
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="audioSourceType"></param>
        /// <param name="isLoop"></param>
        private static void Play(AudioData audioClip, EAudioSourceType audioSourceType, bool isLoop = false)
        {
            AudioSource audioSource = null;
            switch (audioSourceType)
            {
                case EAudioSourceType.BGM:
                    if (Instance.sudioSourceDic.TryGetValue(audioSourceType.ToString(), out audioSource))
                    {
                        audioSource.clip = audioClip.audioClip;
                        audioSource.loop = isLoop;
                        audioSource.volume = audioClip.volume;
                        audioSource.Play();
                    }
                    break;
                case EAudioSourceType.SFX:
                    if (Instance.sudioSourceDic.TryGetValue(audioSourceType.ToString(), out audioSource))
                    {
                        audioSource.loop = isLoop;
                        audioSource.PlayOneShot(audioClip.audioClip, audioClip.volume);
                    }
                    break;
            }
        }
    }
}
