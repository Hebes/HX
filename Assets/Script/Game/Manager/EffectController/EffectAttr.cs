using System;
using UnityEngine;

/// <summary>
/// 效果属性
/// </summary>
[Serializable]
public class EffectAttr
{
    public int id;

    /// <summary>
    /// 是否可见
    /// </summary>
    public bool isFollow;

    /// <summary>
    /// 最大数量
    /// </summary>
    public int maxCount = 20;

    /// <summary>
    /// 效果开始数量
    /// </summary>
    public int effectStartCount = 1;

    public EffectController.FXRotationCondition rotation;

    public Transform effect;

    public Vector3 scale;

    [HideInInspector]
    public string prefabPath;

    public string functionName;
}