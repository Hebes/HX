namespace Framework.Core
{
    public class GameSetting
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
        /// 按键的
        /// </summary>
        public const string JpyStickPanelPath = "AssetsPackage/Prefab/UI/Joystick/Joystick";

        /// <summary>
        /// UI的加载目录
        /// </summary>
        public const string UICanvasPath = "AssetsPackage/Prefab/UI/MainCanvas";
    }
}