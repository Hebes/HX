using System;

public static class StringUtils
{
    public static bool IsInArray(this string value, string[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (value == array[i])
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsInEnum<TEnum>(this string value, bool ignoreCase = false) where TEnum : struct, IConvertible
    {
        return EnumTools.IsInEnum<TEnum>(value, ignoreCase);
    }

    public static TEnum ToEnum<TEnum>(this string value, bool ignoreCase = false) where TEnum : struct, IConvertible
    {
        return EnumTools.ToEnum<TEnum>(value, ignoreCase);
    }
}