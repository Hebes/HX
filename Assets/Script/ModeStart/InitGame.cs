using Core;
using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

/// <summary>
/// 入口脚本
/// </summary>
public class InitGame : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(Init());
    }

    public  IEnumerator Init()
    {
        //初始化核心
        CoreRun coreRun = new CoreRun();
        yield return coreRun.CoreInit();
        //显示主界面
        CoreUI.ShwoUIPanel<MainMenuView>(ConfigPrefab.prefabUIMianMenu);
        //加载子模块
        ModelRun modelRun = new ModelRun();
        yield return modelRun.ModelInit();
    }
}
