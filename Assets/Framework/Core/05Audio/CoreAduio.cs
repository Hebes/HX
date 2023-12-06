using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    音频模块

-----------------------*/


namespace Core
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
    public class AudioData
    {
        public AudioClip audioClip;
        public float volume;
    }

    public class CoreAduio : ICore
    {
        public static CoreAduio Instance;
        private Dictionary<string, AudioData> audioClipDic;         //音效列表
        private Dictionary<string, AudioSource> sudioSourceDic;     //音乐组件列表


        public void ICoreInit()
        {
            Instance = this;
            audioClipDic = new Dictionary<string, AudioData>();
            sudioSourceDic = new Dictionary<string, AudioSource>();

            GameObject AudioManagerGo = new GameObject("ModelAudio");
            GameObject.DontDestroyOnLoad(AudioManagerGo);

            sudioSourceDic.Add(EAudioSourceType.BGM.ToString(), AudioManagerGo.AddComponent<AudioSource>());
            sudioSourceDic.Add(EAudioSourceType.SFX.ToString(), AudioManagerGo.AddComponent<AudioSource>());
            Debug.Log("音频模块初始化成功!");
        }


        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioClipName">音效名称</param>
        /// <param name="audioSourceType">音效的类型</param>
        /// <param name="isLoop">是否循环</param>
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
        /// 暂停播放
        /// </summary>
        /// <param name="audioSourceType"></param>
        public static void StopAudioSource(EAudioSourceType audioSourceType)
        {
            if (Instance.sudioSourceDic.TryGetValue(audioSourceType.ToString(), out AudioSource audioSource))
                audioSource.Stop();
        }

        /// <summary>
        /// 改变音量
        /// </summary>
        /// <param name="v"></param>
        public static void ChangeAudioSourceValue(EAudioSourceType audioSourceType, float v)
        {
            if (Instance.sudioSourceDic.TryGetValue(audioSourceType.ToString(), out AudioSource audioSource))
                audioSource.volume = v;
        }

        /// <summary>
        /// 播放音效
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
