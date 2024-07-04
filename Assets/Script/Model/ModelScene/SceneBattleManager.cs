using Framework.Core;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗管理器
/// </summary>
public class SceneBattleManager : MonoBehaviour
{
    public static SceneBattleManager Instance;
    private UIComponent component;

    //玩家队伍的位置
    private GameObject roleTemplate;//role模板
    private Dictionary<ETeamPoint, Dictionary<ERoleBattlePoint, Transform>> _roleBattlePointDic;

    public UIComponent Component { get => component; }

    private void Awake()
    {
        Instance = this;
        _roleBattlePointDic = new Dictionary<ETeamPoint, Dictionary<ERoleBattlePoint, Transform>>();
        component = GetComponent<UIComponent>();
        GameObject T_RoleTemplate = Component.Get("T_RoleTemplate");
        GameObject T_Left1 = Component.Get("T_Left1");
        GameObject T_Left2 = Component.Get("T_Left2");
        GameObject T_Left3 = Component.Get("T_Left3");
        GameObject T_Left4 = Component.Get("T_Left4");
        GameObject T_Right1 = Component.Get("T_Right1");
        GameObject T_Right2 = Component.Get("T_Right2");
        GameObject T_Right3 = Component.Get("T_Right3");

        roleTemplate = T_RoleTemplate;
        T_RoleTemplate.SetActive(false);

        _roleBattlePointDic.Add(ETeamPoint.Left1, new Dictionary<ERoleBattlePoint, Transform>());
        _roleBattlePointDic[ETeamPoint.Left1].Add(ERoleBattlePoint.Point1, T_Left1.transform.GetChild(0));
        _roleBattlePointDic[ETeamPoint.Left1].Add(ERoleBattlePoint.Point2, T_Left1.transform.GetChild(1));
        _roleBattlePointDic[ETeamPoint.Left1].Add(ERoleBattlePoint.Point3, T_Left1.transform.GetChild(2));
        _roleBattlePointDic[ETeamPoint.Left1].Add(ERoleBattlePoint.Point4, T_Left1.transform.GetChild(3));

        _roleBattlePointDic.Add(ETeamPoint.Left2, new Dictionary<ERoleBattlePoint, Transform>());
        _roleBattlePointDic[ETeamPoint.Left2].Add(ERoleBattlePoint.Point1, T_Left2.transform.GetChild(0));
        _roleBattlePointDic[ETeamPoint.Left2].Add(ERoleBattlePoint.Point2, T_Left2.transform.GetChild(1));
        _roleBattlePointDic[ETeamPoint.Left2].Add(ERoleBattlePoint.Point3, T_Left2.transform.GetChild(2));
        _roleBattlePointDic[ETeamPoint.Left2].Add(ERoleBattlePoint.Point4, T_Left2.transform.GetChild(3));

        _roleBattlePointDic.Add(ETeamPoint.Left3, new Dictionary<ERoleBattlePoint, Transform>());
        _roleBattlePointDic[ETeamPoint.Left3].Add(ERoleBattlePoint.Point1, T_Left3.transform.GetChild(0));
        _roleBattlePointDic[ETeamPoint.Left3].Add(ERoleBattlePoint.Point2, T_Left3.transform.GetChild(1));
        _roleBattlePointDic[ETeamPoint.Left3].Add(ERoleBattlePoint.Point3, T_Left3.transform.GetChild(2));
        _roleBattlePointDic[ETeamPoint.Left3].Add(ERoleBattlePoint.Point4, T_Left3.transform.GetChild(3));

        _roleBattlePointDic.Add(ETeamPoint.Left4, new Dictionary<ERoleBattlePoint, Transform>());
        _roleBattlePointDic[ETeamPoint.Left4].Add(ERoleBattlePoint.Point1, T_Left4.transform.GetChild(0));
        _roleBattlePointDic[ETeamPoint.Left4].Add(ERoleBattlePoint.Point2, T_Left4.transform.GetChild(1));
        _roleBattlePointDic[ETeamPoint.Left4].Add(ERoleBattlePoint.Point3, T_Left4.transform.GetChild(2));
        _roleBattlePointDic[ETeamPoint.Left4].Add(ERoleBattlePoint.Point4, T_Left4.transform.GetChild(3));

        _roleBattlePointDic.Add(ETeamPoint.Right1, new Dictionary<ERoleBattlePoint, Transform>());
        _roleBattlePointDic[ETeamPoint.Right1].Add(ERoleBattlePoint.Point1, T_Right1.transform.GetChild(0));
        _roleBattlePointDic[ETeamPoint.Right1].Add(ERoleBattlePoint.Point2, T_Right1.transform.GetChild(1));
        _roleBattlePointDic[ETeamPoint.Right1].Add(ERoleBattlePoint.Point3, T_Right1.transform.GetChild(2));
        _roleBattlePointDic[ETeamPoint.Right1].Add(ERoleBattlePoint.Point4, T_Right1.transform.GetChild(3));

        _roleBattlePointDic.Add(ETeamPoint.Right2, new Dictionary<ERoleBattlePoint, Transform>());
        _roleBattlePointDic[ETeamPoint.Right2].Add(ERoleBattlePoint.Point1, T_Right2.transform.GetChild(0));
        _roleBattlePointDic[ETeamPoint.Right2].Add(ERoleBattlePoint.Point2, T_Right2.transform.GetChild(1));
        _roleBattlePointDic[ETeamPoint.Right2].Add(ERoleBattlePoint.Point3, T_Right2.transform.GetChild(2));
        _roleBattlePointDic[ETeamPoint.Right2].Add(ERoleBattlePoint.Point4, T_Right2.transform.GetChild(3));

        _roleBattlePointDic.Add(ETeamPoint.Right3, new Dictionary<ERoleBattlePoint, Transform>());
        _roleBattlePointDic[ETeamPoint.Right3].Add(ERoleBattlePoint.Point1, T_Right3.transform.GetChild(0));
        _roleBattlePointDic[ETeamPoint.Right3].Add(ERoleBattlePoint.Point2, T_Right3.transform.GetChild(1));
        _roleBattlePointDic[ETeamPoint.Right3].Add(ERoleBattlePoint.Point3, T_Right3.transform.GetChild(2));
        _roleBattlePointDic[ETeamPoint.Right3].Add(ERoleBattlePoint.Point4, T_Right3.transform.GetChild(3));

        //_roleBattlePointDic.Add(ETeamPoint.Right4, new Dictionary<ERoleBattlePoint, Transform>());
        //_roleBattlePointDic[ETeamPoint.Right4].Add(ERoleBattlePoint.Point1, T_Right4.transform.GetChild(0));
        //_roleBattlePointDic[ETeamPoint.Right4].Add(ERoleBattlePoint.Point2, T_Right4.transform.GetChild(1));
        //_roleBattlePointDic[ETeamPoint.Right4].Add(ERoleBattlePoint.Point3, T_Right4.transform.GetChild(2));
        //_roleBattlePointDic[ETeamPoint.Right4].Add(ERoleBattlePoint.Point4, T_Right4.transform.GetChild(3));
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    /// <summary>
    /// 实例化角色物体
    /// </summary>
    public GameObject InstantiateBattleRole(ETeamPoint teamPoint, RoleData role)
    {
        Transform tr = SetRolePoint(teamPoint, role.RoleBattlePoint); 
        if (tr == null)
            EDebug.Error("父物体设置为空,请检查赋值");
        GameObject gameObject = GameObject.Instantiate(roleTemplate, tr);
        gameObject.transform.localPosition = Vector2.zero;
        gameObject.SetActive(true);
        gameObject.name = $"{role.Name}{role.ID}";
        return gameObject;
    }

    /// <summary>
    /// 设置玩家角色
    /// </summary>
    private Transform SetRolePoint(ETeamPoint teamPoint, ERoleBattlePoint roleBattlePoint) => _roleBattlePointDic[teamPoint][roleBattlePoint];
}
