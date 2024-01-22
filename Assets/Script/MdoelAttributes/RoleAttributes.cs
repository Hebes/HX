using System.Text;

/// <summary>
/// 角色属性
/// </summary>
public class RoleAttributes : IHP, IATK, IColldown
{

    private int _MaxHP;
    private int _CurrentHP;
    private int _MaxATK;
    private int _CurrentATK;
    private float _MaxColldown;
    private float _CurColldown;


    public int MaxHP { get => _MaxHP; set => _MaxHP = value; }
    public int CurrentHP { get => _CurrentHP; set => _CurrentHP = value; }
    public int MaxATK { get => _MaxATK; set => _MaxATK = value; }
    public int CurrentATK { get => _CurrentATK; set => _CurrentATK = value; }
    public float MaxColldown { get => _MaxColldown; set => _MaxColldown = value; }
    public float CurColldown { get => _CurColldown; set => _CurColldown = value; }


    public override string ToString()
    {
        StringBuilder sb=new StringBuilder();
        sb.AppendLine($"最大生命值{_MaxHP}");
        sb.AppendLine($"当前生命值{_CurrentHP}");
        return sb.ToString();
    }
}
