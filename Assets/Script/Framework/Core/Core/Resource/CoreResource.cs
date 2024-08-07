﻿/*--------脚本描述-----------

描述:
    资源加载

-----------------------*/

using System;
using System.Collections;
using Framework.Core;

namespace Framework.Core
{
    /// <summary> 加载类型 </summary>
    public enum ELoadType
    {
        /// <summary> 原生加载 </summary>
        Resources,

        /// <summary> Yooasset加载 </summary>
        YooAsset,
    }

    [CreateCore(typeof(CoreResource), 5)]
    public class CoreResource : ICore
    {
        public static CoreResource Instance;
        private IResLoad iload;


        public void Init()
        {
            Instance = this;
            SwitchModel();
        }

        public IEnumerator AsyncEnter()
        {
            yield break;
        }

        public IEnumerator Exit()
        {
            yield break;
        }


        /// <summary>
        /// 切换模式
        /// </summary>
        public static void SwitchModel(ELoadType loadType = ELoadType.Resources)
        {
            switch (loadType)
            {
                case ELoadType.Resources:
                    Instance.iload = new UnityResLoad();
                    break;
                case ELoadType.YooAsset:
                    //Instance.iload = new YooAssetResLoad();
                    break;
            }
        }

        /// <summary>
        /// 同步加载资源对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resNameValue">资源</param>
        /// <returns></returns>
        public T Load<T>(string resNameValue) where T : UnityEngine.Object => iload.Load<T>(resNameValue);

        /// <summary>
        /// 异步加载资源对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static IEnumerator LoadAsync<T>(string assetName, Action<T> action) where T : UnityEngine.Object
        {
            yield return Instance.iload.LoadAsync<T>(assetName, action);
        }

        /// <summary>
        /// 同步加载子资源对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="location"></param>
        /// <param name="ResName"></param>
        /// <returns></returns>
        //public static T LoadSub<T>(string location, string ResName) where T : UnityEngine.Object
        //{
        //    return Instance.iload.LoadSub<T>(location, ResName);
        //}

        /// <summary>
        /// 同步加载资源包内所有资源对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ResName"></param>
        /// <returns></returns>
        //public static T[] LoadAll<T>(string ResName) where T : UnityEngine.Object
        //{
        //    return Instance.iload.LoadAll<T>(ResName);
        //}

        /// <summary>
        /// 加载原生数据
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        //public static byte[] LoadByteData(string location)
        //{
        //    return Instance.iload.LoadByteData(location);
        //}

        /// <summary>
        /// 资源释放
        /// </summary>
        //public static void UnloadAssets()
        //{
        //    Instance.iload.UnloadAssets();
        //}
    }

    /// <summary>
    /// 资源加载拓展
    /// </summary>
    public static class ResExpand
    {
        public static T Load<T>(this string resNameValue) where T : UnityEngine.Object
        {
            return CoreResource.Instance.Load<T>(resNameValue);
        }
        
        public static T Load<T>(this string resNameValue,string path) where T : UnityEngine.Object
        {
            return CoreResource.Instance.Load<T>(System.IO.Path.Combine(path,resNameValue));
        }
    }
}