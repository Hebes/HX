using UnityEngine;

/// <summary>
/// 创建
/// </summary>
public class OnionCreator : MonoBehaviour
{
    private void Update()
    {
        if (!this._open)
        {
            return;
        }

        OnionCreator.GeneratorMode generatorMode = this.generatorMode;
        if (generatorMode != OnionCreator.GeneratorMode.Time)
        {
            if (generatorMode == OnionCreator.GeneratorMode.Distance)
            {
                if (Vector3.Distance(this._lastPostion, base.transform.position) > this.rate)
                {
                    this.GeneratorGhost(this._lastPostion, base.transform.position, this.rate);
                    this._lastPostion = base.transform.position;
                }
            }
        }
        else
        {
            this._onionTimer += Time.deltaTime;
            if (this._onionTimer >= this.rate)
            {
                this._onionTimer = 0f;
                this.GeneratorGhost();
            }
        }

        if (!this._autoClose)
        {
            return;
        }

        if (Time.time >= this._autoCloseTime)
        {
            this.Close();
        }
    }

    public void Open(bool _autoClose = true, float _autoCloseTime = 1f, GameObject[] cloneds = null)
    {
        if (this._open)
        {
            if (!_autoClose)
            {
                return;
            }

            float num = Time.time + _autoCloseTime;
            if (num < this._autoCloseTime)
            {
                return;
            }
        }

        if (cloneds == null)
        {
            this._needBeClones = new GameObject[]
            {
                base.gameObject
            };
        }
        else
        {
            this._needBeClones = cloneds;
        }

        this._open = true;
        if (_autoClose)
        {
            this._autoClose = true;
            this._autoCloseTime = Time.time + _autoCloseTime;
        }

        if (this.generatorMode == OnionCreator.GeneratorMode.Time)
        {
            if (this.executionMode == OnionCreator.ExecutionMode.Immediately)
            {
                this._onionTimer = float.MaxValue;
            }

            if (this.executionMode == OnionCreator.ExecutionMode.NextUpdate)
            {
                this._onionTimer = 0f;
            }
        }

        if (this.generatorMode == OnionCreator.GeneratorMode.Distance)
        {
            if (this.executionMode == OnionCreator.ExecutionMode.NextUpdate)
            {
                this._lastPostion = base.transform.position;
            }

            if (this.executionMode == OnionCreator.ExecutionMode.Immediately)
            {
                this._lastPostion = Vector3.zero;
            }
        }
    }

    public void Close()
    {
        this._open = false;
        this._autoClose = false;
    }

    private void GeneratorGhost()
    {
        if (this._needBeClones == null || this._needBeClones.Length == 0)
        {
            return;
        }

        this.CloneGhostAtPos(base.transform.position);
    }

    private void GeneratorGhost(Vector3 fromPos, Vector3 toPos, float rate)
    {
        if (this._needBeClones == null || this._needBeClones.Length == 0)
        {
            return;
        }

        float num = Vector3.Distance(fromPos, toPos) / rate;
        int num2 = 0;
        while ((float)num2 < num)
        {
            Vector3 pos = Vector3.Slerp(fromPos, toPos, (float)num2 / num);
            this.CloneGhostAtPos(pos);
            num2++;
        }
    }

    private void CloneGhostAtPos(Vector3 pos)
    {
        for (int i = 0; i < this._needBeClones.Length; i++)
        {
            GameObject gameObject = this._needBeClones[i];
            MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
            if (gameObject.activeSelf && component.enabled)
            {
                Transform transform = R.Effect.Generate(this._onionId, null, pos + new Vector3(0f, 0f, 0.001f), Vector3.zero, default(Vector3), true);
                if (OnionCreator._ghostParent == null)
                {
                    OnionCreator._ghostParent = new GameObject("GhostParent").transform;
                    OnionCreator._ghostParent.parent = R.Effect.transform;
                }

                transform.parent = OnionCreator._ghostParent;
                transform.localScale = base.transform.localScale;
                OnionController component2 = transform.GetComponent<OnionController>();
                component2.Clone(gameObject);
                component2.SetDisappear(this.disappearCurve, this.disappearTime);
            }
        }
    }

    private static Transform _ghostParent;

    public float rate = 0.1f;

    public float disappearTime = 1f;

    public AnimationCurve disappearCurve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

    private bool _open;

    private bool _autoClose;

    private float _autoCloseTime = 1f;

    public OnionCreator.GeneratorMode generatorMode = OnionCreator.GeneratorMode.Time;

    public OnionCreator.ExecutionMode executionMode;

    [SerializeField] private int _onionId = 218;

    private float _onionTimer = 10f;

    private Vector3 _lastPostion;

    private GameObject[] _needBeClones;

    public enum ExecutionMode
    {
        Immediately,
        NextUpdate
    }

    public enum GeneratorMode
    {
        Distance,
        Time
    }
}