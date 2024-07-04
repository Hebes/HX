//using System.Collections;
//using UnityEngine;

///// <summary>
///// https://cloud.tencent.com/developer/article/1637553
///// https://blog.csdn.net/qq_33795300/article/details/131700773
///// </summary>
//public class ManagerScreen : IModel
//{
//    public static ManagerScreen Instance;
//    private Resolution[] resolutions;
    
//    public IEnumerator Exit()
//    {
       
//        yield return null;
//    }

//    public IEnumerator Enter()
//    {
//        Instance = this;
//#if UNITY_STANDALONE_WIN
//        // 获取设置当前屏幕分辩率
//        resolutions = Screen.resolutions;

//        //设置当前分辨率
//        Screen.SetResolution(resolutions[resolutions.Length - 1].width, resolutions[resolutions.Length - 1].height, true);
//        Screen.fullScreen = true; //设置成全屏,
//#endif
//        yield return null;
//    }
//}
