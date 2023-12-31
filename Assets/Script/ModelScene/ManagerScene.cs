using Core;
using Cysharp.Threading.Tasks;

/*--------脚本描述-----------

描述:
	场景加载

-----------------------*/

    public class ManagerScene : IModelInit
    {
        public static ManagerScene Instance;
        private string currentceneName;
        public void Init()
        {
            Instance = this;
            FirstLoad().Forget();
        }

        /// <summary>
        /// 首次加载场景
        /// </summary>
        /// <returns></returns>
        private async UniTask FirstLoad()
        {
            //首次加载场景
            await CoreScene.LoadSceneAsync(ConfigScenes.unityScenePersistent, ELoadSceneModel.Single);
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="loadSceneModel"></param>
        /// <returns></returns>
        public static async UniTask LoadSceneAsync(string sceneName, ELoadSceneModel loadSceneModel = ELoadSceneModel.Additive)
        {
            Instance.currentceneName = sceneName;
            await CoreScene.LoadSceneAsync(sceneName, loadSceneModel);
        }

        /// <summary>
        /// 切换场景
        /// </summary>
        public static async UniTask SwitchScene(string targetScene)
        {
            Instance.currentceneName = targetScene;
            CoreEvent.EventTrigger(EConfigEvent.LoadSceneBefore.ToInt());
            //TODO 这里可以触发场景过度
            //卸载原先的场景
           await CoreScene.UnloadSceneAsync(Instance.currentceneName);
            //加载目标场景
            await CoreScene.LoadSceneAsync(targetScene, ELoadSceneModel.Additive);
            //TODO 这里可以触发场景过度
            CoreEvent.EventTrigger(EConfigEvent.LoadSceneAfter.ToInt());
        }

        //参考
        //private async UniTask SceneTransition(string targetScene, Vector3 targetPosition)
        //{
        //    if (!isFade)//如果是切换场景的情况下
        //    {
        //        isFade = true;
        //        ConfigEvent.BeforeSceneUnloadEvent.EventTrigger();
        //        await ConfigEvent.UIFade.EventTriggerAsync((float)1);
        //        if (!string.IsNullOrEmpty(currentceneName))
        //            currentceneName.UnloadAsync();                                  //卸载原来的场景
        //        SceneOperationHandle sceneOperationHandle = await targetScene.LoadSceneAsyncUnitask(LoadSceneMode.Additive);//加载新的场景
        //        sceneOperationHandle.ActivateScene();                           //设置场景激活
        //        currentceneName = targetScene;                                  //变换当前场景的名称
        //        ConfigEvent.PlayerMoveToPosition.EventTrigger(targetPosition);  //移动人物坐标
        //        ConfigEvent.UIDisplayHighlighting.EventTrigger(string.Empty, -1);//清空所有高亮
        //        await UniTask.DelayFrame(40);
        //        //创建每个场景必要的物体
        //        cropParent = new GameObject(ConfigTag.TagCropParent).transform;
        //        itemParent = new GameObject(ConfigTag.TagItemParent).transform;

        //        ConfigEvent.SwichConfinerShape.EventTrigger();                  //切换场景边界
        //        ConfigEvent.AfterSceneLoadedEvent.EventTrigger();                    //加载场景之后需要做的事情
        //        await ConfigEvent.UIFade.EventTriggerAsync((float)0);
        //        isFade = false;
        //    }
        //}
    }
