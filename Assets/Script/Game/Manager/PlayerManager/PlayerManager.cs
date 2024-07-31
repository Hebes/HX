using System.Linq;
using UnityEngine;

public class PlayerManager
{
    private GameObject _gameObject;
    private Transform _transform;
    private PlayerAction _action;
    private PlayerAbilities _abilities;
    private PlayerTimeController _timeController;
    private StateMachine _stateMachine;
    private Rigidbody2D _rigidbody2D;
    private PlayerActionController _actionController;
    public readonly PlayerAttribute Attribute = new PlayerAttribute();

    public GameObject GameObject
    {
        get
        {
            GameObject result;
            if ((result = _gameObject) == null)
            {
                result = _gameObject = GameObject.FindGameObjectsWithTag("Player")
                    .FirstOrDefault((GameObject p) => p.GetComponent<PlayerAction>() != null);
            }

            return result;
        }
        private set => _gameObject = value;
    }

    public Transform Transform
    {
        get
        {
            Transform result;
            if ((result = _transform) == null)
                result = this.GameObject == null ? null : _transform = GameObject.transform;
            return result;
        }
    }

    /// <summary>
    /// 用户名称
    /// </summary>
    public string UserName => R.GameData.UserName;

    /// <summary>
    /// 角色名称
    /// </summary>
    public string RoleName
    {
        get => R.GameData.RoleName;
        set => R.GameData.RoleName = value;
    }

    /// <summary>
    /// 玩家行动
    /// </summary>
    public PlayerAction Action
    {
        get
        {
            PlayerAction result;
            if ((result = this._action) == null)
                result = _action = GetComponent<PlayerAction>();
            return result;
        }
    }

    /// <summary>
    /// 玩家属性
    /// </summary>
    public PlayerAbilities Abilities
    {
        get
        {
            PlayerAbilities result;
            if ((result = this._abilities) == null)
                result = _abilities = GetComponent<PlayerAbilities>();
            return result;
        }
    }

    public PlayerTimeController TimeController
    {
        get
        {
            PlayerTimeController result;
            if ((result = this._timeController) == null)
            {
                result = (this._timeController = this.GetComponent<PlayerTimeController>());
            }

            return result;
        }
    }

    public StateMachine StateMachine
    {
        get
        {
            StateMachine result;
            if ((result = _stateMachine) == null)
                result = _stateMachine = GetComponent<StateMachine>();
            return result;
        }
    }

    public Rigidbody2D Rigidbody2D
    {
        get
        {
            Rigidbody2D result;
            if ((result = _rigidbody2D) == null)
                result = _rigidbody2D = GetComponent<Rigidbody2D>();
            return result;
        }
    }

    public EnhancementSaveData EnhancementSaveData => R.GameData.EnhancementSaveData;

    public PlayerActionController ActionController
    {
        get
        {
            PlayerActionController result;
            if ((result = _actionController) == null)
                result = _actionController = GetComponent<PlayerActionController>();
            return result;
        }
    }

    public T GetComponent<T>()
    {
        return this.GameObject.GetComponent<T>();
    }

    public void SetPosition(Vector2 position)
    {
        this.Transform.position = new Vector3(position.x, position.y, this.Transform.position.z);
    }

    public void Kill()
    {
        this.Attribute.currentHP = 0;
    }

    public bool IsNearSceneGate()
    {
        for (int i = 0; i < R.SceneGate.GatesInCurrentScene.Count; i++)
        {
            SceneGate sceneGate = R.SceneGate.GatesInCurrentScene[i];
            if (MathfX.isInMiddleRange(this.Transform.position.x, sceneGate.transform.position.x,
                    sceneGate.data.TriggerSize.x))
            {
                return true;
            }
        }

        return false;
    }

    public bool CanExecute()
    {
        return this.Action.pab.execute.CanExecute;
    }
}