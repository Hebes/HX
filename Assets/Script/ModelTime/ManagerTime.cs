using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 游戏的时间系统
/// </summary>
public class ManagerTime : IModel, ISave, IUpdata
{
    #region 本类私有方法
    /// <summary>
    /// 游戏年
    /// </summary>
    private int gameYear = 1;
    /// <summary>
    /// 游戏季节
    /// </summary>
    private ESeason gameSeason = ESeason.Spring;
    /// <summary>
    ///游戏月
    /// </summary>
    private int gameMonth = 1;
    /// <summary>
    /// 游戏天
    /// </summary>
    private int gameDay = 1;
    /// <summary>
    /// 游戏小时
    /// </summary>
    private int gameHour = 6;
    /// <summary>
    /// 游戏分
    /// </summary>
    private int gameMinute = 30;
    /// <summary>
    /// 游戏秒
    /// </summary>
    private int gameSecond = 0;
    /// <summary>
    /// 一周游戏日
    /// </summary>
    private string gameDayOfWeek = "星期一";
    /// <summary>
    /// 游戏时钟暂停
    /// </summary>
    private bool gameClockPaused = false;
    /// <summary>
    /// 游戏时间最小变动
    /// </summary>
    private float gameTick = 0f;
    #endregion


    #region 接口方法
    public IEnumerator Enter()
    {
        ManagerSave.RegisterSavea(this);
        CoreEvent.EventAdd(EConfigEvent.EventLoadSceneBefore.ToInt(), LoadSceneBefore);
        CoreEvent.EventAdd(EConfigEvent.EventLoadSceneAfter.ToInt(), LoadSceneAfter);
        yield return null;
    }
    public IEnumerator Exit()
    {
        yield return null;
    }
    /// <summary>
    /// 加载场景之后需要的做的事情
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void LoadSceneAfter()
    {
        gameClockPaused = false;
    }
    /// <summary>
    /// 加载场景之前需要的做的事
    /// </summary>
    private void LoadSceneBefore()
    {
        gameClockPaused = true;
    }
    public void CoreBehaviourUpdata()
    {
        if (gameClockPaused) return;
        GameTick();
    }
    #endregion


    #region 本类私有方法
    /// <summary>
    /// 游戏时间变动
    /// </summary>
    private void GameTick()
    {
        gameTick += Time.deltaTime;

        if (gameTick >= SettingTime.secondsPerGameSecond)
        {
            gameTick -= SettingTime.secondsPerGameSecond;
            UpdateGameSecond();
        }
    }
    /// <summary>
    /// 更新秒
    /// </summary>
    private void UpdateGameSecond()
    {
        gameSecond++;//秒增加

        if (gameSecond > 59)
        {
            gameSecond = 0;
            gameMinute++;//分增加


            if (gameMinute > 59)
            {
                gameMinute = 0;
                gameHour++;//小时增加

                if (gameHour > 23)
                {
                    gameHour = 0;
                    gameDay++;//天增加

                    if (gameDay > 30)
                    {
                        gameDay = 1;
                        gameMonth++;

                        //季节
                        if (gameMonth % 3 == 0)
                        {
                            gameSeason = (ESeason)(gameMonth / 3);
                            CoreEvent.EventTrigger(EConfigEvent.EventAdvanceGameSeason.ToInt(), this);
                        }


                        if (gameMonth > 12)
                        {
                            gameMonth = 1;
                            gameSeason = (ESeason)(gameMonth / 3);
                            CoreEvent.EventTrigger(EConfigEvent.EventAdvanceGameSeason.ToInt(), this);

                            gameYear++;//年增加
                            if (gameYear > 9999)
                                gameYear = 1;

                            CoreEvent.EventTrigger(EConfigEvent.EventAdvanceGameYear.ToInt(), this);
                        }
                        CoreEvent.EventTrigger(EConfigEvent.EventAdvanceGameMonth.ToInt(), this);
                    }
                    gameDayOfWeek = GetDayOfWeek();
                    CoreEvent.EventTrigger(EConfigEvent.EventAdvanceGameDay.ToInt(), this);
                }
                CoreEvent.EventTrigger(EConfigEvent.EventAdvanceGameHour.ToInt(), this);
            }
            CoreEvent.EventTrigger(EConfigEvent.EventAdvanceGameMinute.ToInt(), this);
        }
        CoreEvent.EventTrigger(EConfigEvent.EventAdvanceGameSecond.ToInt(), this);
    }
    #endregion


    #region 本类方法
    /// <summary>
    /// 获取星期
    /// </summary>
    /// <returns></returns>
    private string GetDayOfWeek()
    {
        int totalDays = (((int)gameSeason) * 30) + gameDay;
        int dayOfWeek = totalDays % 7;

        switch (dayOfWeek)
        {
            case 1: return "星期一";
            case 2: return "星期二";
            case 3: return "星期三";
            case 4: return "星期四";
            case 5: return "星期五";
            case 6: return "星期六";
            case 0: return "星期天";
            default: return "";
        }
    }
    /// <summary>
    /// 获取游戏时间
    /// </summary>
    /// <returns></returns>
    public TimeSpan GetGameTime()
    {
        TimeSpan gameTime = new TimeSpan(gameHour, gameMinute, gameSecond);
        return gameTime;
    }
    /// <summary>
    /// 获取游戏季节
    /// </summary>
    /// <returns></returns>
    public ESeason GetGameSeason()
    {
        return gameSeason;
    }
    /// <summary>
    /// 提前1分钟(测试)
    /// </summary>
    public void TestAdvanceGameMinute()
    {
        for (int i = 0; i < 60; i++)
            UpdateGameSecond();
    }
    /// <summary>
    /// 提前1天(测试)
    /// </summary>
    public void TestAdvanceGameDay()
    {
        for (int i = 0; i < 86400; i++)
            UpdateGameSecond();
    }
    #endregion


    #region 存档
    public void Load(SaveData saveData)
    {
    }
    public void Save()
    {
    }
    #endregion
}
