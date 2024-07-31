using Core;
using UnityEngine;

/// <summary>
/// 虚拟控制器
/// </summary>
public class VirtualController : IController
{
    private MobileInputPlayer _player => SingletonMono<MobileInputPlayer>.Instance;

    public void Update()
    {
        VirtualController.Button1.Update(this._player.GetButton("Button1"));
        VirtualController.Button2.Update(this._player.GetButton("Button2"));
        VirtualController.Button3.Update(this._player.GetButton("Button3"));
        VirtualController.Button4.Update(this._player.GetButton("Button4"));
        VirtualController.Button5.Update(this._player.GetButton("Button5"));
        VirtualController.Button6.Update(this._player.GetButton("Options"));
        VirtualController.Options.Update(this._player.GetButton("Options") || UnityEngine.Input.GetKey(KeyCode.Escape));
        VirtualController.AnyKey.Update(VirtualController.Button1.Pressed || VirtualController.Button2.Pressed || VirtualController.Button3.Pressed ||
                                        VirtualController.Button4.Pressed || VirtualController.Button5.Pressed || UnityEngine.Input.touchCount > 0);
        VirtualController.L2.Update(this._player.GetButton("L2"));
        VirtualController.R2.Update(this._player.GetButton("R2"));
        VirtualController.LeftJoystick.Update(this._player.GetJoystick("Joystick"), default(Vector2));
        VirtualController.LeftSwipe.Update(this._player.GetJoystick("Swipe"), default(Vector2));
    }

    public static readonly InputButtonProcessor AnyKey = new InputButtonProcessor();

    public static readonly InputButtonProcessor Button1 = new InputButtonProcessor();

    public static readonly InputButtonProcessor Button2 = new InputButtonProcessor();

    public static readonly InputButtonProcessor Button3 = new InputButtonProcessor();

    public static readonly InputButtonProcessor Button4 = new InputButtonProcessor();

    public static readonly InputButtonProcessor Button5 = new InputButtonProcessor();

    public static readonly InputButtonProcessor Button6 = new InputButtonProcessor();

    public static readonly InputButtonProcessor Options = new InputButtonProcessor();

    public static readonly InputButtonProcessor L2 = new InputButtonProcessor();

    public static readonly InputButtonProcessor R2 = new InputButtonProcessor();

    public static readonly InputJoystickProcessor LeftJoystick = new InputJoystickProcessor();

    public static readonly InputJoystickProcessor LeftSwipe = new InputJoystickProcessor();
}