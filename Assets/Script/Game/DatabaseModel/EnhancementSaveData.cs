using System;
using System.Collections.Generic;
using Framework.Core;

/// <summary>
/// 增强
/// </summary>
public class EnhancementSaveData
{
    static Dictionary<string, int> _003C_003Ef__switch_0024map19;

    public EnhancementSaveData()
    {
        this.SetToDefault();
    }

    public int this[string name]
    {
        get
        {
            switch (name)
            {
                case "attack":
                    return this.Attack;
                case "knockout":
                    return this.Knockout;
                case "avatarAttack":
                    return this.AvatarAttack;
                case "airAttack":
                    return this.AirAttack;
                case "airCombo2":
                    return this.AirCombo2;
                case "airAvatarAttack":
                    return this.AirAvatarAttack;
                case "airCombo1":
                    return this.AirCombo1;
                case "tripleAttack":
                    return this.TripleAttack;
                case "combo2":
                    return this.Combo2;
                case "upperChop":
                    return this.UpperChop;
                case "combo1":
                    return this.Combo1;
                case "bladeStorm":
                    return this.BladeStorm;
                case "shadeAttack":
                    return this.ShadeAttack;
                case "charging":
                    return this.Charging;
                case "chase":
                    return this.Chase;
                case "hitGround":
                    return this.HitGround;
                case "maxHP":
                    return this.MaxHp;
                case "maxEnergy":
                    return this.MaxEnergy;
                case "flashAttack":
                    return this.FlashAttack;
                case "recover":
                    return this.Recover;
            }

            throw new ArgumentOutOfRangeException("name", name, "This skill is not exist.");
        }
        set
        {
            if (name != null)
            {
                if (EnhancementSaveData._003C_003Ef__switch_0024map19 == null)
                {
                    EnhancementSaveData._003C_003Ef__switch_0024map19 = new Dictionary<string, int>(20)
                    {
                        {
                            "attack",
                            0
                        },
                        {
                            "knockout",
                            1
                        },
                        {
                            "avatarAttack",
                            2
                        },
                        {
                            "airAttack",
                            3
                        },
                        {
                            "airCombo2",
                            4
                        },
                        {
                            "airAvatarAttack",
                            5
                        },
                        {
                            "airCombo1",
                            6
                        },
                        {
                            "tripleAttack",
                            7
                        },
                        {
                            "combo2",
                            8
                        },
                        {
                            "upperChop",
                            9
                        },
                        {
                            "combo1",
                            10
                        },
                        {
                            "bladeStorm",
                            11
                        },
                        {
                            "shadeAttack",
                            12
                        },
                        {
                            "charging",
                            13
                        },
                        {
                            "chase",
                            14
                        },
                        {
                            "hitGround",
                            15
                        },
                        {
                            "maxHP",
                            16
                        },
                        {
                            "maxEnergy",
                            17
                        },
                        {
                            "flashAttack",
                            18
                        },
                        {
                            "recover",
                            19
                        }
                    };
                }

                int num;
                if (EnhancementSaveData._003C_003Ef__switch_0024map19.TryGetValue(name, out num))
                {
                    switch (num)
                    {
                        case 0:
                            this.Attack = value;
                            break;
                        case 1:
                            this.Knockout = value;
                            break;
                        case 2:
                            this.AvatarAttack = value;
                            break;
                        case 3:
                            this.AirAttack = value;
                            break;
                        case 4:
                            this.AirCombo2 = value;
                            break;
                        case 5:
                            this.AirAvatarAttack = value;
                            break;
                        case 6:
                            this.AirCombo1 = value;
                            break;
                        case 7:
                            this.TripleAttack = value;
                            break;
                        case 8:
                            this.Combo2 = value;
                            break;
                        case 9:
                            this.UpperChop = value;
                            break;
                        case 10:
                            this.Combo1 = value;
                            break;
                        case 11:
                            this.BladeStorm = value;
                            break;
                        case 12:
                            this.ShadeAttack = value;
                            break;
                        case 13:
                            this.Charging = value;
                            break;
                        case 14:
                            this.Chase = value;
                            break;
                        case 15:
                            this.HitGround = value;
                            break;
                        case 16:
                            this.MaxHp = value;
                            break;
                        case 17:
                            this.MaxEnergy = value;
                            break;
                        case 18:
                            this.FlashAttack = value;
                            break;
                        case 19:
                            this.Recover = value;
                            break;
                        case 20:
                            goto IL_27A;
                        default:
                            goto IL_27A;
                    }

                    return;
                }
            }

            IL_27A:
            throw new KeyNotFoundException();
        }
    }

    public void SetAllTo(int value)
    {
        this.Attack = value;
        this.Knockout = value;
        this.AvatarAttack = value;
        this.AirAttack = value;
        this.AirCombo2 = value;
        this.AirAvatarAttack = value;
        this.AirCombo1 = value;
        this.TripleAttack = value;
        this.Combo2 = value;
        this.UpperChop = value;
        this.Combo1 = value;
        this.BladeStorm = value;
        this.ShadeAttack = value;
        this.Charging = value;
        this.Chase = value;
        this.HitGround = value;
        this.MaxHp = value;
        this.Post("maxHP", value);
        this.MaxEnergy = value;
        this.FlashAttack = value;
        this.Recover = value;
        R.Player.Attribute.flashLevel = value;
    }

    public void SetToDefault()
    {
        this.Attack = 1;
        this.Knockout = 0;
        this.AvatarAttack = 0;
        this.AirAttack = 1;
        this.AirCombo2 = 0;
        this.AirCombo1 = 0;
        this.AirAvatarAttack = 0;
        this.AirCombo1 = 1;
        this.TripleAttack = 1;
        this.Combo2 = 0;
        this.UpperChop = 1;
        this.Combo1 = 1;
        this.BladeStorm = 0;
        this.ShadeAttack = 0;
        this.Charging = 1;
        this.Chase = 0;
        this.HitGround = 1;
        this.MaxHp = 1;
        this.MaxEnergy = 1;
        this.FlashAttack = 1;
        this.Recover = 1;
    }

    public void Post(string name, int upToLevel)
    {
        EGameEvent.EnhanceLevelup.Trigger((this, new EnhanceArgs(name, upToLevel)));
    }

    public int AirAttack;

    public int AirAvatarAttack;

    public int AirCombo1;

    public int AirCombo2;

    public int Attack;

    public int AvatarAttack;

    /// <summary>
    /// 刀刃风暴
    /// </summary>
    public int BladeStorm;

    public int Charging;

    public int Chase;

    public int Combo1;

    public int Combo2;

    public int FlashAttack;

    public int HitGround;

    public int Knockout;

    public int MaxEnergy;

    public int MaxHp;

    public int ShadeAttack;

    public int TripleAttack;

    public int UpperChop;

    public int Recover;
}