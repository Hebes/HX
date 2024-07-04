using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum EAIType
{
    /// <summary>
    /// 感知
    /// 感知：是AI角色与游戏世界的接口，负责在游戏过程中不断感知周围的环境，读取游戏的状态和数据，为思考和决策收集信息。例如：周围是否有敌人靠近等等。
    /// </summary>
    Sense = 1,
    /// <summary>
    /// 思考
    /// 利用感知的结果选择行为，在多种可能性之间切换。例如：决定是战斗还是逃跑？逃跑东在哪里？等等
    /// </summary>
    Think,
    /// <summary>
    /// 行动
    /// 行动：发出命令、更新状态、寻路、播放背景音乐和动画、生命值增减等等。
    /// </summary>
    Action,
}
