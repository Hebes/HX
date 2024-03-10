using Core;
using Cysharp.Threading.Tasks;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections;
using UnityEngine;


/*--------脚本描述-----------

描述:
    Unity加载

-----------------------*/

namespace Core
{
    public class UnityResLoad : IResLoad
    {
        
        public T Load<T>(string AssetName) where T : UnityEngine.Object
        {
            T t = Resources.Load<T>(AssetName);
            if (t == null)
                Debug.Error($"资源为空{AssetName}");
            return t;
        }

        public IEnumerator LoadAsync<T>(string AssetName, Action<T> action) where T : UnityEngine.Object
        {
            ResourceRequest resourceRequest = Resources.LoadAsync<T>(AssetName);
            while (!resourceRequest.isDone)
                yield return null;
            if (resourceRequest.isDone == false)
                Debug.Error($"资源为空{AssetName}");
            action.Invoke(resourceRequest.asset as T);
        }

        //public T[] LoadAll<T>(string AssetName) where T : UnityEngine.Object
        //{
        //    T[] values = Resources.LoadAll<T>(AssetName);
        //    if (values == null && values.Length <= 0)
        //        Debug.Error($"资源为空{AssetName}");
        //    return values;
        //}

        //public IEnumerator LoadAllAsync<T>(string location) where T : UnityEngine.Object
        //{
        //    return default;
        //}



        //public T LoadSub<T>(string location, string AssetName) where T : UnityEngine.Object
        //{
        //    return null;
        //}

        //public IEnumerator<T> LoadSubAsync<T>(string location, string AssetName) where T : UnityEngine.Object
        //{
        //    return default;
        //}

        //public void ReleaseAsset(string AssetName = null)
        //{
        //}

        //public void UnloadAssets()
        //{
        //    Resources.UnloadUnusedAssets();
        //}

        //public byte[] LoadByteData(string location)
        //{
        //    return Load<TextAsset>(location).bytes;
        //}

        //public IEnumerator<byte[]> LoadByteDataAsync(string location)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
