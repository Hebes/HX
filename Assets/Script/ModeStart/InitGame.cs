using Core;
using UnityEngine;

/// <summary>
/// 入口脚本
/// </summary>
public class InitGame : MonoBehaviour
{
    private void Awake()
    {
        //初始化核心
        new CoreRun();
        //加载子模块
        new ModelRun();
        //显示主界面
        CoreUI.ShwoUIPanel<MainMenuView>(ConfigPrefab.prefabMianMenu);
    }
}
