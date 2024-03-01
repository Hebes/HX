using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine.SceneManagement;


/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    场景加载管理

-----------------------*/

namespace Core
{
    public class CoreScene : ICore
    {
        public static CoreScene Instance;
        private ISceneLoad sceneLoad;


        public IEnumerator ICoreInit()
        {
            Instance = this;
            SwitchModel();
            sceneLoad.CoreSceneInit();
            yield return null;
        }


        public void SwitchModel(ELoadType loadType = ELoadType.Resources)
        {
            switch (loadType)
            {
                case ELoadType.Resources:
                    sceneLoad = new UnityLoadScene();
                    break;
                case ELoadType.YooAsset:
                    //sceneLoad = new YooAssetLoadScene();
                    break;
                default:
                    break;
            }
        }
        public static IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
        {
            yield return Instance.sceneLoad.LoadSceneAsync(sceneName, loadSceneMode);
        }
        public static IEnumerator UnloadSceneAsync(string sceneName)
        {
           yield return Instance.sceneLoad.UnloadSceneAsync(sceneName);
        }
    }
}
