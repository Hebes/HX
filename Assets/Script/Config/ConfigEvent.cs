/// <summary>
/// Event事件
/// </summary>
public enum EConfigEvent
{
    /// <summary>
    /// 加载场景之前需要的做的事
    /// </summary>
    EventLoadSceneBefore = 1001,

    /// <summary>
    /// 加载场景之后需要的做的事情
    /// </summary>
    EventLoadSceneAfter = 1002,

    /// <summary>
    /// 前进一年事件
    /// </summary>
    EventAdvanceGameYear = 1003,

    /// <summary>
    /// 前进一个季节事件
    /// </summary>
    EventAdvanceGameSeason = 1004,

    /// <summary>
    /// 前进一天事件
    /// </summary>
    EventAdvanceGameDay = 1005,

    /// <summary>
    /// 前进一个小时事件
    /// </summary>
    EventAdvanceGameHour = 1006,

    /// <summary>
    /// 前进一分事件
    /// </summary>
    EventAdvanceGameMinute = 1007,

    /// <summary>
    /// 前进一秒事件
    /// </summary>
    EventAdvanceGameSecond = 1008,

    /// <summary>
    /// 前进一月事件
    /// </summary>
    EventAdvanceGameMonth = 1009,
}
