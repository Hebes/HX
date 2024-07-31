using System;
using Core;

public class DS4Controller : IController
{
    public void Update()
    {
        bool button = this._player.GetButton("DpadLeft");
        bool button2 = this._player.GetButton("DpadRight");
        bool button3 = this._player.GetButton("DpadUp");
        bool button4 = this._player.GetButton("DpadDown");
        bool button5 = this._player.GetButton("LSLeft");
        bool button6 = this._player.GetButton("LSRight");
        bool button7 = this._player.GetButton("LSUp");
        bool button8 = this._player.GetButton("LSDown");
        bool button9 = this._player.GetButton("Cross");
        bool button10 = this._player.GetButton("Circle");
        bool button11 = this._player.GetButton("Triangle");
        bool button12 = this._player.GetButton("Square");
        bool button13 = this._player.GetButton("R1");
        bool button14 = this._player.GetButton("L1");
        bool button15 = this._player.GetButton("L2");
        bool button16 = this._player.GetButton("R2");
        bool button17 = this._player.GetButton("L3");
        bool button18 = this._player.GetButton("R3");
        bool button19 = this._player.GetButton("Options");
        DS4Controller.Down.Update(button4);
        DS4Controller.Up.Update(button3);
        DS4Controller.Left.Update(button);
        DS4Controller.Right.Update(button2);
        DS4Controller.Cross.Update(button9);
        DS4Controller.Circle.Update(button10);
        DS4Controller.Triangle.Update(button11);
        DS4Controller.Square.Update(button12);
        DS4Controller.R1.Update(button13);
        DS4Controller.R2.Update(button16);
        DS4Controller.R3.Update(button18);
        DS4Controller.L1.Update(button14);
        DS4Controller.L2.Update(button15);
        DS4Controller.L3.Update(button17);
        DS4Controller.Options.Update(button19);
        DS4Controller.LSDown.Update(button8);
        DS4Controller.LSUp.Update(button7);
        DS4Controller.LSLeft.Update(button5);
        DS4Controller.LSRight.Update(button6);
        bool isPressed = DS4Controller.Down.Pressed || DS4Controller.Up.Pressed || DS4Controller.Left.Pressed || DS4Controller.Right.Pressed ||
                         DS4Controller.Cross.Pressed || DS4Controller.Circle.Pressed || DS4Controller.Triangle.Pressed ||
                         DS4Controller.Square.Pressed || DS4Controller.R1.Pressed || DS4Controller.R2.Pressed || DS4Controller.R3.Pressed ||
                         DS4Controller.L1.Pressed || DS4Controller.L2.Pressed || DS4Controller.L3.Pressed || DS4Controller.LSDown.Pressed ||
                         DS4Controller.LSUp.Pressed || DS4Controller.LSLeft.Pressed || DS4Controller.LSRight.Pressed;
        DS4Controller.AnyKey.Update(isPressed);
        DS4Controller.LS.Update(this._player.GetJoystick("LS"), this._player.GetJoystickRaw("LS"));
        DS4Controller.RS.Update(this._player.GetJoystick("RS"), this._player.GetJoystickRaw("RS"));
    }

    private readonly IInputPlayer _player = new PS4InputPlayer();

    public static readonly InputButtonProcessor AnyKey = new InputButtonProcessor();

    public static readonly InputButtonProcessor Down = new InputButtonProcessor();

    public static readonly InputButtonProcessor Up = new InputButtonProcessor();

    public static readonly InputButtonProcessor Left = new InputButtonProcessor();

    public static readonly InputButtonProcessor Right = new InputButtonProcessor();

    public static readonly InputButtonProcessor Cross = new InputButtonProcessor();

    public static readonly InputButtonProcessor Circle = new InputButtonProcessor();

    public static readonly InputButtonProcessor Triangle = new InputButtonProcessor();

    public static readonly InputButtonProcessor Square = new InputButtonProcessor();

    public static readonly InputButtonProcessor R1 = new InputButtonProcessor();

    public static readonly InputButtonProcessor R2 = new InputButtonProcessor();

    public static readonly InputButtonProcessor R3 = new InputButtonProcessor();

    public static readonly InputButtonProcessor L1 = new InputButtonProcessor();

    public static readonly InputButtonProcessor L2 = new InputButtonProcessor();

    public static readonly InputButtonProcessor L3 = new InputButtonProcessor();

    public static readonly InputButtonProcessor Options = new InputButtonProcessor();

    public static readonly InputButtonProcessor LSDown = new InputButtonProcessor();

    public static readonly InputButtonProcessor LSUp = new InputButtonProcessor();

    public static readonly InputButtonProcessor LSLeft = new InputButtonProcessor();

    public static readonly InputButtonProcessor LSRight = new InputButtonProcessor();

    public static readonly InputJoystickProcessor LS = new InputJoystickProcessor();

    public static readonly InputJoystickProcessor RS = new InputJoystickProcessor();
}