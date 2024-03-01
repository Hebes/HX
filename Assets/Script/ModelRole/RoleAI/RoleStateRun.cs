using Core;
using System.Collections;
using UnityEngine;
using Debug = Core.Debug;

/// <summary>
/// 角色移动状态
/// </summary>
public class RoleStateRun : IRoleState
{
    public RoleData RoleData { get ; set; }

    public ERoleSateType RoleSateType => ERoleSateType.Run;

    public long ID { get; set; }


    #region 本类特有

    #endregion

    public void StateEnter()
    {
        RoleData.gameObject = CoreResource.Load<GameObject>(ConfigPrefab.prefabCommonRole);
    }

    public void StateExit()
    {
    }

    public void StateUpdata()
    {
        //switch (roleType)
        //{
        //    case ERoleOrTeamType.Player:
        //        break;
        //    case ERoleOrTeamType.NPC:
        //        break;
        //    case ERoleOrTeamType.Enemy:
        //        break;
        //    default:
        //        break;
        //}
    }
}