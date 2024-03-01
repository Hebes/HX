//using Cysharp.Threading.Tasks;
//using System.Collections.Generic;
//using UnityEngine.SceneManagement;
//using YooAsset;

//namespace Core
//{
//    public class YooAssetLoadScene : ISceneLoad
//    {
//        public const string LoadingEvenName = "进度条更新";
//        private ResourcePackage package;

//        public Dictionary<string, SceneHandle> sceneHandleDic;
//        public void CoreSceneInit()
//        {
//            sceneHandleDic = new Dictionary<string, SceneHandle>();
//            package = YooAssets.GetPackage(ConfigCore.YooAseetPackage);
//        }

//        public object GetManagerDic()
//        {
//            return null;
//        }

//        public void LoadScene(string sceneName, ELoadSceneModel loadSceneModel)
//        {

//        }

//        public IEnumerator LoadSceneAsync(string sceneName, ELoadSceneModel loadSceneModel)
//        {
//            LoadSceneMode loadSceneModelTemp = LoadSceneMode.Single;
//            switch (loadSceneModel)
//            {
//                case ELoadSceneModel.Additive:
//                    loadSceneModelTemp = LoadSceneMode.Additive;
//                    break;
//                case ELoadSceneModel.Single:
//                    loadSceneModelTemp = LoadSceneMode.Single;
//                    break;
//            }

//            bool suspendLoad = false;   //场景加载到90%自动挂起
//            uint priority = 100;         //优先级
//            SceneHandle sceneHandle = package.LoadSceneAsync(sceneName, loadSceneModelTemp, suspendLoad, priority);
//            yield return sceneHandle.ToIEnumerator();
//            if (sceneHandle.Status == EOperationStatus.Succeed)
//                Debug.Log($"{sceneName}场景加载成功");

//            if (sceneHandleDic.TryGetValue(sceneName, out sceneHandle))
//                sceneHandleDic[sceneName] = sceneHandle;
//            else
//                sceneHandleDic.Add(sceneName, sceneHandle);
//        }

//        public IEnumerator ChangeSceneAsync(string oldSceneName, string newSceneName, ELoadSceneModel loadSceneModel)
//        {
//            yield return UnloadSceneAsync(oldSceneName);
//            yield return LoadSceneAsync(newSceneName, loadSceneModel);
//        }

//        public IEnumerator UnloadSceneAsync(string sceneName)
//        {
//            if (sceneHandleDic.TryGetValue(sceneName, out SceneHandle sceneHandle))
//                yield return sceneHandle.UnloadAsync();
//        }
//    }
//}
