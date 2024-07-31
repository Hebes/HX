using UnityEngine;

/// <summary>
/// 输入按钮处理器
/// </summary>
public class InputButtonProcessor
{
    /// <summary>
    /// 输入是否开启
    /// </summary>
    public bool IsOpen = true;

    private bool _wasPressed;

    private bool _isPressed;

    private float _pressedTime;

    private int _pressedFrame;

    private const float LongPressTime = 0.3f;

    public bool OnClick => OnReleased && _pressedTime < 0.3f;

    public bool OnPressed => Pressed && !_wasPressed;

    public bool OnPressedForSeveralSeconds(float second = 0.333333343f, bool accelerate = true)
    {
        int num = WorldTime.SecondToFrame(second);
        int num2 = WorldTime.SecondToFrame(0.0833333358f);
        int b = num2;
        if (accelerate)
        {
            int a = num - this._pressedFrame / num2;
            num = Mathf.Max(a, b);
        }

        return OnPressed || (Pressed && _pressedFrame != 0 && _pressedFrame % num == 0);
    }

    public bool OnReleased => Input.JoystickIsOpen && IsOpen && !_isPressed && _wasPressed;

    public bool Pressed => Input.JoystickIsOpen && IsOpen && _isPressed;

    public bool LongPressed => Pressed && _pressedTime >= 0.3f;

    public bool Released => Input.JoystickIsOpen && IsOpen && !_isPressed;

    public void Update(bool isPressed)
    {
        if (_wasPressed && isPressed)
        {
            _pressedTime += Time.unscaledDeltaTime;
            _pressedFrame++;
        }
        else if (!_wasPressed && !isPressed)
        {
            _pressedTime = 0f;
            _pressedFrame = 0;
        }

        _wasPressed = _isPressed;
        _isPressed = isPressed;
    }
}