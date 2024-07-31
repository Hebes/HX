using System;
using System.Collections.Generic;
using Framework.Core;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 效果控制器
/// </summary>
public class EffectController : SingletonMono<EffectController>
{
    public List<EffectAttr> fxSerializedData
    {
        get => _fxSerializedData;
        set
        {
            _fxSerializedData = value;
            _fxData = null;
        }
    }

    public Dictionary<int, EffectAttr> fxData
    {
        get
        {
            if (_fxData == null)
            {
                _fxData = new Dictionary<int, EffectAttr>();
                for (var i = 0; i < _fxSerializedData.Count; i++)
                {
                    EffectAttr effectAttr = _fxSerializedData[i];
                    _fxData.Add(effectAttr.id, effectAttr);
                }
            }

            return _fxData;
        }
    }

    private void Start()
    {
        if (AllowPreload)
            Preload();
    }

    public Transform Generate(int effectId, Transform target = null, Vector3 position = default(Vector3), Vector3 rotation = default(Vector3),
        Vector3 scale = default(Vector3), bool useFxZNum = true)
    {
        GameObject gameObject = UsePool(fxData[effectId].effect.name);
        Transform transform = gameObject == null ? Instantiate(fxData[effectId].effect) : gameObject.transform;
        if (target != null)
        {
            transform.parent = target;
            transform.localPosition = position;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            if (!fxData[effectId].isFollow)
            {
                transform.parent = null;
                transform.position = target.position + position;
            }
        }
        else
        {
            transform.parent = null;
            transform.position = position;
        }

        switch (fxData[effectId].rotation)
        {
            case FXRotationCondition.HaveEnemyDirectionY:
                transform.localRotation = Quaternion.Euler(new Vector3(rotation.x, (target.localScale.x <= 0f) ? 0 : 180, rotation.z));
                break;
            case FXRotationCondition.RandomRotationZ:
                transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(0, 360)));
                break;
            case FXRotationCondition.Preference:
                transform.localRotation = Quaternion.Euler(rotation);
                break;
            case FXRotationCondition.HaveEnemyDirectionZ:
                transform.localRotation = Quaternion.Euler(new Vector3(rotation.x, rotation.y, (target.localScale.x <= 0f) ? 0 : 180));
                break;
            case FXRotationCondition.FollowPlayer:
                transform.localRotation = Quaternion.Euler(new Vector3(rotation.x, (target.localScale.x >= 0f) ? 0 : 180, rotation.z));
                break;
            case FXRotationCondition.FollowTargeRotation:
                transform.localRotation = target.rotation;
                break;
        }

        transform.localScale = ((!(scale != Vector3.zero)) ? fxData[effectId].scale : scale);
        if (useFxZNum)
        {
            Vector3 position2 = transform.position;
            position2.z = LayerManager.ZNum.Fx;
            transform.position = position2;
        }

        transform.gameObject.SetActive(true);
        return transform;
    }

    public GameObject UsePool(string effectName)
    {
        if (_objectPoolDict.ContainsKey(effectName))
        {
            return _objectPoolDict[effectName].GetObject();
        }

        return null;
    }

    public static void TerminateEffect(GameObject target)
    {
        AutoDisableEffect component = target.GetComponent<AutoDisableEffect>();
        if (component)
        {
            component.Disable(target.transform);
        }
        else
        {
            Destroy(target);
        }
    }

    private void Preload()
    {
        _objectPoolDict = PoolController.EffectDict;
        for (int i = 0; i < fxSerializedData.Count; i++)
        {
            try
            {
                EffectAttr effectAttr = fxSerializedData[i];
                if (effectAttr.effect != null)
                {
                    AutoDisableEffect component = effectAttr.effect.GetComponent<AutoDisableEffect>();
                    if (!_objectPoolDict.ContainsKey(effectAttr.effect.name) && component != null)
                    {
                        ObjectPool objectPool = new ObjectPool();
                        StartCoroutine(objectPool.Init(effectAttr.effect.gameObject, gameObject, effectAttr.effectStartCount,
                            effectAttr.maxCount));
                        _objectPoolDict.Add(effectAttr.effect.name, objectPool);
                    }
                }
            }
            catch (Exception)
            {
                i.ToString().Error();
            }
        }
    }

    [SerializeField] private List<EffectAttr> _fxSerializedData;

    private Dictionary<int, EffectAttr> _fxData;

    private Dictionary<string, ObjectPool> _objectPoolDict;

    public static bool AllowPreload = true;

    /// <summary>
    /// 效果旋转情况
    /// </summary>
    public enum FXRotationCondition
    {
        /// <summary>
        /// 敌人方向是Y
        /// </summary>
        HaveEnemyDirectionY = 1,

        /// <summary>
        /// 随机旋转Z
        /// </summary>
        RandomRotationZ,

        /// <summary>
        /// 优先
        /// </summary>
        Preference,

        /// <summary>
        /// 敌人方向是Z
        /// </summary>
        HaveEnemyDirectionZ,

        /// <summary>
        ///  跟随玩家
        /// </summary>
        FollowPlayer,

        /// <summary>
        /// 跟随目标旋转
        /// </summary>
        FollowTargeRotation
    }
}