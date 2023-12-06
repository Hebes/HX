using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;


/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
     场景接口加载

-----------------------*/

namespace Core
{
    public class UnityLoadScene : ISceneLoad
    {
        public void LoadScene(string sceneName, ELoadSceneModel loadSceneModel = ELoadSceneModel.Additive)
        {
            LoadSceneMode loadSceneModeTemp = LoadSceneMode.Single;
            switch (loadSceneModel)
            {
                case ELoadSceneModel.Additive: loadSceneModeTemp = LoadSceneMode.Additive; break;
                case ELoadSceneModel.Single: loadSceneModeTemp = LoadSceneMode.Single; break;
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, loadSceneModeTemp);
        }

        public async UniTask LoadSceneAsync(string sceneName, ELoadSceneModel loadSceneModel = ELoadSceneModel.Additive)
        {
            LoadSceneMode loadSceneModeTemp = LoadSceneMode.Single;
            switch (loadSceneModel)
            {
                case ELoadSceneModel.Additive: loadSceneModeTemp = LoadSceneMode.Additive; break;
                case ELoadSceneModel.Single: loadSceneModeTemp = LoadSceneMode.Single; break;
            }
            UnityEngine.AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, loadSceneModeTemp);
            await asyncOperation.ToUniTask();
            if (asyncOperation.isDone == false)
                Debug.Log("场景加载失败");
        }

        public async UniTask UnloadSceneAsync(string sceneName)
        {
            UnityEngine.AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
            await asyncOperation.ToUniTask();
            if (asyncOperation.isDone == false)
                Debug.Error("卸载场景失败，请检查");
        }

        public async UniTask ChangeSceneAsync(string oldSceneName, string newSceneName, ELoadSceneModel loadSceneModel = ELoadSceneModel.Additive)
        {
            await UnloadSceneAsync(oldSceneName);
            await LoadSceneAsync(newSceneName, loadSceneModel);
        }

        public object GetManagerDic()
        {
            return default;
        }

        public void CoreSceneInit()
        {
        }
    }
}
