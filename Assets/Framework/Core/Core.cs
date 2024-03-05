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
        /// ң�м���·��
        /// </summary>
        public const string jpyStickPanelPath = "AssetsPackage/Prefab/UI/Joystick/Joystick";

        /// <summary>
        /// ���·��
        /// </summary>
        public const string uiCanvasPath = "AssetsPackage/Prefab/UI/Canvas";
    }

    /// <summary>
    /// �����¼� 
    /// </summary>
    public enum ECoreEvent
    {
        ON_SCREEN_RESOLUTION_CHANGE = 1,

        /// <summary>
        /// ���س����¼�
        /// </summary>
        LoadSceneEvent,
    }

    public interface ICore
    {
        /// <summary>
        /// ���ĳ�ʼ��
        /// </summary>
        /// <returns></returns>
        public IEnumerator ICoreInit();
    }

    public interface IID
    {
        public long ID { get; set; }
    }
}