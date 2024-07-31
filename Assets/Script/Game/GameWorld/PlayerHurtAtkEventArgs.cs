using System;
using LitJson;
using UnityEngine;

/// <summary>
/// 玩家伤害攻击事件
/// </summary>
public class PlayerHurtAtkEventArgs : EventArgs
{
    public PlayerHurtAtkEventArgs(GameObject _hurted, GameObject _sender, GameObject _origin, int _damage, int _atkId, JsonData1 _data,
        bool _forceHurt = false)
    {
        this.hurted = _hurted;
        this.sender = _sender;
        damage = _damage;
        this.atkId = _atkId;
        this.data = _data;
        this.origin = _origin;
        this.forceHurt = _forceHurt;
    }

    public int atkId;

    public int damage;

    public JsonData1 data;

    public GameObject hurted;

    public GameObject origin;

    public GameObject sender;

    public bool forceHurt;
}