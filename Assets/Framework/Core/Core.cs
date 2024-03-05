using System.Collections;

namespace Core
{
    public class SettingCore
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

    /// <summary>
    /// 核心事件 
    /// </summary>
    public enum ECoreEvent
    {
        ON_SCREEN_RESOLUTION_CHANGE = 1,

        /// <summary>
        /// 加载场景事件
        /// </summary>
        LoadSceneEvent,
    }

    public interface ICore
    {
        /// <summary>
        /// 核心初始化
        /// </summary>
        /// <returns></returns>
        public IEnumerator ICoreInit();
    }

    public interface IID
    {
        public long ID { get; set; }
    }
}