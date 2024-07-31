using UnityEngine;

/// <summary>
/// 玩家输入接口
/// </summary>
public interface IInputPlayer
{
    /// <summary>
    /// 操作遥感
    /// </summary>
    /// <param name="axis"></param>
    /// <returns></returns>
    Vector2 GetJoystick(string axis);

    Vector2 GetJoystickRaw(string axis);

    /// <summary>
    /// 获取按钮
    /// </summary>
    /// <param name="buttonName"></param>
    /// <returns></returns>
    bool GetButton(string buttonName);

    void SetVibration(float leftMotorValue, float rightMotorValue);
}