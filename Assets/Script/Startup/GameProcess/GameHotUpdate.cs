using System.Collections;
using Framework.Core;

/// <summary>
/// 游戏热更流程
/// </summary>
[GameProcess(typeof(GameHotUpdate), 5)]
public class GameHotUpdate : IProcessStateNode
{
    private ProcessFsmSystem _processFsmSystem;
    private GameLunch _gameLunch;

    public void OnCreate(object obj)
    {
        _processFsmSystem = (ProcessFsmSystem)obj;
        _gameLunch = (GameLunch)_processFsmSystem.Owner;
    }


    public void OnEnter(object obj)
    {
        _gameLunch.StartCoroutine(EnterHotUpdate());
    }

    public void OnUpdate()
    {
    }

    public void OnExit()
    {
    }

    /// <summary>
    /// 进入热更流程
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnterHotUpdate()
    {
        //         var asset = ResMgr.Instance.GetAssetCache<TextAsset>("HotUpdateDlls/Game.dll.bytes");
        //         if (asset != null)
        //         {
        // #if !UNITY_EDITOR
        //            this.entryAssembly = System.Reflection.Assembly.Load(asset.bytes);
        // #else
        //             this.entryAssembly = AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == "Game");
        // #endif
        //         }
        //         else
        //         {
        //             Debug2.LogColor($"加载huatuo热更 失败! asset != null : {asset != null}" ,LogColor.Cyan);
        //             yield break;
        //         }
        //

        yield return null;
         EnterGameApp(true);
        yield break;
    }

    private void EnterGameApp(bool isHotupdate)
    {
        // if (isHotupdate)
        // {
        //     //必须用反射调用hotdll 不然编译死了 就没有解释执行的空间了
        //     var appType = this.entryAssembly.GetType("Main");
        //     var mainMethod = appType.GetMethod("RunMain");
        //     mainMethod.Invoke(null, null);
        // }
        // else
        // {
        //     //Main.RunMainNative(); //热更代码 实例调用 不然无法热更 打包会出错
        //     //Debug2.LogColor($"加载huatuo热更 失败! isHotupdate : {isHotupdate}" ,LogColor.Cyan);
        // }
         
          new GameStart().GameRun();
    }
}