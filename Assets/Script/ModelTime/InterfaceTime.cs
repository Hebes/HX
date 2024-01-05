using System;

/// <summary>
/// 生日接口
/// https://blog.csdn.net/qq_29406323/article/details/86182706
/// </summary>
public interface ITimeBirthday
{
    /// <summary>
    /// 获取此实例所表示日期的年份部分。
    /// </summary>
    int Year {  get; set; }

    /// <summary>
    /// 获取此实例所表示日期的月份部分。
    /// </summary>
    int Month {  get; set; }

    /// <summary>
    /// 获取此实例所表示日期的小时部分。
    /// </summary>
    int Hour {  get; set; }

    /// <summary>
    /// 获取此实例所表示日期的分钟部分。
    /// </summary>
    int Minute {  get; set; }

    /// <summary>
    /// 获取此实例所表示日期的秒部分。
    /// </summary>
    int Second {  get; set; }

    /// <summary>
    /// 获取此实例所表示日期的毫秒部分。
    /// </summary>
    int Millisecond {  get; set; }

    DateTime DateTime { get; set; }

    //public void ttt()
    //{
    //    DateTime.Now.Millisecond
    //}
}
