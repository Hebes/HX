using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// 输入驱动
/// </summary>
public class InputDriver : MonoBehaviour
{
    public static int Solution { get; set; }

    private IEnumerator CheckForControllers()
    {
        for (;;)
        {
            string[] controllers = UnityEngine.Input.GetJoystickNames();
            if (this._isControllerConnected || controllers.Length <= 0)
            {
                goto IL_92;
            }

            string[] array = controllers;
            if (InputDriver._003C_003Ef__mg_0024cache0 == null)
            {
                InputDriver._003C_003Ef__mg_0024cache0 = new Predicate<string>(string.IsNullOrEmpty);
            }

            if (Array.TrueForAll<string>(array, InputDriver._003C_003Ef__mg_0024cache0))
            {
                goto IL_92;
            }

            this._isControllerConnected = true;
            UnityEngine.Debug.Log("Connected");
            IL_F2:
            yield return new WaitForSeconds(1f);
            continue;
            IL_92:
            if (this._isControllerConnected)
            {
                if (controllers.Length != 0)
                {
                    string[] array2 = controllers;
                    if (InputDriver._003C_003Ef__mg_0024cache1 == null)
                    {
                        InputDriver._003C_003Ef__mg_0024cache1 = new Predicate<string>(string.IsNullOrEmpty);
                    }

                    if (!Array.TrueForAll<string>(array2, InputDriver._003C_003Ef__mg_0024cache1))
                    {
                        goto IL_F2;
                    }
                }

                this._isControllerConnected = false;
                UnityEngine.Debug.Log("Disconnected");
                goto IL_F2;
            }

            goto IL_F2;
        }

        yield break;
    }

    private void Awake()
    {
        base.StartCoroutine(this.CheckForControllers());
    }

    private void Update()
    {
        for (int i = 0; i < this._controllers.Length; i++)
        {
            this._controllers[i].Update();
        }

        this.UpdateGlobalInput();
        this.UpdateGameInput();
        this.UpdateUIInput();
        this.UpdateStoryInput();
        this.UpdateShiInput();
    }

    private void UpdateGlobalInput()
    {
        Input.AnyKey.Update(DS4Controller.AnyKey.Pressed || VirtualController.AnyKey.Pressed);
    }

    private void UpdateGameInput()
    {
        Vector2 vector = Vector2.zero;
        bool flag = false;
        bool flag2 = false;
        bool flag3 = false;
        bool flag4 = false;
        bool flag5 = false;
        bool flag6 = false;
        bool flag7 = false;
        bool flag8 = false;
        bool flag9 = false;
        bool flag10 = false;
        bool flag11 = false;
        bool flag12 = false;
        bool flag13 = false;
        bool flag14 = false;
        bool flag15 = false;
        bool flag16 = false;
        bool flag17 = false;
        bool flag18 = false;
        bool flag19 = false;
        bool flag20 = false;
        bool flag21 = false;
        bool flag22 = false;
        bool flag23 = false;
        bool flag24 = false;
        bool flag25 = false;
        bool flag26 = false;
        bool flag27 = false;
        bool flag28 = false;
        bool flag29 = false;
        if (this._isControllerConnected)
        {
            Vector2 value = DS4Controller.LS.Value;
            vector += new Vector2(Mathf.Abs(value.x), Mathf.Abs(value.y));
            flag |= (Vector2.Angle(Vector2.up, vector) <= 45f);
            flag2 |= (DS4Controller.LS.Value.x > 0f);
            flag3 |= (DS4Controller.LS.Value.x < 0f);
            flag4 |= (DS4Controller.LS.Value.y < 0f && flag);
            flag5 |= (DS4Controller.LS.Value.y > 0f && flag);
            bool pressed = DS4Controller.Right.Pressed;
            bool pressed2 = DS4Controller.Left.Pressed;
            bool pressed3 = DS4Controller.Up.Pressed;
            bool pressed4 = DS4Controller.Down.Pressed;
            flag2 = (flag2 || pressed);
            flag3 = (flag3 || pressed2);
            flag4 = (flag4 || pressed3);
            flag5 = (flag5 || pressed4);
            flag17 |= (Vector2.Angle(Vector2.up, vector) <= 22.5f);
            flag18 |= (Vector2.Angle(Vector2.right, vector) <= 22.5f);
            flag19 |= (vector != Vector2.zero && Vector2.Angle(Vector2.up, vector) > 22.5f && Vector2.Angle(Vector2.right, vector) > 22.5f);
            bool flag30 = flag17 && DS4Controller.LS.Value.y < 0f;
            bool flag31 = flag17 && DS4Controller.LS.Value.y > 0f;
            bool flag32 = flag18 && DS4Controller.LS.Value.x < 0f;
            bool flag33 = flag18 && DS4Controller.LS.Value.x > 0f;
            bool flag34 = flag19 && DS4Controller.LS.Value.y < 0f && DS4Controller.LS.Value.x > 0f;
            bool flag35 = flag19 && DS4Controller.LS.Value.y > 0f && DS4Controller.LS.Value.x > 0f;
            bool flag36 = flag19 && DS4Controller.LS.Value.y < 0f && DS4Controller.LS.Value.x < 0f;
            bool flag37 = flag19 && DS4Controller.LS.Value.y > 0f && DS4Controller.LS.Value.x > 0f;
            flag22 |= (pressed2 || flag32);
            flag23 |= (pressed || flag33);
            flag20 |= (pressed3 || flag30);
            flag21 |= (pressed4 || flag31);
            flag24 |= ((pressed && pressed3) || flag34);
            flag25 |= ((pressed && pressed4) || flag35);
            flag26 |= ((pressed2 && pressed3) || flag36);
            flag27 |= ((pressed2 && pressed4) || flag37);
            flag10 |= DS4Controller.Square.Pressed;
            flag8 |= DS4Controller.Triangle.Pressed;
            flag9 |= DS4Controller.Cross.Pressed;
            flag11 |= DS4Controller.Square.Pressed;
            flag7 |= DS4Controller.Circle.Pressed;
            flag13 |= (flag10 && flag5);
            flag14 |= (flag10 && flag4);
            flag15 |= (flag5 && flag9);
            flag16 |= DS4Controller.R1.Pressed;
            bool pressed5 = DS4Controller.L1.Pressed;
            flag6 |= (pressed5 && flag10);
            flag12 |= DS4Controller.Circle.Pressed;
            flag28 |= DS4Controller.L2.Pressed;
            flag29 |= DS4Controller.R2.Pressed;
        }

        Vector2 value2 = VirtualController.LeftJoystick.Value;
        vector += new Vector2(Mathf.Abs(value2.x), Mathf.Abs(value2.y));
        flag |= (Vector2.Angle(Vector2.up, vector) <= 45f);
        flag2 |= (VirtualController.LeftJoystick.Value.x > 0f);
        flag3 |= (VirtualController.LeftJoystick.Value.x < 0f);
        flag4 |= (VirtualController.LeftJoystick.Value.y > 0f && flag);
        flag5 |= (VirtualController.LeftJoystick.Value.y < 0f && flag);
        flag6 |= VirtualController.Button1.Pressed;
        flag7 |= VirtualController.Button3.Pressed;
        flag8 = (flag8 || flag7);
        flag9 |= VirtualController.Button4.Pressed;
        flag10 |= VirtualController.Button5.Pressed;
        flag11 = (flag11 || flag10);
        flag12 = (flag12 || flag10);
        flag28 |= (VirtualController.L2.Pressed || VirtualController.Button6.Pressed);
        flag29 |= VirtualController.R2.Pressed;
        int solution = InputDriver.Solution;
        if (solution != 0)
        {
            if (solution == 1)
            {
                flag14 |= VirtualController.Button2.Pressed;
                flag13 = (flag13 || flag14);
                Vector2 value3 = VirtualController.LeftSwipe.Value;
                Vector2 vector2 = new Vector2(Mathf.Abs(value3.x), Mathf.Abs(value3.y));
                flag17 |= (Vector2.Angle(Vector2.up, vector2) <= 22.5f);
                flag18 |= (Vector2.Angle(Vector2.right, vector2) <= 22.5f);
                flag19 |= (vector2 != Vector2.zero && Vector2.Angle(Vector2.up, vector2) > 22.5f && Vector2.Angle(Vector2.right, vector2) > 22.5f);
                flag20 |= (flag17 && VirtualController.LeftSwipe.Value.y > 0f);
                flag21 |= (flag17 && VirtualController.LeftSwipe.Value.y < 0f);
                flag22 |= (flag18 && VirtualController.LeftSwipe.Value.x < 0f);
                flag23 |= (flag18 && VirtualController.LeftSwipe.Value.x > 0f);
                flag24 |= (flag19 && VirtualController.LeftSwipe.Value.x > 0f && VirtualController.LeftSwipe.Value.y > 0f);
                flag25 |= (flag19 && VirtualController.LeftSwipe.Value.x > 0f && VirtualController.LeftSwipe.Value.y < 0f);
                flag26 |= (flag19 && VirtualController.LeftSwipe.Value.x < 0f && VirtualController.LeftSwipe.Value.y > 0f);
                flag27 |= (flag19 && VirtualController.LeftSwipe.Value.x < 0f && VirtualController.LeftSwipe.Value.y < 0f);
            }
        }
        else
        {
            flag13 |= (flag10 && flag5);
            flag14 |= (flag10 && flag4);
            flag17 |= (Vector2.Angle(Vector2.up, vector) <= 22.5f);
            flag18 |= (Vector2.Angle(Vector2.right, vector) <= 22.5f);
            flag19 |= (vector != Vector2.zero && Vector2.Angle(Vector2.up, vector) > 22.5f && Vector2.Angle(Vector2.right, vector) > 22.5f);
            flag20 |= (flag17 && VirtualController.LeftJoystick.Value.y > 0f);
            flag21 |= (flag17 && VirtualController.LeftJoystick.Value.y < 0f);
            flag22 |= (flag18 && VirtualController.LeftJoystick.Value.x < 0f);
            flag23 |= (flag18 && VirtualController.LeftJoystick.Value.x > 0f);
            flag24 |= (flag19 && VirtualController.LeftJoystick.Value.x > 0f && VirtualController.LeftJoystick.Value.y > 0f);
            flag25 |= (flag19 && VirtualController.LeftJoystick.Value.x > 0f && VirtualController.LeftJoystick.Value.y < 0f);
            flag26 |= (flag19 && VirtualController.LeftJoystick.Value.x < 0f && VirtualController.LeftJoystick.Value.y > 0f);
            flag27 |= (flag19 && VirtualController.LeftJoystick.Value.x < 0f && VirtualController.LeftJoystick.Value.y < 0f);
            flag16 |= VirtualController.Button2.Pressed;
        }

        Input.Game.MoveDown.Update(flag5);
        Input.Game.MoveLeft.Update(flag3);
        Input.Game.MoveRight.Update(flag2);
        Input.Game.MoveUp.Update(flag4);
        Input.Game.JumpDown.Update(flag15);
        Input.Game.BladeStorm.Update(flag6);
        Input.Game.UpRising.Update(flag14);
        Input.Game.HitGround.Update(flag13);
        Input.Game.Execute.Update(flag7);
        Input.Game.CirtAtk.Update(flag8);
        Input.Game.Jump.Update(flag9);
        Input.Game.Atk.Update(flag10);
        Input.Game.Charging.Update(flag11);
        Input.Game.Search.Update(flag12);
        Input.Game.Flash.Left.Update(flag22 && flag16);
        Input.Game.Flash.Right.Update(flag23 && flag16);
        Input.Game.Flash.Up.Update(flag20 && flag16);
        Input.Game.Flash.Down.Update(flag21 && flag16);
        Input.Game.Flash.RightUp.Update(flag24 && flag16);
        Input.Game.Flash.RightDown.Update(flag25 && flag16);
        Input.Game.Flash.LeftUp.Update(flag26 && flag16);
        Input.Game.Flash.LeftDown.Update(flag27 && flag16);
        Input.Game.Flash.FaceDirection.Update(flag16);
        Input.Game.L2.Update(flag28);
        Input.Game.R2.Update(flag29);
    }

    private void UpdateUIInput()
    {
        Input.UI.Up.Update(DS4Controller.Up.Pressed || DS4Controller.LSUp.Pressed);
        Input.UI.Down.Update(DS4Controller.Down.Pressed || DS4Controller.LSDown.Pressed);
        Input.UI.Left.Update(DS4Controller.Left.Pressed || DS4Controller.LSLeft.Pressed);
        Input.UI.Right.Update(DS4Controller.Right.Pressed || DS4Controller.LSRight.Pressed);
        bool pressed = DS4Controller.Circle.Pressed;
        bool pressed2 = DS4Controller.Cross.Pressed;
        bool pressed3 = DS4Controller.Options.Pressed;
        bool pressed4 = VirtualController.Button5.Pressed;
        bool flag = VirtualController.Button4.Pressed || UnityEngine.Input.GetKey(KeyCode.Escape);
        bool pressed5 = VirtualController.Options.Pressed;
        Input.UI.Confirm.Update(pressed || pressed4);
        Input.UI.Cancel.Update(pressed2 || flag);
        Input.UI.Pause.Update(pressed3 || pressed5);
        Input.UI.Debug.Update(DS4Controller.R3.Pressed || UnityEngine.Input.GetKey(KeyCode.BackQuote));
    }

    private void UpdateStoryInput()
    {
        Input.Story.Skip.Update(DS4Controller.Triangle.Pressed || UnityEngine.Input.GetKey(KeyCode.Escape) || VirtualController.Button3.Pressed);
    }

    private void UpdateShiInput()
    {
        Vector2 to = new Vector2(Mathf.Abs(VirtualController.LeftJoystick.Value.x), Mathf.Abs(VirtualController.LeftJoystick.Value.y));
        bool flag = Vector2.Angle(Vector2.up, to) <= 45f;
        bool flag2 = VirtualController.LeftJoystick.Value.x > 0f;
        bool flag3 = VirtualController.LeftJoystick.Value.x < 0f;
        bool flag4 = VirtualController.LeftJoystick.Value.y > 0f && flag;
        bool flag5 = VirtualController.LeftJoystick.Value.y < 0f && flag;
        Input.Shi.Down.Update(DS4Controller.Down.Pressed || DS4Controller.LSDown.Pressed || flag5);
        Input.Shi.Up.Update(DS4Controller.Up.Pressed || DS4Controller.LSUp.Pressed || flag4);
        Input.Shi.Left.Update(DS4Controller.Left.Pressed || DS4Controller.LSLeft.Pressed || flag3);
        Input.Shi.Right.Update(DS4Controller.Right.Pressed || DS4Controller.LSRight.Pressed || flag2);
        Input.Shi.Jump.Update(DS4Controller.Square.Pressed || DS4Controller.Cross.Pressed || VirtualController.Button5.Pressed);
        Input.Shi.Pause.Update(DS4Controller.Options.Pressed);
    }

    private IController[] _controllers = new IController[]
    {
        new DS4Controller(),
        new VirtualController()
    };

    private bool _isControllerConnected;

    [CompilerGenerated] private static Predicate<string> _003C_003Ef__mg_0024cache0;

    [CompilerGenerated] private static Predicate<string> _003C_003Ef__mg_0024cache1;

    public static class Vibration
    {
        public static void SetVibration(float leftMotorValue, float rightMotorValue)
        {
        }
    }
}