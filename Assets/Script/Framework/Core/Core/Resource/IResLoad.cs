using System;
using System.Collections;

/*--------脚本描述-----------
				
描述:
    资源加载接口

-----------------------*/

namespace Framework.Core
{
    public interface IResLoad
    {
        /// <summary>
        /// 同步加载资源对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="AssetName"></param>
        /// <returns></returns>
        public T Load<T>(string AssetName) where T : UnityEngine.Object;

        /// <summary>
        /// 异步加载资源对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="AssetName"></param>
        /// <returns></returns>
        public IEnumerator LoadAsync<T>(string AssetName, Action<T> action) where T : UnityEngine.Object;

        /// <summary>
        /// 同步加载子资源对象
        /// 例如：通过TexturePacker创建的图集，如果需要访问图集的精灵对象，可以通过子对象加载接口。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        //public T LoadSub<T>(string location, string AssetName) where T : UnityEngine.Object;

        /// <summary>
        /// 异步加载子资源对象
        /// 例如：通过TexturePacker创建的图集，如果需要访问图集的精灵对象，可以通过子对象加载接口。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="location">加载资源的地址</param>
        /// <param name="AssetName">资源的名称</param>
        /// <param name="callback"></param>
        //public IEnumerator LoadSubAsync<T>(string location, string AssetName) where T : UnityEngine.Object;

        /// <summary>
        /// 同步加载资源包内所有资源对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        //public T[] LoadAll<T>(string AssetName) where T : UnityEngine.Object;

        /// <summary>
        /// 异步加载资源包内所有资源对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        //public IEnumerator<T[]> LoadAllAsync<T>(string location) where T : UnityEngine.Object;


        /// <summary>
        /// 加载原生数据
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        //public byte[] LoadByteData(string location);

        /// <summary>
        /// 异步加载原生数据
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        //public IEnumerator<byte[]> LoadByteDataAsync(string location);

        /// <summary>
        /// 资源卸载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="AssetName">资源的名称</param>
        //public void ReleaseAsset(string AssetName = null);

        /// <summary>
        /// 资源释放
        /// 可以在切换场景之后调用资源释放方法或者写定时器间隔时间去释放。
        /// 注意：只有调用资源释放方法，资源对象才会在内存里被移除。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ResName"></param>
        /// <param name="callback"></param>
        //public void UnloadAssets();
    }
}
