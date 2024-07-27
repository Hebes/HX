public class WorldConfig
{
    // =========================================================

    /// <summary>
    /// 欢迎语
    /// </summary>
    public const string welcomtTip = "正在加载游戏配置";
        
    /// <summary>
    /// 默认的ip地址
    /// </summary>
    // public static readonly string default_editor_mode_ip = "192.168.2.18";
    public static readonly string default_editor_mode_ip = "192.168.112.1";

    //真机上使用的服务器资源路径
    //如果是世界，要更改URL最后的世界名zh_TW，如
    public static readonly string abDownloadUrl = "https://www.zhao.net/AssetBundle/AssetBundle_u5/";

    //热更地址
    public static readonly string hotupdateServer = $"http://{default_editor_mode_ip}:82/WebServer/www_root/";

    // =========================================================

    /// <summary>
    /// 要不要显示羁绊、品质按钮
    /// </summary>
    public const bool EnableJipanAndZizhiButton = true;
    
    #region  ↓↓↓↓ 版署版号+抵制不良游戏 ↓↓↓↓
    /// <summary>
    /// 版号说明
    /// </summary>
    public const string banhao_shuoming = "";

    /// <summary>
    /// 版署，工信部，广电要求的抵制不良游戏
    /// </summary>
    public const string game_warning_tips = "";
    //"抵制不良游戏，拒绝盗版游戏，注意自我保护，谨防受骗上当。适度游戏益脑，沉迷游戏伤身。合理安排时间，享受健康生活。";
    #endregion ↑↑↑↑ 版署版号+抵制不良游戏 ↑↑↑↑
}