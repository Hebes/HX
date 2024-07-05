using System.Collections;
using Framework.Core;
using UnityEngine.SceneManagement;


/*--------脚本描述-----------

描述:
    场景加载管理

-----------------------*/

namespace Framework.Core
{
    [CreateCore(typeof(CoreScene), 8)]
    public class CoreScene : ICore
    {
        public static CoreScene Instance;
        private ISceneLoad sceneLoad;

        public void Init()
        {
            Instance = this;
            SwitchModel();
            sceneLoad.CoreSceneInit();
        }

        public IEnumerator AsyncInit()
        {
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
