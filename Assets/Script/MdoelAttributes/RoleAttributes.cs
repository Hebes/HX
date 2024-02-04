using System.Text;

/// <summary>
/// 角色属性
/// </summary>
public class RoleAttributes : AttributesData, IATK, IHP, IColldown
{
    public RoleAttributes(int atk, int hp, float colldown)
    {
        MaxATK = CurrentATK = atk;
        MaxHP = CurrentHP = hp;
        MaxColldown = CurColldown = colldown;
    }

    public int MaxATK { get; set; }
    public int CurrentATK { get; set; }
    public int MaxHP { get; set; }
    public int CurrentHP { get; set; }
    public float MaxColldown { get; set; }
    public float CurColldown { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"最大生命值{MaxHP}");
        sb.AppendLine($"当前生命值{CurrentHP}");
        return sb.ToString();
    }
}
