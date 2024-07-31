public static class StringTool
{
    public static string Combine(this string name, string path)
    {
        return System.IO.Path.Combine(path, name);
    }
    
    public static string[] ParseStringArray(string text, char separator = ',')
    {
        string text2 = text.Trim();
        return text2.Substring(1, text2.Length - 2).Split(new char[]
        {
            separator
        });
    }

    public static int?[] ParseIntArray(string text, char separator = ',')
    {
        string[] array = ParseStringArray(text, separator);
        int?[] array2 = new int?[array.Length];
        for (int i = 0; i < array2.Length; i++)
        {
            int value;
            if (int.TryParse(array[i], out value))
            {
                array2[i] = new int?(value);
            }
            else
            {
                array2[i] = null;
            }
        }
        return array2;
    }

    public static readonly string[] Int2String = new string[]
    {
        "0",
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9"
    };
}