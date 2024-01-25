﻿using Cysharp.Threading.Tasks;
using Farm2D;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    资源加载

-----------------------*/

namespace Core
{
    /// <summary> 加载类型 </summary>
    public enum ELoadType
    {
        /// <summary> 原生加载 </summary>
        Resources,
        /// <summary> Yooasset加载 </summary>
        YooAsset,
    }

    public class CoreResource : ICore
    {
        public static CoreResource Instance;
        private IResLoad iload;

        public void ICoreInit()
        {
            Instance = this;
            SwitchModel();
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
                default:
                    break;
            }
        }

        /// <summary>
        /// 同步加载资源对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ResName"></param>
        /// <returns></returns>
        public static T Load<T>(string ResName) where T : UnityEngine.Object
        {
            return Instance.iload.Load<T>(ResName);
        }

        /// <summary>
        /// 异步加载资源对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static UniTask<T> LoadAsync<T>(string assetName) where T : UnityEngine.Object
        {
            return Instance.iload.LoadAsync<T>(assetName);
        }

        /// <summary>
        /// 同步加载子资源对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="location"></param>
        /// <param name="ResName"></param>
        /// <returns></returns>
        public static T LoadSub<T>(string location, string ResName) where T : UnityEngine.Object
        {
            return Instance.iload.LoadSub<T>(location, ResName);
        }

        /// <summary>
        /// 同步加载资源包内所有资源对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ResName"></param>
        /// <returns></returns>
        public static T[] LoadAll<T>(string ResName) where T : UnityEngine.Object
        {
            return Instance.iload.LoadAll<T>(ResName);
        }

        /// <summary>
        /// 加载原生数据
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static byte[] LoadByteData(string location)
        {
            return Instance.iload.LoadByteData(location);
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public static void UnloadAssets()
        {
            Instance.iload.UnloadAssets();
        }
    }
}
