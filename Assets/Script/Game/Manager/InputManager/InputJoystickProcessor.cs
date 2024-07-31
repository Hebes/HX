using System;
using UnityEngine;

namespace Core
{
    public class InputJoystickProcessor
    {
        public bool OnPressed
        {
            get
            {
                return Input.JoystickIsOpen && this.IsOpen && this._isPressed && !this._wasPressed;
            }
        }

        public bool Pressed
        {
            get
            {
                return Input.JoystickIsOpen && this.IsOpen && this._isPressed;
            }
        }

        public bool OnReleased
        {
            get
            {
                return Input.JoystickIsOpen && this.IsOpen && !this._isPressed && this._wasPressed;
            }
        }

        public bool Released
        {
            get
            {
                return Input.JoystickIsOpen && this.IsOpen && !this._isPressed;
            }
        }

        public void Update(Vector2 axis, Vector2 axisRaw = default(Vector2))
        {
            if (PlayerInput.Instance.IsUseKeyboard)
            {
                var moveInputX = UnityEngine.Input.GetAxis("Horizontal"); // 获取水平方向的输入值
                var moveInputY = UnityEngine.Input.GetAxis("Vertical"); // 获取垂直方向的输入值
                axis.x = moveInputX;
                axis.y = moveInputY;
            }
			
            if (this.IsOpen)
            {
                this.Value = axis;
                this.ValueRaw = axisRaw;
            }
            else
            {
                this.Value = default(Vector2);
                this.ValueRaw = default(Vector2);
            }
            this._wasPressed = this._isPressed;
            this._isPressed = (axis.sqrMagnitude >= 0.0100000007f);
        }

        private const float Threshold = 0.0100000007f;

        private bool _wasPressed;

        private bool _isPressed;

        public bool IsOpen = true;

        public Vector2 Value;

        public Vector2 ValueRaw;
    }
}