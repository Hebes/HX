﻿using Framework.Core;
using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine;

/*--------脚本描述-----------

描述:
    场景加载

-----------------------*/

[ModelCreat(typeof(ManagerScene), 1)]
public class ManagerScene : IModel
{
    public static ManagerScene Instance;

    /// <summary>
    /// 获取当前场景名称
    /// </summary>
    public string CurrentSceneName { get; set; }

    public void Init()
    {
        Instance = this;
    }

    public IEnumerator AsyncEnter()
    {
        // 首次加载场景
        yield return CoreScene.LoadSceneAsync(ConfigScenes.unityScenePersistent, LoadSceneMode.Single);
        yield return CoreResource.LoadAsync<GameObject>(ConfigPrefab.prefabCommonRole, LoadOver);
        yield break;

        void LoadOver(GameObject gameObject)
        {
            var rolePlayer = new RolePlayer();
            var gameObject1 = Object.Instantiate(gameObject);
            rolePlayer.gameObject = gameObject1;
            rolePlayer.SetRole(id: 1, "测试玩家", ERoleOrTeamType.Player, ERoleBattlePoint.Point1);
            rolePlayer.SwitchRoleState<RoleStatePlayerRun>();
        }
    }

    public IEnumerator Exit()
    {
        yield return null;
    }

    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="loadSceneModel"></param>
    /// <returns></returns>
    public static IEnumerator LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Additive)
    {
        Instance.CurrentSceneName = sceneName;
        yield return CoreScene.LoadSceneAsync(sceneName, loadSceneMode);
        CoreEvent.EventTrigger(EConfigEvent.EventLoadSceneAfter.ToInt());
    }

    /// <summary>
    /// 切换场景
    /// </summary>
    public static IEnumerator SwitchScene(string targetScene)
    {
        Instance.CurrentSceneName = targetScene;
        CoreEvent.EventTrigger(EConfigEvent.EventLoadSceneBefore.ToInt());
        //TODO 这里可以触发场景过度
        //卸载原先的场景
        yield return CoreScene.UnloadSceneAsync(Instance.CurrentSceneName);
        //加载目标场景
        yield return CoreScene.LoadSceneAsync(targetScene, LoadSceneMode.Additive);
        //TODO 这里可以触发场景过度
        CoreEvent.EventTrigger(EConfigEvent.EventLoadSceneAfter.ToInt());
    }


    //参考
    //private  IEnumerator SceneTransition(string targetScene, Vector3 targetPosition)
    //{
    //    if (!isFade)//如果是切换场景的情况下
    //    {
    //        isFade = true;
    //        ConfigEvent.BeforeSceneUnloadEvent.EventTrigger();
    //        await ConfigEvent.UIFade.EventTrigger((float)1);
    //        if (!string.IsNullOrEmpty(currentceneName))
    //            currentceneName.Unload();                                  //卸载原来的场景
    //        SceneOperationHandle sceneOperationHandle = await targetScene.LoadSceneIEnumerator(LoadSceneMode.Additive);//加载新的场景
    //        sceneOperationHandle.ActivateScene();                           //设置场景激活
    //        currentceneName = targetScene;                                  //变换当前场景的名称
    //        ConfigEvent.PlayerMoveToPosition.EventTrigger(targetPosition);  //移动人物坐标
    //        ConfigEvent.UIDisplayHighlighting.EventTrigger(string.Empty, -1);//清空所有高亮
    //        await IEnumerator.DelayFrame(40);
    //        //创建每个场景必要的物体
    //        cropParent = new GameObject(ConfigTag.TagCropParent).transform;
    //        itemParent = new GameObject(ConfigTag.TagItemParent).transform;

    //        ConfigEvent.SwichConfinerShape.EventTrigger();                  //切换场景边界
    //        ConfigEvent.AfterSceneLoadedEvent.EventTrigger();                    //加载场景之后需要做的事情
    //        await ConfigEvent.UIFade.EventTrigger((float)0);
    //        isFade = false;
    //    }
    //}
}