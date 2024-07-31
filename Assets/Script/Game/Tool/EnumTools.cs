using System;

public static class EnumTools
{
    public static bool IsInEnum<TEnum>(string value, bool ignoreCase = false) where TEnum : struct, IConvertible
    {
        if (!ignoreCase)
        {
            return Enum.IsDefined(typeof(TEnum), value);
        }

        string[] names = Enum.GetNames(typeof(TEnum));
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i].Equals(value, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    public static bool TryParse<TEnum>(string value, out TEnum result) where TEnum : struct, IConvertible
    {
        return EnumTools.TryParse<TEnum>(value, false, out result);
    }

    public static bool TryParse<TEnum>(string value, bool ignoreCase, out TEnum result) where TEnum : struct
    {
        if (Enum.IsDefined(typeof(TEnum), value))
        {
            result = (TEnum)((object)Enum.Parse(typeof(TEnum), value, true));
            return true;
        }

        if (ignoreCase)
        {
            string[] names = Enum.GetNames(typeof(TEnum));
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i].Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    result = (TEnum)((object)Enum.Parse(typeof(TEnum), names[i]));
                    return true;
                }
            }
        }

        result = default(TEnum);
        return false;
    }

    public static TEnum ToEnum<TEnum>(string value, bool ignoreCase = false) where TEnum : struct, IConvertible
    {
        TEnum result;
        EnumTools.TryParse<TEnum>(value, ignoreCase, out result);
        return result;
    }
}