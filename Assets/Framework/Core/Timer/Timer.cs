﻿using System;

/*--------脚本描述-----------

描述:
	计时任务抽象基类

-----------------------*/

namespace Core
{
    public abstract class Timer
    {
        public Action<string> LogFunc;      //日志信息
        public Action<string> WarnFunc;     //异常信息
        public Action<string> ErrorFunc;    //错误信息

        protected int tid = 0;                  //任务编号

        protected abstract int GenerateTid();   //生成Tid

        /// <summary>
        /// 创建定时任务
        /// </summary>
        /// <param name="delay">定时任务时间</param>
        /// <param name="taskCB">定时任务回调</param>
        /// <param name="cancelCB">取消任务回调</param>
        /// <param name="count">任务重复计数</param>
        /// <returns>当前计时器唯一任务ID</returns>
        public abstract int AddTask(uint delay, Action<int> taskCB, Action<int> cancelCB, int count = 1);

        /// <summary>
        /// 删除定时任务
        /// </summary>
        /// <param name="tid">定时任务ID</param>
        /// <returns>删除操作结果</returns>
        public abstract bool DeleteTask(int tid);

        /// <summary>
        /// 重置定时器
        /// </summary>
        public abstract void Reset();

    }
}
