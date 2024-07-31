using System;

public class DatabaseDatabase
{
    public string Name { get; set; }

    public int Lv1Price { get; set; }

    public int Lv1EnhanceEffect { get; set; }

    public int Lv2Price { get; set; }

    public int Lv2EnhanceEffect { get; set; }

    public int Lv3Price { get; set; }

    public int Lv3EnhanceEffect { get; set; }

    public int GetNextLevelPrice(int level)
    {
        switch (level)
        {
            case 0:
                return this.Lv1Price;
            case 1:
                return this.Lv2Price;
            case 2:
                return this.Lv3Price;
            case 3:
                return int.MaxValue;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public int GetEnhanceEffect(int level)
    {
        switch (level)
        {
            case 0:
                return 0;
            case 1:
                return this.Lv1EnhanceEffect;
            case 2:
                return this.Lv2EnhanceEffect;
            case 3:
                return this.Lv3EnhanceEffect;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public string GetDesc(int level)
    {
        string text = ScriptLocalization.Get(string.Format("ui/enhancement/{0}Lv{1}Desc", this.Name, level));
        if (string.IsNullOrEmpty(text))
        {
            text = ScriptLocalization.Get(string.Format("ui/enhancement/{0}Lv0Desc", this.Name));
        }

        return text;
    }

    public string GetNextLevelDesc(int level)
    {
        string text = ScriptLocalization.Get(string.Format("ui/enhancement/{0}Lv{1}NextLevelDesc", this.Name, level));
        if (string.IsNullOrEmpty(text))
        {
            text = ScriptLocalization.Get(string.Format("ui/enhancement/{0}Lv0NextLevelDesc", this.Name));
        }

        return text;
    }

    public string GetControlKeys(int level)
    {
        string text = ScriptLocalization.Get(string.Format("mobile/enhancement/{0}Lv{1}ControlKeys", this.Name, level));
        if (string.IsNullOrEmpty(text))
        {
            text = ScriptLocalization.Get(string.Format("mobile/enhancement/{0}Lv0ControlKeys", this.Name));
        }

        return text;
    }

    public static DatabaseDatabase FindByName(string name)
    {
        return DB.Enhancements[name];
    }

    public static DatabaseDatabase SetValue(string[] strings)
    {
        return new DatabaseDatabase
        {
            Name = strings[0],
            Lv1Price = int.Parse(strings[1]),
            Lv1EnhanceEffect = int.Parse(strings[2]),
            Lv2Price = int.Parse(strings[3]),
            Lv2EnhanceEffect = int.Parse(strings[4]),
            Lv3Price = int.Parse(strings[5]),
            Lv3EnhanceEffect = int.Parse(strings[6])
        };
    }
}