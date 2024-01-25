using Core;
using System.Collections;
using UnityEngine;
using Debug = Core.Debug;

/// <summary>
/// 角色移动状态
/// </summary>
public class RoleStateRun : IRoleState
{
    private long _id;
    private ERoleSateType _roleSateType = ERoleSateType.Run;
    private IRoleInstance _role;

    public long ID { get => _id; set => _id = value; }
    public ERoleSateType RoleSateType { get => _roleSateType; set => _roleSateType = value; }
    public IRoleInstance RoleInstance { get => _role; set => _role = value; }

    private ERoleOrTeamType roleType => _role.RoleInfo.RoleType;
    private RoleData roleData=> _role.RoleInfo;

    #region 本类特有
    
    #endregion

    public void StateEnter()
    {
        roleData.gameObject = CoreResource.Load<GameObject>(ConfigPrefab.prefabCommonRole);
    }

    public void StateExit()
    {
    }

    public void StateUpdata()
    {
        switch (roleType)
        {
            case ERoleOrTeamType.Player:
                break;
            case ERoleOrTeamType.NPC:
                break;
            case ERoleOrTeamType.Enemy:
                break;
            default:
                break;
        }
    }
}