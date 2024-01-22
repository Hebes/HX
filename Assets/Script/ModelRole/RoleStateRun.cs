using Core;
using System.Collections;
using UnityEngine;
using Debug = Core.Debug;

/// <summary>
/// 角色普通状态
/// </summary>
public class RoleStateRun : IRoleState
{
    private long _id;
    private ERoleSateType _roleSateType = ERoleSateType.Run;
    private IRoleInstance _role;

    public long ID { get => _id; set => _id = value; }
    public ERoleSateType RoleSateType { get => _roleSateType; set => _roleSateType = value; }

    public void RoleInit()
    {
    }

    public void RoleRemove()
    {
    }

    public void RoleUpdata()
    {
    }
}

