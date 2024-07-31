using System;
using UnityEngine;

/// <summary>
/// 敌人伤害攻击事件
/// </summary>
public class EnemyHurtAtkEventArgs : EventArgs
{
    public EnemyHurtAtkEventArgs(GameObject _hurted, GameObject _sender, int _attackId, Vector3 _hurtPos, HurtCheck.BodyType _body,
        PlayerNormalAtkData _attackData, bool _forceHurt = false)
    {
        hurted = _hurted;
        sender = _sender;
        attackId = _attackId;
        hurtPos = _hurtPos;
        body = _body;
        attackData = _attackData;
        forceHurt = _forceHurt;
        hurtType = HurtTypeEnum.Normal;
    }

    public EnemyHurtAtkEventArgs(GameObject _hurted, HurtTypeEnum type)
    {
        hurted = _hurted;
        hurtType = type;
    }

    public EnemyHurtAtkEventArgs(GameObject _hurted, HurtTypeEnum type, string playerState)
    {
        hurted = _hurted;
        hurtType = type;
        attackData = new PlayerNormalAtkData(playerState);
    }

    public PlayerNormalAtkData attackData;

    public int attackId;

    public HurtCheck.BodyType body;

    public bool forceHurt;

    public GameObject hurted;

    public Vector3 hurtPos;

    public HurtTypeEnum hurtType;

    public GameObject sender;

    /// <summary>
    /// 伤害类型枚举
    /// </summary>
    public enum HurtTypeEnum
    {
        Normal,
        ExecuteFollow,
        Execute,
        QTEHurt,
        Flash
    }

    public class PlayerNormalAtkData : EventArgs
    {
        public PlayerNormalAtkData(JsonData1 atkData, bool _firstHurt)
        {
            damagePercent = atkData.Get("damagePercent", 1f);
            atkName = atkData.Get("atkName", "Atk1");
            camShakeFrame = atkData.Get("shakeClip", 0);
            shakeStrength = atkData.Get("shakeOffset", 0f);
            shakeType = atkData.Get("shakeType", 0);
            frozenFrame = atkData.Get("frozenClip", 0);
            shakeFrame = atkData.Get("frameShakeClip", 0);
            joystickShakeNum = atkData.Get("joystickShakeNum", -1);
            firstHurt = _firstHurt;
        }

        public PlayerNormalAtkData(string _atkName)
        {
            atkName = _atkName;
        }

        public float damagePercent = 1f;

        public string atkName;

        public bool firstHurt;

        public int camShakeFrame;

        public float shakeStrength = 1f;

        public int shakeType;

        public int frozenFrame;

        public int shakeFrame;

        public int joystickShakeNum;
    }
}