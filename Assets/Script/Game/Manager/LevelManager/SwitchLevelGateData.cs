﻿using System;
using UnityEngine;

/// <summary>
/// 切换关卡大门数据
/// </summary>
[Serializable]
public class SwitchLevelGateData
{
    [Header("目标场景名")] public string ToLevelId;

    [Header("目标场景大门编号")] public int ToId;

    [Header("自身大门编号")] public int MyId;

    [Header("打开方式")] [NonSerialized] public SceneGate.OpenType OpenType = SceneGate.OpenType.None;

    [Header("是否在空中")] [NonSerialized] public bool InAir;

    [Header("自己的位置")] [NonSerialized] public Vector3 SelfPosition;

    [Header("目标位置")] [NonSerialized] public Vector3 TargetPosition;

    [Header("目标大小")] [NonSerialized] public Vector2 TriggerSize;
}