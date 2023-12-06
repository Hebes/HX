using Cysharp.Threading.Tasks;
using Farm2D;


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
    public enum ELoadSceneModel
    {
        /// <summary> 添加模式 </summary>
        Additive,
        /// <summary> 单独模式 </summary>
        Single,
    }

    public class CoreScene : ICore
    {
        public static CoreScene Instance;
        private ISceneLoad sceneLoad;

        public void ICoreInit()
        {
            Instance = this;
            SwitchModel();
            sceneLoad.CoreSceneInit();
        }

        public void SwitchModel(ELoadType loadType = ELoadType.Resources)
        {
            switch (loadType)
            {
                case ELoadType.Resources:
                    sceneLoad = new UnityLoadScene();
                    break;
                case ELoadType.YooAsset:
                    sceneLoad = new YooAssetLoadScene();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="loadSceneModel"></param>
        /// <returns></returns>
        public static async UniTask LoadSceneAsync(string sceneName, ELoadSceneModel loadSceneModel = ELoadSceneModel.Additive)
        {
            await Instance.sceneLoad.LoadSceneAsync(sceneName, loadSceneModel);
        }

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public static async UniTask UnloadSceneAsync(string sceneName)
        {
            await Instance.sceneLoad.UnloadSceneAsync(sceneName);
        }

        /// <summary>
        /// 切换场景
        /// </summary>
        /// <param name="oldSceneName"></param>
        /// <param name="newSceneName"></param>
        /// <param name="loadSceneModel"></param>
        /// <returns></returns>
        public static async UniTask ChangeSceneAsync(string oldSceneName, string newSceneName, ELoadSceneModel loadSceneModel = ELoadSceneModel.Additive)
        {
            await Instance.sceneLoad.ChangeSceneAsync(oldSceneName, newSceneName, loadSceneModel);
        }
    }
}
