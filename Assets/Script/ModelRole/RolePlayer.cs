//using Core;
//using System.Collections.Generic;
//using UnityEngine;
//using Debug = Core.Debug;

//public class RolePlayer : IRole, IRoleBehaviour, ISkillCarrier, IBuffCarrier
//{
//    private ERoleType _roleType = ERoleType.Player;
//    private ERoleBattlePoint _roleBattlePoint = ERoleBattlePoint.Point1;
//    private uint _id;
//    private string _name;
//    private Dictionary<ESkillType, List<ISkill>> _skillDataDic;
//    private List<IBuff> _buffList;
//    private float _max_colldown;
//    private ETurnState turnState;
//    private GameObject _go;

//    public ERoleType RoleType { get => _roleType; set => _roleType = value; }
//    public ERoleBattlePoint RoleBattlePoint { get => _roleBattlePoint; set => _roleBattlePoint = value; }
//    public uint ID { get => _id; set => _id = value; }
//    public string Name { get => _name; set => _name = value; }
//    public Dictionary<ESkillType, List<ISkill>> SkillDataDic { get => _skillDataDic; set => _skillDataDic = value; }
//    public List<IBuff> BuffList { get => _buffList; set => _buffList = value; }
//    public float Max_colldown { get => _max_colldown; set => _max_colldown = value; }
//    public ETurnState TurnState { get => turnState; set => turnState = value; }
//    public UnityEngine.GameObject Go { get => _go; set => _go = value; }

//    public void RoleRemove()
//    {
//        Debug.Log("玩家初移除");
//    }

//    public void RoleInit()
//    {
//        turnState = ETurnState.PROCESSING;
//        Debug.Log("玩家初始化");
//    }

//    public void RoleUpdata()
//    {
//    }
//}
