using Core;
using System.Collections;
using UnityEngine;

/// <summary>
/// 入口脚本
/// </summary>
public class Root : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(Init());
    }

    public IEnumerator Init()
    {
        //初始化核心
        yield return new CoreRun().CoreInit();
        //显示主界面
        CoreUI.ShwoUIPanel<MainMenuView>(ConfigPrefab.prefabUIMianMenu);
        //加载子模块
        yield return new ModelRun().ModelInit();
    }
}
