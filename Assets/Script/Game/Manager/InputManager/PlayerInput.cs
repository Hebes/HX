using System;
using UnityEngine;

/// <summary>
/// 玩家输入
/// </summary>
public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance;
    public bool IsUseKeyboard = true;

    private void Awake()
    {
        Instance = this;
        pab = GetComponent<PlayerAbilities>();
        pAttr = R.Player.Attribute;
    }

    private void Update()
    {
        if (!R.SceneData.isPausing)
        {
            UpdateInput();
        }
    }

    private void UpdateInput()
    {
        if (pAttr.isDead)
        {
            return;
        }

        if (!battlePause)
        {
            if (Input.Game.JumpDown.OnPressed)
            {
                pab.jumpDown.JumpDown();
                return;
            }

            if (Input.Game.BladeStorm.OnPressed)
            {
                pab.skill.BladeStorm();
                return;
            }

            if (Input.Game.ShadeAtk.OnPressed)
            {
                pab.skill.ShadeAtk();
                return;
            }

            if (Setting.CanFlash)
            {
                if (Input.Game.Flash.RightUp.OnPressed)
                {
                    pab.flash.FlashRightUp();
                    return;
                }

                if (Input.Game.Flash.RightDown.OnPressed)
                {
                    pab.flash.FlashRightDown();
                    return;
                }

                if (Input.Game.Flash.LeftUp.OnPressed)
                {
                    pab.flash.FlashLeftUp();
                    return;
                }

                if (Input.Game.Flash.LeftDown.OnPressed)
                {
                    pab.flash.FlashLeftDown();
                    return;
                }

                if (Input.Game.Flash.Left.OnPressed)
                {
                    pab.flash.FlashLeft();
                    return;
                }

                if (Input.Game.Flash.Right.OnPressed)
                {
                    pab.flash.FlashRight();
                    return;
                }

                if (Input.Game.Flash.Up.OnPressed)
                {
                    pab.flash.FlashUp();
                    return;
                }

                if (Input.Game.Flash.Down.OnPressed)
                {
                    pab.flash.FlashDown();
                    return;
                }

                if (Input.Game.Flash.FaceDirection.OnPressed)
                {
                    pab.flash.FlashFace();
                    return;
                }
            }

            if (Input.Game.UpRising.OnPressed && Setting.CanUpRising)
            {
                pab.upRising.UpJumpAttack();
            }

            if (Input.Game.HitGround.OnPressed)
            {
                pab.hitGround.HitGround();
            }

            if (Input.Game.FlashAttack.OnPressed && pab.flashAttack.PressFlashAttack())
            {
                return;
            }

            if (Input.Game.Execute.OnPressed && Setting.CanExecute)
            {
                pab.execute.Execute();
                return;
            }

            if (Input.Game.Chase.OnPressed)
            {
                pab.chase.Chase();
            }

            if (Input.Game.Jump.OnPressed && Setting.CanJump)
            {
                pab.jump.Jump();
            }

            if (Input.Game.Atk.OnClick && Setting.CanAttack)
            {
                if (Input.Game.MoveLeft.Pressed)
                {
                    pab.attack.PlayerAttack(-1, false);
                }
                else if (Input.Game.MoveRight.Pressed)
                {
                    pab.attack.PlayerAttack(1, false);
                }
                else
                {
                    pab.attack.PlayerAttack(3, false);
                }
            }

            if (Input.Game.CirtAtk.OnClick && Setting.CanAttack)
            {
                if (Input.Game.MoveLeft.Pressed)
                {
                    pab.attack.PlayerAttack(-1, true);
                }
                else if (Input.Game.MoveRight.Pressed)
                {
                    pab.attack.PlayerAttack(1, true);
                }
                else
                {
                    pab.attack.PlayerAttack(3, true);
                }
            }

            if (Input.Game.CirtAtk.LongPressed && Setting.CanAttack)
            {
                if (Input.Game.MoveLeft.Pressed)
                {
                    pab.attack.PlayerCirtPressAttack(-1);
                }
                else if (Input.Game.MoveRight.Pressed)
                {
                    pab.attack.PlayerCirtPressAttack(1);
                }
                else
                {
                    pab.attack.PlayerCirtPressAttack(3);
                }
            }

            if (Input.Game.CirtAtk.OnReleased && Setting.CanAttack)
            {
                pab.attack.PlayerCirtPressAttackReleasd();
            }

            if (Input.Game.Charging.LongPressed && Setting.CanCharging)
            {
                pab.charge.Charging();
            }
        }

        if (Setting.CanMove)
        {
            if (Input.Game.MoveLeft.Pressed)
            {
                pab.move.Move(-1);
            }
            else if (Input.Game.MoveRight.Pressed)
            {
                pab.move.Move(1);
            }

            if (Input.Game.MoveLeft.OnReleased || Input.Game.MoveRight.OnReleased)
            {
                R.Player.Action.tempDir = 3;
            }
        }
    }

    private const int LEFT = -1;

    private const int RIGHT = 1;

    private const int CURRENT = 3;

    private PlayerAttribute pAttr;

    private PlayerAbilities pab;

    public bool battlePause;

    public class Setting
    {
        private static void SetFlag(InputType inputType, bool value)
        {
            if (value)
            {
                AllowInput |= inputType;
            }
            else
            {
                AllowInput &= ~inputType;
            }
        }

        private static bool GetFlag(InputType inputType)
        {
            return (inputType & AllowInput) == inputType;
        }

        public static bool CanJump
        {
            get { return GetFlag(InputType.Jump); }
            set { SetFlag(InputType.Jump, value); }
        }

        public static bool CanMove
        {
            get { return GetFlag(InputType.Move); }
            set { SetFlag(InputType.Move, value); }
        }

        public static bool CanAttack
        {
            get { return GetFlag(InputType.Attack); }
            set { SetFlag(InputType.Attack, value); }
        }

        public static bool CanFlash
        {
            get { return GetFlag(InputType.Flash); }
            set { SetFlag(InputType.Flash, value); }
        }

        public static bool CanSkill
        {
            get { return GetFlag(InputType.Skill); }
            set { SetFlag(InputType.Skill, value); }
        }

        public static bool CanUpRising
        {
            get { return GetFlag(InputType.UpRising); }
            set { SetFlag(InputType.UpRising, value); }
        }

        public static bool CanHitGround
        {
            get { return GetFlag(InputType.HitGround); }
            set { SetFlag(InputType.HitGround, value); }
        }

        public static bool CanCharging
        {
            get { return GetFlag(InputType.Charging); }
            set { SetFlag(InputType.Charging, value); }
        }

        public static bool CanExecute
        {
            get { return GetFlag(InputType.Execute); }
            set { SetFlag(InputType.Execute, value); }
        }

        public static InputType AllowInput = InputType.All;

        [Flags]
        public enum InputType
        {
            None = 0,
            Move = 1,
            Attack = 2,
            Flash = 4,
            Skill = 8,
            HitGround = 16,
            UpRising = 32,
            Jump = 64,
            Charging = 128,
            Execute = 256,
            Option = 512,
            Map = 1024,
            All = 2047
        }
    }
}