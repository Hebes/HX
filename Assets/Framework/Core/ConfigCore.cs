/*--------�ű�����-----------

�������䣺
    1607388033@qq.com
����:
    ����
����:
    ���������ļ�

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
    /// ң�м���·��
    /// </summary>
    public const string jpyStickPanelPath = "AssetsPackage/Prefab/UI/Joystick/Joystick";

    /// <summary>
    /// ���·��
    /// </summary>
    public const string uiCanvasPath = "AssetsPackage/Prefab/UI/Canvas";
}
