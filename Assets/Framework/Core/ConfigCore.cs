/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    核心配置文件

-----------------------*/

public class ConfigCore
{

#if UNITY_ANDROID
		public const string YooAseetPackage = "Android";
#elif UNITY_IOS
		public const string YooAseetPackage = "IOS";
#elif UNITY_STANDALONE_WIN
    public const string YooAseetPackage = "PC";
#elif UNITY_STANDALONE_OSX
		public const string YooAseetPackage = "PC";
#else
		public const string YooAseetPackage = "PC";
#endif
    //public const string YooAseetPackage = "Android";

    /// <summary>
    /// 遥感加载路径
    /// </summary>
    public const string jpyStickPanelPath = "AssetsPackage/Prefab/UI/Joystick/Joystick";

    /// <summary>
    /// 面板路径
    /// </summary>
    public const string uiCanvasPath = "AssetsPackage/Prefab/UI/Canvas";
}
